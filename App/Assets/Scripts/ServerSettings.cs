using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSettings : MonoBehaviour
{
    public static ServerSettings serverSettings;
    public string IP;
    public int port;
    
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

    public void ReadIPInput(string strIP) { IP = strIP; }

    public void ReadPortInput(string strPort) { port = int.Parse(strPort); }

    public string GetIP() { return IP; }
    public int GetPort() { return port; }
}
