using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPose
{
    public string command;
    public string _id;
    public string playerCreator;
    public string playerEditor;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public string objectMesh;
    public bool IsSelected;

    private int previousMovement;
    private int currentMovement;
    private bool firstPose;

    public ObjectPose()
    {
        command = "";
        _id = "";
        playerCreator = "";
        position = new Vector3(0,0,0);
        rotation = new Quaternion(0,0,0,0);
        objectMesh = "";
    }

    public void setCommand(string _command) { command = _command; }
    public void setID(string _objectID) { _id = _objectID; }
    public void setPlayerCreator(string _playerID) { playerCreator = _playerID; }
    public void setPlayerEditor(string _playerEditor) { playerEditor = _playerEditor; }
    public void setPosition(Vector3 _position) { position = _position; }
    public void setRotation(Quaternion _rotation) { rotation = _rotation; }
    public void setScale(Vector3 _scale) { scale = _scale; }
    public void setObjectMesh(string _objectMesh) { objectMesh = _objectMesh; }
    public void setIsSelected(bool _isSelected) { IsSelected = _isSelected; }
    public void setCurrentMovement(int _currentMov) { currentMovement = _currentMov; }
    public void setPreviousMovement() { previousMovement = currentMovement; }

    public int getPreviousMovement () { return previousMovement; }
    public int getCurrentMovement ()  { return currentMovement; }
}