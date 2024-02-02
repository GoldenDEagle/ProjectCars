using Assets.Codebase.Presenter.Base;
using UniRx;

public interface IIngamePresenter : IPresenter
{
    public ReactiveProperty<string> PositionString { get; }
    public ReactiveProperty<string> LapString { get; }
}
