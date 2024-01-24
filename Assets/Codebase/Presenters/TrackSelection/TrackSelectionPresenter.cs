using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;

public class TrackSelectionPresenter : BasePresenter, ITrackSelectionPresenter
{
    public TrackSelectionPresenter()
    {
        CorrespondingViewId = ViewId.TrackSelection;
    }
}
