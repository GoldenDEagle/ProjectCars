using Assets.Codebase.Data.Cars.Enemy;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Gameplay.Racing;
using Assets.Codebase.Infrastructure.Initialization;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Utils.Extensions;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Codebase.Models.Gameplay
{
    public class GameplayModel : BaseModel, IGameplayModel
    {
        private const string EnemyCarsDescriptionPath = "Cars/Descriptions/EnemyCarsDescription";
        private const string PlayerCarsDescriptionPath = "Cars/Descriptions/PlayerCarsDescription";
        private const string TrackDescriptionPath = "Tracks/TrackDescriptions";

        // Internal
        private ReactiveProperty<GameState> _state;
        private ReactiveProperty<ViewId> _activeViewId;
        private ReactiveProperty<Race> _activeRace;
        private ReactiveProperty<int> _currentPosition;
        private ReactiveProperty<int> _currentLap;
        private Subject<ViewId> _onViewClosed;
        private Subject<GameState> _onGameStateChanged; 
        private ReactiveProperty<int> _currentReward;
        private SceneLoader _sceneLoader;
        private PlayerCarDescriptions _playerCarsDescription;
        private EnemyCarDescriptions _enemyCarsDescriptions;
        private TrackDescriptions _trackDescriptions;
        private List<EnemyCarId> _availableEnemyIds;
        private bool _isMobile;

        // Game config
        int _lapsInRace = 2;
        int _raceEnemyCount = 3;

        // Public properties
        public ReactiveProperty<GameState> State => _state;
        public ReactiveProperty<ViewId> ActiveViewId => _activeViewId;
        public Subject<ViewId> OnViewClosed => _onViewClosed;
        public Subject<GameState> OnGameStateChanged => _onGameStateChanged;
        public ReactiveProperty<Race> ActiveRace => _activeRace;
        public ReactiveProperty<int> CurrentReward => _currentReward;
        public ReactiveProperty<int> CurrentPosition => _currentPosition;
        public ReactiveProperty<int> CurrentLap => _currentLap;
        public bool IsMobile => _isMobile;

        public GameplayModel()
        {
            _sceneLoader = new SceneLoader();
            _state = new ReactiveProperty<GameState>(GameState.Bootstrap);
            _activeViewId = new ReactiveProperty<ViewId>(ViewId.None);
            _activeRace = new ReactiveProperty<Race>();
            _onViewClosed = new Subject<ViewId>();
            _onGameStateChanged = new Subject<GameState>();
            _currentReward = new ReactiveProperty<int>(0);
            _currentPosition = new ReactiveProperty<int>(1);
            _currentLap = new ReactiveProperty<int>(1);
        }

        public void InitModel()
        {
            var assetProvider = ServiceLocator.Container.Single<IAssetProvider>();
            _enemyCarsDescriptions = assetProvider.LoadResource<EnemyCarDescriptions>(EnemyCarsDescriptionPath);
            _playerCarsDescription = assetProvider.LoadResource<PlayerCarDescriptions>(PlayerCarsDescriptionPath);
            _trackDescriptions = assetProvider.LoadResource<TrackDescriptions>(TrackDescriptionPath);

            var adService = ServiceLocator.Container.Single<IAdsService>();
            _isMobile = adService.IsDeviceMobile();
            adService.OnAdStarted.Subscribe(_ => PauseGame()).AddTo(CompositeDisposable);
            adService.OnAdEnded.Subscribe(_ => UnPauseGame()).AddTo(CompositeDisposable);
        }

        public void ActivateView(ViewId viewId)
        {
            if (ActiveViewId.Value == viewId) { return; }

            _onViewClosed.OnNext(ActiveViewId.Value);

            ActiveViewId.Value = viewId;
        }

        public void ChangeGameState(GameState state)
        {
            if (State.Value == state) return;

            State.Value = state;
            _onGameStateChanged?.OnNext(state);
        }

        public void LoadScene(string name, Action onLoaded = null)
        {
            _sceneLoader.Load(name, onLoaded);
        }


        public PlayerCarInfo GetPlayerCarInfo(PlayerCarId carId)
        {
            return _playerCarsDescription.CarsList.FirstOrDefault(x => x.CarId == carId);
        }
        public List<PlayerCarInfo> GetListOfAvailablePlayerCars()
        {
            return _playerCarsDescription.CarsList;
        }
        public EnemyCarInfo GetEnemyCarInfo(EnemyCarId carId)
        {
            return _enemyCarsDescriptions.CarsList.FirstOrDefault(x => x.CarId == carId);
        }
        public TrackInfo GetTrackInfo(TrackId trackId)
        {
            return _trackDescriptions.Tracks.FirstOrDefault(x => x.TrackId == trackId);
        }
        public List<TrackInfo> GetTracks()
        {
            return _trackDescriptions.Tracks;
        }

        public void CreateNewRace(TrackId trackId)
        {
            if (_activeRace.Value != null)
            {
                _activeRace.Value = null;
            }

            if (_availableEnemyIds == null)
            {
                CollectAllEnemyIds();
            }

            _availableEnemyIds.Shuffle();

            _currentLap.Value = 1;
            //_activeRace.Value = new Race(trackId, _lapsInRace, _availableEnemyIds.Take(_raceEnemyCount).ToList());
            _activeRace.Value = new Race(trackId, _lapsInRace, new List<EnemyCarId> { EnemyCarId.First });
        }

        public int CalculateReward()
        {
            var race = _activeRace.Value;

            if (race == null) { return 0; }

            int positionReward = (race.EnemiesList.Count - race.Result.Position + 2) * Calculations.RewardPerPosition;
            //int lapCountReward = (int)(positionReward * 0.5f * (race.TotalLaps - 1));
            int totalReward = positionReward;
            _currentReward.Value = totalReward;

            return totalReward;
        }

        public void IncreaseReward()
        {
            _currentReward.Value *= 2;
        }

        public void PauseGame()
        {
            if (State.Value == GameState.Pause) return;

            State.Value = GameState.Pause;
            AudioListener.pause = true;
            Time.timeScale = 0;
        }

        public void UnPauseGame(GameState newGameState = GameState.Menu)
        {
            State.Value = newGameState;
            AudioListener.pause = false;
            Time.timeScale = 1;
        }

        /////////////////// Internal /////////////////
        private void CollectAllEnemyIds()
        {
            _availableEnemyIds = new List<EnemyCarId>();
            foreach (var enemy in _enemyCarsDescriptions.CarsList)
            {
                _availableEnemyIds.Add(enemy.CarId);
            }
        }
    }
}
