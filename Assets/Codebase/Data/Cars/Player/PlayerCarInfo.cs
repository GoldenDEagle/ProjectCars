using Assets.Codebase.Gameplay.Cars;
using System;
using UnityEngine;

namespace Assets.Codebase.Data.Cars.Player
{
    [Serializable]
    public class PlayerCarInfo
    {
        [SerializeField] private PlayerCarId _carId;
        [SerializeField] private int _price;
        [SerializeField] private PlayerCar _prefab;

        public PlayerCarId CarId => _carId;
        public int Price => _price;
        public PlayerCar Prefab => _prefab;
    }
}
