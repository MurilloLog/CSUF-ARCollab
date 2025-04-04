using UnityEngine;
using System;

public class PlacementObject : MonoBehaviour
{
    public string command;
    public string _id;
    public Vector3 position;
    public Quaternion rotation;
    public string objectMesh;
    public bool IsSelected;
    public bool IsCreated;
    public bool IsDeleted;
    public GameObject prefab;

    private bool IsNativeObject;
    
    public Events backendEvents;

    public bool Selected { get { return this.IsSelected;    } set { IsSelected = value;    }}
    public bool Created  { get { return this.IsCreated;     } set { IsCreated = value;     }}
    public bool Deleted  { get { return this.IsDeleted;     } set { IsDeleted = value;     }}
    public bool Native   { get { return this.IsNativeObject;} set { IsNativeObject = value;}}

    /* APP's FUNCTION LIST */
    /* To select the AR object */
    public void setIsSelected()
    {
        string[] subString;
        objectMesh = gameObject.GetComponent<MeshFilter>().mesh.ToString();
        subString = objectMesh.Split(' ');
        objectMesh = subString[0];
        IsSelected = true;
        backendEvents.objectPose.setCommand("UPDATE_OBJECT_POSE");
        backendEvents.objectPose.setID(gameObject.name);
        backendEvents.objectPose.setPlayerCreator(backendEvents.id);
        backendEvents.objectPose.setPlayerEditor(backendEvents.id);
        backendEvents.objectPose.setObjectMesh(objectMesh);
        backendEvents.objectPose.setIsSelected(IsSelected);
        backendEvents.JSONPackage = JsonUtility.ToJson(backendEvents.objectPose, true);
        backendEvents.sendAction(backendEvents.JSONPackage);
    }
    
    /* To unselect the AR object */
    public void setIsNotSelected()
    {
        IsSelected = false;
        backendEvents.objectPose.setCommand("UPDATE_OBJECT_POSE");
        backendEvents.objectPose.setID(gameObject.name);
        backendEvents.objectPose.setIsSelected(IsSelected);
        backendEvents.JSONPackage = JsonUtility.ToJson(backendEvents.objectPose, true);
        backendEvents.sendAction(backendEvents.JSONPackage);
    }

    void Awake()
    {
        backendEvents = FindObjectOfType<Events>();
    }

    /* To create an AR object */
    public void isCreated()
    {
        IsCreated = true;
        string[] subString;
        objectMesh = gameObject.GetComponent<MeshFilter>().mesh.ToString();
        subString = objectMesh.Split(' ');
        objectMesh = subString[0];
        Debug.Log("Mensaje de creación de objeto");
        
		//Debug.Log("La malla de este objeto es: " + objectMesh);
        //Debug.Log("Objeto creado con pose:");
        //Debug.Log("Posicion: " + prefab.transform.position);
        //Debug.Log("Rotacion: " + prefab.transform.rotation);
    }

    public void setIsDeleted()
    {
        IsDeleted = true;
        // Modificar referencia a eliminar
        Destroy(prefab.gameObject);
    }

    void FixedUpdate()
    {
        if(IsSelected)
        {
            backendEvents.objectPose.setPosition(prefab.transform.position);
            backendEvents.objectPose.setRotation(prefab.transform.rotation);
            backendEvents.objectPose.setScale(prefab.transform.localScale);

            backendEvents.objectPose.setCurrentMovement( (int) Math.Sqrt( Math.Pow(backendEvents.objectPose.position.x, 2) + Math.Pow(backendEvents.objectPose.position.y, 2) + Math.Pow(backendEvents.objectPose.position.z, 2) ) );
            
            if(backendEvents.objectPose.getCurrentMovement() != backendEvents.objectPose.getPreviousMovement() )
            {
                backendEvents.JSONPackage = JsonUtility.ToJson(backendEvents.objectPose, true);
                backendEvents.sendAction(backendEvents.JSONPackage);

                backendEvents.objectPose.setPreviousMovement();
                Debug.Log("Object Update Successfuly");
            }
            //Debug.Log("Posicion: " + position);
            //Debug.Log("Rotacion: " + rotation);
            //networking.objectPose.objectPosition = position;
            //networking.objectPose.objectRotation = rotation;
            //networking.sendAction("OBJECT_POSE_UPDATE");
        }
    }
}