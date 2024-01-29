using Assets.Codebase.Presenter.Base;

namespace Assets.Codebase.Presenters.CarSelection
{
    public interface ICarSelectionPresenter : IPresenter
    {
        public void ConfirmSelectionButtonClicked();
        public void RightArrowClicked();
        public void LeftArrowClicked();
    }
}
