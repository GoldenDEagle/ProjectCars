using Assets.Codebase.Infrastructure.ServicesManagment;
using Assets.Codebase.Infrastructure.ServicesManagment.Ads;
using Assets.Codebase.Infrastructure.ServicesManagment.ModelAccess;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Codebase.Utils.UI
{
    public class AdPopupWindow : MonoBehaviour
    {
        [SerializeField] private Button _claimButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _adsUnavailableText;

        private CompositeDisposable _disposables = new CompositeDisposable();
        private IAdsService _adService;
        private IModelAccessService _models;
        private IDisposable _rewardedSubscription;

        private void Awake()
        {
            _adService = ServiceLocator.Container.Single<IAdsService>();
            _models = ServiceLocator.Container.Single<IModelAccessService>();
        }

        private void OnEnable()
        {
            var isAdAvailable = _adService.CheckIfRewardedIsAvailable();
            _claimButton.gameObject.SetActive(isAdAvailable);
            _adsUnavailableText.gameObject.SetActive(!isAdAvailable);

            _claimButton.OnClickAsObservable().Subscribe(_ => ClaimRewardClicked()).AddTo(_disposables);
            _closeButton.OnClickAsObservable().Subscribe(_ => CloseWindowClicked()).AddTo(_disposables);
        }

        private void OnDisable()
        {
            _disposables.Dispose();
        }

        private void ClaimRewardClicked()
        {
            if (!_adService.CheckIfRewardedIsAvailable()) return;

            _rewardedSubscription = _adService.OnRewardedSuccess.Subscribe(_ => OnAdSuccess()).AddTo(_disposables);
            _adService.ShowRewarded();
        }

        private void CloseWindowClicked()
        {
            Destroy(gameObject);
        }

        private void OnAdSuccess()
        {
            _disposables.Remove(_rewardedSubscription);
            _models.ProgressModel.ModifyCoinAmount(50);
            CloseWindowClicked();
        }
    }
}