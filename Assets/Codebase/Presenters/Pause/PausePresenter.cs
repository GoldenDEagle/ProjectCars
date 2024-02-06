using Assets.Codebase.Gameplay.Cars;
using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation;
using Assets.Codebase.Models.Base;
using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Codebase.Presenters.Pause
{
    public class PausePresenter : BasePresenter, IPausePresenter
    {
        private bool _sceneLoadInProgress = false;

        public PausePresenter()
        {
            CorrespondingViewId = ViewId.Pause;
        }

        public override void CreateView()
        {
            base.CreateView();

            if (GameplayModel.IsMobile)
            {
                var mobileInput = ServiceLocator.Container.Single<IViewProvider>().MobileInput;
                mobileInput.gameObject.SetActive(false);
            }
        }

        public void ContinueClicked()
        {
            if (GameplayModel.IsMobile)
            {
                var mobileInput = ServiceLocator.Container.Single<IViewProvider>().MobileInput;
                mobileInput.gameObject.SetActive(true);
            }

            GameplayModel.UnPauseGame(GameState.Race);
            GameplayModel.ActivateView(ViewId.Ingame);
        }

        public void QuitClicked()
        {
            if (_sceneLoadInProgress) { return; }

            _sceneLoadInProgress = true;
            GameplayModel.LoadScene(SceneNames.Menu, OnMenuLoaded);
        }

        public void RestartClicked()
        {
            if (_sceneLoadInProgress) { return; }

            _sceneLoadInProgress = true;
            GameplayModel.LoadScene(SceneManager.GetActiveScene().name, OnLevelReloaded);
        }

        public void SoundButtonClicked()
        {
            ProgressModel.SwitchSound();
        }

        private void OnMenuLoaded()
        {
            GameplayModel.UnPauseGame(GameState.Menu);
            _sceneLoadInProgress = false;
            GameplayModel.ActivateView(ViewId.CarSelection);
        }

        private void OnLevelReloaded()
        {
            GameplayModel.UnPauseGame(GameState.Race);
            _sceneLoadInProgress = false;
            GameplayModel.ActivateView(ViewId.Ingame);
        }
    }
}
