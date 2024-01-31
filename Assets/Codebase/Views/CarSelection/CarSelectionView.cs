using Assets.Codebase.Models.Progress;
using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Utils.Extensions;
using Assets.Codebase.Views.Base;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.CarSelection
{
    public class CarSelectionView : BaseView
    {
        [SerializeField] private TMP_Text _totalCoinsText;
        [SerializeField] private TMP_Text _buyButtonText;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _confirmSelectionButton;
        [SerializeField] private Button _leftArrowButton;
        [SerializeField] private Button _rightArrowButton;

        private ICarSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ICarSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.ConfirmSelectionButtonActiveState.Subscribe(value => _confirmSelectionButton.gameObject.SetActive(value)).AddTo(CompositeDisposable);
            _presenter.BuyButtonActiveState.Subscribe(value => _buyButton.gameObject.SetActive(value)).AddTo(CompositeDisposable);
            _presenter.TotalCoinsString.SubscribeToTMPText(_totalCoinsText).AddTo(CompositeDisposable);
            _presenter.BuyButtonString.SubscribeToTMPText(_buyButtonText).AddTo(CompositeDisposable);
        }

        protected override void SubscribeToUserInput()
        {
            _confirmSelectionButton.OnClickAsObservable().Subscribe(_ => _presenter.ConfirmSelectionButtonClicked()).AddTo(CompositeDisposable);
            _leftArrowButton.OnClickAsObservable().Subscribe(_ => _presenter.LeftArrowClicked()).AddTo(CompositeDisposable);
            _rightArrowButton.OnClickAsObservable().Subscribe(_ => _presenter.RightArrowClicked()).AddTo(CompositeDisposable);
            _buyButton.OnClickAsObservable().Subscribe(_ => _presenter.BuyButtonClicked()).AddTo(CompositeDisposable);
        }
    }
}