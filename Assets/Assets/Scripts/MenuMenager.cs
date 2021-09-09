using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour
{
    public int currentMenu = 0;

    public Animator startText;

    public GameObject startMenu;
    public GameObject mainMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && currentMenu == 0 || Input.GetKeyDown(KeyCode.Space)&&currentMenu == 0)
        {
            startText.SetBool("Pressed", true);
            LoadMainMenu();
        }
    }

    public void LoadMainMenu()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;
    }
}
