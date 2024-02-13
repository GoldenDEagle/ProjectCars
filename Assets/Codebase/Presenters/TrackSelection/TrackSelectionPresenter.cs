using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Infrastructure.ServicesManagment.Leaderboard;
using Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class TrackSelectionPresenter : BasePresenter, ITrackSelectionPresenter
{
    public ReactiveProperty<Sprite> SelectedTrackIcon { get; private set; }
    public ReactiveProperty<string> TotalCoinsString { get; private set; }

    private List<TrackInfo> _availableTracks;
    private int _selectedTrackIndex;
    private IDisposable _fullscreenSubscription;

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
        _selectedTrackIndex = 0;
        SelectedTrackIcon.Value = _availableTracks[_selectedTrackIndex].TrackIcon;
    }

    public void GoButtonClicked()
    {
        GameplayModel.CreateNewRace(_availableTracks[_selectedTrackIndex].TrackId);

        var adService = ServiceLocator.Container.Single<IAdsService>();

        if (adService.CheckIfFullscreenIsAvailable())
        {
            _fullscreenSubscription = adService.OnFullscreenClosed.Subscribe(_ => AfterFullscreenWatched()).AddTo(CompositeDisposable);
            adService.ShowFullscreen();
        }
        else
        {
            GameplayModel.LoadScene(_availableTracks[_selectedTrackIndex].TrackSceneName, () => GameplayModel.ActivateView(ViewId.Ingame));
        }
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

    public void FreeCoinsButtonClicked()
    {
        ServiceLocator.Container.Single<IViewProvider>().CreateAdPopupWindow();
    }

    public void BackButtonClicked()
    {
        GameplayModel.ActivateView(ViewId.CarSelection);
    }

    public void LeaderBoardButtonClicked()
    {
        ServiceLocator.Container.Single<ILeaderboardService>().OpenLeaderboard(_availableTracks[_selectedTrackIndex].TrackId);
    }


    private void AfterFullscreenWatched()
    {
        CompositeDisposable.Remove(_fullscreenSubscription);
        GameplayModel.LoadScene(_availableTracks[_selectedTrackIndex].TrackSceneName, () => GameplayModel.ActivateView(ViewId.Ingame));
    }
}
