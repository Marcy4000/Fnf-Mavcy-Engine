using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalDataSfutt
{

    public static string songNameToLoad;

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
