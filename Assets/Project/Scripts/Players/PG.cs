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

    [SerializeField] public PlayerType playerType;
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
    public abstract int ChooseCoinTrajectory(Trajectory[] trajectories, out int itemIndex);
    public abstract int ChooseEnemyTrajectory(Trajectory[] trajectories);
    protected abstract void CheckForThrow();
    protected abstract void CheckForShoot();
    public void Throw() { CanTrow = false; onPlayerThrow?.Invoke(this); }
    public virtual void Shoot() { onPlayerShoot?.Invoke(this); }
}
