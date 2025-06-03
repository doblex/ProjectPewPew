using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public enum DialogueType
    {
        tailsDialogue,
        headsDialogue,
        easyThrowDialogue,
        mediumThrowDialogue,
        difficultThrowDialogue,
        failedShootDialogue,
        succesShootDialogue,
        playerWinDialogue,
        playerLoseDialogue,
        EnemyDifficultySelectionDialogue,
        EnemyEndTurnDialogue
    }

    static DialogueManager Instance;

    [SerializeField] DialogueSystem _dialogueSystem;

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

    public void StartDialogue(DialogueType dialogueType) 
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

        StartDialogue(dialogue);
    }
    public void StartDialogue(DialogueInfo[] possibleDialogues)
    { 
        int index = Random.Range(0, possibleDialogues.Length);

        _dialogueSystem.StartDialogue(possibleDialogues[index], 0);
    }
}
