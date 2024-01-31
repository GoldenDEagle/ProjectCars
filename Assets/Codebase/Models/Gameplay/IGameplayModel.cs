using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Gameplay.Racing;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Views.Base;
using System;
using System.Collections.Generic;
using UniRx;

namespace Assets.Codebase.Models.Gameplay
{
    /// <summary>
    /// Model responsible for game flow.
    /// </summary>
    public interface IGameplayModel : IModel
    {
        /// <summary>
        /// Is platform mobile
        /// </summary>
        public bool IsMobile { get; }
        /// <summary>
        /// Currently active view
        /// </summary>
        public ReactiveProperty<ViewId> ActiveViewId { get; }
        /// <summary>
        /// Called when target view is closed
        /// </summary>
        public Subject<ViewId> OnViewClosed { get; }
        /// <summary>
        /// Current race
        /// </summary>
        public ReactiveProperty<Race> ActiveRace { get; }
        /// <summary>
        /// Last race reward
        /// </summary>
        public ReactiveProperty<int> CurrentReward { get; }
        /// <summary>
        /// Use to switch between views (deactivates all others)
        /// </summary>
        /// <param name="viewId"></param>
        public void ActivateView(ViewId viewId);
        /// <summary>
        /// Loads scene async
        /// </summary>
        /// <param name="name"></param>
        /// <param name="onLoaded"></param>
        public void LoadScene(string name, Action onLoaded = null);

        public PlayerCarInfo GetPlayerCarInfo(PlayerCarId carId);
        public List<PlayerCarInfo> GetListOfAvailablePlayerCars();
        public EnemyCarInfo GetEnemyCarInfo(EnemyCarId carId);
        public TrackInfo GetTrackInfo(TrackId trackId);
        public List<TrackInfo> GetTracks();
        /// <summary>
        /// Creates new race and sets it as active race
        /// </summary>
        public void CreateNewRace(TrackId trackId);
        /// <summary>
        /// Calculates reward
        /// </summary>
        /// <returns></returns>
        public int CalculateReward();

        public ReactiveProperty<GameState> State { get; }

        public void ChangeGameState(GameState state);
    }
}
