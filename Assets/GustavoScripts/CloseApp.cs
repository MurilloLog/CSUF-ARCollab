using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseApp : MonoBehaviour
{
    [SerializeField] private Button BtnExit;
    private bool BtnExitState = false;
    
    private void Start()
    {
        BtnExit.onClick.AddListener(ToggleState);
    }

    private void ToggleState()
    {
        BtnExitState = !BtnExitState;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || BtnExitState)
            Application.Quit();
    }
}
