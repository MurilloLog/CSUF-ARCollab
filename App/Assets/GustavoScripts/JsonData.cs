using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

[System.Serializable]
public class JsonData
{
    public string command;
    public string _id;
    public string objectAttribute1;
    public bool objectAttribute2;
    public Vector3 objectAttribute3;
    public int collabScoreNum;
    
    public JsonData() { }
    public string getCommand() { return command; }
    public string getID() { return _id; }
    public int getCollabScoreNum() {return collabScoreNum; }
    public string getObjectAttribute1() { return objectAttribute1; }
    public bool getObjectAttribute2() { return objectAttribute2; }
    public Vector3 getObjectAttribute3() { return objectAttribute3; }
}