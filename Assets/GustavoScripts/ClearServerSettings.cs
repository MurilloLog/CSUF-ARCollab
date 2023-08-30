using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearServerSettings : MonoBehaviour
{
    public GameObject serverSettings;
    
    // Start is called before the first frame update
    void Start()
    {
        serverSettings = GameObject.Find("ServerSettings");
        if(serverSettings!=null)
            Destroy(serverSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
