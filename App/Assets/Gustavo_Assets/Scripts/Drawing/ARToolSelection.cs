using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARToolSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] gameColors; // Lista de Colores para interactuar
    [SerializeField] private GameObject[] gameObjects; // Lista de GameObjects para interactuar
    [SerializeField] private float selectedHeight = 700f; // Altura cuando es seleccionado
    [SerializeField] private float defaultHeight = 610f;  // Altura cuando no esta seleccionado

    /// <summary>
    /// Activa el GameObject seleccionado y desactiva el resto.
    /// </summary>
    /// <param name="selectedObject">El GameObject que fue seleccionado.</param>
    public void ActivateOnly(GameObject selectedObject)
    {
        foreach (GameObject obj in gameObjects)
        {
            // Activar background del gameobject
            if (obj != null)
            {
                // Activar solo el seleccionado, desactivar los demás
                obj.SetActive(obj == selectedObject);
            }
        }
    }

    public void ScaleActivate(GameObject selectedObject)
    {
        foreach (GameObject obj in gameColors)
        {
            // Escalar el gameobject
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Cambiar la altura segun si es el seleccionado o no
                float newHeight = (obj == selectedObject) ? selectedHeight : defaultHeight;
                Vector2 newSize = new Vector2(rectTransform.sizeDelta.x, newHeight);
                rectTransform.sizeDelta = newSize;
            }
        }
    }
}
