using Assets.Codebase.Presenter.Base;

namespace Assets.Codebase.Presenters.Pause
{
    public interface IPausePresenter : IPresenter
    {
        public void ContinueClicked();
        public void QuitClicked();
        public void RestartClicked();
    }
}
