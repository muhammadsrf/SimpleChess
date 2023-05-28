using UnityEngine;
using Malee.List;

namespace SyarifRee.AudioNMusic
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Agate/AudioData", order = 0)]
    public class AudioData : ScriptableObject
    {
        public bool isMusicVolume = true;
        public bool isSfxVolume = true;

        [Header("Main Song")]
        public AudioClip gameplaySong;

        [Header("Clip Effect")]
        [Reorderable]
        public AudioArray audios;

        [System.Serializable]
        public class AudioArray : ReorderableArray<AudioClass> { }
    }
}
