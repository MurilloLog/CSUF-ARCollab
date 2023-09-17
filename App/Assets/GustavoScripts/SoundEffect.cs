using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;

    /*public void Button1()
    {
        src.clip = sfx1;
        src.Play();
    }*/

    private void Start()
    {
        src.clip = sfx1;
        src.Play();
    }
}
