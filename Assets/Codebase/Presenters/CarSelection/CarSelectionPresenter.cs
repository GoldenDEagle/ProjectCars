using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;
using System.Collections.Generic;
using UniRx;

public class CarSelectionPresenter : BasePresenter, ICarSelectionPresenter
{
    public ReactiveProperty<PlayerCarId> DisplayedCar { get; private set; }
    public ReactiveProperty<string> TotalCoinsString { get; private set; }

    private List<PlayerCarInfo> _availableCars;
    private int _selectedCarIndex;

    public CarSelectionPresenter()
    {
        CorrespondingViewId = ViewId.CarSelection;
        DisplayedCar = new ReactiveProperty<PlayerCarId>();
        TotalCoinsString = new ReactiveProperty<string>();
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        ProgressModel.SessionProgress.TotalCoins.Subscribe(value => TotalCoinsString.Value = "Coins: " + value).AddTo(CompositeDisposable);
    }

    public override void CreateView()
    {
        base.CreateView();

        //TotalCoinsString.Value = "Coins: " + ProgressModel.SessionProgress.TotalCoins.Value;
        _availableCars = GameplayModel.GetListOfAvailablePlayerCars();
        DisplayedCar.Value = ProgressModel.SessionProgress.SelectedCar.Value;
        _selectedCarIndex = _availableCars.FindIndex(x => x.CarId == DisplayedCar.Value);
    }

    public void ConfirmSelectionButtonClicked()
    {
        ProgressModel.SessionProgress.SelectedCar.Value = _availableCars[_selectedCarIndex].CarId;
        GameplayModel.ActivateView(ViewId.TrackSelection);
    }

    public void RightArrowClicked()
    {
        _selectedCarIndex++;

        if (_selectedCarIndex >= _availableCars.Count)
        {
            _selectedCarIndex = 0;
        }

        DisplayedCar.Value = _availableCars[_selectedCarIndex].CarId;
    }

    public void LeftArrowClicked()
    {
        _selectedCarIndex--;

        if (_selectedCarIndex < 0)
        {
            _selectedCarIndex = _availableCars.Count - 1;
        }

        DisplayedCar.Value = _availableCars[_selectedCarIndex].CarId;
    }
}
