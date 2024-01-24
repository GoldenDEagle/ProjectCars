using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;

public class IngamePresenter : BasePresenter, IIngamePresenter
{
    public IngamePresenter()
    {
        CorrespondingViewId = ViewId.Ingame;
    }
}
