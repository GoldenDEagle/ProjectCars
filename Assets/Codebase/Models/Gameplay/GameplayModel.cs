﻿using Assets.Codebase.Infrastructure.Initialization;
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

namespace Assets.Codebase.Models.Gameplay
{
    public class GameplayModel : BaseModel, IGameplayModel
    {
        private const string EnemyCarsDescriptionPath = "Cars/Descriptions/EnemyCarsDescription";
        private const string PlayerCarsDescriptionPath = "Cars/Descriptions/PlayerCarsDescription";

        // Internal
        private ReactiveProperty<GameState> _state;
        private ReactiveProperty<ViewId> _activeViewId;
        private Subject<ViewId> _onViewClosed;
        private SceneLoader _sceneLoader;
        private PlayerCarDescriptions _playerCarsDescription;
        private EnemyCarDescriptions _enemyCarsDescriptions;

        // Public properties
        public ReactiveProperty<GameState> State => _state;
        public ReactiveProperty<ViewId> ActiveViewId => _activeViewId;
        public Subject<ViewId> OnViewClosed => _onViewClosed;

        public GameplayModel()
        {
            _sceneLoader = new SceneLoader();
            _state = new ReactiveProperty<GameState>(GameState.None);
            _activeViewId = new ReactiveProperty<ViewId>(ViewId.None);
            _onViewClosed = new Subject<ViewId>();
        }

        public void InitModel()
        {
            var assetProvider = ServiceLocator.Container.Single<IAssetProvider>();
            _enemyCarsDescriptions = assetProvider.LoadResource<EnemyCarDescriptions>(EnemyCarsDescriptionPath);
            _playerCarsDescription = assetProvider.LoadResource<PlayerCarDescriptions>(PlayerCarsDescriptionPath);
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

        public EnemyCarInfo GetEnemyCarInfo(EnemyCarId carId)
        {
            return _enemyCarsDescriptions.CarsList.FirstOrDefault(x => x.CarId == carId);
        }
    }
}