using Assets.Codebase.Models.Gameplay.Data;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.Values;
using Assets.Codebase.Views.Base;
using System;

namespace Assets.Codebase.Presenters.Pause
{
    public class PausePresenter : BasePresenter, IPausePresenter
    {
        private bool _quitInProgress = false;

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
            if (_quitInProgress) { return; }

            _quitInProgress = true;
            GameplayModel.LoadScene(SceneNames.Menu, OnMenuLoaded);
        }

        private void OnMenuLoaded()
        {
            GameplayModel.UnPauseGame(GameState.Menu);
            _quitInProgress = false;
            GameplayModel.ActivateView(ViewId.CarSelection);
        }
    }
}
