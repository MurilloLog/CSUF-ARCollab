using UnityEngine;

public class ToolManagement : MonoBehaviour
{
    public enum Tool
    { Place, Draw, Clear }

    public Tool currentTool;
    [SerializeField] private ObjectSelection obj;

    public void PlaceSelect()
    {
        currentTool = Tool.Place;
    }

    public void DrawTool()
    {
        currentTool = Tool.Draw;
        //obj.currentObj = ObjectSelection.Objects.None;
        obj.currentLayer = ObjectSelection.Layers.None;
    }

    public void ClearTool()
    {
        // Cambiar la herramienta actual a Clear
        currentTool = Tool.Clear;

        // Encontrar todos los objetos etiquetados con "AR Object"
        /*
        El siguiente codigo es funcional, pero no es necesario. Lo que hace es buscar los objetos
        que tienen etiqueta AR Object, escencialmente las lineas AR, y las borra de la vista del usuario, sin embargo,
        la informacion de cada anchor sigue almacenado en la lista ARAnchor del script ARDrawManager, por 
        lo que he emigrado el proceso de borrar lineas y anchors a ese script para borrar adecuadamente esta
        informacion.
        No obstante, esta funcion sigue llamandose conforme se me proporciono este codigo inicial, contemplando
        que pueda utilizarse para los fines en el que inicialmente fue supuesto.

        GameObject[] ARObjects = GameObject.FindGameObjectsWithTag("AR Object");
        foreach (GameObject obj in ARObjects)
        {
            // Obtener el componente ARAnchor
            var anchor = obj.GetComponent<UnityEngine.XR.ARFoundation.ARAnchor>();
            if (anchor != null)
            {
                // Eliminar el ARAnchor destruyendo su componente
                GameObject.Destroy(anchor);
            }

            // Destruir el GameObject asociado al objeto AR
            GameObject.Destroy(obj);
        }

        Debug.Log("All objects with AR Object label and their anchors were deleted!");
        */
    }

    public void Debugging()
    {
        Debug.Log("I have been pressed");
    }
}