using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using DavidJalbert.TinyCarControllerAdvance;
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

        private List<EnemyCar> _enemyCars;
        private PlayerCar _playerCar; 

        private IModelAccessService _models;

        private void Awake()
        {
            _models = ServiceLocator.Container.Single<IModelAccessService>();
        }

        private void Start()
        {
            SpawnCars();
        }

        private void SpawnCars()
        {
            _playerCar = _carSpawner.SpawnPlayer();
            _camera.carController = _playerCar.CarController;
            _standartInput.carController = _playerCar.CarController;
            _mobileInput.carController = _playerCar.CarController;

            _enemyCars = _carSpawner.SpawnEnemies();
            foreach (var enemy in _enemyCars)
            {
                enemy.WaypointTracker.AttachCircuit(_waypointCircuit);
                enemy.gameObject.SetActive(true);
            }
        }
    }
}