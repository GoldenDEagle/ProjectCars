using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Data.Cars.Enemy
{
    [Serializable]
    [CreateAssetMenu(menuName = "Create Enemy cars description", fileName = "EnemyCarsDescription", order = 51)]
    public class EnemyCarDescriptions : ScriptableObject
    {
        [SerializeField] private List<EnemyCarInfo> _carsList;

        public List<EnemyCarInfo> CarsList => _carsList;
    }
}
