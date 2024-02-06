﻿using Assets.Codebase.Data.Audio;

namespace Assets.Codebase.Infrastructure.ServicesManagment.Audio
{
    /// <summary>
    /// Manages all sounds.
    /// </summary>
    public interface IAudioService : IService
    {
        /// <summary>
        /// Plays single sound.
        /// </summary>
        /// <param name="soundId"></param>
        public void PlaySfxSound(SoundId soundId);

        /// <summary>
        /// Changes music theme.
        /// </summary>
        /// <param name="musicId"></param>
        public void ChangeMusic(SoundId musicId);
        /// <summary>
        /// Turns music on or off
        /// </summary>
        /// <param name="isEnabled"></param>
        public void EnableMusic(bool isEnabled);

        /// <summary>
        /// Mutes all audio.
        /// </summary>
        public void MuteAll();

        /// <summary>
        /// Unmutes all audio.
        /// </summary>
        public void UnmuteAll();

        /// <summary>
        /// Changes SFX volume.
        /// </summary>
        /// <param name="value"></param>
        public void SetSFXVolume(float value);

        /// <summary>
        /// Changes music volume.
        /// </summary>
        /// <param name="value"></param>
        public void SetMusicVolume(float value);
    }
}
