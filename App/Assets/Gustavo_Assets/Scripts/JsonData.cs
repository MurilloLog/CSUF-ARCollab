using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

//[System.Serializable]
public class JsonData
{
    public string command;
    public string _id;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    
    public string objectMesh;
    public bool IsSelected;
    
    public JsonData() { }
    public string getCommand() { return command; }
    public string getID() { return _id; }
    public Vector3 getPosition() { return position; }
    public Quaternion getRotation() { return rotation; }
    public Vector3 getScale() { return scale; }
    public string getObjectMesh() { return objectMesh; }
    public bool isObjectSelected() { return IsSelected; }
}