using UnityEngine;

public class SelectorToggle : MonoBehaviour
{
    [SerializeField] RectTransform TrajectorySelector_ref;
    [SerializeField] Vector2 TrajectoryOnePosition_ref;
    [SerializeField] Vector2 TrajectoryTwoPosition_ref;
    [SerializeField] Vector2 TrajectoryThreePosition_ref;

    public TrajectoryType trajectoryType;

    Vector2 StartPosition;
    Vector2 DestinationPosition;

    [SerializeField] float MovingTime;
    float timer;
    private void OnEnable()
    {
        StartPosition = TrajectorySelector_ref.anchoredPosition;
        timer = 0;
        switch (trajectoryType)
        {
            case TrajectoryType.EASY:
                DestinationPosition = TrajectoryOnePosition_ref;
                break;
            case TrajectoryType.MEDIUM:
                DestinationPosition = TrajectoryTwoPosition_ref;
                break;
            case TrajectoryType.HARD:
                DestinationPosition = TrajectoryThreePosition_ref;
                break;
        }
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (TrajectorySelector_ref.anchoredPosition != DestinationPosition)
        {
            Debug.LogError("Sposto il selettore " + TrajectorySelector_ref);
            Debug.LogError("Attualmente nella posizione " + TrajectorySelector_ref.anchoredPosition);
            TrajectorySelector_ref.anchoredPosition = Vector2.Lerp(StartPosition, DestinationPosition, timer / MovingTime);
        }
        else
        {
            Debug.LogError("Disattivo lo script");
            this.enabled = false;
        }
    }
}
