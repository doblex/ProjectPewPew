
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    public static HUD Instance;

    public LayerMask objectsLayermask;

    public Material HighlightMaterial_ref;

    public delegate void OnStartGame();
    public delegate void OnChooseCoin(int index);
    public delegate void OnChooseCoinTrajectory(int trajectoryIndex, int itemIndex);
    public delegate void OnChooseEnemyTrajectory();

    public OnStartGame onStartGame;
    public OnChooseCoin onChooseCoin;
    public OnChooseCoinTrajectory onChooseCoinTrajectory;
    public OnChooseEnemyTrajectory onChooseEnemyTrajectory;

    CoinType ChoosenCoin;

    [SerializeField, Header("Info"), Space(10f)] bool b_IsOptionsPanelOpen;
    [SerializeField] bool b_IsChooseInitialPlayerPanelOpen;
    [SerializeField] public bool b_IsTrajectoriesPanelOpen;

    [SerializeField, Header("Setup HUD"), Space(10f)] GameObject StartMenuPanel_ref;
    [SerializeField] GameObject GamePanel_ref;
    [SerializeField] GameObject OptionsPanel_ref;
    [SerializeField] GameObject ConfirmPanel_ref;
    [SerializeField] GameObject CoinDifficultyPanel_ref;
    [SerializeField] GameObject TrajectoriesPanel_ref;
    [SerializeField] GameObject TwoTrajectoriesPanel_ref;
    [SerializeField] GameObject ThreeTrajectoriesPanel_ref;
    [SerializeField] int MinNumberOfTrajectories;
    [SerializeField] int MaxNumberOfTrajectories;
    SelectorToggle SelectorToggle_ref;

    [SerializeField, Header("Setup Turn Indicator"), Space(10f)] GameObject TurnIndicator_ref;
    [SerializeField] Vector3 PlayerTurnIdicatorPosition;
    [SerializeField] Vector3 IATurnIdicatorPosition;

    DialogueSystem DialogueSystem_ref;

    [SerializeField, Header("Setup Dialogue"), Space(10f)] DialogueInfo StartDialogueInfo_ref;

    float timer;
    [SerializeField] float DelayPassDialogue;

    GameObject ActualHittedObject;

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

    void Start()
    {
        DialogueSystem_ref = gameObject.GetComponent<DialogueSystem>();
        TurnManager.Instance.onTurnEnd += ToggleTurnIndicatorPosition;
        TurnManager.Instance.onPlayerChooseCoin += OpenCoinPanel;
        TurnManager.Instance.onPlayerChooseCoinTrajectory += OpenTrajectoriesPanel;
        TurnManager.Instance.onPlayerChooseEnemyTrajectory += OpenTrajectoriesPanel;

        //TODO Attaccarsi ai player;
    }

    void Update()
    {
        CheckMousePosition();
    }

    public void StartGame()
    {
        StartMenuPanel_ref.SetActive(false);
        GamePanel_ref.SetActive(true);
        DialogueSystem_ref.StartDialogue(StartDialogueInfo_ref, 0, () => { onStartGame.Invoke(); } );
    }

    public void ExitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
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
        DialogueSystem_ref.ContinueDialogue();
    }

    public void ActivateCycleDialogue()
    {
        Invoke("CycleDialogue", DelayPassDialogue);
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

    public void OpenCoinPanel() 
    {
        CoinDifficultyPanel_ref.SetActive(true);
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

    public void ToggleTurnIndicatorPosition(PlayerType playerType)
    {
        if (playerType == PlayerType.PLAYER)
        {
            TurnIndicator_ref.transform.localPosition = PlayerTurnIdicatorPosition;
        }
        else
        {
            TurnIndicator_ref.transform.localPosition = IATurnIdicatorPosition;
        }
    }

    public void LaunchCoin()
    {
        //TODO Inserire animazione lancio della moneta
        Debug.LogError("Inizia il giocatore");
    }

    public void ChooseCoin(int coinType)
    {
        ChoosenCoin = (CoinType)coinType;
        CoinDifficultyPanel_ref.SetActive(false);
        onChooseCoin?.Invoke(coinType);
    }

    public void OpenTrajectoriesPanel(int TrajectoriesQuantity, Item[] items)
    {
        //TODO ITEMS
        if (TrajectoriesQuantity < MinNumberOfTrajectories || TrajectoriesQuantity > MaxNumberOfTrajectories)
        {
            Debug.LogError("Quantità traiettorie invalida");
        }
        else
        {
            TrajectoriesPanel_ref.SetActive(true);
            switch (TrajectoriesQuantity)
            {
                case 1:
                    OnConfirmTrajectory(false);
                    break;

                case 2:
                    TwoTrajectoriesPanel_ref.SetActive(true);
                    SelectorToggle_ref = TwoTrajectoriesPanel_ref.transform.Find("SelectorCenter").GetComponent<SelectorToggle>();
                    TwoTrajectoriesPanel_ref.GetComponent<TrajectoriesToggle>().b_PanelIsOpenig = false;
                    TwoTrajectoriesPanel_ref.GetComponent<TrajectoriesToggle>().enabled = true;
                    break;
                case 3:
                    ThreeTrajectoriesPanel_ref.SetActive(true);
                    SelectorToggle_ref = ThreeTrajectoriesPanel_ref.transform.Find("SelectorCenter").GetComponent<SelectorToggle>();
                    TwoTrajectoriesPanel_ref.GetComponent<TrajectoriesToggle>().b_PanelIsOpenig = false;
                    ThreeTrajectoriesPanel_ref.GetComponent<TrajectoriesToggle>().enabled = true;
                    break;
            }  
        }
    }
    
    public void EnableTrajectories()
    {
        ThreeTrajectoriesPanel_ref.GetComponent<TrajectoriesToggle>().enabled = true;
    }

    public void CloseTrajectoriesPanel()
    {
        TrajectoriesPanel_ref.SetActive(false);
        if (TwoTrajectoriesPanel_ref.activeInHierarchy)
        {
            TwoTrajectoriesPanel_ref.SetActive(false);
        }
        if (ThreeTrajectoriesPanel_ref.activeInHierarchy)
        {
            ThreeTrajectoriesPanel_ref.SetActive(false);
        }
    }

    public void ConfirmTrajectory() 
    {
        OnConfirmTrajectory();
    }

    public void OnConfirmTrajectory(bool isSelected = true)
    {
        int trajectoryIndex = 0;
        int itemIndex = -1;

        if (isSelected)
        {
            trajectoryIndex = (int)SelectorToggle_ref.trajectoryType;
            itemIndex = -1; //TODO Sistemare items
        }
         
        CloseTrajectoriesPanel();
        onChooseCoinTrajectory?.Invoke(trajectoryIndex, itemIndex);
    }

    public void ChooseTrajectory(int TrajectoryIndex)
    {
        SelectorToggle_ref.trajectoryType = (TrajectoryType)TrajectoryIndex;
        SelectorToggle_ref.enabled = true;
    }

    void CheckMousePosition()
    {
        RaycastHit HittedObject;
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out HittedObject, 100f , objectsLayermask) && !isOverUI)
        {
            if (HittedObject.collider.gameObject != ActualHittedObject)
            {
                if (ActualHittedObject != null)
                {
                    ActualHittedObject.GetComponent<I_ObjectReaction>().ObjectRemoveHighlight();
                }
                ActualHittedObject = HittedObject.collider.gameObject;
                ActualHittedObject.GetComponent<I_ObjectReaction>().ObjectHighlight();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ActualHittedObject.GetComponent<I_ObjectReaction>().ObjectInteract();
            }
        }
        else
        {
            if (ActualHittedObject != null)
            {
                I_ObjectReaction i_ObjectReaction = null;

                ActualHittedObject.TryGetComponent<I_ObjectReaction>(out i_ObjectReaction);

                if (i_ObjectReaction != null)
                { 
                    ActualHittedObject.GetComponent<I_ObjectReaction>().ObjectRemoveHighlight();
                }
            }
            ActualHittedObject = null;
        }
    }

}
