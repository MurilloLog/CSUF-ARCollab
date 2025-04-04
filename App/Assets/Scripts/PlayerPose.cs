using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerPose
{
    [SerializeField] private string command;
    [SerializeField] private string _id;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;
    
    private string data;
    private int previousMovement;
    private int currentMovement;
    private bool firstPose;
    
    public PlayerPose() { command = ""; _id = ""; position = new Vector3(0,0,0); previousMovement = 0; firstPose = true; }
    
    public void setCommand(string _command) { command = _command; }
    public void setPlayerID(string _playerID) { _id = _playerID; }
    public void setPreviousMovement() { previousMovement = currentMovement; }
    public void setPosition(Vector3 _position) {  position = _position; }
    public void setRotation(Quaternion _rotation) {  rotation = _rotation; }
    public void setFirstPose() { firstPose = false; }

    public int getPreviousMovement() { return previousMovement; }
    public int getCurrentMovement()  { return currentMovement; }
    public bool isFirstPose() { return firstPose; }
    public string getData() { return data; }
    
    public void poseUpdate()
    {
        position = Camera.main.transform.position;
        rotation = Camera.main.transform.rotation;

        currentMovement = (int) Math.Sqrt( Math.Pow((int) position.x, 2) + Math.Pow((int) position.y, 2) + Math.Pow((int) position.z, 2) );
    }
    
    public string ToJson() { return JsonUtility.ToJson(this, true); }
}