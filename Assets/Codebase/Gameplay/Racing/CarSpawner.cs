using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.CarCreation;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class CarSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _carSpawnPoints;

        private IModelAccessService _models;
        private ICarFactory _carFactory;

        private void Awake()
        {
            _models = ServiceLocator.Container.Single<IModelAccessService>();
            _carFactory = ServiceLocator.Container.Single<ICarFactory>();
        }

        public PlayerCar SpawnPlayer()
        {
            var playerCar = _carFactory.CreatePlayerCar(_models.ProgressModel.SessionProgress.SelectedCar.Value);
            playerCar.transform.position = _carSpawnPoints[0].transform.position;
            playerCar.transform.forward = _carSpawnPoints[0].transform.forward;
            return playerCar;
        }

        public List<EnemyCar> SpawnEnemies()
        {
            var enemyCars = new List<EnemyCar>();

            int positionIndex = 1;
            foreach (var carId in _models.GameplayModel.ActiveRace.Value.EnemiesList)
            {
                var enemy = _carFactory.CreateEnemyCar(carId);
                //enemy.transform.position = _carSpawnPoints[positionIndex].transform.position;
                //enemy.transform.forward = _carSpawnPoints[positionIndex].transform.forward;
                enemy.transform.position = Vector3.zero;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetPosition(_carSpawnPoints[positionIndex].transform);
                enemyCars.Add(enemy);
                positionIndex++;
            }

            return enemyCars;
        }
    }
}
