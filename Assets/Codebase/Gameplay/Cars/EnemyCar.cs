using Assets.Codebase.Data.Cars.Enemy;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Vehicles.Car;

namespace Assets.Codebase.Gameplay.Cars
{
    public class EnemyCar : MonoBehaviour, ICar
    {
        [SerializeField] private EnemyCarId _carId;
        [SerializeField] private WaypointProgressTracker _waypointTracker;
        [SerializeField] private CarAIControl _aiControl;

        private int _lapNumber = 1;
        private Coroutine _movementCheckRoutine;
        private Transform _closestWaypoint;

        public WaypointProgressTracker WaypointTracker => _waypointTracker;
        public CarAIControl AIControl => _aiControl;
        public int LapNumber => _lapNumber;

        public void StartCheckingMovement()
        {
            _movementCheckRoutine = StartCoroutine(CheckMovementRoutine());
        }

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

            _closestWaypoint = bestTarget;
            return bestTarget;
        }

        private IEnumerator CheckMovementRoutine()
        {
            while (true)
            {
                var initialPosition = transform.position;

                yield return new WaitForSeconds(3f);
                
                // if position didn't change significantly => reset position
                if ((transform.position - initialPosition).magnitude < 1f)
                {
                    _aiControl.SetPosition(_closestWaypoint);
                }
            }
        }

        public void RespawnAtClosestWaypoint()
        {
            _aiControl.SetPosition(_closestWaypoint);
        }
    }
}