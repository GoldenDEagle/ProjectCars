using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
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

        public CarAIControl CreateEnemyCar(EnemyCarId carId)
        {
            var prefab = _modelAccessService.GameplayModel.GetEnemyCarInfo(carId).Prefab;
            CarAIControl aiController = _assets.Instantiate(prefab).GetComponent<CarAIControl>();
            return aiController;
        }

        public TCCAPlayer CreatePlayerCar(PlayerCarId carId)
        {
            var prefab = _modelAccessService.GameplayModel.GetPlayerCarInfo(carId).Prefab;
            TCCAPlayer carController = _assets.Instantiate(prefab).GetComponent<TCCAPlayer>();
            return carController;
        }
    }
}
