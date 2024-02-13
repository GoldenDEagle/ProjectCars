using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class RaceResult
    {
        [SerializeField] private int _position;
        [SerializeField] private float _time;

        public int Position => _position;
        public float Time => _time;

        public RaceResult(int playerPosition, float time = 999999999)
        {
            _position = playerPosition;
            _time = time;
        }
    }
}
