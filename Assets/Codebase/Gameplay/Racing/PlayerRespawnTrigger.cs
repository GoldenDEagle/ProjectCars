using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation;
using System.Collections;
using UnityEngine;

namespace Assets.Codebase.Gameplay.Racing
{
    public class PlayerRespawnTrigger : MonoBehaviour
    {
        private PlayerCar _playerCar;
        private Coroutine _lastContactReseter;
        private Coroutine _positionResetter;

        private IViewProvider _viewProvider;

        private void Awake()
        {
            _viewProvider = ServiceLocator.Container.Single<IViewProvider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var newContact = other.gameObject.GetComponentInParent<PlayerCar>();

            if (newContact == null) return;
            if (newContact == _playerCar) return;

            _playerCar = newContact;

            // Launch position reset coroutine
            if (_positionResetter != null)
            {
                StopCoroutine(_positionResetter);
                _positionResetter = null;
            }

            _positionResetter = StartCoroutine(RespawnTargetAfterDelay(_playerCar));


            // Launch contact resetter
            if (_lastContactReseter != null)
            {
                StopCoroutine(_lastContactReseter);
                _lastContactReseter = null;
            }

            _lastContactReseter = StartCoroutine(ForgetLastContactAfterDelay());
        }

        private void OnTriggerExit(Collider other)
        {
            var newContact = other.gameObject.GetComponentInParent<PlayerCar>();

            if (newContact == null) return;

            // Stop position reset
            if (_positionResetter != null)
            {
                StopCoroutine(_positionResetter);
                _positionResetter = null;
                _viewProvider.WayWarning.gameObject.SetActive(false);
            }
        }

        private IEnumerator ForgetLastContactAfterDelay()
        {
            yield return new WaitForSeconds(1f);

            _playerCar = null;
        }

        private IEnumerator RespawnTargetAfterDelay(PlayerCar carToRespawn)
        {
            _viewProvider.WayWarning.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            carToRespawn.CarController.enabled = false;
            var closestWaypoint = carToRespawn.ClosestWaypoint;
            carToRespawn.CarController.setRotation(closestWaypoint.rotation);
            carToRespawn.CarController.setPosition(closestWaypoint.position + Vector3.up);
            carToRespawn.CarController.enabled = true;
            _viewProvider.WayWarning.gameObject.SetActive(false);
        }
    }
}
