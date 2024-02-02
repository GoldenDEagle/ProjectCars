using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;
using UniRx;

public class IngamePresenter : BasePresenter, IIngamePresenter
{
    public ReactiveProperty<string> PositionString { get; private set; }

    private int _numberOfEnemies = 0;

    public IngamePresenter()
    {
        CorrespondingViewId = ViewId.Ingame;
        PositionString = new ReactiveProperty<string>();
    }

    public override void CreateView()
    {
        base.CreateView();
        _numberOfEnemies = GameplayModel.ActiveRace.Value.EnemiesList.Count;
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        GameplayModel.CurrentPosition.Subscribe(value => CalculatePosition(value)).AddTo(CompositeDisposable);
    }

    private void CalculatePosition(int currentPosition)
    {
        PositionString.Value = currentPosition + " / " + (_numberOfEnemies + 1);
    }
}
