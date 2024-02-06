using Assets.Codebase.Data.Audio;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Audio;
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
        private IAudioService _audio;
        private CompositeDisposable _disposables = new CompositeDisposable();

        public List<PlayerCar> AvailableCars => _availableCars;

        private void Awake()
        {
            _presenterService = ServiceLocator.Container.Single<IPresentersService>();
            _audio = ServiceLocator.Container.Single<IAudioService>();
        }

        private void Start()
        {
            _audio.ChangeMusic(SoundId.MainTheme);
            _audio.EnableMusic(true);
        }

        private void OnEnable()
        {
            var presenter = _presenterService.GetPresenter(ViewId.CarSelection) as ICarSelectionPresenter;
            presenter.DisplayedCar.Subscribe(value => ShowSelectedCar(value)).AddTo(_disposables);
            presenter.OnCloseView.Subscribe(_ => HideAllCars()).AddTo(_disposables);
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

        private void HideAllCars()
        {
            foreach (var car in _availableCars)
            {
                car.gameObject.SetActive(false);
            }
        }
    }
}
