using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField] float speed = 4f;

   private TouchControls controls;
   private Coroutine zoomCoroutine;
    private Transform camTransform;
   private void Awake()
   {
       controls = new TouchControls();
        camTransform = Camera.main.transform;
   }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Touch.SecondaryTouchContact.started += _ => StartZoom();
        controls.Touch.SecondaryTouchContact.canceled += _ => EndZoom();
    }

    void StartZoom()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    void EndZoom()
    {
        StopCoroutine(zoomCoroutine);
    }


    IEnumerator ZoomDetection()
    {
        float prevDist = 0.0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                controls.Touch.SecondaryFingerPostion.ReadValue<Vector2>());

            if(distance > prevDist )
            {
                //zoom out
                Vector3 targetPos = camTransform.position;
                targetPos.z -= 1;
                camTransform.position = Vector3.Slerp(camTransform.position, targetPos, Time.deltaTime * speed);

            }
            else if(distance < prevDist)
            {
                //zoom in
                Vector3 targetPos = camTransform.position;
                targetPos.z += 1;
                camTransform.position = Vector3.Slerp(camTransform.position, targetPos, Time.deltaTime * speed);

            }

            prevDist = distance;
            yield return null;
        }
    }
}
