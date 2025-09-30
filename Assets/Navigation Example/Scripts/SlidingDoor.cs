using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [SerializeField] AnimationCurve _slidingAnimation;
    [SerializeField] float _slidingTime = 1.5f;
    [SerializeField] float _slidingDistance = 4f;
    private Vector3 mClosePosition;
    private Vector3 mOpenPosition;
    private DoorState mDoorState;
    // Start is called before the first frame update
    void Start()
    {
        mClosePosition = transform.position;
        mOpenPosition = mClosePosition + _slidingDistance * Vector3.left;
        mDoorState = DoorState.Close;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && mDoorState != DoorState.Animating)
        {
            StartCoroutine(SlideDoor(mDoorState==DoorState.Open?DoorState.Close:DoorState.Open));
        }
    }

    IEnumerator SlideDoor(DoorState newState)
    {
        Vector3 startPosition = mDoorState == DoorState.Close ? mClosePosition : mOpenPosition;
        Vector3 endPosition = mDoorState == DoorState.Close ? mOpenPosition : mClosePosition;
        float currentTime = 0f;
        mDoorState = DoorState.Animating;
        while (currentTime <= _slidingTime)
        {
            float t = currentTime / _slidingTime;
            transform.position = Vector3.Lerp(startPosition,endPosition,_slidingAnimation.Evaluate(t));
            currentTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
        mDoorState = newState;
    }
}

public enum DoorState
{
    Open, Close, Animating
}
