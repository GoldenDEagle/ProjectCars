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

        public void ContinueClicked()
        {
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
