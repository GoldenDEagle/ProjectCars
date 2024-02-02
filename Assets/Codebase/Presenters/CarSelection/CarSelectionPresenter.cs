using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;
using System.Collections.Generic;
using UniRx;

public class CarSelectionPresenter : BasePresenter, ICarSelectionPresenter
{
    public ReactiveProperty<bool> ConfirmSelectionButtonActiveState { get; private set; }
    public ReactiveProperty<bool> BuyButtonActiveState { get; private set; }
    public ReactiveProperty<string> BuyButtonString { get; private set; }
    public ReactiveProperty<PlayerCarId> DisplayedCar { get; private set; }
    public ReactiveProperty<string> TotalCoinsString { get; private set; }

    private List<PlayerCarInfo> _availableCars;
    private int _selectedCarIndex;

    public CarSelectionPresenter()
    {
        CorrespondingViewId = ViewId.CarSelection;
        DisplayedCar = new ReactiveProperty<PlayerCarId>();
        TotalCoinsString = new ReactiveProperty<string>();
        BuyButtonString = new ReactiveProperty<string>();
        BuyButtonActiveState = new ReactiveProperty<bool>();
        ConfirmSelectionButtonActiveState = new ReactiveProperty<bool>();
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        ProgressModel.SessionProgress.TotalCoins.Subscribe(value => TotalCoinsString.Value = value.ToString()).AddTo(CompositeDisposable);
    }

    public override void CreateView()
    {
        base.CreateView();

        //TotalCoinsString.Value = "Coins: " + ProgressModel.SessionProgress.TotalCoins.Value;
        _availableCars = GameplayModel.GetListOfAvailablePlayerCars();
        DisplayedCar.Value = ProgressModel.SessionProgress.SelectedCar.Value;
        _selectedCarIndex = _availableCars.FindIndex(x => x.CarId == DisplayedCar.Value);
        UpdateButtonStates();
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
        UpdateButtonStates();
    }

    public void LeftArrowClicked()
    {
        _selectedCarIndex--;

        if (_selectedCarIndex < 0)
        {
            _selectedCarIndex = _availableCars.Count - 1;
        }

        DisplayedCar.Value = _availableCars[_selectedCarIndex].CarId;
        UpdateButtonStates();
    }

    public void BuyButtonClicked()
    {
        var price = _availableCars[_selectedCarIndex].Price;
        if (price > ProgressModel.SessionProgress.TotalCoins.Value)
        {
            return;
        }

        ProgressModel.ModifyCoinAmount(-price);
        ProgressModel.UnlockNewCar(_availableCars[_selectedCarIndex].CarId);
        ProgressModel.SaveProgress();
        UpdateButtonStates();
    }

    private void UpdateButtonStates()
    {
        if (ProgressModel.SessionProgress.UnlockedCars.Contains(DisplayedCar.Value))
        {
            BuyButtonActiveState.Value = false;
            ConfirmSelectionButtonActiveState.Value = true;
            return;
        }

        var price = _availableCars[_selectedCarIndex].Price;
        BuyButtonString.Value = price.ToString();
        BuyButtonActiveState.Value = true;
        ConfirmSelectionButtonActiveState.Value = false;
    }
}
