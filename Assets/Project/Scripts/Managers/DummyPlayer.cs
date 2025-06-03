using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    public delegate void OnChooseCoinEnded(int index);
    public delegate void OnChooseCoinTrajectoryEnded(int trajectoryIndex, int itemIndex = -1);
    public delegate void OnChooseShootingTrajectoryEnded(int shootingTrajectoryIndex, int itemIndex = -1);

    public OnChooseCoinEnded onChooseCoinEnded;
    public OnChooseCoinTrajectoryEnded onChooseCoinTrajectoryEnded;
    public OnChooseShootingTrajectoryEnded onChooseShootingTrajectoryEnded;


    public void Setup() 
    {
        TurnManager.Instance.onPlayerChooseCoin += OnPlayerChooseCoin;
        TurnManager.Instance.onPlayerChooseCoinTrajectory += OnPlayerChooseCoinTrajectory;
        TurnManager.Instance.onPlayerChooseEnemyTrajectory += OnPlayerChooseEnemyTrajectory;
    }

    private void OnPlayerChooseEnemyTrajectory(Trajectory[] trajectories, Item[] item)
    {
        int trajectoryIndex = 0;
        int itemIndex = -1;

        onChooseShootingTrajectoryEnded?.Invoke(trajectoryIndex, itemIndex);
    }

    private void OnPlayerChooseEnemyTrajectory(Trajectory[] trajectories)
    {

    }

    private void OnPlayerChooseCoinTrajectory(Trajectory[] trajectories, Item[] item)
    {
        int trajectoriesIndex = 0;
        int itemIndex = -1;

        onChooseCoinTrajectoryEnded?.Invoke(trajectoriesIndex, itemIndex);
    }

    private void OnPlayerChooseCoin(Coin[] coins)
    {
        int index = 0;
        onChooseCoinEnded?.Invoke(index);
    }
}
