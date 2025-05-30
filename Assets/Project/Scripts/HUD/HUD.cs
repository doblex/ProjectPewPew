using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public delegate void OnChooseCoin();
    public delegate void OnChooseCoinTrajectory();
    public delegate void OnChooseEnemyTrajectory();

    public OnChooseCoin onChooseCoin;
    public OnChooseCoinTrajectory onChooseCoinTrajectory;
    public OnChooseEnemyTrajectory onChooseEnemyTrajectory;

    [SerializeField, Header("Info"), Space(10f)] bool b_IsOptionsPanelOpen;
    [SerializeField] bool b_IsChooseInitialPlayerPanelOpen;

    [SerializeField, Header("Setup HUD"), Space(10f)] GameObject StartMenuPanel_ref;
    [SerializeField] GameObject GamePanel_ref;
    [SerializeField] GameObject OptionsPanel_ref;
    [SerializeField] GameObject ConfirmPanel_ref;

    [SerializeField, Header("Setup Turn Indicator"), Space(10f)] GameObject TurnIndicator_ref;
    [SerializeField] Vector3 PlayerTurnIdicatorPosition;
    [SerializeField] Vector3 IATurnIdicatorPosition;

    DialogueSystem DialogueSystem_ref;

    [SerializeField, Header("Setup Dialogue"), Space(10f)] DialogueInfo StartDialogueInfo_ref;

    float timer;
    [SerializeField] float DelayPassDialogue;

    bool b_CycleDialogueIsRunning;
    void Start()
    {
        timer = 0;
        DialogueSystem_ref = gameObject.GetComponent<DialogueSystem>();
    }

    void Update()
    {
        if (b_CycleDialogueIsRunning)
        {
            CycleDialogue();
        }
    }

    public void StartGame()
    {
        //TODO Aggiungere start animazione del nemico (Raccoglie monete) 
        StartMenuPanel_ref.SetActive(false);
        GamePanel_ref.SetActive(true);
        DialogueSystem_ref.StartDialogue(StartDialogueInfo_ref,0);
        //TODO Breve spiegazione con dialogo testuale del gioco
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

    public void StartDialogue()
    {
        
    }

    public void CycleDialogue()
    {
        timer += Time.deltaTime;
        if (timer >= DelayPassDialogue)
        {
            timer = 0;
            b_CycleDialogueIsRunning = false;
            DialogueSystem_ref.ContinueDialogue();
        }
    }

    public void ActivateCycleDialogue()
    {
        b_CycleDialogueIsRunning = true;
    }

    public void ToggleOptionsMenu()
    {
        if (b_IsOptionsPanelOpen)
        {
            OptionsPanel_ref.SetActive(false);
            b_IsOptionsPanelOpen = false;
        }
        else
        {
            OptionsPanel_ref.SetActive(true);
            b_IsOptionsPanelOpen = true;
        }
    }

    public void OpenConfirmPanel()
    {
        ConfirmPanel_ref.SetActive(true);
    }

    public void CloseConfirmPanel()
    {
        ConfirmPanel_ref.SetActive(false);
    }

    public void ToggleChoosePlayerPanel(PlayerType playerType)
    {
        //TODO aggiunta pannello di scelta randomica del giocatore iniziale
        if (b_IsChooseInitialPlayerPanelOpen)
        {
            if (playerType == PlayerType.PLAYER)
            {
                //TODO start animazione moneta su lato Player
            }
            else
            {
                //TODO start animazione moneta su lato IA
            }
            b_IsChooseInitialPlayerPanelOpen = true;
        }
        else
        {
            b_IsChooseInitialPlayerPanelOpen = false;
        }
    }

    public void ToggleTurnIndicatorPosition()
    {
        //TODOChiedere l'aggiunta di un delegato per il passaggio di turno a cui legare questa funzione
        //TODOin caso sia Player
        TurnIndicator_ref.transform.localPosition = PlayerTurnIdicatorPosition;
        //TODOin caso sia IA
        TurnIndicator_ref.transform.localPosition = IATurnIdicatorPosition;
    }

    
}
