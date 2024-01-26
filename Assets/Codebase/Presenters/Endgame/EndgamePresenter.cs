using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using UniRx;

public class EndgamePresenter : BasePresenter, IEndgamePresenter
{
    public ReactiveProperty<string> PositionString { get; private set; }

    public EndgamePresenter()
    {
        CorrespondingViewId = ViewId.EndGame;
        PositionString = new ReactiveProperty<string>();
    }

    public override void CreateView()
    {
        base.CreateView();
        PositionString.Value = "Position: " + GameplayModel.ActiveRace.Value.Result.Position.ToString();
    }

    public void ContinueButtonClicked()
    {
        GameplayModel.LoadScene(SceneNames.Menu, () => GameplayModel.ActivateView(ViewId.CarSelection));
    }
}
