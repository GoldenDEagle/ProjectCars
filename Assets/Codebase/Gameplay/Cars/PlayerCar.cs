using DavidJalbert.TinyCarControllerAdvance;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Cars
{
    public class PlayerCar : MonoBehaviour
    {
        [SerializeField] private TCCAPlayer _carController;

        public TCCAPlayer CarController => _carController;
    }
}