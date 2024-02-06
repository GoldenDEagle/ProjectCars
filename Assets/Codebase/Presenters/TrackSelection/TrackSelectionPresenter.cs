using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TrackSelectionPresenter : BasePresenter, ITrackSelectionPresenter
{
    public ReactiveProperty<Sprite> SelectedTrackIcon { get; private set; }
    public ReactiveProperty<string> TotalCoinsString { get; private set; }

    private List<TrackInfo> _availableTracks;
    private int _selectedTrackIndex;

    public TrackSelectionPresenter()
    {
        CorrespondingViewId = ViewId.TrackSelection;
        SelectedTrackIcon = new ReactiveProperty<Sprite>();
        TotalCoinsString = new ReactiveProperty<string>();
    }

    protected override void SubscribeToModelChanges()
    {
        base.SubscribeToModelChanges();
        ProgressModel.SessionProgress.TotalCoins.Subscribe(value => TotalCoinsString.Value = value.ToString()).AddTo(CompositeDisposable);
    }

    public override void CreateView()
    {
        base.CreateView();

        _availableTracks = GameplayModel.GetTracks();
        _selectedTrackIndex = 1;
        SelectedTrackIcon.Value = _availableTracks[_selectedTrackIndex].TrackIcon;
    }

    public void GoButtonClicked()
    {
        GameplayModel.CreateNewRace(_availableTracks[_selectedTrackIndex].TrackId);
        GameplayModel.LoadScene(_availableTracks[_selectedTrackIndex].TrackSceneName, () => GameplayModel.ActivateView(ViewId.Ingame));
    }

    public void RightArrowClicked()
    {
        _selectedTrackIndex++;

        if (_selectedTrackIndex >= _availableTracks.Count)
        {
            _selectedTrackIndex = 0;
        }

        SelectedTrackIcon.Value = _availableTracks[_selectedTrackIndex].TrackIcon;
    }

    public void LeftArrowClicked()
    {
        _selectedTrackIndex--;

        if (_selectedTrackIndex < 0)
        {
            _selectedTrackIndex = _availableTracks.Count - 1;
        }

        SelectedTrackIcon.Value = _availableTracks[_selectedTrackIndex].TrackIcon;
    }

    public void SoundButtonClicked()
    {
        ProgressModel.SwitchSound();
    }

    public void BackButtonClicked()
    {
        GameplayModel.ActivateView(ViewId.CarSelection);
    }
}
