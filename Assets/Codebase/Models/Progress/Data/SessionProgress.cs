﻿using Assets.Codebase.Data.Cars.Player;
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

        public ReactiveProperty<int> SampleValue;
        public ReactiveProperty<float> MusicVolume;
        public ReactiveProperty<float> SFXVolume;
        public ReactiveProperty<PlayerCarId> SelectedCar;
        public ReactiveProperty<int> TotalCoins;
        public List<PlayerCarId> UnlockedCars;

        // .

        /// <summary>
        /// Creates new progress with default values.
        /// </summary>
        public SessionProgress()
        {
            SampleValue = new ReactiveProperty<int>(0);
            MusicVolume = new ReactiveProperty<float>(1f);
            SFXVolume = new ReactiveProperty<float>(1f);
            SelectedCar = new ReactiveProperty<PlayerCarId>(PlayerCarId.Haumea);
            TotalCoins = new ReactiveProperty<int>(200);
            UnlockedCars = new List<PlayerCarId> { PlayerCarId.Haumea };
        }

        /// <summary>
        /// Creates new progress from persistant data.
        /// </summary>
        /// <param name="progress"></param> Progress to initialize from
        public SessionProgress(PersistantProgress progress)
        {
            SampleValue = new ReactiveProperty<int>(progress.SampleValue);
            MusicVolume = new ReactiveProperty<float>();
            SFXVolume = new ReactiveProperty<float>();
            SelectedCar = new ReactiveProperty<PlayerCarId>(progress.SelectedCar);
            TotalCoins = new ReactiveProperty<int>(progress.TotalCoins);
            UnlockedCars = new List<PlayerCarId>(progress.UnlockedCars);
        }
    }
}
