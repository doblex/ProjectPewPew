using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] GameObject DialogueSystemPanel_ref;
    [SerializeField] TMP_Text DialogueName_ref;
    [SerializeField] TMP_Text DialogueText_ref;
    DialogueInfo ActualDialogueInfo_ref;
    HUD HUD_ref;
    int ActualDestinationIndex;
    int StartDialogueIndex;
    private void Start()
    {
        HUD_ref = gameObject.GetComponent<HUD>();
    }

    public void StartDialogue(DialogueInfo dialogueInfo, int StartIndex)
    {
        SetupDialogue( dialogueInfo, StartIndex);
        DialogueSystemPanel_ref.SetActive(true);
    }

    public void EndDialogue()
    {
        DialogueSystemPanel_ref.SetActive(false);
    }

    public void SetupDialogue(DialogueInfo dialogueInfo_ref, int StartIndex)
    {
        StartDialogueIndex = StartIndex;
        ActualDialogueInfo_ref = dialogueInfo_ref;
        DialogueName_ref.text = ActualDialogueInfo_ref.DialogueSlots[StartIndex].DialogueName;
        DialogueText_ref.text = ActualDialogueInfo_ref.DialogueSlots[StartIndex].DialogueText;
        ActualDestinationIndex = ActualDialogueInfo_ref.DialogueSlots[StartIndex].DialogueDestionationIndex;
        HUD_ref.ActivateCycleDialogue();
    }

    public void ContinueDialogue()
    {
        if (ActualDestinationIndex >= 0)
        {
            DialogueName_ref.text = ActualDialogueInfo_ref.DialogueSlots[ActualDestinationIndex].DialogueName;
            DialogueText_ref.text = ActualDialogueInfo_ref.DialogueSlots[ActualDestinationIndex].DialogueText;
            ActualDestinationIndex = ActualDialogueInfo_ref.DialogueSlots[ActualDestinationIndex].DialogueDestionationIndex;
            HUD_ref.ActivateCycleDialogue();
        }
        else
        {
            EndDialogue();
        }
    }
}
