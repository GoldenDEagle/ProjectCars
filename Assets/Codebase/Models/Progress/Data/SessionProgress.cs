using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Utils.CustomTypes;
using System.Collections.Generic;
using UniRx;

namespace Assets.Codebase.Models.Progress.Data
{
    /// <summary>
    /// Used at runtime.
    /// </summary>
    public class SessionProgress
    {
        // All the properties that need to be saved...

        public ReactiveProperty<float> MusicVolume;
        public ReactiveProperty<float> SFXVolume;
        public ReactiveProperty<PlayerCarId> SelectedCar;
        public ReactiveProperty<int> TotalCoins;
        public ReactiveProperty<bool> MobileTutorialCompleted;
        public ReactiveProperty<bool> PCTutorialCompleted;
        public List<PlayerCarId> UnlockedCars;
        /// <summary>
        /// Best results for each track, time in ms
        /// </summary>
        public SerializableDictionary<TrackId, float> BestResults;

        // .

        /// <summary>
        /// Creates new progress with default values.
        /// </summary>
        public SessionProgress()
        {
            MusicVolume = new ReactiveProperty<float>(1f);
            SFXVolume = new ReactiveProperty<float>(1f);
            SelectedCar = new ReactiveProperty<PlayerCarId>(PlayerCarId.Haumea);
            TotalCoins = new ReactiveProperty<int>(0);
            UnlockedCars = new List<PlayerCarId> { PlayerCarId.Haumea };
            MobileTutorialCompleted = new ReactiveProperty<bool>(false);
            PCTutorialCompleted = new ReactiveProperty<bool>(false);
            BestResults = new SerializableDictionary<TrackId, float>
            {
                { TrackId.City, 99999999f },
                { TrackId.Mountains, 99999999f },
                { TrackId.RedDesert, 99999999f }
            };

        }

        /// <summary>
        /// Creates new progress from persistant data.
        /// </summary>
        /// <param name="progress"></param> Progress to initialize from
        public SessionProgress(PersistantProgress progress)
        {
            MusicVolume = new ReactiveProperty<float>(progress.MusicVolume);
            SFXVolume = new ReactiveProperty<float>(progress.SFXVolume);
            SelectedCar = new ReactiveProperty<PlayerCarId>(progress.SelectedCar);
            TotalCoins = new ReactiveProperty<int>(progress.TotalCoins);
            UnlockedCars = new List<PlayerCarId>(progress.UnlockedCars);
            MobileTutorialCompleted = new ReactiveProperty<bool>(progress.MobileTutorialCompleted);
            PCTutorialCompleted = new ReactiveProperty<bool>(progress.PCTutorialCompleted);

            BestResults = new SerializableDictionary<TrackId, float>();
            foreach (var key in progress.BestResults.Keys)
            {
                BestResults.Add(key, progress.BestResults[key]);
            }
        }
    }
}
