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


        public Transform GetClosestWaypoint(Transform[] waypoints)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = _waypointTracker.transform.position;
            foreach (Transform potentialTarget in waypoints)
            {
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            return bestTarget;
        }
    }
}