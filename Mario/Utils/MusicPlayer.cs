using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;

namespace Utils
{
    public class MusicPlayer
    {
        private ContentManager Content;
        private Dictionary<string, SoundEffect> soundEffects;
        private Song bgm;
        private bool muted;
        public MusicPlayer(ContentManager content)
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            Content = content;
            muted = false;
        }
        public void LoadBGM(string file)
        {
            bgm = Content.Load<Song>(file);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(bgm);
            MediaPlayer.Stop();
        }
        public void ToggleMute()
        {
            muted = !muted;
            MediaPlayer.IsMuted = muted;
            if(muted)
            {
                SoundEffect.MasterVolume = 0f;
            }
            else
            {
                SoundEffect.MasterVolume = 1f;
            }
        }
        public static void StopBGM()
        {
            MediaPlayer.Stop();
        }
        public static void PauseBGM()
        {
            MediaPlayer.Pause();
        }
        public static void UnpauseBGM() => MediaPlayer.Resume();
        public void RestartBGM()
        {
            MediaPlayer.Play(bgm);
        }
        public void LoadSoundEffect(string file, string name)
        {
            soundEffects[name] = Content.Load<SoundEffect>(file);
        }

        public void PlaySoundEffect(string name)
        {
            soundEffects[name].Play();
        }
    }
}