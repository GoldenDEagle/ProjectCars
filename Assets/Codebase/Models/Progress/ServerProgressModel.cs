﻿using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Progress.Data;
using Assets.Codebase.Utils.Extensions;
using GamePush;
using UnityEngine;

namespace Assets.Codebase.Models.Progress
{
    public class ServerProgressModel: BaseModel, IProgressModel
    {
        private const string ProgressKey = "Progress";
        public SessionProgress SessionProgress { get; private set; }

        private PersistantProgress _persistantProgress;


        public ServerProgressModel()
        {
            LoadProgress();
        }

        public void InitModel()
        {
        }

        protected bool CanFindSave()
        {
            GP_Player.Sync();
            if (GP_Player.Has(ProgressKey))
            {
                if (GP_Player.GetString(ProgressKey) == string.Empty)
                {
                    return false;
                }
                return true;
            }

            return false;
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
            GP_Player.Set(ProgressKey, _persistantProgress.ToJson());
            GP_Player.Sync(true);
        }

        public void LoadProgress()
        {
            if (CanFindSave())
            {
                GetProgressFromServer();
            }
            else
            {
                CreateNewProgress();
            }
        }

        private void GetProgressFromServer()
        {
            var progress = GP_Player.GetString(ProgressKey).ToDeserealized<PersistantProgress>();
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
    }
}
