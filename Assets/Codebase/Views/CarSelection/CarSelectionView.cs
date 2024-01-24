using Assets.Codebase.Presenter.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;

namespace Assets.Codebase.Views.CarSelection
{
    public class CarSelectionView : BaseView
    {
        private ICarSelectionPresenter _presenter;

        public override void Init(IPresenter presenter)
        {
            _presenter = presenter as ICarSelectionPresenter;

            base.Init(_presenter);
        }

        protected override void SubscribeToUserInput()
        {
        }
    }
}