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

    public delegate void OnPlayerChooseCoin();
    public delegate void OnPlayerChooseCoinTrajectory(int trajectoriesCount, Item[] item);
    public delegate void OnPlayerChooseEnemyTrajectory(int trajectoriesCount, Item[] item);
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

    bool isThrowing = false;
    Trajectory[] validTrajectories;

    private int NextPlayerIndex => (currentActivePlayer + 1) % players.Length;

    public PG[] Players  { get => players; }

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
                StartCoroutine(Delay(AITimeBetweenActions,
                    () =>
                    {
                        SetCoin(players[currentActivePlayer].ChooseCoinDifficulty(throwableCoins));
                        PrepareThrow();
                    }));
            }
        }
    }

    void OnStartGame() 
    {
        ChooseFirstPlayer();
    }

    public void OnSetCoin(int index)
    {
        SetCoin(index);
        PrepareThrow();
    }

    public void ChooseFirstPlayer()
    {
        //ThrowCoinAnimation

        currentActivePlayer = Random.Range(0, players.Length);
        TurnPhase = TurnPhase.NextPlayer;
    }

    public void NextPlayer()
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

    public void SetCoin(int index)
    {
        currentSelectedCoin = index;
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
            onPlayerChooseCoinTrajectory?.Invoke(validTrajectories.Length, itemManager.GetAllItems()); //TODO Collegare Parte Grafica
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
            onPlayerChooseEnemyTrajectory?.Invoke(validTrajectories.Length, itemManager.GetAllItems()); //TODO Collegare Parte Grafica
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
        if (validTrajectories[shootingTrajectoryIndex] == validTrajectories[gTrajectoryIndex])
        {
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

        TurnPhase = TurnPhase.ActivePointAssign;
        if(hit)
            players[currentActivePlayer].AwardPoints(throwableCoins[currentSelectedCoin].Points);

        CheckForWinCondition();

        NextPlayer();
    }

    private void CheckForWinCondition()
    {
        TurnPhase = TurnPhase.VictoryChecks;
        if (players[currentActivePlayer].Points >= pointsCap)
        {
            Debug.Log("SOMEONE WINs!!!!!");
        }
    }

    private void OnGUI()
    {
        GUIStyle coloredStyle = new GUIStyle(EditorStyles.label);
        coloredStyle.normal.textColor = Color.black;

        EditorGUILayout.LabelField(TurnPhase.ToString(), coloredStyle);
        EditorGUILayout.LabelField(currentActivePlayer.ToString(), coloredStyle);


        foreach (var player in players)
        {
            EditorGUILayout.LabelField(player.playerType.GetType().ToString(),player.Points.ToString(), coloredStyle);
        }
    }

    public IEnumerator Delay(float delay, Action action) 
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
        
    }
}
