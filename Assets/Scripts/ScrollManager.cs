using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "SpeedManager", menuName = "Scriptable Objects/SpeedManager")]
public class ScrollManager : MonoBehaviour
{
    public static ScrollManager instance;
    [SerializeField] public float speed = 1.0f;
    [SerializeField] public bool showDebugLines;
    [SerializeField] public bool showLabels;
    [SerializeField] public float startX;
    [SerializeField] public float height;
    [SerializeField] public int segments; 
    [SerializeField] public int segmentDivisions;
    private float audioLength = 28.44f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public float GetSpeed()
    {
        return speed;
    }

    private void OnDrawGizmos()
    {
        if (showDebugLines)
        {
            Vector3 firstPoint = new Vector3(startX, -50, 0);
            float unit = audioLength / segmentDivisions;
            Vector3 increment = new Vector3(unit * speed, 0, 0);
            Vector3 secondPoint = firstPoint;

            for (int r = 0; r < segments; r++)
            {
                for (int i = 0; i <= segmentDivisions; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(secondPoint, secondPoint + Vector3.up * height);
                    firstPoint = secondPoint;
                    if (showLabels) Handles.Label(firstPoint + new Vector3(0, 50, 0), $"Segment {r}\nDivision {i}\nTime: {Math.Round((i * unit * (1+r)), 2)}s");
                    secondPoint = firstPoint + increment;
                }
            }
        }
    }
}
