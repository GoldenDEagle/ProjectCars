using Assets.Codebase.Data.Cars.Player;
using DavidJalbert.TinyCarControllerAdvance;
using UniRx;
using UnityEngine;
using UnityStandardAssets.Utility;

namespace Assets.Codebase.Gameplay.Cars
{
    public class PlayerCar : MonoBehaviour, ICar
    {
        [SerializeField] private PlayerCarId _carId;
        [SerializeField] private TCCAPlayer _carController;
        [SerializeField] private WaypointProgressTracker _waypointTracker;

        private Transform _closestWaypoint;
        private int _lapNumber = 1;

        public PlayerCarId CarId => _carId;
        public TCCAPlayer CarController => _carController;
        public WaypointProgressTracker WaypointTracker => _waypointTracker;
        public Transform ClosestWaypoint => _closestWaypoint;

        public int LapNumber => _lapNumber;
        public Subject<int> OnLapCompleted = new Subject<int>();
        

        public void AddLap()
        {
            _lapNumber++;
            OnLapCompleted?.OnNext(_lapNumber);
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
    }
}