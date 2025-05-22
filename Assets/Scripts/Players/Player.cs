using UnityEngine;

public class Player : PG
{
    [SerializeField] KeyCode ThrowKey;

    public override int ChooseCoinDifficulty(Coin[] coins)
    {
        int index = -1;
        //TODO Parte Grafica Monete
        return index;
    }

    public override Trajectory ChooseCoinTrajectory(Trajectory[] trajectories, out Item item)
    {
        int index = -1;
        item = null;
        //TODO Parte Grafica sia Item che traiettoria
        return trajectories[index];
    }

    public override Trajectory ChooseEnemyTrajectory(Trajectory[] trajectories)
    {
        int index = -1;
        //TODO Parte Grafica Scelta traiettoria nemica
        return trajectories[index];
    }

    protected override void CheckForThrow()
    {
        if (Input.GetKeyDown(ThrowKey) && CanTrow)
        {
            Throw();
        }
    }
}
