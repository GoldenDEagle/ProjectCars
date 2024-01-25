using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using DavidJalbert.TinyCarControllerAdvance;
using System.Collections;
using System.Collections.Generic;
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

        private int _playerPosition;

        private IModelAccessService _models;

        private void Awake()
        {
            _models = ServiceLocator.Container.Single<IModelAccessService>();
        }

        private void Start()
        {
            SpawnCars();
            _playerPosition = _enemyCars.Count;
            _isRaceActive = true;
            StartCoroutine(PositionTracker());
        }

        private void SpawnCars()
        {
            _playerCar = _carSpawner.SpawnPlayer();
            _playerCar.WaypointTracker.AttachCircuit(_waypointCircuit);
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
                yield return new WaitForSeconds(1f);
                CheckPositions();
                Debug.Log($"Player position: {_playerPosition}, Lap: {_playerCar.LapNumber}");
            }
        }

        private void CheckPositions()
        {
            int tempPlayerPosition = _enemyCars.Count + 1;
            var playerWaypointIndex =  _waypointCircuit.GetIndexOfTheWaypoint(_playerCar.GetClosestWaypoint(_waypointCircuit.waypointList.items));

            List<int> enemiesWaypoints = new List<int>();
            foreach (var enemy in _enemyCars)
            {
                // if lap difference
                if (_playerCar.LapNumber > enemy.LapNumber)
                {
                    tempPlayerPosition--;
                    continue;
                }
                else if (_playerCar.LapNumber > enemy.LapNumber)
                {
                    continue;
                }
                
                // if on the same lap

                var enemyWaypoint = _waypointCircuit.GetIndexOfTheWaypoint(enemy.GetClosestWaypoint(_waypointCircuit.waypointList.items));
                if (playerWaypointIndex > enemyWaypoint)
                {
                    tempPlayerPosition--;
                }
            }

            _playerPosition = tempPlayerPosition;
        }
    }
}