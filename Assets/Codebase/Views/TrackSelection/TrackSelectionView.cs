using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Utils.Extensions;
using Assets.Codebase.Utils.UI;
using Assets.Codebase.Views.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.TrackSelection
{
    public class TrackSelectionView : BaseView
    {
        [SerializeField] private TMP_Text _totalCoinsText;
        [SerializeField] private Button _goButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Image _trackImage;
        [SerializeField] private Button _rightArrow;
        [SerializeField] private Button _leftArrow;
        [SerializeField] private SoundButton _soundButton;
        [SerializeField] private Button _freeCoinsButton;

        private ITrackSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ITrackSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
            _goButton.OnClickAsObservable().Subscribe(_ => _presenter.GoButtonClicked()).AddTo(CompositeDisposable);
            _rightArrow.OnClickAsObservable().Subscribe(_ => _presenter.RightArrowClicked()).AddTo(CompositeDisposable);
            _leftArrow.OnClickAsObservable().Subscribe(_ => _presenter.LeftArrowClicked()).AddTo(CompositeDisposable);
            _soundButton.Button.OnClickAsObservable().Subscribe(_ => SoundButtonClicked()).AddTo(CompositeDisposable);
            _backButton.OnClickAsObservable().Subscribe(_ => _presenter.BackButtonClicked()).AddTo(CompositeDisposable);
            _freeCoinsButton.OnClickAsObservable().Subscribe(_ => _presenter.FreeCoinsButtonClicked()).AddTo(CompositeDisposable);
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.SelectedTrackIcon.Subscribe(value => { _trackImage.sprite = value; }).AddTo(CompositeDisposable);
            _presenter.TotalCoinsString.SubscribeToTMPText(_totalCoinsText).AddTo(CompositeDisposable);
        }

        private void SoundButtonClicked()
        {
            _presenter.SoundButtonClicked();
            _soundButton.SetIcon();
        }
    }
}