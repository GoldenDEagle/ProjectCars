using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Presenter.Base;
using UniRx;
using UnityEngine;

public interface ITrackSelectionPresenter : IPresenter
{
    public ReactiveProperty<Sprite> SelectedTrackIcon { get; }

    public void GoButtonClicked();
    public void RightArrowClicked();
    public void LeftArrowClicked();
}
