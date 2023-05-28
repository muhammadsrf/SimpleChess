using SyarifRee.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SyarifRee.Item
{
    public class TimerController : MonoBehaviour
    {
        [SerializeField] Image _fill;

        private ProceduralManager _proceduralManager;

        private void Start()
        {
            _proceduralManager = FindObjectOfType<ProceduralManager>();

            _fill.fillAmount = 1f;
        }

        private void Update()
        {
            _fill.fillAmount = _proceduralManager._timerRunning / _proceduralManager._timer; 
        }
    }
}