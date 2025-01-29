using ARDrawing.Core.Singletons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using System.Linq;

[RequireComponent(typeof(ARAnchorManager))]
public class ARDrawManager : Singleton<ARDrawManager>
{
    // Definir el area de dibujo como un porcentaje del tamanio de la pantalla para evitar crear anchors cuando se cambia de color
    [SerializeField]
    private float areaWidthPercentage = 1.0f;  // 100% del ancho de la pantalla
    [SerializeField]
    private float areaHeightPercentage = 0.5f; // 50% de la altura de la pantalla
    [SerializeField]
    private float areaXPositionPercentage = 0.05f; // 5% desde el borde izquierdo
    [SerializeField]
    private float areaYPositionPercentage = 0.05f; // 5% desde el borde superior
    [SerializeField]
    private Rect drawingArea;  // Area a detectar para dibujar

    [SerializeField]
    private LineSettings lineSettings;

    [SerializeField]
    private int currentSelectedColor = 5; // Default color

    [SerializeField]
    private UnityEvent OnDraw;

    [SerializeField]
    private ARAnchorManager anchorManager;

    [SerializeField]
    private Camera arCamera;

    private List<ARAnchor> arAnchors = new List<ARAnchor>();
    private Dictionary<int, ARLine> Lines = new Dictionary<int, ARLine>();

    private bool CanDraw { get; set; }

    public Events backendEvents;

    void Awake()
    {
        backendEvents = FindObjectOfType<Events>();
    }

    void Start()
    {
        // Calcular las dimensiones del area de dibujo basadas en el tamanio de la pantalla
        UpdateDrawingArea();
    }

    private void UpdateDrawingArea()
    {
        // Calcular las coordenadas y el tamanio del area de dibujo en funcion de la resolucion actual
        float areaWidth = Screen.width * areaWidthPercentage;
        float areaHeight = Screen.height * areaHeightPercentage;
        float areaX = Screen.width * areaXPositionPercentage;
        float areaY = Screen.height * areaYPositionPercentage;

        drawingArea = new Rect(areaX, areaY, areaWidth, areaHeight);

        Debug.Log($"Area de dibujo ajustada a: {drawingArea}");
    }

    public void DeserializeAndAddAnchor(string json)
    {
        // Deserializar el JSON recibido
        Drawing drawingData = JsonUtility.FromJson<Drawing>(json);

        if (drawingData != null)
        {
            CreateAnchorFromData(drawingData);
            Debug.Log("El anchor del otro jugador se creo correctamente.");
        }
        else
        {
            Debug.LogError("No se pudo deserializar el JSON.");
        }
    }

    private void CreateAnchorFromData(Drawing drawingData)
    {
        // Crear un GameObject para el Anchor
        GameObject anchorObject = new GameObject($"Anchor_{drawingData.anchorID}");
        anchorObject.transform.position = drawingData.anchorPosition;
        anchorObject.transform.rotation = drawingData.anchorRotation;

        // Agregar un ARAnchor al GameObject
        ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();

        if (anchor == null)
        {
            Debug.LogError("Error al crear el anclaje desde los datos recibidos.");
            return;
        }

        // Agregar el Anchor a la lista local
        arAnchors.Add(anchor);

        // Crear una nueva linea y aniadir los puntos recibidos
        ARLine line = new ARLine(lineSettings);
        currentSelectedColor = line.GetCurrentColor();
        lineSettings.SelectColor(drawingData.lineColor);
        line.AddNewLineRenderer(transform, anchor, drawingData.anchorPosition);
        foreach (Vector3 point in drawingData.linePoints)
        {
            line.AddPoint(point);
        }

        // Aniadir la linea al diccionario local
        //Lines.Add(Lines.Count, line);
        lineSettings.SelectColor(currentSelectedColor);
        Debug.Log($"Anclaje y linea creados con exito desde los datos: {drawingData.anchorID}");
        backendEvents.drawing = false;
    }

    private void Update()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;

            // Verificar si el toque esta dentro del area de dibujo
            if (drawingArea.Contains(touchPosition))
            {
                DrawOnTouch();
            }
        }
#else
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;

            // Verificar si el clic esta dentro del area de dibujo
            if (drawingArea.Contains(mousePos))
            {
                DrawOnMouse();
            }
        }

#endif
    }

    public void AllowDraw(bool isAllow)
    {
        CanDraw = isAllow;
    }

    private void DrawOnTouch()
    {
        DrawOnTouch(anchorManager);
    }

    void DrawOnTouch(ARAnchorManager anchorManager)
    {
        if (!CanDraw) return;
        
        Touch touch = Input.GetTouch(0);
        Vector3 touchPosition = arCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, lineSettings.distanceFromCamera));

        if (touch.phase == TouchPhase.Began)
        {
            OnDraw?.Invoke();

            // Crear un GameObject en la posicion del toque
            GameObject anchorObject = new GameObject("ARAnchor");
            anchorObject.transform.position = touchPosition;
            anchorObject.transform.rotation = Quaternion.identity;
            
            // Agregar un componente ARAnchor al GameObject creado
            ARAnchor anchor = anchorObject.AddComponent<ARAnchor>();
            
            // Validar la creacion del anclaje
            if (anchor == null)
                Debug.LogError("Error creating reference point");
            else
            {
                arAnchors.Add(anchor);
                ShowAnchorInfo(anchor);
            }

            ARLine line = new ARLine(lineSettings);
            Lines.Add(touch.fingerId, line);
            line.AddNewLineRenderer(transform, anchor, touchPosition);
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            Lines[touch.fingerId].AddPoint(touchPosition);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            // Obtener la linea y su ancla asociada
            ARLine line = Lines[touch.fingerId];
            ARAnchor anchor = arAnchors[arAnchors.Count - 1]; // Suponiendo que el ultimo ancla es el asociado

            // Redondear los valores antes de serializar
            Vector3 roundedAnchorPosition = RoundVector3(anchor.transform.position);
            Quaternion roundedAnchorRotation = RoundQuaternion(anchor.transform.rotation);
            List<Vector3> roundedLinePoints = line.GetPoints().Select(RoundVector3).ToList();

            // Crear la instancia de SerializedAnchorLine
            Drawing serializedData = new Drawing(
                backendEvents.id,
                backendEvents.roomId,
                anchor.trackableId.ToString(),
                roundedAnchorPosition,
                roundedAnchorRotation,
                roundedLinePoints,
                line.GetCurrentColor() // Modificar con la obtencion del color actual
            );

            // Convertir a JSON
            string json = JsonUtility.ToJson(serializedData, false) + "|";
            Debug.Log($"JSON a enviar: {json}");

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("JSON data is empty.");
            }
            else
            {
                if (json.Length > 5e+6)
                {
                    Debug.LogError("El JSON supera el tamanio del buffer de 5e+6 B.");
                    // Implementar una estrategia para dividir los datos?
                }
                else
                {
                    Debug.Log($"Tamanio del json: {json.Length}");
                    Debug.Log($"Serialized JSON for anchors and lines: {json}");

                    // Enviar al servidor
                    backendEvents.sendRoomAction(json);
                }
            }
            Lines.Remove(touch.fingerId);
        }
    }

    private Vector3 RoundVector3(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x * 1000f) / 1000f,
            Mathf.Round(vector.y * 1000f) / 1000f,
            Mathf.Round(vector.z * 1000f) / 1000f
        );
    }

    private Quaternion RoundQuaternion(Quaternion quaternion)
    {
        return new Quaternion(
            Mathf.Round(quaternion.x * 1000f) / 1000f,
            Mathf.Round(quaternion.y * 1000f) / 1000f,
            Mathf.Round(quaternion.z * 1000f) / 1000f,
            Mathf.Round(quaternion.w * 1000f) / 1000f
        );
    }

    public void ClearAnchors()
    {
        int anchorCount = arAnchors.Count;
        int lineCount = Lines.Count;

        // Borrar los anchors almacenados en la lista cuando se selecciona el borrador
        foreach (ARAnchor anchor in arAnchors)
        {
            if (anchor != null)
            {
                // Eliminar el componente ARAnchor
                Destroy(anchor.gameObject);
            }
        }
        // Limpiar la lista despues de borrar los anchors
        arAnchors.Clear();

        Debug.Log("All lines and their anchors were deleted!");
        Debug.Log($"The number of ARAnchors is: {anchorCount}");
        Debug.Log($"The number of ARLines is: {lineCount}");
    }

    private void ShowAnchorInfo(ARAnchor anchor)
    {
        /*  
         *  Proporcionar informacion sobre el anchor creado
         *  Este metodo es exclusivo para observar y comprender el comportamiento 
         *  de los anchors dentro de la escena RA. Puede comentarse cuando ya no 
         *  sea necesario.
        */

        int anchorCount = arAnchors.Count;
        int lineCount = Lines.Count;

        // Posicion del anchor
        Vector3 anchorPosition = anchor.transform.position;
        // Rotacion del anchor
        Quaternion anchorRotation = anchor.transform.rotation;
        // ID del anchor
        string anchorID = anchor.trackableId.ToString();

        Debug.Log("A new anchor was created!");
        Debug.Log($"Anchor position: {anchorPosition}");
        Debug.Log($"Anchor rotation: {anchorRotation}");
        Debug.Log($"Anchor trackableId: {anchorID}");
        
        Debug.Log($"Now there are {arAnchors.Count} anchors in the AR world");
        Debug.Log($"The number of ARAnchors is: {anchorCount}");
        Debug.Log($"The number of ARLines is: {lineCount}");
    }

    private void DrawOnMouse()
    {
        if (!CanDraw) return;

        Vector3 mousePos = arCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, lineSettings.distanceFromCamera));
        if (Input.GetMouseButtonDown(0))
        {
            OnDraw?.Invoke();
            if (Lines.Keys.Count == 0)
            {
                ARLine line = new ARLine(lineSettings);
                Lines.Add(0, line);
                line.AddNewLineRenderer(transform, null, mousePos);
            }
            else
            {
                Lines[0].AddPoint(mousePos);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Lines.Remove(0);
        }
    }
}