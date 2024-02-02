using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
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
        [SerializeField] private CarSpawner _carSpawner;
        [SerializeField] private WaypointCircuit _waypointCircuit;
        [SerializeField] private TCCACamera _camera;
        [SerializeField] private TCCAStandardInput _standartInput;
        [SerializeField] private TCCAMobileInput _mobileInput;
        [SerializeField] private Finish _finish;

        private List<EnemyCar> _enemyCars;
        private PlayerCar _playerCar;
        private bool _isRaceActive = false;
        private Coroutine _positionChecker;

        private int _playerPosition;

        private IModelAccessService _models;

        private void Awake()
        {
            _models = ServiceLocator.Container.Single<IModelAccessService>();
        }

        private void Start()
        {
            SpawnCars();
            _camera.resetCamera();
            StartCoroutine(RacingCountdown());
        }

        private void SpawnCars()
        {
            _playerCar = _carSpawner.SpawnPlayer();
            _playerCar.WaypointTracker.AttachCircuit(_waypointCircuit);
            _playerCar.OnLapCompleted.Subscribe(lap => PlayerPassedLap(lap));
            _camera.carController = _playerCar.CarController;
            _standartInput.carController = _playerCar.CarController;
            _mobileInput.carController = _playerCar.CarController;
            _playerCar.gameObject.SetActive(true);

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
                Debug.Log($"Player position: {_playerPosition}, Lap: {_playerCar.LapNumber}");
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator RacingCountdown()
        {
            int time = 3;

            while (time > 0)
            {
                Debug.Log($"Race in {time}");
                yield return new WaitForSeconds(1f);
                time--;
            }

            Debug.Log("Start!");
            StartRace();

            yield return new WaitForSeconds(1f);
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
                if (playerWaypointIndex > enemyWaypoint)
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
            Debug.Log("Race Finished!");

            _models.GameplayModel.ActivateView(ViewId.EndGame);
        }


        private void SetInputState(bool isEnabled)
        {
            if (_models.GameplayModel.IsMobile)
            {
                _mobileInput.gameObject.SetActive(isEnabled);
            }
            else
            {
                _standartInput.enabled = isEnabled;
            }
        }
    }
}