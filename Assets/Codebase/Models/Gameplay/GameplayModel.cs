using Assets.Codebase.Infrastructure.Initialization;
using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Views.Base;
using System;
using UniRx;
using Assets.Codebase.Data.Cars.Player;
using Assets.Codebase.Data.Cars.Enemy;
using System.Linq;
using Assets.Codebase.Gameplay.Racing;
using System.Collections.Generic;
using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Utils.Extensions;
using Unity.VisualScripting;
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
        private Subject<ViewId> _onViewClosed;
        private SceneLoader _sceneLoader;
        private PlayerCarDescriptions _playerCarsDescription;
        private EnemyCarDescriptions _enemyCarsDescriptions;
        private TrackDescriptions _trackDescriptions;
        private List<EnemyCarId> _availableEnemyIds;
        private bool _isMobile;

        // Public properties
        public ReactiveProperty<GameState> State => _state;
        public ReactiveProperty<ViewId> ActiveViewId => _activeViewId;
        public Subject<ViewId> OnViewClosed => _onViewClosed;
        public ReactiveProperty<Race> ActiveRace => _activeRace;
        public bool IsMobile => _isMobile;

        public GameplayModel()
        {
            _sceneLoader = new SceneLoader();
            _state = new ReactiveProperty<GameState>(GameState.None);
            _activeViewId = new ReactiveProperty<ViewId>(ViewId.None);
            _activeRace = new ReactiveProperty<Race>();
            _onViewClosed = new Subject<ViewId>();
        }

        public void InitModel()
        {
            var assetProvider = ServiceLocator.Container.Single<IAssetProvider>();
            _enemyCarsDescriptions = assetProvider.LoadResource<EnemyCarDescriptions>(EnemyCarsDescriptionPath);
            _playerCarsDescription = assetProvider.LoadResource<PlayerCarDescriptions>(PlayerCarsDescriptionPath);
            _trackDescriptions = assetProvider.LoadResource<TrackDescriptions>(TrackDescriptionPath);
            _isMobile = ServiceLocator.Container.Single<IAdsService>().IsDeviceMobile();
        }

        public void ActivateView(ViewId viewId)
        {
            if (ActiveViewId.Value == viewId) { return; }

            _onViewClosed.OnNext(ActiveViewId.Value);

            ActiveViewId.Value = viewId;
        }

        public void ChangeGameState(GameState state)
        {
            State.Value = state;
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

            _activeRace.Value = new Race(trackId, 1, _availableEnemyIds.Take(5).ToList());
        }

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
