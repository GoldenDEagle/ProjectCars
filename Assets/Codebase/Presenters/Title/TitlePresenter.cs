using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;

public class TitlePresenter : BasePresenter, ITitlePresenter
{
    public TitlePresenter()
    {
        CorrespondingViewId = ViewId.Title;
    }
}
