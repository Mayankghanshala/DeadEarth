using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentExample : MonoBehaviour
{
    [SerializeField] private AIWayPointNetwork _aIWayPointNetwork;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private int _currentIndex;
    [SerializeField] private bool _hasPath;
    [SerializeField] private bool _pathPending;
    [SerializeField] private bool _isPathStale;
    [SerializeField] private NavMeshPathStatus _pathStatus;
    private void Start()
    {
        //_navMeshAgent.updatePosition = false;
        //_navMeshAgent.updateRotation = false;
        SetDestination(false);
    }

    void SetDestination(bool shouldIncrement)
    {
        if (_aIWayPointNetwork == null || _aIWayPointNetwork.WayPoints == null)
            return;
        int incStep = shouldIncrement ? 1 : 0;
        int targetIndex = _currentIndex + incStep >= _aIWayPointNetwork.WayPoints.Count ? 0 : _currentIndex + incStep;
        Transform targetTransform = _aIWayPointNetwork.WayPoints[targetIndex];
        if (targetTransform != null)
        {
            _navMeshAgent.destination = targetTransform.position;
            _currentIndex = targetIndex;
            return;
        }
        _currentIndex++;

    }

    private void Update()
    {
        _hasPath = _navMeshAgent.hasPath;
        _pathPending = _navMeshAgent.pathPending;
        _isPathStale = _navMeshAgent.isPathStale;
        _pathStatus = _navMeshAgent.pathStatus;

        if ((!_hasPath && !_pathPending) || _pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetDestination(true);
        }
        else if (_isPathStale)
            SetDestination(false);
    }
}
