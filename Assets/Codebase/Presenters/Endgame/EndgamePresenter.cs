using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
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
        GameplayModel.CurrentReward.Subscribe(value => CoinRewardString.Value = "Reward: " + value).AddTo(CompositeDisposable);
    }

    public override void CreateView()
    {
        base.CreateView();
        //CoinRewardString.Value = "Reward: " + GameplayModel.CurrentReward.Value;
        DoubleRewardButtonActiveState.Value = ServiceLocator.Container.Single<IAdsService>().CheckIfRewardedIsAvailable();
        PositionString.Value = "Position: " + GameplayModel.ActiveRace.Value.Result.Position.ToString();
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
}
