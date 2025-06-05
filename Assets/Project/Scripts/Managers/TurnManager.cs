using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [SerializeField] Coin OxydizedCoin;
    [SerializeField] DummyPlayer dummyPlayer;
    [SerializeField] float AITimeBetweenActions = 2f;
    [SerializeField] CoinFlipAnimation coinFlipAnimation;
    [SerializeField] bool debugMode = false;

    public delegate void OnPlayerChooseCoin();
    public delegate void OnPlayerChooseCoinTrajectory(int trajectoriesCount);
    public delegate void OnPlayerChooseEnemyTrajectory(int trajectoriesCount);
    public delegate void OnTurnEnd(PlayerType playerType);

    public OnPlayerChooseCoin onPlayerChooseCoin;
    public OnPlayerChooseCoinTrajectory onPlayerChooseCoinTrajectory;
    public OnPlayerChooseEnemyTrajectory onPlayerChooseEnemyTrajectory;
    public OnTurnEnd onTurnEnd;

    [SerializeField] int pointsCap = 25;

    [SerializeField] PG[] players;
    [SerializeField] Coin[] throwableCoins;
    [SerializeField] ItemManager itemManager;

    [Header("Debug variables DONT TOUCH")]
    public TurnPhase TurnPhase;
    [SerializeField] int currentActivePlayer = -1;
    [SerializeField] int currentSelectedCoin = -1;

    bool doublePoints;
    bool isThrowing = false;
    Trajectory[] validTrajectories;

    private int NextPlayerIndex => (currentActivePlayer + 1) % players.Length;

    public PG[] Players  { get => players; }
    public ItemManager ItemManager { get => itemManager;}

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

    private void Start()
    {
        if (dummyPlayer != null)
        {
            dummyPlayer.Setup();
            dummyPlayer.onChooseCoinEnded += OnSetCoin;
            dummyPlayer.onChooseCoinTrajectoryEnded += OnSetThrowingTrajectory;
            dummyPlayer.onChooseShootingTrajectoryEnded += OnSetShootingTrajectory;
        }

        HUD.Instance.onStartGame += OnStartGame;
        HUD.Instance.onChooseCoin += OnSetCoin;
        HUD.Instance.onChooseCoinTrajectory += OnSetThrowingTrajectory;
        HUD.Instance.onChooseEnemyTrajectory += OnSetShootingTrajectory;
        HUD.Instance.onCoinFlipEnd += OnFlippedCoin;

        TrajectoryManager.Instance.onThrowEnded += OnThrowEnded;
    }

    private void Update()
    {
        CheckForNextPlayer();
    }

    private void CheckForNextPlayer()
    {
        if (currentActivePlayer != -1 && TurnPhase == TurnPhase.NextPlayer)
        {
            isThrowing = true;
            TurnPhase = TurnPhase.ActiveCoinSelection;
             //TODO dialogo

            if (players[currentActivePlayer].playerType == PlayerType.PLAYER)
            {
                onPlayerChooseCoin?.Invoke(); //TODO Collegare Parte Grafica
            }
            else
            {
                DialogueManager.Instance.StartDialogue(DialogueType.EnemyDifficultySelectionDialogue, () =>
                {
                    StartCoroutine(Delay(AITimeBetweenActions,
                    () =>
                    {
                        SetCoin(players[currentActivePlayer].ChooseCoinDifficulty(throwableCoins));
                        PrepareThrow();
                    }));
                });
            }
        }
    }

    void OnStartGame() 
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerType == PlayerType.AI)
            {
                ((AI)players[i]).Animator.SetTrigger("trTakeCoins");
                ((AI)players[i]).GetComponent<AnimationController>().onEndGrabAnimation += () =>
                {
                    ChooseFirstPlayer();
                };
                break;
            }
        }
    }

    bool isPlayerFirst;
    public void ChooseFirstPlayer()
    {
        coinFlipAnimation.ThrowCoin(() => {
            currentActivePlayer = Random.Range(0, players.Length);
            isPlayerFirst = players[currentActivePlayer].playerType == PlayerType.PLAYER;
            HUD.Instance.ShowCoinFace(isPlayerFirst);
        });
    }

    public void OnFlippedCoin() 
    {
        if (isPlayerFirst)
        {
            DialogueManager.Instance.StartDialogue(DialogueType.headsDialogue, () =>
            {
                TurnPhase = TurnPhase.NextPlayer;
            });
        }
        else
        {
            DialogueManager.Instance.StartDialogue(DialogueType.tailsDialogue, () =>
            {
                TurnPhase = TurnPhase.NextPlayer;
            });
        }
        
    }

    public void OnSetCoin(int index)
    {

        SetCoin(index);
        PrepareThrow();
    }

    public void NextPlayer()
    {
        if (players[currentActivePlayer].playerType == PlayerType.AI)
        {
            DialogueManager.Instance.StartDialogue(DialogueType.EnemyEndTurnDialogue, () =>
            {
                TurnPhase = TurnPhase.NextPlayer;
                currentActivePlayer = NextPlayerIndex;
                currentSelectedCoin = -1;
                isThrowing = false;

                StartCoroutine(Delay(AITimeBetweenActions,
                           () =>
                           {
                               onTurnEnd?.Invoke(players[currentActivePlayer].playerType);
                           }));
            });
        }
        else 
        {
            TurnPhase = TurnPhase.NextPlayer;
            currentActivePlayer = NextPlayerIndex;
            currentSelectedCoin = -1;
            isThrowing = false;

            StartCoroutine(Delay(AITimeBetweenActions,
                       () =>
                       {
                           onTurnEnd?.Invoke(players[currentActivePlayer].playerType);
                       }));
        }
    }

    public void SetCoin(int index)
    {
        DialogueType dialogueType = DialogueType.easyThrowDialogue;

        switch (throwableCoins[index].Type)
        {
            case CoinType.EASY:
                dialogueType = DialogueType.easyThrowDialogue;
                break;
            case CoinType.MEDIUM:
                dialogueType = DialogueType.mediumThrowDialogue;
                break;
            case CoinType.HARD:
                dialogueType = DialogueType.difficultThrowDialogue;
                break;
        }

        DialogueManager.Instance.StartDialogue(dialogueType, () =>
        {
            currentSelectedCoin = index;
        });
        
    }

    Trajectory[] GetTrajectoriesByCoinDifficulty(Trajectory[] trajectories, CoinType coinType)
    {
        List<Trajectory> coinTrajectory = new List<Trajectory>();


        if (coinType == CoinType.HARD)
        {
            return trajectories;
        }

        foreach (Trajectory trajectory in trajectories)
        {
            switch (coinType)
            {
                case CoinType.EASY:
                    if (trajectory.Type == TrajectoryType.EASY) { coinTrajectory.Add(trajectory); }
                    break;
                case CoinType.MEDIUM:
                    if (trajectory.Type == TrajectoryType.MEDIUM) { coinTrajectory.Add(trajectory); }
                    break;
            }
        }

        return coinTrajectory.ToArray();
    }

    public void PrepareThrow()
    {
        Coin selectedCoin = throwableCoins[currentSelectedCoin];
        validTrajectories = GetTrajectoriesByCoinDifficulty(players[NextPlayerIndex].Trajectories, selectedCoin.Type);

        Debug.Log("Number of trajectories: " + validTrajectories.Length);

        TurnPhase = TurnPhase.PassiveTrajectorySelection;

        if (players[NextPlayerIndex].playerType == PlayerType.PLAYER)
        { 
            onPlayerChooseCoinTrajectory?.Invoke(validTrajectories.Length); //TODO Collegare Parte Grafica
        }
        else
        {
            int trajectoryIndex = 0;
            int itemIndex = -1;

            StartCoroutine(Delay(AITimeBetweenActions,
                   () => {
                       trajectoryIndex = players[NextPlayerIndex].ChooseCoinTrajectory(
                    validTrajectories,
                    out itemIndex
                    );

                       OnSetThrowingTrajectory(trajectoryIndex, itemIndex);
                   }));
        }
    }

    int gTrajectoryIndex = -1;

    public void OnSetThrowingTrajectory(int trajectoryIndex, int itemIndex = -1)
    {
        if (itemIndex == 1)
        {
            currentSelectedCoin = -1;
        }

        gTrajectoryIndex = trajectoryIndex;
        TurnPhase = TurnPhase.ActiveTrajectorySelection;

        if (players[currentActivePlayer].playerType == PlayerType.PLAYER)
        {
            onPlayerChooseEnemyTrajectory?.Invoke(validTrajectories.Length); //TODO Collegare Parte Grafica
        }
        else 
        {
            StartCoroutine(Delay(AITimeBetweenActions,
            () => {
                OnSetShootingTrajectory(players[currentActivePlayer].ChooseEnemyTrajectory(validTrajectories));
                }));
        }
    }

    public void OnSetShootingTrajectory(int shootingTrajectoryIndex, int itemIndex = -1) 
    {
        if (itemIndex == 0)
        {
            doublePoints = true;
        }

        if (validTrajectories[shootingTrajectoryIndex] == validTrajectories[gTrajectoryIndex])
        {
            HUD.Instance.ShowCommand(players[currentActivePlayer].playerType == PlayerType.PLAYER ? CommandType.shoot : CommandType.flip);
            if (currentSelectedCoin >= 0)
                TrajectoryManager.Instance.SpawnCoin(players[currentActivePlayer], players[NextPlayerIndex], validTrajectories[shootingTrajectoryIndex], throwableCoins[currentSelectedCoin]);
            else
            {
                TrajectoryManager.Instance.SpawnCoin(players[currentActivePlayer], players[NextPlayerIndex], validTrajectories[shootingTrajectoryIndex], OxydizedCoin);
            }
        }
        else
        {
            Debug.Log("Failed Trajectory");
            OnThrowEnded(false);
        }
    }

    private void OnThrowEnded(bool hit)
    {
        Debug.Log("is Coin Hit? " + hit);

        HUD.Instance.ShowCommand();

        if (hit)
        {
            DialogueManager.Instance.StartDialogue(DialogueType.failedShootDialogue, () =>
            {
                TurnPhase = TurnPhase.ActivePointAssign;

                int pointsToAward = doublePoints ? throwableCoins[currentSelectedCoin].Points * 2 : throwableCoins[currentSelectedCoin].Points;
                doublePoints = false;

                if (hit)
                    players[currentActivePlayer].AwardPoints(pointsToAward);

                CheckForWinCondition();

                NextPlayer();
            });
        }
        else
        {
            DialogueManager.Instance.StartDialogue(DialogueType.succesShootDialogue, () =>
            {
                TurnPhase = TurnPhase.ActivePointAssign;

                int pointsToAward = doublePoints ? throwableCoins[currentSelectedCoin].Points * 2 : throwableCoins[currentSelectedCoin].Points;
                doublePoints = false;

                if (hit)
                    players[currentActivePlayer].AwardPoints(pointsToAward);

                CheckForWinCondition();

                NextPlayer();
            });
        }
    }

    private void CheckForWinCondition()
    {
        TurnPhase = TurnPhase.VictoryChecks;
        if (players[currentActivePlayer].Points >= pointsCap)
        {
            if (players[currentActivePlayer].playerType == PlayerType.PLAYER)
            {
                DialogueManager.Instance.StartDialogue(DialogueType.playerWinDialogue, () =>
                {
                    HUD.Instance.ResetScene();
                });
            }
            else
            {
                DialogueManager.Instance.StartDialogue(DialogueType.playerLoseDialogue, () =>
                {
                    HUD.Instance.ResetScene();
                });
            }
        }
    }

    private void OnGUI()
    {
        if (debugMode)
        {
            GUIStyle coloredStyle = new GUIStyle(EditorStyles.label);
            coloredStyle.normal.textColor = Color.black;
            coloredStyle.fontSize = 30;

            EditorGUILayout.LabelField(TurnPhase.ToString(), coloredStyle);
            EditorGUILayout.LabelField(currentActivePlayer.ToString(), coloredStyle);


            //foreach (var player in players)
            //{
            //    EditorGUILayout.LabelField(player.playerType.ToString(),player.Points.ToString(), coloredStyle);
            //}
        }
    }

    public IEnumerator Delay(float delay, Action action) 
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
