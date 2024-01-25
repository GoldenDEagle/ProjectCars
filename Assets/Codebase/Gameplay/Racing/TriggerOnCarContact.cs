using Assets.Codebase.Gameplay.Cars;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class TriggerOnCarContact : MonoBehaviour
    {
        public event Action<ICar> OnCarContact;

        private ICar _lastContactedCar;
        private Coroutine _lastContactReseter;

        private void OnTriggerEnter(Collider other)
        {
            var car = other.gameObject.GetComponentInParent<ICar>();

            if (car != null )
            {
                if (car == _lastContactedCar)
                {
                    return;
                }

                _lastContactedCar = car;
                OnCarContact?.Invoke(car);

                if (_lastContactReseter != null)
                {
                    StopCoroutine( _lastContactReseter );
                    _lastContactReseter = null;
                }

                _lastContactReseter = StartCoroutine(ForgetLastContactAfterDelay());
            }
        }


        private IEnumerator ForgetLastContactAfterDelay()
        {
            yield return new WaitForSeconds(10f);

            _lastContactedCar = null;
        }
    }
}
