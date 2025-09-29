using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(AIWayPointNetwork))]
public class AIWayPointNetworkEditor : Editor
{

    public override void OnInspectorGUI()
    {
        AIWayPointNetwork network = (AIWayPointNetwork)target;
        network.DisplayMode = (DisplayMode)EditorGUILayout.EnumPopup("Display Mode",network.DisplayMode);
        network.UIStart = (int)EditorGUILayout.IntSlider("UIStart", network.UIStart, 0,network.WayPoints.Count-1);
        network.UIEnd = (int)EditorGUILayout.IntSlider("UIStart", network.UIEnd, 0, network.WayPoints.Count - 1);
        DrawDefaultInspector();
    }
    private void OnSceneGUI()
    {
        AIWayPointNetwork network = (AIWayPointNetwork)target;
        for (int i = 0; i < network.WayPoints.Count; ++i)
        {
            if (network.WayPoints[i] != null)
            {
                Handles.Label(network.WayPoints[i].position,$"WayPoint {i+1}");
            }
        }

        if (network.DisplayMode == DisplayMode.Connections)
        {
            Vector3[] linePositions = new Vector3[network.WayPoints.Count+1];
            for (int i = 0; i <= network.WayPoints.Count; ++i)
            {
                int index = i != network.WayPoints.Count ? i : 0;
                linePositions[i] = network.WayPoints[index] != null ? network.WayPoints[index].position : new Vector3(Mathf.Infinity,Mathf.Infinity);
            }
            Handles.color = Color.cyan;
            Handles.DrawPolyLine(linePositions);
        }
        else if(network.DisplayMode == DisplayMode.Paths)
        {
            if (network.WayPoints[network.UIStart] != null && network.WayPoints[network.UIEnd] != null)
            {
                NavMeshPath path = new NavMeshPath();
                Vector3 from = network.WayPoints[network.UIStart].position;
                Vector3 to = network.WayPoints[network.UIEnd].position;
                NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
                Handles.color = Color.yellow;
                Handles.DrawPolyLine(path.corners);
            }
        }
    }
}
