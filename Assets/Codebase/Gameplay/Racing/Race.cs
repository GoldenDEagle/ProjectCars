using Assets.Codebase.Data.Cars.Enemy;
using System.Collections.Generic;

namespace Assets.Codebase.Gameplay.Racing
{
    public class Race
    {
        private List<EnemyCarId> _enemiesList;
        private int _lapsCount;

        public List<EnemyCarId> EnemiesList => _enemiesList;

        public Race(int lapsCount, List<EnemyCarId> enemies)
        {
            _lapsCount = lapsCount;
            _enemiesList = enemies;
        }
    }
}
