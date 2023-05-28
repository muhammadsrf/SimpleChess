using Pathfinding;
using SyarifRee.AudioNMusic;
using SyarifRee.Core;
using SyarifRee.Data;
using UnityEngine;

namespace SyarifRee.Item
{
    public class ItemController : MonoBehaviour
    {
        [SerializeField] Transform _parentEffect;

        public string pionName;
        public GameObject bidak = null;
        public ProceduralManager proceduralManager = null;
        private GridNode _node;

        public void SetNode(GridNode newNode)
        {
            _node = newNode;
        }

        private void OnMouseDown()
        {
            if (proceduralManager.gameOver) { return; }
            if (bidak != null) { return; }

            // play a sound effect
            AudioManager.instance.PlayClip(AudioID.put_in);

            // fill new bidak
            bidak = proceduralManager.CreateBidak(position: transform.position, parent: transform, node: _node);

            if (bidak != null)
            {
                proceduralManager.UpdateScore(bidakTransform: _parentEffect);

                proceduralManager.UpdateNextBox();
            }
        }

        public GridNode GetNode()
        {
            return _node;
        }
    }
}