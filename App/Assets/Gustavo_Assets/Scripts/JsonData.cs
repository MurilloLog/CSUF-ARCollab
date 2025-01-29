using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

[System.Serializable]
public class JsonData
{
    /*** General data for user ***/
    public string command; // Command for server-side processing (e.g., "DRAW", "CLEAR", "UPDATE_PLAYER_POSE")
    public string _id; // Unique identifier for the user provided by the server
    public string roomId; // Unique identifier for the user provided by the server
    public Vector3 position; // Local position of the user
    public Quaternion rotation; // Local rotation of the user
    
    /*** General data for anchor ***/
    public string anchorID; // Unique identifier for the anchor
    public Vector3 anchorPosition; // Position of the anchor
    public Quaternion anchorRotation; // Rotation of the anchor
    
    /*** General data for line ***/
    public List<Vector3> linePoints; // List of points in the line
    public int lineColor; // Color of the line in HEX or RGB format

    /*** Object selection and metadata ***/
    public bool IsSelected; // Indicates if the object is selected by the user
    public string objectMesh; // Optional: name or ID of a mesh associated with the object
    public Vector3 scale;
    
    /*** Constructors ***/
    public JsonData() { linePoints = new List<Vector3>(); }
    
    /*** Methods for serialization ***/
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static JsonData FromJson(string json)
    {
        return JsonUtility.FromJson<JsonData>(json);
    }
    public string getCommand() { return command; }
    public string getID() { return _id; }
    public string getRoomID() { return roomId; }
    public string getAnchorID() { return anchorID; }
    public Vector3 getPosition() { return position; }
    public Quaternion getRotation() { return rotation; }
    public Vector3 getScale() { return scale; }
    public Vector3 getAnchorPosition() { return anchorPosition; }
    public Quaternion getAnchorRotation() { return anchorRotation; }
    public List<Vector3> getLinePoints() { return linePoints; }
    public int getLineColor() { return lineColor; }
    public string getObjectMesh() { return objectMesh; }
    public bool isObjectSelected() { return IsSelected; }
}