using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System;

public class UpdateScore
{
    [SerializeField] private string command;
    [SerializeField] private string _id;
    [SerializeField] private int score;

    public UpdateScore() { command = ""; _id = ""; }
    public void setCommand(string _command) { command = _command; }
    public void setPlayerID(string _playerID) { _id = _playerID; }
    public void setScore(int _score) { score = _score; }    
}