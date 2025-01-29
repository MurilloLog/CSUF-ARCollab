using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing
{
    // Datos del ancla
    public string command;
    public string _id;
    public string roomId;
    public string anchorID;
    public Vector3 anchorPosition;
    public Quaternion anchorRotation;

    // Datos de la linea
    public List<Vector3> linePoints;
    public int lineColor;

    public Drawing(string playerId, string roomID, string anchorId, Vector3 position, Quaternion rotation, List<Vector3> points, int color)
    {
        command = "DRAWING";
        _id = playerId;
        roomId = roomID;
        anchorID = anchorId;
        anchorPosition = position;
        anchorRotation = rotation;
        linePoints = points;
        lineColor = color;
    }
}
