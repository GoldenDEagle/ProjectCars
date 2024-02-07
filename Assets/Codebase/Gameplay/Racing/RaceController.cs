using Assets.Codebase.Data.Audio;
using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Audio;
using Assets.Codebase.Infrastructure.ServicesManagment.Localization;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation;
using Assets.Codebase.Views.Base;
using DavidJalbert.TinyCarControllerAdvance;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Assets.Codebase.Gameplay.Racing
{
    public class RaceController : MonoBehaviour
    {
        private const string CountdownEndKey = "countdown_go";

        [SerializeField] private CarSpawner _carSpawner;
        [SerializeField] private WaypointCircuit _waypointCircuit;
        [SerializeField] private TCCACamera _camera;
        [SerializeField] private TCCAStandardInput _standartInput;
        [SerializeField] private Finish _finish;

        private List<EnemyCar> _enemyCars;
        private PlayerCar _playerCar;
        private bool _isRaceActive = false;
        private Coroutine _positionChecker;
        private TCCAMobileInput _mobileInput;
        private WaitForSeconds _oneSecDelay = new WaitForSeconds(1f);

        private int _playerPosition;

        private IModelAccessService _models;
        private IViewProvider _viewProvider;
        private IAudioService _audio;

        private void Awake()
        {
            _models = ServiceLocator.Container.Single<IModelAccessService>();
            _viewProvider = ServiceLocator.Container.Single<IViewProvider>();
            _audio = ServiceLocator.Container.Single<IAudioService>();
        }

        private void Start()
        {
            _audio.EnableMusic(false);
            SpawnCars();
            _camera.resetCamera();

            if (_models.GameplayModel.IsMobile && !_models.ProgressModel.SessionProgress.MobileTutorialCompleted.Value)
            {
                // Show mobile tutorial
                _mobileInput.tutorialObject.gameObject.SetActive(true);
                _mobileInput.tutorialObject.OnTutorialClosed += TutorialWasClosed;
                _mobileInput.tutorialObject.ActivateTutorial();
            }
            else if (!_models.GameplayModel.IsMobile && !_models.ProgressModel.SessionProgress.PCTutorialCompleted.Value)
            {
                // Show PC tutorial
            }
            else
            {
                StartCoroutine(RacingCountdown());
            }
        }

        private void SpawnCars()
        {
            _playerCar = _carSpawner.SpawnPlayer();

            // Attach player car to camera and race events
            _playerCar.WaypointTracker.AttachCircuit(_waypointCircuit);
            _playerCar.OnLapCompleted.Subscribe(lap => PlayerPassedLap(lap));
            _camera.carController = _playerCar.CarController;
            
            // Input activation
            _standartInput.carController = _playerCar.CarController;
            if (_models.GameplayModel.IsMobile)
            {
                _mobileInput = _viewProvider.MobileInput;
                _mobileInput.carController = _playerCar.CarController;
                _mobileInput.gameObject.SetActive(true);
            }

            // Car activation
            _playerCar.gameObject.SetActive(true);

            // Enemies spawn
            _enemyCars = _carSpawner.SpawnEnemies();
            foreach (var enemy in _enemyCars)
            {
                enemy.WaypointTracker.AttachCircuit(_waypointCircuit);
                enemy.gameObject.SetActive(true);
            }

            List<ICar> cars = new List<ICar>();
            cars.Add(_playerCar);
            cars.AddRange(_enemyCars);

            _finish.SetCars(cars);
        }

        private IEnumerator PositionTracker()
        {
            while (_isRaceActive)
            {
                CheckPositions();
                //Debug.Log($"Player position: {_playerPosition}, Lap: {_playerCar.LapNumber}");
                yield return _oneSecDelay;
            }
        }

        private IEnumerator RacingCountdown()
        {
            int time = 3;
            _viewProvider.Countdown.Activate(true);

            while (time > 0)
            {
                _audio.PlaySfxSound(SoundId.CountdownBeep);
                _viewProvider.Countdown.ShowText(time.ToString());
                yield return _oneSecDelay;
                time--;
            }

            _audio.PlaySfxSound(SoundId.CountdownEnd);
            _viewProvider.Countdown.ShowText(ServiceLocator.Container.Single<ILocalizationService>().LocalizeTextByKey(CountdownEndKey)); ;
            StartRace();

            yield return _oneSecDelay;
            _viewProvider.Countdown.Activate(false);
            // Remove counter
        }

        private void StartRace()
        {
            _playerPosition = 1;
            _isRaceActive = true;
            SetInputState(true);
            foreach (var enemy in _enemyCars)
            {
                enemy.AIControl.enabled = true;
                enemy.StartCheckingMovement();
            }
            _positionChecker = StartCoroutine(PositionTracker());
            StartCoroutine(ActivateFinishAfterDelay());
        }

        private IEnumerator ActivateFinishAfterDelay()
        {
            yield return new WaitForSeconds(10f);
            _finish.SetCollidersState(true);
        }

        private void CheckPositions()
        {
            int tempPlayerPosition = _enemyCars.Count + 1;
            var playerWaypointIndex =  _waypointCircuit.GetIndexOfTheWaypoint(_playerCar.GetClosestWaypoint(_waypointCircuit.waypointList.items));

            List<int> enemiesWaypoints = new List<int>();
            foreach (var enemy in _enemyCars)
            {
                var enemyWaypoint = _waypointCircuit.GetIndexOfTheWaypoint(enemy.GetClosestWaypoint(_waypointCircuit.waypointList.items));

                // if lap difference
                if (_playerCar.LapNumber > enemy.LapNumber)
                {
                    tempPlayerPosition--;
                    continue;
                }
                else if (_playerCar.LapNumber < enemy.LapNumber)
                {
                    continue;
                }
                
                // if on the same lap
                if (playerWaypointIndex >= enemyWaypoint)
                {
                    tempPlayerPosition--;
                }
            }

            _playerPosition = tempPlayerPosition;
            _models.GameplayModel.CurrentPosition.Value = _playerPosition;
        }

        private void PlayerPassedLap(int lapNumber)
        {
            if (lapNumber > _models.GameplayModel.ActiveRace.Value.TotalLaps)
            {
                FinishRace();
            }
            else
            {
                _audio.PlaySfxSound(SoundId.LapPassed);
            }

            // Update Lap counter
            _models.GameplayModel.CurrentLap.Value = lapNumber;
        }

        private void FinishRace()
        {
            StopCoroutine(_positionChecker);
            _isRaceActive = false;
            SetInputState(false);
            _playerCar.CarController.setMotor(0f);

            // Save race results in model

            _models.GameplayModel.ActiveRace.Value.WriteRaceResult(_playerPosition);
            _models.GameplayModel.CalculateReward();

            if (_playerPosition <= 3)
            {
                _audio.ChangeMusic(SoundId.WinJingle);
            }
            else
            {
                _audio.ChangeMusic(SoundId.LoseJingle);
            }
            _audio.EnableMusic(true);

            _models.GameplayModel.ActivateView(ViewId.EndGame);
        }


        private void SetInputState(bool isEnabled)
        {
            if (_models.GameplayModel.IsMobile)
            {
                _mobileInput.enabled = isEnabled;
            }
            else
            {
                _standartInput.enabled = isEnabled;
            }
        }


        private void TutorialWasClosed()
        {
            _mobileInput.tutorialObject.OnTutorialClosed -= TutorialWasClosed;
            _models.ProgressModel.SessionProgress.MobileTutorialCompleted.Value = true;

            // Start the race
            StartCoroutine(RacingCountdown());
        }
    }
}