using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScriptEurope : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(-10f, 0f, 0f) * Time.deltaTime;
    }
}
