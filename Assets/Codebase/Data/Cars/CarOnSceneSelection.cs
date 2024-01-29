using Assets.Codebase.Gameplay.Cars;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Data.Cars
{
    public class CarOnSceneSelection : MonoBehaviour
    {
        [SerializeField] private List<PlayerCar> _availableCars;

        public List<PlayerCar> AvailableCars => _availableCars;
    }
}
