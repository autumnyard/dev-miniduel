using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public sealed class AudioHandler : SingletonMonoBehaviour<AudioHandler>
    {
        [SerializeField] private Settings _settings;
        [SerializeField] private AudioSource _source;

        public float Play(EAudioSFX audio)
        {
            var setting = _settings.GetAudioSFXSetting(audio);
            //_source.clip = setting.clip;
            _source.PlayOneShot(setting.clip);
            return setting.clip.length;
        }


        #region TEST

        [ContextMenu("TEST PlaySFX 1")]
        private void TEST_PlaySFX_1()
        {
            Play(EAudioSFX.SetPiece);
        }

        [ContextMenu("TEST PlaySFX 2")]
        private void TEST_PlaySFX_2()
        {
            Play(EAudioSFX.Clash);
        }

        #endregion
    }
}
