using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;


public class ProfileAccess : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        ReadFromText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReadFromText()
    {
        string path = Application.dataPath + "/UserScore.txt";
        StreamReader sr = new StreamReader(path);
        scoreText.text = sr.ReadToEnd();

    }
}
