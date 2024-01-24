using Assets.Codebase.Gameplay.Cars;
using System;
using UnityEngine;

namespace Assets.Codebase.Data.Cars.Enemy
{
    [Serializable]
    public class EnemyCarInfo
    {
        [SerializeField] private EnemyCarId _carId;
        [SerializeField] private EnemyCar _prefab;

        public EnemyCarId CarId => _carId;
        public EnemyCar Prefab => _prefab;
    }
}
