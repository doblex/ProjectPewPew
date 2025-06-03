using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinInfo : MonoBehaviour
{
    [SerializeField] public CoinSlot CoinSlot_ref;

    [Serializable]
    public struct CoinSlot
    {
        public string CoinName;
        public string CoinDescription;
        public Vector3 InfoPanelPosition;
    }
}
