using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Leaderboard;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Progress.Data;
using Assets.Codebase.Utils.Extensions;
using UnityEngine;

namespace Assets.Codebase.Models.Progress
{
    /// <summary>
    /// Model for local saving case (PlayerPrefs).
    /// </summary>
    public class LocalProgressModel : BaseModel, IProgressModel
    {
        private const string ProgressKey = "Progress";

        public SessionProgress SessionProgress { get; private set; }

        private PersistantProgress _persistantProgress;

        public LocalProgressModel()
        {
            LoadProgress();
        }

        public void InitModel()
        {
        }

        protected bool CanFindSave()
        {
            return PlayerPrefs.HasKey(ProgressKey);
        }

        protected void CreateNewProgress()
        {
            SessionProgress = new SessionProgress();
        }

        public void SaveProgress()
        {
            // New PersistantProgress is created on first save
            if (_persistantProgress == null)
            {
                _persistantProgress = new PersistantProgress();
            }

            _persistantProgress.SetValues(SessionProgress);
            PlayerPrefs.SetString(ProgressKey, _persistantProgress.ToJson());
        }

        public void LoadProgress()
        {
            if (CanFindSave())
            {
                GetProgressFromPrefs();
            }
            else
            {
                CreateNewProgress();
            }
        }

        private void GetProgressFromPrefs()
        {
            var progress = PlayerPrefs.GetString(ProgressKey).ToDeserealized<PersistantProgress>();
            SessionProgress = new SessionProgress(progress);
        }



        //////// Working with progress properties //////////
        public void ModifyCoinAmount(int delta)
        {
            var newAmount = SessionProgress.TotalCoins.Value + delta;
            
            if (newAmount < 0)
            {
                Debug.Log("New coin amount is negative!");
                return;
            }

            SessionProgress.TotalCoins.Value = newAmount;
        }

        public void UnlockNewCar(PlayerCarId carId)
        {
            if (SessionProgress.UnlockedCars.Contains(carId)) return;

            SessionProgress.UnlockedCars.Add(carId);
        }

        public void SwitchSound()
        {
            if (SessionProgress.SFXVolume.Value > 0.5f)
            {
                SessionProgress.SFXVolume.Value = 0f;
            }
            else
            {
                SessionProgress.SFXVolume.Value = 1f;
            }
        }

        public void SyncBestResults(TrackId trackId, float time)
        {
            // Result isn't better
            if (time >= SessionProgress.BestResults[trackId]) return;

            SessionProgress.BestResults[trackId] = time;
            ServiceLocator.Container.Single<ILeaderboardService>().PushNewResultToLeaderBoard(trackId, time);
            SaveProgress();
        }
    }
}
