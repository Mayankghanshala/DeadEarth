using System.Collections.Generic;
using UnityEngine;
public enum DisplayMode
{
    None,
    Connections,
    Paths
}
public class AIWayPointNetwork : MonoBehaviour
{
    [HideInInspector]
    public DisplayMode DisplayMode = DisplayMode.Connections;
    [HideInInspector]
    public int UIStart = 0;
    [HideInInspector]
    public int UIEnd = 0;
    public List<Transform> WayPoints;
}
