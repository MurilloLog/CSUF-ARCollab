using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frameMovement : MonoBehaviour
{
    public Events backendEvents;
    float smooth = 10.0f;

    void Start()
    {
        backendEvents = FindObjectOfType<Events>();
    }

    void FixedUpdate()
    {
        if(backendEvents.updatingPlayerPose)
        {
            transform.position = Vector3.Slerp(transform.position, backendEvents.JSONPackageReceived.getPosition(), Time.deltaTime * smooth);
            transform.rotation = Quaternion.Slerp(transform.rotation, backendEvents.JSONPackageReceived.getRotation(),  Time.deltaTime * smooth);
            backendEvents.updatingPlayerPose = false;
        }      
    }

    void Update()
    {
        if(backendEvents.conectionStatus.playerIsAlone)
        {
            backendEvents.playerFrame.SetActive(false);
            Destroy(backendEvents.playerFrame);
        }
    }
}
