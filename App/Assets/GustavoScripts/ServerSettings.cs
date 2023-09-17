using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSettings : MonoBehaviour
{
    public static ServerSettings serverSettings;
    public bool isHost; 
    public string IP;
    public int port;
    public string inviteCode;
    private string input;

    void Awake()
    {
        if(serverSettings==null)
        {
            serverSettings=this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(serverSettings!=this)
                Destroy(gameObject);
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadIPInput(string strIP) { IP = strIP; }

    public void ReadPortInput(string strPort) { port = int.Parse(strPort); }

    public void ReadInviteCodeInput(string strIC) { inviteCode = strIC; }

    public string GetIP() { return IP; }
    public int GetPort() { return port; }
    public string GetInviteCode() { return inviteCode; }
}