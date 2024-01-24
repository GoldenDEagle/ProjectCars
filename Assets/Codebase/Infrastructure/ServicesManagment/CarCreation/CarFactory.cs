using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using DavidJalbert.TinyCarControllerAdvance;
using System;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Codebase.Infrastructure.ServicesManagment.CarCreation
{
    public class CarFactory : ICarFactory
    {
        private IAssetProvider _assets;
        private IModelAccessService _modelAccessService;

        public CarFactory(IAssetProvider assets, IModelAccessService modelAccess)
        {
            _assets = assets;
            _modelAccessService = modelAccess;
        }

        public EnemyCar CreateEnemyCar(EnemyCarId carId)
        {
            var prefab = _modelAccessService.GameplayModel.GetEnemyCarInfo(carId).Prefab;
            EnemyCar enemyCar = _assets.Instantiate(prefab.gameObject).GetComponent<EnemyCar>();
            return enemyCar;
        }

        public PlayerCar CreatePlayerCar(PlayerCarId carId)
        {
            var prefab = _modelAccessService.GameplayModel.GetPlayerCarInfo(carId).Prefab;
            PlayerCar carController = _assets.Instantiate(prefab.gameObject).GetComponent<PlayerCar>();
            return carController;
        }
    }
}
