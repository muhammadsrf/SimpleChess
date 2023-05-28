using UnityEngine;

namespace SyarifRee.Item
{
    public class Disappear : MonoBehaviour
    {
        public void DisappearNow()
        {
            GetComponent<Animator>().Play("disappear");
        }

        void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}