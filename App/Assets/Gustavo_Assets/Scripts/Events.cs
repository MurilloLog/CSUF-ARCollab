/* 
    *********************************************************
    In this script is defined all backend Network functions 
        Install-Package System.Json -Version 4.0.20126.16343
    *********************************************************
*/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;

public class Events : MonoBehaviour
{
    public Networking networkBehaviour;
    //public CollabScreenManager buttonManagerMod;
    public ServerSettings serverSettings;
    public ConectionStatus conectionStatus;
    
    public string id = "";  // _id assigned by the server
    public string roomId = ""; // roomId assigned by the server
    public bool readingFromServer = false;
    public bool writingToServer = false;
    public bool searchingRoom = false;
    public bool searchingPlayer = false;
    public bool paired = false;
    public bool error = false;
    public bool updatingPlayerPose = false;
    public bool updatingObjectPose = false;
    public bool planeDetected = false;
    public bool syncronized = false;
    public bool drawing = false;
    
    public SearchRoom searchRoom = new SearchRoom();
    public PlayerPose playerPose = new PlayerPose();
    public ObjectPose objectPose = new ObjectPose();
    public GameObject playerFrame;
    public GameObject objectPrefab;

    public string JSONPackage = "";
    public JsonData JSONPackageReceived = new JsonData();

    //private NTPClient ntpClient;

    void Start()
    {
        //ntpClient = new NTPClient();
    }

    void Awake()
    {
        networkBehaviour = FindObjectOfType<Networking>();
        conectionStatus = FindObjectOfType<ConectionStatus>();

        // Check if ServerSettings gameobject exists
        GameObject serverSettingsObject = GameObject.Find("ServerSettings");
        if (serverSettingsObject != null)
        {
            //serverSettings = GameObject.Find("ServerSettings").GetComponent<ServerSettings>();
            serverSettings = serverSettingsObject.GetComponent<ServerSettings>();
            if (serverSettings != null)
            {
                networkBehaviour.IP = serverSettings.GetIP();  // Local server IP address
                networkBehaviour.PORT = serverSettings.GetPort(); // Local server PORT
            }
            else
            {
                Debug.LogError("'ServerSettings' has no attributes.");
            }
        }
        else
        {
            // Default data
            networkBehaviour.IP = "192.168.0.127"; // Default local server IP address
            networkBehaviour.PORT = 8080; // Default local server PORT
        }
        
        //conectionStatus = FindObjectOfType<ConectionStatus>();
        conectionStatus.playerIsAlone = false;
        conectionStatus.playerIsWaiting = true;
    }

    public void changePlaneStatus() { planeDetected = !planeDetected; }
    public void setSync() { syncronized = !syncronized; }

    // Receive a command from server and do ...
    public void readAction(string JsonFromServer)
    {
        // T4: Timestamp en el que se recibe un JSON desde el servidor
        long T4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //long T4 = ntpClient.GetNetworkTime();

        Debug.Log("The message from server is: " + JsonFromServer);
        if(JsonFromServer.StartsWith("id:"))
        {
            id = JsonFromServer.Replace("id: ", "");
            Debug.Log("Player ID from server received");
            Debug.Log("My player ID is: " + id);
            searchingRoom = true;
            searchRoom.setCommand("SEARCH_ROOM");
            searchRoom.setPlayerID(id);
            JSONPackage = searchRoom.ToJson() + "|";
            //JSONPackage = JsonUtility.ToJson(searchRoom, true);
            Debug.Log("My Json sent: " + JSONPackage);
            sendRoomAction(JSONPackage);
            JSONPackage = "{}";
        }
        else
        {
            // Command deserialization
            JSONPackageReceived = JsonUtility.FromJson<JsonData>(JsonFromServer);
            switch (JSONPackageReceived.getCommand())
            {
                case "WAITING_PLAYER":
                    searchingPlayer = true;
                    paired = false;
                    conectionStatus.playerIsWaiting = true;
                    Debug.Log("Waiting player...");
                break;

                case "ROOM_CREATED":
                    roomId = JSONPackageReceived.getRoomID();
                    paired = true;
                    searchingPlayer = false;
                    searchingRoom = false;
                    conectionStatus.playerIsWaiting = false;
                    Debug.Log("Room created and players paired...");
                    //buttonManagerMod.MultiplayerGame();
                break;

                case "DRAWING":
                    drawing = true;
                    ARDrawManager.Instance.DeserializeAndAddAnchor(JsonFromServer, T4);
                    Debug.Log("Other player has drown...");
                break;

                case "UPDATE_PLAYER_POSE":
                    //Debug.Log("Position: " + JSONPackageReceived.getPosition());
                    //Debug.Log("Rotation: " + JSONPackageReceived.getRotation());
                    updatingPlayerPose = true;
                    playerPose.setCommand(JSONPackageReceived.getCommand());
                    playerPose.setPlayerID(JSONPackageReceived.getID());
                    Debug.Log("Other player update its pose:");
                break;

                case "UPDATE_OBJECT_POSE":
                    updatingObjectPose = true;
                    Debug.Log("Updating object pose from server...");
                break;

                case "PLAYER_OFFLINE":
                    conectionStatus.playerIsAlone = true;
                    Debug.Log("Other player is offline...");
                break;

                default:
                    Debug.Log("No valid command...");
                break;
            }
        }
    }

    // Send a serialized object to server ...
    public void sendRoomAction(string sendJson)
    {
        writingToServer = true;
        networkBehaviour.stream.BeginWrite(Encoding.UTF8.GetBytes(sendJson), 0, sendJson.Length, new AsyncCallback(endWritingProcess), networkBehaviour.stream);
        networkBehaviour.stream.Flush();
    }

    public void sendAction(string sendJson)
    {
        if(writingToServer)
            return;
        try
        {
            if(!error)
            {
                writingToServer = true;
                networkBehaviour.stream.BeginWrite(Encoding.UTF8.GetBytes(sendJson), 0, sendJson.Length, new AsyncCallback(endWritingProcess), networkBehaviour.stream);
                networkBehaviour.stream.Flush();
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Exception Message: " + ex.Message);
            error = true;
        }
    }

    void endWritingProcess(IAsyncResult _IAsyncResult)
    {
        writingToServer = false;
        networkBehaviour.stream.EndWrite(_IAsyncResult);
    }

    private void Update()
    {
        if(networkBehaviour.isRunning)
        {
            if(networkBehaviour.stream.DataAvailable)
            {
                readingFromServer = true;
                networkBehaviour.stream.BeginRead(networkBehaviour.data, 0, networkBehaviour.data.Length, new AsyncCallback(endReadingProcess), networkBehaviour.stream);
            }
            else
            {
                if (paired && planeDetected && syncronized)
                {
                    UpdatePlayerPose();
                    UpdateEnvironment();
                }
            }
        }
    }

    void endReadingProcess(IAsyncResult _IAsyncResult)
    {
        readingFromServer = false;
        int size = networkBehaviour.stream.EndRead(_IAsyncResult);
        string action = Encoding.UTF8.GetString(networkBehaviour.data, 0, size);
        readAction(action);
    }

    private void OnApplicationQuit()
    {
        networkBehaviour.isRunning = false;
    }

    private void UpdatePlayerPose()
    {
        playerPose.poseUpdate();
        if(playerPose.getPreviousMovement() != playerPose.getCurrentMovement())
        {
            playerPose.setPreviousMovement();
            playerPose.setCommand("UPDATE_PLAYER_POSE");
            playerPose.setPlayerID(id);
            JSONPackage = JsonUtility.ToJson(playerPose, true);
            sendAction(JSONPackage);
            if(playerPose.isFirstPose())
            {
                playerFrame.name = "playerFrame";
                playerFrame = (GameObject) Instantiate(playerFrame);
                playerPose.setFirstPose();
            }
        }
    }

    private void UpdateEnvironment()
    {
        // Searching for a GameObject
        /* Comentario de seguridad
        float smooth = 10.0f;
        try
        {
            if( Lean.Common.LeanSpawn.objects.ContainsKey(JSONPackageReceived.getID()) )
            {
                Debug.Log("Si existe un GameObject con el ID recibido");
                GameObject syncronizedObject;
                if (Lean.Common.LeanSpawn.objects.TryGetValue(JSONPackageReceived.getID(), out syncronizedObject))
                {
                    syncronizedObject.transform.position = Vector3.Slerp(syncronizedObject.transform.position, JSONPackageReceived.getPosition(), Time.deltaTime * smooth);
                    syncronizedObject.transform.rotation = Quaternion.Slerp(syncronizedObject.transform.rotation, JSONPackageReceived.getRotation(),  Time.deltaTime * smooth);
                    syncronizedObject.transform.localScale = JSONPackageReceived.getScale();
                    Debug.Log("Objecto actualizado por otro jugador");
                }
                else
                {
                    Debug.Log("Objecto no encontrado");
                }
            }
            else
            {
                if (updatingObjectPose)
                {
                    Debug.Log("No existe un GameObject con el ID recibido");
                    GameObject syncronizedObject;
                    syncronizedObject = Instantiate(Resources.Load<GameObject>($"Prefabs/Environment/{JSONPackageReceived.getObjectMesh()}"));
                    //Instantiate(objectPrefab);
                    syncronizedObject.name = JSONPackageReceived.getID();
                    //syncronizedObject.objectMesh = JSONPackageReceived.getObjectMesh();
                    syncronizedObject.transform.position = JSONPackageReceived.getPosition();
                    syncronizedObject.transform.rotation = JSONPackageReceived.getRotation();
                    
                    //objectToCreate.Push(syncronizedObject);

                    Lean.Common.LeanSpawn.objects.Add(JSONPackageReceived.getID(), syncronizedObject.gameObject);
                    Debug.Log("Se creo el nuevo objeto");
                    updatingObjectPose = false;
                }
            }
        }
        catch (ArgumentException)
        {
            Debug.Log("Error with dictionary key");
        }
    */
    }
}