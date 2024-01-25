using Assets.Codebase.Data.Cars.Enemy;
using System.Collections.Generic;

namespace Assets.Codebase.Gameplay.Racing
{
    public class Race
    {
        private List<EnemyCarId> _enemiesList;
        private int _totalLaps;

        public List<EnemyCarId> EnemiesList => _enemiesList;
        public int TotalLaps => _totalLaps;

        public Race(int lapsCount, List<EnemyCarId> enemies)
        {
            _totalLaps = lapsCount;
            _enemiesList = enemies;
        }
    }
}
