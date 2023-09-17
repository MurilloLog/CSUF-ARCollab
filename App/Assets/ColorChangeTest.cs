    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeTest : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;

    private Text questionText;
    private Color originalColor;

    private void Start()
    {
        questionText = GetComponent<Text>();
        originalColor = questionText.color;
        
    }

   

    IEnumerator SmoothColorChange(Text text, Color A, Color B, float duration, float fadeDuration)
    {
        Debug.Log("Color Changing starting");
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Debug.Log("Elapsed time: {0} while Duration: {1}");
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            text.color = Color.Lerp(A, B, t);
            yield return null;
        }

        //wait for fadeDuration
        Debug.Log("Waiting for fade duration");
        yield return new WaitForSeconds(fadeDuration);
        //return to original color
        Debug.Log("Returning to original color");
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            text.color = Color.Lerp(B, A, t);
            yield return null;
        }

        text.color = A;
        Debug.Log("Color Chang finish");

    }

}
