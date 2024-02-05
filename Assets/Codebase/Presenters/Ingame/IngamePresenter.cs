using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;
using Assets.SimpleLocalization.Scripts;
using System;
using UniRx;
using UnityEditor.Localization.Editor;

public class IngamePresenter : BasePresenter, IIngamePresenter
{
    public ReactiveProperty<string> PositionString { get; private set; }
    public ReactiveProperty<string> LapString { get; private set; }

    private const string ValueSeparator = " / ";

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
        CalculatePosition(GameplayModel.CurrentPosition.Value);
        CalculateLaps(GameplayModel.CurrentLap.Value);
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        GameplayModel.CurrentPosition.Subscribe(value => CalculatePosition(value)).AddTo(CompositeDisposable);
        GameplayModel.CurrentLap.Subscribe(value => CalculateLaps(value)).AddTo(CompositeDisposable);
    }

    private void CalculatePosition(int currentPosition)
    {
        PositionString.Value = currentPosition + ValueSeparator + (_numberOfEnemies + 1);
    }

    private void CalculateLaps(int currentLap)
    {
        LapString.Value = currentLap + ValueSeparator + (_numberOfLaps);
    }
}
