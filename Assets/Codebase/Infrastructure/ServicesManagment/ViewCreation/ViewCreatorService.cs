﻿using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Views.Base;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation
{
    /// <summary>
    /// Creates new views using IAssetProvider.
    /// </summary>
    public class ViewCreatorService : IViewCreatorService
    {
        // All presenters
        private List<BasePresenter> _presenters;

        // Root for all UI
        private RectTransform _uiRoot;

        // Paths to all view prefabs
        private const string ExampleViewPath = "Views/ExampleView";
        private const string TitleViewPath = "Views/TitleView";
        private const string CarSelectionViewPath = "Views/CarSelectionView";
        private const string TrackSelectionViewPath = "Views/TrackSelectionView";
        private const string IngameViewPath = "Views/IngameView";
        private const string EndgameViewPath = "Views/EndgameView";
        private const string PauseViewPath = "Views/PauseView";

        private IAssetProvider _assets;

        public ViewCreatorService(IAssetProvider assets, List<BasePresenter> presenters, RectTransform uiRoot)
        {
            _assets = assets;
            _presenters = presenters;
            _uiRoot = uiRoot;
        }

        public BaseView CreateView(ViewId viewId)
        {
            // Find target presenter
            var presenter = _presenters.FirstOrDefault(x => x.GetCorrespondingViewId() == viewId);

            if (presenter == null)
            {
                throw new System.NotImplementedException("Couldn't find corresponding presenter");
            }

            var path = string.Empty;

            switch (viewId)
            {
                case ViewId.None:
                    new System.ArgumentException(nameof(viewId));
                    break;
                case ViewId.ExampleView:
                    path = ExampleViewPath;
                    break;
                case ViewId.Title:
                    path = TitleViewPath;
                    break;
                case ViewId.CarSelection:
                    path = CarSelectionViewPath;
                    break;
                case ViewId.TrackSelection:
                    path = TrackSelectionViewPath;
                    break;
                case ViewId.Ingame:
                    path = IngameViewPath;
                    break;
                case ViewId.EndGame:
                    path = EndgameViewPath;
                    break;
                case ViewId.Pause:
                    path = PauseViewPath;
                    break;
                default:
                    throw new System.ArgumentException(nameof(viewId));
            }

            // Create and init view
            var view = _assets.Instantiate(path).GetComponent<BaseView>();
            view.transform.SetParent(_uiRoot, false);
            return view;
        }
    }
}
