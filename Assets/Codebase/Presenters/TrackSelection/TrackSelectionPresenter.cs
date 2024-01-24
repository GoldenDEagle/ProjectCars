using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;

public class TrackSelectionPresenter : BasePresenter, ITrackSelectionPresenter
{
    public TrackSelectionPresenter()
    {
        CorrespondingViewId = ViewId.TrackSelection;
    }

    public void GoButtonClicked()
    {
        GameplayModel.LoadScene(SceneNames.CityTrack, () => GameplayModel.ActivateView(ViewId.Ingame));
    }
}
