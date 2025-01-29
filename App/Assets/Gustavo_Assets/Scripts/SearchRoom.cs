using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class SearchRoom
{
    [SerializeField] private string command;
    [SerializeField] private string _id;
    
    public SearchRoom() { command = ""; _id = ""; }
    public void setCommand(string _command) { command = _command; }
    public void setPlayerID(string _playerID) { _id = _playerID; }
    public string ToJson() { return JsonUtility.ToJson(this, true); }
}