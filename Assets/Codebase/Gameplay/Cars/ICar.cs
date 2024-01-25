using UnityEngine;

namespace Assets.Codebase.Gameplay.Cars
{
    public interface ICar
    {
        public int LapNumber { get; }

        public Transform GetClosestWaypoint(Transform[] waypoints);
        public void AddLap();
    }
}
