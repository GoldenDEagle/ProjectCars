using DavidJalbert.TinyCarControllerAdvance;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Codebase.Infrastructure.ServicesManagment.CarCreation
{
    public interface ICarFactory : IService
    {
        public TCCAPlayer CreatePlayerCar();
        public CarAIControl CreateEnemyCar();
    }
}
