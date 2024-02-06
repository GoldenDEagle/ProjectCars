using Assets.Codebase.Gameplay.Cars;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private List<TriggerOnCarContact> _carTriggers;

        private Dictionary<ICar, int> _contactCounter;

        private void OnEnable()
        {
            foreach (var trigger in _carTriggers)
            {
                trigger.OnCarContact += OnCarPassedTrigger;
            }
        }

        private void OnDisable()
        {
            foreach (var trigger in _carTriggers)
            {
                trigger.OnCarContact -= OnCarPassedTrigger;
            }
        }

        public void SetCars(List<ICar> cars)
        {
            _contactCounter = new Dictionary<ICar, int>();
            foreach (ICar car in cars)
            {
                _contactCounter.Add(car, 0);
            }
        }

        private void OnCarPassedTrigger(ICar car)
        {
            _contactCounter[car] += 1;

            if (_contactCounter[car] >= _carTriggers.Count)
            {
                car.AddLap();
                _contactCounter[car] = 0;
            }
        }

        public void SetCollidersState(bool areEnabled)
        {
            foreach (var trigger in _carTriggers)
            {
                trigger.gameObject.SetActive(areEnabled);
            }
        }
    }
}
