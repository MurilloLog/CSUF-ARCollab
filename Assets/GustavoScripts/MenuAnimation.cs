using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] float animationDuration; // Creacion de atributo privado pero visible en el inspector Unity
    [SerializeField] float delay;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] RectTransform target;
    [SerializeField] Vector2 startingPoint;
    [SerializeField] Vector2 finalPoint;

    public void FadeIn(){
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(startingPoint, finalPoint));
    }
    public void FadeOut(){
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine(finalPoint, startingPoint));
    }

    IEnumerator FadeInCoroutine(Vector2 a, Vector2 b){
        Vector2 starPoint = a;
        Vector2 finalPoint = b;
        float elapsed = 0;
        while(elapsed < delay){
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0;
        while(elapsed<=animationDuration){
            float percentage = elapsed / animationDuration;
            float curvePercentage = animationCurve.Evaluate(percentage);
            elapsed += Time.deltaTime;
            Vector2 currentPosition = Vector2.LerpUnclamped(startingPoint, finalPoint, curvePercentage);
            target.anchoredPosition = currentPosition;
            yield return null;
        }

        target.anchoredPosition = finalPoint;
    }

    
}
