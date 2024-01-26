using System;
using UnityEngine;

namespace Assets.Codebase.Data.Tracks
{
    [Serializable]
    public class TrackInfo
    {
        [SerializeField] private TrackId _trackId;
        [SerializeField] private string _trackSceneName;
        [SerializeField] private int _numberOfEnemies;

        public TrackId TrackId => _trackId;
        public string TrackSceneName => _trackSceneName;
        public int NumberOfEnemies => _numberOfEnemies;
    }
}
