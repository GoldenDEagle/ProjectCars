using Assets.Codebase.Data.Tracks;

namespace Assets.Codebase.Infrastructure.ServicesManagment.Leaderboard
{
    public interface ILeaderboardService : IService
    {
        /// <summary>
        /// Shows leaderboard for selected track
        /// </summary>
        /// <param name="trackId"></param>
        public void OpenLeaderboard(TrackId trackId);
        /// <summary>
        /// Pushes new best result to leaderboard
        /// </summary>
        /// <param name="trackId"></param>
        /// <param name="time"></param>
        public void PushNewResultToLeaderBoard(TrackId trackId, float time);
    }
}
