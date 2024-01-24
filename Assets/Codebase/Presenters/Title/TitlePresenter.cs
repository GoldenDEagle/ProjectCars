using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;

public class TitlePresenter : BasePresenter, ITitlePresenter
{
    public TitlePresenter()
    {
        CorrespondingViewId = ViewId.Title;
    }

    public void StartButtonClicked()
    {
        GameplayModel.LoadScene(SceneNames.Menu, () => GameplayModel.ActivateView(ViewId.CarSelection));
    }
}
