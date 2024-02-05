using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Infrastructure.ServicesManagment.Localization;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using System;
using UniRx;

public class EndgamePresenter : BasePresenter, IEndgamePresenter
{
    public ReactiveProperty<bool> DoubleRewardButtonActiveState { get; private set; }
    public ReactiveProperty<string> PositionString { get; private set; }
    public ReactiveProperty<string> CoinRewardString { get; private set; }

    private const string FirstPlaceKey = "placement_first";
    private const string SecondPlaceKey = "placement_second";
    private const string ThirdPlaceKey = "placement_third";
    private const string FourthPlaceKey = "placement_fourth";
    private const string FifthPlaceKey = "placement_fifth";
    private const string SixthPlaceKey = "placement_sixth";
    private const string DefaultPlacementKey = "placement_default";

    private IDisposable _rewardedSubscription;

    public EndgamePresenter()
    {
        CorrespondingViewId = ViewId.EndGame;
        PositionString = new ReactiveProperty<string>();
        CoinRewardString = new ReactiveProperty<string>();
        DoubleRewardButtonActiveState = new ReactiveProperty<bool>();
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        GameplayModel.CurrentReward.Subscribe(value => CoinRewardString.Value = value.ToString()).AddTo(CompositeDisposable);
    }

    public override void CreateView()
    {
        base.CreateView();
        //CoinRewardString.Value = "Reward: " + GameplayModel.CurrentReward.Value;
        DoubleRewardButtonActiveState.Value = ServiceLocator.Container.Single<IAdsService>().CheckIfRewardedIsAvailable();
        PositionString.Value = CreatePlacementString(GameplayModel.ActiveRace.Value.Result.Position);
    }

    public void ContinueButtonClicked()
    {
        ProgressModel.ModifyCoinAmount(GameplayModel.CurrentReward.Value);
        ProgressModel.SaveProgress();
        GameplayModel.LoadScene(SceneNames.Menu, () => GameplayModel.ActivateView(ViewId.CarSelection));
    }

    public void DoubleRewardButtonClicked()
    {
        var adService = ServiceLocator.Container.Single<IAdsService>();

        if (!adService.CheckIfRewardedIsAvailable()) return;

        DoubleRewardButtonActiveState.Value = false;
        _rewardedSubscription = adService.OnRewardedSuccess.Subscribe(_ => OnRewardGranted()).AddTo(CompositeDisposable);
        adService.ShowRewarded();
    }

    private void OnRewardGranted()
    {
        CompositeDisposable.Remove(_rewardedSubscription);
        GameplayModel.IncreaseReward();
    }

    private string CreatePlacementString(int position)
    {
        var localizationService = ServiceLocator.Container.Single<ILocalizationService>();
        string placementString = string.Empty;

        switch (position)
        {
            case 1:
                placementString = localizationService.LocalizeTextByKey(FirstPlaceKey);
                break;
            case 2:
                placementString = localizationService.LocalizeTextByKey(SecondPlaceKey);
                break;
            case 3:
                placementString = localizationService.LocalizeTextByKey(ThirdPlaceKey);
                break;
            case 4:
                placementString = localizationService.LocalizeTextByKey(FourthPlaceKey);
                break;
            case 5:
                placementString = localizationService.LocalizeTextByKey(FifthPlaceKey);
                break;
            case 6:
                placementString = localizationService.LocalizeTextByKey(SixthPlaceKey);
                break;
            default:
                placementString = localizationService.LocalizeTextByKey(DefaultPlacementKey);
                break;
        }

        return placementString;
    }
}
