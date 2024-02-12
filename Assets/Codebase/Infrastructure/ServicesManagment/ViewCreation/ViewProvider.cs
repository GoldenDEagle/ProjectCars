using Assets.Codebase.Gameplay.Tutorial;
using Assets.Codebase.Infrastructure.ServicesManagment.Assets;
using Assets.Codebase.Presenters.Base;
using Assets.Codebase.Utils.UI;
using Assets.Codebase.Views.Base;
using DavidJalbert.TinyCarControllerAdvance;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Codebase.Infrastructure.ServicesManagment.ViewCreation
{
    /// <summary>
    /// Creates new views using IAssetProvider.
    /// </summary>
    public class ViewProvider : IViewProvider
    {
        // Additional UI elements
        private Countdown _countdown;
        private TCCAMobileInput _mobileInput;
        private WrongWayWarning _wrongWayWarning;

        public Countdown Countdown
        {
            get
            {
                if (_countdown == null)
                {
                    _countdown = CreateCountdown();
                }
                return _countdown;
            }
        }

        public TCCAMobileInput MobileInput
        {
            get
            {
                if (_mobileInput == null)
                {
                    _mobileInput = CreateMobileInput();
                }
                return _mobileInput;
            }
        }

        public WrongWayWarning WayWarning
        {
            get
            {
                if (_wrongWayWarning == null)
                {
                    _wrongWayWarning = CreateWrongWayWarning();
                }
                return _wrongWayWarning;
            }
        }



        // All presenters
        private List<BasePresenter> _presenters;

        // Root for all UI
        private RectTransform _uiRoot;

        // Paths to all view prefabs
        private const string TitleViewPath = "Views/TitleView";
        private const string CarSelectionViewPath = "Views/CarSelectionView";
        private const string TrackSelectionViewPath = "Views/TrackSelectionView";
        private const string IngameViewPath = "Views/IngameView";
        private const string EndgameViewPath = "Views/EndgameView";
        private const string PauseViewPath = "Views/PauseView";
        private const string CountdownPath = "UI/Countdown";
        private const string MobileInputPath = "UI/MobileInput";
        private const string AdPopupPath = "UI/AdPopupWindow";
        private const string PCTutorialPath = "UI/PCTutorial";
        private const string WrongWayWarningPath = "UI/WrongWayWarning";
        private IAssetProvider _assets;

        public ViewProvider(IAssetProvider assets, List<BasePresenter> presenters, RectTransform uiRoot)
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

        private Countdown CreateCountdown()
        {
            var element = _assets.Instantiate(CountdownPath).GetComponent<Countdown>();
            element.transform.SetParent(_uiRoot, false);
            return element;
        }

        public AdPopupWindow CreateAdPopupWindow()
        {
            var element = _assets.Instantiate(AdPopupPath).GetComponent<AdPopupWindow>();
            element.transform.SetParent(_uiRoot, false);
            element.gameObject.SetActive(true);
            return element;
        }

        public WrongWayWarning CreateWrongWayWarning()
        {
            var element = _assets.Instantiate(WrongWayWarningPath).GetComponent<WrongWayWarning>();
            element.transform.SetParent(_uiRoot, false);
            return element;
        }

        public PCTutorial CreatePCTutorial()
        {
            var element = _assets.Instantiate(PCTutorialPath).GetComponent<PCTutorial>();
            element.transform.SetParent(_uiRoot, false);
            return element;
        }

        private TCCAMobileInput CreateMobileInput()
        {
            var element = _assets.Instantiate(MobileInputPath).GetComponent<TCCAMobileInput>();
            return element;
        }
    }
}
