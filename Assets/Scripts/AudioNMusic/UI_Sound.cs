using UnityEngine;

namespace SyarifRee.AudioNMusic
{
    public class UI_Sound : MonoBehaviour
    {
        public void PutIn()
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayClip(AudioID.put_in);
            }
        }

        public void Same3()
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayClip(AudioID.same3);
            }
        }

        public void Lose()
        {
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlayClip(AudioID.lose);
            }
        }
    }
}