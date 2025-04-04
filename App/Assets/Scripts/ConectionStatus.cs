using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConectionStatus : MonoBehaviour
{
    public bool playerIsAlone;
    public bool playerIsWaiting;
    public GameObject panel;
    public GameObject placeholder;
    public GameObject startDrawing;

    public void enablePanelStatus(bool show)
    {
        panel.SetActive(show);
    }

    public void enablePlaceholder(bool show)
    {
        placeholder.SetActive(show);
        enableDrawing(false);
    }

    public void enableDrawing(bool show)
    {
        startDrawing.SetActive(show);
    }

    void Update()
    {
        if(playerIsWaiting)
        {
            enablePanelStatus(true);
            enablePlaceholder(false);
            enableDrawing(false);
        }
        else
        {
            enablePanelStatus(false);
            enableDrawing(true);
        }
    }
}
