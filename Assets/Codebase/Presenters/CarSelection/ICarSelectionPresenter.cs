using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Presenter.Base;
using UniRx;

namespace Assets.Codebase.Presenters.CarSelection
{
    public interface ICarSelectionPresenter : IPresenter
    {
        public ReactiveProperty<string> TotalCoinsString { get; }
        public ReactiveProperty<PlayerCarId> DisplayedCar { get; }

        public void ConfirmSelectionButtonClicked();
        public void RightArrowClicked();
        public void LeftArrowClicked();
    }
}
