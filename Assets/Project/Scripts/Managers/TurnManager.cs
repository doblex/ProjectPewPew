using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public delegate void OnPlayerChooseCoin(Coin[] coins);
    public delegate void OnPlayerChooseCoinTrajectory(Trajectory[] trajectories,Item[] item);
    public delegate void OnPlayerChooseEnemyTrajectory(Trajectory[] trajectories);

    public OnPlayerChooseCoin onPlayerChooseCoin;
    public OnPlayerChooseCoinTrajectory onPlayerChooseCoinTrajectory;
    public OnPlayerChooseEnemyTrajectory onPlayerChooseEnemyTrajectory;

    [SerializeField] int pointsCap = 25;

    [SerializeField] PG[] players;
    [SerializeField] Coin[] throwableCoins;
    [SerializeField] Item[] items;

    [Header("Debug variables DONT TOUCH")]
    public TurnPhase TurnPhase;
    [SerializeField] int currentActivePlayer = -1;
    [SerializeField] int currentSelectedCoin = -1;

    bool isThrowing = false;
    Trajectory[] validTrajectories;

    private int NextPlayerIndex => (currentActivePlayer + 1) % players.Length;

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
        TrajectoryManager.Instance.onThrowEnded += OnThrowEnded;
        ChooseFirstPlayer();
    }

    private void Update()
    {
        if (currentActivePlayer != -1 && TurnPhase == TurnPhase.NextPlayer)
        {
            isThrowing = true;
            TurnPhase = TurnPhase.ActiveCoinSelection;
            if (players[currentActivePlayer].playerType == PlayerType.PLAYER)
            {
                onPlayerChooseCoin?.Invoke(throwableCoins); //TODO Collegare Parte Grafica
            }
            else
            {
                SetCoin(players[currentActivePlayer].ChooseCoinDifficulty(throwableCoins));
                PrepareThrow();
            }
        }
    }

    public void OnSetCoin(int index)
    {
        SetCoin(index);
        PrepareThrow();
    }


    public void ChooseFirstPlayer()
    {
        //ThrowCoinAnimation
        //currentActivePlayer = Random.Range(0, players.Length);
        currentActivePlayer = 1;
        TurnPhase = TurnPhase.NextPlayer;
    }

    public void NextPlayer()
    {
        TurnPhase = TurnPhase.NextPlayer;
        currentActivePlayer = NextPlayerIndex;
        currentSelectedCoin = -1;
        isThrowing = false;
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
            onPlayerChooseCoinTrajectory?.Invoke(validTrajectories, items); //TODO Collegare Parte Grafica
        }
        else
        {
               int trajectoryIndex = players[NextPlayerIndex].ChooseCoinTrajectory(
                    validTrajectories,
                    out int itemIndex
                    );

            OnSetThrowingTrajectory(trajectoryIndex, itemIndex);
        }
    }

    int gTrajectoryIndex = -1;

    public void OnSetThrowingTrajectory(int trajectoryIndex, int itemIndex = -1)
    {
        gTrajectoryIndex = trajectoryIndex;
        TurnPhase = TurnPhase.ActiveTrajectorySelection;

        if (players[currentActivePlayer].playerType == PlayerType.PLAYER)
        {
            onPlayerChooseEnemyTrajectory?.Invoke(validTrajectories); //TODO Collegare Parte Grafica
        }
        else 
        {
            OnSetShootingTrajectory(players[currentActivePlayer].ChooseEnemyTrajectory(validTrajectories));
        }
    }

    public void OnSetShootingTrajectory(int shootingTrajectoryIndex) 
    {
        if (validTrajectories[shootingTrajectoryIndex] == validTrajectories[gTrajectoryIndex])
        {
            TrajectoryManager.Instance.SpawnCoin(players[currentActivePlayer], players[NextPlayerIndex], validTrajectories[shootingTrajectoryIndex], throwableCoins[currentSelectedCoin]);
        }
        else
        {
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
}
