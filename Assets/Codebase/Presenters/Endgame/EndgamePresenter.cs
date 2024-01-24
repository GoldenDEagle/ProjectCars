using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Endgame;
using Assets.Codebase.Views.Base;

public class EndgamePresenter : BasePresenter, IEndgamePresenter
{
    public EndgamePresenter()
    {
        CorrespondingViewId = ViewId.EndGame;
    }
}
