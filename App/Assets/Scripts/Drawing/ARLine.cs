using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

/// <summary>
/// The ARLine class manages the creation and manipulation of 3D lines in an AR environment.
/// It uses a LineRenderer to render the line and provides methods for adding points dynamically.
/// This class is suitable for drawing trajectories, shapes, or paths in AR scenes.
/// </summary>
/// <remarks>
/// - The class integrates with Unity's AR Foundation.
/// - It uses configurable settings provided by a LineSettings object.
/// - Lines are created as child objects of a parent or an ARAnchor.
/// - This script was moved from Many Scripts >> Old AR Scripts folder
/// </remarks>

public class ARLine
{
    private int positionCount = 0;
    private Vector3 prevPointDistance = Vector3.zero;
    private LineRenderer lineRenderer { get; set; }
    private LineSettings settings;
    private int currentColor;

    public ARLine(LineSettings settings)
    {
        this.settings = settings;
    }

    public void AddPoint(Vector3 position)
    {
        if (prevPointDistance == null)
        {
            prevPointDistance = position;
        }
        if (prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, position)) >= settings.minDistanceBeforeNewPoint)
        {
            prevPointDistance = position;
            positionCount++;

            lineRenderer.positionCount = positionCount;

            //index 0 position count must be -1
            lineRenderer.SetPosition(positionCount - 1, position);

            //applies simplification if reminder is 0
            if (lineRenderer.positionCount % settings.applySimplifyAfterPoints == 0 && settings.allowSimplification)
            {
                lineRenderer.Simplify(settings.tolerance);
            }
        }
    }

    public void AddNewLineRenderer(Transform parent, ARAnchor anchor, Vector3 position)
    {
        positionCount = 2;
        GameObject go = new GameObject($"LineRenderer");

        go.transform.parent = anchor?.transform ?? parent;
        go.transform.position = position;
        go.tag = "AR Object";
        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
        goLineRenderer.startWidth = settings.startWidth;
        goLineRenderer.endWidth = settings.endWidth;
        goLineRenderer.material = settings.defaultColorMaterial;
        goLineRenderer.useWorldSpace = true;
        goLineRenderer.positionCount = positionCount;
        goLineRenderer.numCornerVertices = settings.cornerVertices;
        goLineRenderer.numCapVertices = settings.endCapVertices;
        goLineRenderer.SetPosition(0, position);
        goLineRenderer.SetPosition(1, position);

        lineRenderer = goLineRenderer;
    }

    public List<Vector3> GetPoints()
    {
        if (lineRenderer == null || lineRenderer.positionCount == 0)
        {
            return new List<Vector3>(); // Retorna un array vacio si no hay puntos
        }

        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            points.Add(lineRenderer.GetPosition(i));
        }

        return points;
    }

    public int GetCurrentColor() { return settings.GetLineColor(); }
}