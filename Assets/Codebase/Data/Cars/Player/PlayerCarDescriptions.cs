using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Data.Cars.Player
{
    [Serializable]
    [CreateAssetMenu(menuName = "Create Player cars description", fileName = "PlayerCarsDescription", order = 51)]
    public class PlayerCarDescriptions : ScriptableObject
    {
        [SerializeField] private List<PlayerCarInfo> _carsList;

        public List<PlayerCarInfo> CarsList => _carsList;
    }
}
