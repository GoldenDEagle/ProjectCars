using Assets.Codebase.Presenter.Base;
using UniRx;

namespace Assets.Codebase.Presenters.Endgame
{
    public interface IEndgamePresenter : IPresenter
    {
        public ReactiveProperty<bool> DoubleRewardButtonActiveState { get; }
        public ReactiveProperty<string> PositionString { get; }
        public ReactiveProperty<string> CoinRewardString { get; }

        public void ContinueButtonClicked();
        public void DoubleRewardButtonClicked();
    }
}
