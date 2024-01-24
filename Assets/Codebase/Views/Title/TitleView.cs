using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Views.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.Title
{
    public class TitleView : BaseView
    {
        [SerializeField] private Button _startButton;

        private ITitlePresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ITitlePresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
            _startButton.OnClickAsObservable().Subscribe(_ => _presenter.StartButtonClicked()).AddTo(CompositeDisposable);
        }
    }
}