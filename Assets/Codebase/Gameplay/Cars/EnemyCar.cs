using Assets.Codebase.Data.Cars.Enemy;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Assets.Codebase.Gameplay.Cars
{
    public class EnemyCar : MonoBehaviour, ICar
    {
        [SerializeField] private EnemyCarId _carId;
        [SerializeField] private WaypointProgressTracker _waypointTracker;

        private int _lapNumber = 1;

        public WaypointProgressTracker WaypointTracker => _waypointTracker;
        public int LapNumber => _lapNumber;

        public void AddLap()
        {
            _lapNumber++;
        }

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