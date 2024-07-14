using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollabScreenManager : MonoBehaviour
{
    /// <summary>
    /// Script taken from Many scripts to go between different scenes into the study group.
    /// </summary>

    public void ChangeToDeveloperLinker()
    {
        SceneManager.LoadScene("DeveloperLinker");
    }

    public void ChangeToStudyGroup()
    {
        SceneManager.LoadScene("StudyGroup");
    }

    public void ChangeToLinker()
    {
        SceneManager.LoadScene("Linker");
    }

    public void ChangeToNewMenu()
    {
        SceneManager.LoadScene("NewMenu");
    }
}
