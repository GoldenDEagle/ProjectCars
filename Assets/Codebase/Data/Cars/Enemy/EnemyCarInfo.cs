using System;
using UnityEngine;

namespace Assets.Codebase.Data.Cars.Enemy
{
    [Serializable]
    public class EnemyCarInfo
    {
        [SerializeField] private EnemyCarId _carId;
        [SerializeField] private GameObject _prefab;

        public EnemyCarId CarId => _carId;
        public GameObject Prefab => _prefab;
    }
}
