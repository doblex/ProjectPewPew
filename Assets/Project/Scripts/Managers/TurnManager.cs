using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [SerializeField] int pointsCap = 25;

    [SerializeField] PG[] players;
    [SerializeField] Coin[] throwableCoins;

    [Header("Debug variables DONT TOUCH")]
    public TurnPhase TurnPhase;
    [SerializeField] int currentActivePlayer = -1;
    [SerializeField] int currentSelectedCoin = -1;

    bool isThrowing = false;

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
            SetCoin(players[currentActivePlayer].ChooseCoinDifficulty(throwableCoins));
            PrepareThrow();
        }
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
        Trajectory[] validTrajectories = GetTrajectoriesByCoinDifficulty(players[NextPlayerIndex].Trajectories, selectedCoin.Type);

        Debug.Log("Number of trajectories: " + validTrajectories.Length);

        TurnPhase = TurnPhase.PassiveTrajectorySelection;
        Trajectory selectedTrajectory = 
            players[NextPlayerIndex].ChooseCoinTrajectory(
                validTrajectories,
                out Item itemUsed
                );

        TurnPhase = TurnPhase.ActiveTrajectorySelection;
        if (players[currentActivePlayer].ChooseEnemyTrajectory(validTrajectories) == selectedTrajectory)
        {
            TrajectoryManager.Instance.SpawnCoin(players[currentActivePlayer], players[NextPlayerIndex], selectedTrajectory, selectedCoin);
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
