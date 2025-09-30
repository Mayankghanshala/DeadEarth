using System.Collections;
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
    [SerializeField] private bool _IsOnOffMeshLink;
    [SerializeField] private NavMeshPathStatus _pathStatus;
    [SerializeField] private AnimationCurve _animationCurve;
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
        _IsOnOffMeshLink = _navMeshAgent.isOnOffMeshLink;

        if (_navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(UpdateAgentOnOffMesh(1f));
            return;
        }

        if ((_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_pathPending) || _pathStatus == NavMeshPathStatus.PathInvalid)
        {
            SetDestination(true);
        }
        else if (_isPathStale)
            SetDestination(false);
    }

    private IEnumerator UpdateAgentOnOffMesh(float timeToComplete)
    {
        OffMeshLinkData offMeshLinkData = _navMeshAgent.currentOffMeshLinkData;
        Vector3 startingPosition = _navMeshAgent.transform.position;
        Vector3 endingPosition = offMeshLinkData.endPos + _navMeshAgent.baseOffset * Vector3.up;
        float currentTime = 0f;
        while (currentTime <= timeToComplete)
        {
            float t = currentTime / timeToComplete;
            _navMeshAgent.transform.position = Vector3.Lerp(startingPosition, endingPosition, t) + _animationCurve.Evaluate(t) * Vector3.up;
            currentTime += Time.deltaTime;
            yield return null;
        }
        _navMeshAgent.CompleteOffMeshLink();
    }
}
