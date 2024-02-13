using Assets.Codebase.Data.Tracks;
using Assets.Codebase.Utils.Helpers;
using GamePush;

namespace Assets.Codebase.Infrastructure.ServicesManagment.Leaderboard
{
    public class GPLeaderboardService : ILeaderboardService
    {
        private const string LeaderboardGlobalTag = "BestTimes";
        private const string TimeValueKey = "time_number";
        private const string DisplayedTimeKey = "time_string";
        private const string TimeStringKey = "_time";

        public void OpenLeaderboard(TrackId trackId)
        {
            GP_LeaderboardScoped.Open(LeaderboardGlobalTag, trackId.ToString(), Order.ASC, 10, 5, trackId.ToString() + TimeStringKey, trackId.ToString() + TimeStringKey, WithMe.last);
        }

        public void PushNewResultToLeaderBoard(TrackId trackId, float time)
        {
            GP_Player.Set(trackId.ToString() + TimeStringKey, TimeConverter.TimeWithMS(time));
            GP_LeaderboardScoped.PublishRecord(LeaderboardGlobalTag, trackId.ToString(), true, TimeValueKey, time);
        }
    }
}
