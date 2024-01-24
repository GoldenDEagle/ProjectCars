using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Views.CarSelection
{
    public class CarSelectionView : BaseView
    {
        [SerializeField] private Button _confirmSelectionButton;

        private ICarSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ICarSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
            _confirmSelectionButton.OnClickAsObservable().Subscribe(_ => _presenter.ConfirmSelectionButtonClicked()).AddTo(CompositeDisposable);
        }
    }
}