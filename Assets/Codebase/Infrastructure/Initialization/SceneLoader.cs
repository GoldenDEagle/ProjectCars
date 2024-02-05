﻿using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Codebase.Infrastructure.Initialization
{
    public class SceneLoader
    {
        public void Load(string name, Action onLoaded)
        {
            LoadScene(name, onLoaded).Forget();
        }

        private async UniTask LoadScene(string nextScene, Action onLoaded = null)
        {
            AsyncOperation waitNewScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNewScene.isDone)
            {
                await UniTask.DelayFrame(1);
            }

            AudioListener.volume = ServiceLocator.Container.Single<IModelAccessService>().ProgressModel.SessionProgress.SFXVolume.Value;
            onLoaded?.Invoke();
        }
    }
}
