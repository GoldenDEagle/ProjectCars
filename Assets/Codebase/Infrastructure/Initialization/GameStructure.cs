﻿using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Infrastructure.ServicesManagment.Audio;
using Assets.Codebase.Infrastructure.ServicesManagment.CarCreation;
using Assets.Codebase.Infrastructure.ServicesManagment.Leaderboard;
using Assets.Codebase.Infrastructure.ServicesManagment.Localization;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Assets.Codebase.Infrastructure.ServicesManagment.PresenterManagement;
using Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation;
using Assets.Codebase.Models.Gameplay;
using Assets.Codebase.Models.Progress;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Presenters.Example;
using Assets.Codebase.Presenters.Pause;
using GamePush;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Codebase.Infrastructure.Initialization
{
    /// <summary>
    /// Responsible for creation of game structure.
    /// </summary>
    public class GameStructure
    {
        public static bool IsGameInitialized = false;
        public static GameLaunchParams GameLaunchParameters { get; private set; }

        // Needed from outside
        private RectTransform _uiRoot;
        private AudioSource _effectsSource;
        private AudioSource _musicSource;

        // Created inside
        private IProgressModel _progressModel;
        private IGameplayModel _gameplayModel;
        private List<BasePresenter> _presenters;
        private bool _isEditor = false;

        public GameStructure(RectTransform uiRoot, AudioSource effectsSource, AudioSource musicSource, GameLaunchParams launchParams = null)
        {
            if (IsGameInitialized) { return; }
            IsGameInitialized = true;

#if UNITY_EDITOR
            _isEditor = true;
#endif

            _uiRoot = uiRoot;
            _effectsSource = effectsSource;
            _musicSource = musicSource;

            GameLaunchParameters = launchParams ?? new GameLaunchParams();

            ApplyPreloadParams();

            CreateMVPStructure();
            RegisterServices();
            // Fills data using asset provider
            _gameplayModel.InitModel();

            ApplyAfterLoadParams();
            _musicSource = musicSource;
        }

        // MVP structure
        private void CreateMVPStructure()
        {
            CreateModels();
            CreatePresenters();
        }

        private void CreateModels()
        {
            if (_isEditor)
            {
                _progressModel = new LocalProgressModel();
            }
            else
            {
                _progressModel = new ServerProgressModel();
            }

            _gameplayModel = new GameplayModel();
        }

        private void CreatePresenters()
        {
            // Create presenter for each view
            _presenters = new List<BasePresenter>
            {
                new TitlePresenter(),
                new CarSelectionPresenter(),
                new TrackSelectionPresenter(),
                new IngamePresenter(),
                new EndgamePresenter(),
                new PausePresenter(),
            };

            foreach (var presenter in _presenters)
            {
                presenter.SetupModels(_progressModel, _gameplayModel);
            }
        }


        /// <summary>
        /// Registering all game services.
        /// </summary>
        private void RegisterServices()
        {
            var services = ServiceLocator.Container;

            services.RegisterSingle<IAssetProvider>(new AssetProvider());
            services.RegisterSingle<IViewProvider>(new ViewProvider(services.Single<IAssetProvider>(), _presenters, _uiRoot));
            services.RegisterSingle<IAudioService>(new AudioService(services.Single<IAssetProvider>(), _progressModel, _effectsSource, _musicSource));
            services.RegisterSingle<IAdsService>(new GamePushAdService(services.Single<IAudioService>()));
            services.RegisterSingle<IModelAccessService>(new ModelAccessService(_progressModel, _gameplayModel));
            services.RegisterSingle<ILocalizationService>(new GoogleSheetLocalizationService());
            services.RegisterSingle<IPresentersService>(new PresentersService(_presenters));
            services.RegisterSingle<ICarFactory>(new CarFactory(services.Single<IAssetProvider>(), services.Single<IModelAccessService>()));
            services.RegisterSingle<ILeaderboardService>(new GPLeaderboardService());
        }


        // Launch params handling.
        /// <summary>
        /// Before model and service initialization.
        /// </summary>
        private void ApplyPreloadParams()
        {
            if (GameLaunchParameters.ManualParamSet)
            {
                if (GameLaunchParameters.ClearPlayerPrefs)
                {
                    PlayerPrefs.DeleteAll();
                }
            }
        }

        /// <summary>
        /// After model and service initialization.
        /// </summary>
        private void ApplyAfterLoadParams()
        {
            var services = ServiceLocator.Container;

            if (GameLaunchParameters.ManualParamSet)
            {
                services.Single<ILocalizationService>().SetLanguage(GameLaunchParameters.Language);
            }
            else
            {
                services.Single<ILocalizationService>().SetLanguage(GP_Language.Current());
            }
        }
    }
}
