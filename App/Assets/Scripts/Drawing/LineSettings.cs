using UnityEngine;

/* El script no puede ser un ScriptableObject al heredar MonoBehaviour, herencia indispensable ya que 
// forma parte de un GameObject. 
*/
// [CreateAssetMenu(fileName = "LineSettings", menuName = "Create Line Settings", order = 0)]
public class LineSettings : MonoBehaviour
{
    public string lineTagName = "Line";

    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public float distanceFromCamera = 0.3f;

    public Material defaultColorMaterial;
    public Material[] materialOptions;
    public int currentColor;
    public int cornerVertices = 5;

    public int endCapVertices = 5;

    public float startWidth = 0.01f;
    public float endWidth = 0.01f;

    [Range(0, 1.0f)]
    public float minDistanceBeforeNewPoint = 0.01f;

    [Header("Tolerance Options")]
    public bool allowSimplification = false;

    public float tolerance = 0.001f;

    public float applySimplifyAfterPoints = 20.0f;

    public void SelectColor(int c)
    {
        currentColor = c;
        defaultColorMaterial = materialOptions[c];
    }

    public int GetLineColor() { return currentColor; }
}