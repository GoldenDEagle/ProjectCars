using UnityEngine;
using UnityStandardAssets.Utility;

namespace Assets.Codebase.Gameplay.Cars
{
    public class EnemyCar : MonoBehaviour
    {
        [SerializeField] private WaypointProgressTracker _waypointTracker;

        public WaypointProgressTracker WaypointTracker => _waypointTracker;
    }
}