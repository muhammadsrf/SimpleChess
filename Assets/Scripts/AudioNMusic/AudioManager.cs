using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace SyarifRee.AudioNMusic
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField]
        AudioSource main_audioMain;
        [SerializeField]
        AudioSource main_audioEffect;
        public AudioData main_audioData;

        private void Awake()
        {
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Start()
        {
            main_audioMain.clip = main_audioData.gameplaySong;

            // play main song
            main_audioMain.Play();
        }

        public void PlayClip(AudioID audioID)
        {
            string idFromAudio = audioID.ToString();

            foreach (AudioClass audioClass in main_audioData.audios)
            {
                if (audioClass.id != idFromAudio) { continue; }

                main_audioEffect.pitch = 1f;

                PlayClipEffect(audioClass.audioClip);

                break;
            }
        }

        public void PlayClipPitch1_25f(AudioID audioID)
        {
            string idFromAudio = audioID.ToString();

            foreach (AudioClass audioClass in main_audioData.audios)
            {
                if (audioClass.id != idFromAudio) { continue; }

                main_audioEffect.pitch = 1.25f;

                PlayClipEffect(audioClass.audioClip);

                break;
            }

        }

        public void PlayClipPitch_67f(AudioID audioID)
        {
            string idFromAudio = audioID.ToString();

            foreach (AudioClass audioClass in main_audioData.audios)
            {
                if (audioClass.id != idFromAudio) { continue; }

                main_audioEffect.pitch = .67f;

                PlayClipEffect(audioClass.audioClip);

                break;
            }

        }

        private void PlayClipEffect(AudioClip audioClip)
        {
            main_audioEffect.PlayOneShot(audioClip);
        }

        public void PlayMainClip()
        {
            main_audioMain.Play();
        }

        public void StopMainClip()
        {
            main_audioMain.Stop();
        }

        public void SetMusicOn(bool newBool)
        {
            main_audioMain.mute = !newBool;
        }

        public void SetSFXOn(bool newBool)
        {
            main_audioEffect.mute = !newBool;
        }

    }
}