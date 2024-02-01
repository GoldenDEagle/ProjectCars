using Assets.Codebase.Gameplay.Cars;
using System.Collections;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class RespawnTrigger : MonoBehaviour
    {
        private EnemyCar _lastContactedEnemy;
        private Coroutine _lastContactReseter;


        private void OnTriggerEnter(Collider other)
        {
            var newContact = other.gameObject.GetComponentInParent<EnemyCar>();

            if (newContact == null) return;
            if (newContact == _lastContactedEnemy) return;

            _lastContactedEnemy = newContact;
            StartCoroutine(RespawnTargetAfterDelay(_lastContactedEnemy));

            if (_lastContactReseter != null)
            {
                StopCoroutine(_lastContactReseter);
                _lastContactReseter = null;
            }

            _lastContactReseter = StartCoroutine(ForgetLastContactAfterDelay());
        }

        private IEnumerator ForgetLastContactAfterDelay()
        {
            yield return new WaitForSeconds(3f);

            _lastContactedEnemy = null;
        }

        private IEnumerator RespawnTargetAfterDelay(EnemyCar carToRespawn)
        {
            yield return new WaitForSeconds(1f);

            carToRespawn.RespawnAtClosestWaypoint();
        }
    }
}
