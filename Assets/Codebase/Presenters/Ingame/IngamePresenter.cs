using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;
using System;
using UniRx;

public class IngamePresenter : BasePresenter, IIngamePresenter
{
    public ReactiveProperty<string> PositionString { get; private set; }
    public ReactiveProperty<string> LapString { get; private set; }

    private int _numberOfEnemies = 0;
    private int _numberOfLaps = 1;

    public IngamePresenter()
    {
        CorrespondingViewId = ViewId.Ingame;
        PositionString = new ReactiveProperty<string>();
        LapString = new ReactiveProperty<string>();
    }

    public override void CreateView()
    {
        base.CreateView();
        _numberOfEnemies = GameplayModel.ActiveRace.Value.EnemiesList.Count;
        _numberOfLaps = GameplayModel.ActiveRace.Value.TotalLaps;
        CalculatePosition(1);
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        GameplayModel.CurrentPosition.Subscribe(value => CalculatePosition(value)).AddTo(CompositeDisposable);
        GameplayModel.CurrentLap.Subscribe(value => CalculateLaps(value)).AddTo(CompositeDisposable);
    }

    private void CalculatePosition(int currentPosition)
    {
        PositionString.Value = currentPosition + " / " + (_numberOfEnemies + 1);
    }

    private void CalculateLaps(int currentLap)
    {
        LapString.Value = currentLap + " / " + (_numberOfLaps);
    }
}
