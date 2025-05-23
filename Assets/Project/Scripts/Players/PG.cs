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
    private bool canShoot;

    public Trajectory[] Trajectories { get => trajectories;}
    public bool CanTrow { get => canTrow; set => canTrow = value; }
    public bool CanShoot { get => canShoot; set => canShoot = value; }
    public int Points { get => points; }

    private void Update()
    {
        CheckForThrow();
        CheckForShoot();
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
    protected abstract void CheckForShoot();
    protected void Throw() { onPlayerThrow?.Invoke(this); }
    protected void Shoot() { onPlayerShoot?.Invoke(this); }
}
