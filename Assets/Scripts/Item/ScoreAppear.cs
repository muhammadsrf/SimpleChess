using UnityEngine;
using UnityEngine.UI;

namespace SyarifRee.Item
{
    public class ScoreAppear : MonoBehaviour
    {
        [SerializeField] Text _textAppear;
        [SerializeField] RectTransform _rectTransform;

        public void SetText(int score, Vector3 position)
        {
            // set position
            _rectTransform.anchoredPosition = position;

            // set text
            _textAppear.text = "+" + score;
        }

        void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}