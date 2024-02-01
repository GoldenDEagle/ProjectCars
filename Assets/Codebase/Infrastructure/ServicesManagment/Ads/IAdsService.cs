using UniRx;

namespace Assets.Codebase.Infrastructure.ServicesManagment.Ads
{
    /// <summary>
    /// Managing ads.
    /// </summary>
    public interface IAdsService : IService
    {
        /// <summary>
        /// Fired when rewarded was watched successfully
        /// </summary>
        public Subject<Unit> OnRewardedSuccess { get; }
        // Rewarded Ad
        public bool CheckIfRewardedIsAvailable();
        public void ShowRewarded();

        // Fullscreen Ad
        public bool CheckIfFullscreenIsAvailable();
        public void ShowFullscreen();

        /// <summary>
        /// Enables or disables ads.
        /// </summary>
        /// <param name="adsEnabled"></param>
        public void SetAdsStatus(bool adsEnabled);



        public bool IsDeviceMobile();
    }
}
