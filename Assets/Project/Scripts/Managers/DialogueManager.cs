using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] DialogueSystem _dialogueSystem;
    [SerializeField] bool showWarning = true;

    [SerializeField] DialogueInfo[] tailsDialogue;
    [SerializeField] DialogueInfo[] headsDialogue;
    [SerializeField] DialogueInfo[] easyThrowDialogue;
    [SerializeField] DialogueInfo[] mediumThrowDialogue;
    [SerializeField] DialogueInfo[] difficultThrowDialogue;
    [SerializeField] DialogueInfo[] failedShootDialogue;
    [SerializeField] DialogueInfo[] succesShootDialogue;
    [SerializeField] DialogueInfo[] playerWinDialogue;
    [SerializeField] DialogueInfo[] playerLoseDialogue;
    [SerializeField] DialogueInfo[] EnemyDifficultySelectionDialogue;
    [SerializeField] DialogueInfo[] EnemyEndTurnDialogue;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartDialogue(DialogueType dialogueType, Action action = null) 
    {
        DialogueInfo[] dialogue = null;

        switch (dialogueType)
        {
            case DialogueType.tailsDialogue:
                dialogue = tailsDialogue;
                break;
            case DialogueType.headsDialogue:
                dialogue = headsDialogue;
                break;
            case DialogueType.easyThrowDialogue:
                dialogue = easyThrowDialogue;
                break;
            case DialogueType.mediumThrowDialogue:
                dialogue = mediumThrowDialogue;
                break;
            case DialogueType.difficultThrowDialogue:
                dialogue = difficultThrowDialogue;
                break;
            case DialogueType.failedShootDialogue:
                dialogue = failedShootDialogue;
                break;
            case DialogueType.succesShootDialogue:
                dialogue = succesShootDialogue;
                break;
            case DialogueType.playerWinDialogue:
                dialogue = playerWinDialogue;
                break;
            case DialogueType.playerLoseDialogue:
                dialogue = playerLoseDialogue;
                break;
            case DialogueType.EnemyDifficultySelectionDialogue:
                dialogue = EnemyDifficultySelectionDialogue;
                break;
            case DialogueType.EnemyEndTurnDialogue:
                dialogue = EnemyEndTurnDialogue;
                break;
        }

        StartDialogue(dialogue, action);
    }
    public void StartDialogue(DialogueInfo[] possibleDialogues, Action action = null)
    { 
        if(possibleDialogues == null || possibleDialogues.Length == 0)
        {
            if(showWarning)
                Debug.LogWarning("No dialogues available.");
            action?.Invoke();
            return;
        }

        int index = UnityEngine.Random.Range(0, possibleDialogues.Length);
        _dialogueSystem.StartDialogue(possibleDialogues[index], 0, action);
    }
}
