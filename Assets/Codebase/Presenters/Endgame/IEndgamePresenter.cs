using Assets.Codebase.Presenter.Base;
using UniRx;

namespace Assets.Codebase.Presenters.Endgame
{
    public interface IEndgamePresenter : IPresenter
    {
        public ReactiveProperty<string> PositionString { get; }

        public void ContinueButtonClicked();
    }
}
