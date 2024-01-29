using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.PresenterManagement;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Assets.Codebase.Data.Cars
{
    public class CarOnSceneSelection : MonoBehaviour
    {
        [SerializeField] private List<PlayerCar> _availableCars;

        private IPresentersService _presenterService;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public List<PlayerCar> AvailableCars => _availableCars;

        private void Awake()
        {
            _presenterService = ServiceLocator.Container.Single<IPresentersService>();
        }

        private void OnEnable()
        {
            var presenter = _presenterService.GetPresenter(ViewId.CarSelection) as ICarSelectionPresenter;
            presenter.DisplayedCar.Subscribe(value => ShowSelectedCar(value)).AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.Dispose();
        }

        private void ShowSelectedCar(PlayerCarId carId)
        {
            foreach (var car in _availableCars)
            {
                if (car.CarId == carId)
                {
                    car.gameObject.SetActive(true);
                }
                else
                {
                    car.gameObject.SetActive(false);
                }
            }
        }
    }
}
