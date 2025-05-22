using System;
using UnityEngine;

public abstract class PG : MonoBehaviour
{
    public delegate void OnPlayerThrow(PG player);
    public delegate void OnPlayerShoot(PG player);

    public delegate void OnPlayerPointsChanged(PG pg);

    public OnPlayerThrow onPlayerThrow;
    public OnPlayerShoot onPlayerShoot;
    public OnPlayerPointsChanged onPlayerPointsChanged;

    [SerializeField] protected PlayerType playerType;
    [SerializeField] protected Trajectory[] trajectories;
    [SerializeField] int points = 0;

    private bool canTrow;

    public Trajectory[] Trajectories { get => trajectories;}
    public bool CanTrow { get => canTrow; set => canTrow = value; }
    public int Points { get => points; }

    protected void Update()
    {
        CheckForThrow();
    }

    public void AwardPoints(int pointsAwarded)
    {
        points += pointsAwarded;
        onPlayerPointsChanged?.Invoke(this);
    }

    public abstract int ChooseCoinDifficulty(Coin[] coins); //Selezione della difficoltà del round 
    public abstract Trajectory ChooseCoinTrajectory(Trajectory[] trajectories, out Item item);
    public abstract Trajectory ChooseEnemyTrajectory(Trajectory[] trajectories);
    protected abstract void CheckForThrow();
    protected void Throw() { onPlayerThrow?.Invoke(this); }
}
