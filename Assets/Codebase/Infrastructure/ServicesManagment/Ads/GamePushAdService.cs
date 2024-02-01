using GamePush;
using UniRx;
using UnityEngine;

namespace Assets.Codebase.Infrastructure.ServicesManagment.Ads
{
    public class GamePushAdService : IAdsService
    {
        private bool _adsEnabled = true;
        public Subject<Unit> OnRewardedSuccess { get; private set; }

        public GamePushAdService()
        {
            OnRewardedSuccess = new Subject<Unit>();
            GP_Ads.OnRewardedReward += RewardedSuccess;
        }

        public void SetAdsStatus(bool adsEnabled)
        {
            _adsEnabled = adsEnabled;
        }

        public bool CheckIfFullscreenIsAvailable()
        {
            return GP_Ads.IsFullscreenAvailable();
        }

        public void ShowFullscreen()
        {
            if (!_adsEnabled)
            {
                Debug.Log("Ads are disabled!");
                return;
            }

            if (CheckIfFullscreenIsAvailable())
            {
                GP_Ads.ShowFullscreen();
            }
        }

        public bool CheckIfRewardedIsAvailable()
        {
            return GP_Ads.IsRewardedAvailable();
        }

        public void ShowRewarded()
        {
            GP_Ads.ShowRewarded();
        }

        private void RewardedSuccess(string key)
        {
            OnRewardedSuccess?.OnNext(Unit.Default);
        }



        public bool IsDeviceMobile()
        {
            return GP_Device.IsMobile();
        }
    }
}
