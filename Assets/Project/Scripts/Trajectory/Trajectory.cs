using UnityEngine;

[ExecuteInEditMode]
public class Trajectory : MonoBehaviour
{
    [SerializeField] TrajectoryType type;

    [SerializeField] private Spline _splineVisualization;
    [SerializeField] public Vector3 _startPoint, _endPoint;
    [SerializeField, Min(0.01f)] private float _heightOffset = 1;
    [SerializeField, Range(0.25f, 0.75f)] private float _placementOffset = 0.5f;

    public Spline Spline { get => _splineVisualization;}

    public TrajectoryType Type { get => type; }

    public Vector3 Start { get => transform.TransformPoint(_startPoint); }

#if UNITY_EDITOR

    private void Update()
    {
        if (_splineVisualization != null)
        {
            Vector3 start = transform.TransformPoint(_startPoint);
            Vector3 end = transform.TransformPoint(_endPoint);
            Vector3 midPointPosition = Vector3.Lerp(start, end, _placementOffset);
            midPointPosition.y += _heightOffset;
            _splineVisualization.SetPoints(
                start,
                midPointPosition,
                end);
        }
    }

#endif

}