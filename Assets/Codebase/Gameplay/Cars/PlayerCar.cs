using DavidJalbert.TinyCarControllerAdvance;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Assets.Codebase.Gameplay.Cars
{
    public class PlayerCar : MonoBehaviour
    {
        [SerializeField] private TCCAPlayer _carController;
        [SerializeField] private WaypointProgressTracker _waypointTracker;

        public TCCAPlayer CarController => _carController;
        public WaypointProgressTracker WaypointTracker => _waypointTracker;
    }
}