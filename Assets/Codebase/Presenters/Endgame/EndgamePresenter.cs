using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using UniRx;

public class EndgamePresenter : BasePresenter, IEndgamePresenter
{
    public ReactiveProperty<string> PositionString { get; private set; }
    public ReactiveProperty<string> CoinRewardString { get; private set; }

    public EndgamePresenter()
    {
        CorrespondingViewId = ViewId.EndGame;
        PositionString = new ReactiveProperty<string>();
        CoinRewardString = new ReactiveProperty<string>();
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
        PositionString.Value = "Position: " + GameplayModel.ActiveRace.Value.Result.Position.ToString();
    }

    public void ContinueButtonClicked()
    {
        ProgressModel.ModifyCoinAmount(GameplayModel.CurrentReward.Value);
        GameplayModel.LoadScene(SceneNames.Menu, () => GameplayModel.ActivateView(ViewId.CarSelection));
    }
}
