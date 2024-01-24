using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
using DavidJalbert.TinyCarControllerAdvance;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Codebase.Infrastructure.ServicesManagment.CarCreation
{
    public interface ICarFactory : IService
    {
        public TCCAPlayer CreatePlayerCar(PlayerCarId carId);
        public CarAIControl CreateEnemyCar(EnemyCarId carId);
    }
}
