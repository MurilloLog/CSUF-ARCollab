using UnityEngine;
using UnityEngine.UI;

public class LoadingGif : MonoBehaviour
{
    public Image imageToRotate;
    public float rotationSpeed = 100f;

    void Update()
    {
        if (imageToRotate != null)
        {
            float angle = rotationSpeed * Time.deltaTime;
            imageToRotate.rectTransform.Rotate(Vector3.back, angle);
        }
        else
        {
            Debug.LogWarning("Set an imagen to rotate.");
        }
    }
}
