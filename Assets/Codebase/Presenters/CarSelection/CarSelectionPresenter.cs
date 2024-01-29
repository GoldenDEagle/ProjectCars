using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.CarSelection;
using Assets.Codebase.Views.Base;

public class CarSelectionPresenter : BasePresenter, ICarSelectionPresenter
{
    public CarSelectionPresenter()
    {
        CorrespondingViewId = ViewId.CarSelection;
    }

    public void ConfirmSelectionButtonClicked()
    {
        GameplayModel.ActivateView(ViewId.TrackSelection);
    }

    public void RightArrowClicked()
    {

    }

    public void LeftArrowClicked()
    {

    }
}
