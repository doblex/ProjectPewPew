using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInfo : MonoBehaviour
{
    [Header("Contact info"), Space(10f), SerializeField] public Sprite ContactSprite;
    [SerializeField] public List<DialogueSlot> DialogueSlots;

    [Serializable]
    public struct DialogueSlot
    {
        public int DialogueIndex;
        public int DialogueDestionationIndex;

        public string DialogueName;
        public string DialogueText;
    }
}
