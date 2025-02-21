using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScriptNA : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0f, 0f, 30f) * Time.deltaTime;
        
    }
}
