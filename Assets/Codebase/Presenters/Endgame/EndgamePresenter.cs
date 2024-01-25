using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;

public class EndgamePresenter : BasePresenter, IEndgamePresenter
{
    public EndgamePresenter()
    {
        CorrespondingViewId = ViewId.EndGame;
    }

    public void ContinueButtonClicked()
    {
        GameplayModel.LoadScene(SceneNames.Menu, () => GameplayModel.ActivateView(ViewId.CarSelection));
    }
}
