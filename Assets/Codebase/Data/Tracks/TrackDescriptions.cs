using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Data.Tracks
{
    [Serializable]
    [CreateAssetMenu(menuName = "Create Track descriptions", fileName = "TrackDescriptions", order = 51)]
    public class TrackDescriptions : ScriptableObject
    {
        [SerializeField] private List<TrackInfo> _tracks;

        public List<TrackInfo> Tracks => _tracks;
    }
}
