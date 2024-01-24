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

        private ITrackSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ITrackSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
            _goButton.OnClickAsObservable().Subscribe(_ => _presenter.GoButtonClicked()).AddTo(CompositeDisposable);
        }
    }
}