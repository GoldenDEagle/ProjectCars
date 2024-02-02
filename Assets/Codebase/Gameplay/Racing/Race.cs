using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Tracks;
using System.Collections.Generic;
using UniRx;

namespace Assets.Codebase.Gameplay.Racing
{
    public class Race
    {
        private TrackId _trackId;
        private List<EnemyCarId> _enemiesList;
        private int _totalLaps;
        private RaceResult _result;

        public TrackId TrackId => _trackId;
        public List<EnemyCarId> EnemiesList => _enemiesList;
        public int TotalLaps => _totalLaps;
        public RaceResult Result => _result;

        public Race(TrackId trackId, int lapsCount, List<EnemyCarId> enemies)
        {
            _trackId = trackId;
            _totalLaps = lapsCount;
            _enemiesList = enemies;
        }

        public void WriteRaceResult(int playerPosition)
        {
            _result = new RaceResult(playerPosition);
        }
    }
}
