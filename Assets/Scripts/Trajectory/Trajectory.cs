using Unity.AI.Navigation;
using UnityEngine;

[ExecuteInEditMode]
public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private Spline _splineVisualization;
    [SerializeField] Vector3 _startPoint, _endPoint;
    [SerializeField, Min(0.01f)]
    private float _heightOffset = 1;
    [SerializeField, Range(0.25f, 0.75f)]
    private float _placementOffset = 0.5f;

# if UNITY_EDITOR

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