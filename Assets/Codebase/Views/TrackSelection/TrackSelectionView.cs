using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Views.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.TrackSelection
{
    public class TrackSelectionView : BaseView
    {
        [SerializeField] private Button _goButton;
        [SerializeField] private Image _trackImage;
        [SerializeField] private Button _rightArrow;
        [SerializeField] private Button _leftArrow;

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
        }

        protected override void SubscribeToPresenterEvents()
        {
            base.SubscribeToPresenterEvents();
            _presenter.SelectedTrackIcon.Subscribe(value => { _trackImage.sprite = value; }).AddTo(CompositeDisposable);
        }
    }
}