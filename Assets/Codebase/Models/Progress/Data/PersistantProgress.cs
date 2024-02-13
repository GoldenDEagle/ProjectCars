using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Utils.CustomTypes;
using System;
using System.Collections.Generic;

namespace Assets.Codebase.Models.Progress.Data
{
    /// <summary>
    /// Representation of ReactiveProgress that can be serialized and saved.
    /// </summary>
    [Serializable]
    public class PersistantProgress
    {
        // All the same properties as ReactiveProgress, but Serializable
        public float MusicVolume;
        public float SFXVolume;
        public PlayerCarId SelectedCar;
        public int TotalCoins;
        public List<PlayerCarId> UnlockedCars;
        public bool MobileTutorialCompleted;
        public bool PCTutorialCompleted;
        public SerializableDictionary<TrackId, float> BestResults;

        public void SetValues(SessionProgress progress)
        {
            // Fill all properties
            MusicVolume = progress.MusicVolume.Value;
            SFXVolume = progress.SFXVolume.Value;
            SelectedCar = progress.SelectedCar.Value;
            TotalCoins = progress.TotalCoins.Value;
            UnlockedCars = progress.UnlockedCars;
            MobileTutorialCompleted = progress.MobileTutorialCompleted.Value;
            PCTutorialCompleted = progress.PCTutorialCompleted.Value;
            BestResults = progress.BestResults;
        }
    }
}
