using System.Collections.Generic;
using UnityEngine;

namespace SyarifRee.Data
{
    [CreateAssetMenu(fileName = "PionData", menuName = "Agate/PionData", order = 0)]
    public class PionData : ScriptableObject
    {
        public List<Pion> pions = new List<Pion>();
    }

    [System.Serializable]
    public class Pion
    {
        public string id;
        public Sprite icon;
        public int value;
    }
}