using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Gameplay.Cars;

namespace Assets.Codebase.Infrastructure.ServicesManagment.CarCreation
{
    public interface ICarFactory : IService
    {
        public PlayerCar CreatePlayerCar(PlayerCarId carId);
        public EnemyCar CreateEnemyCar(EnemyCarId carId);
    }
}
