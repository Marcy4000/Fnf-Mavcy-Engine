using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator storyButton;
    public Animator freeplayButton;
    public Animator optionsButton;

    public int selectedMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedMenu++;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedMenu--;
        }

        if (selectedMenu >= 3)
        {
            selectedMenu = 0;
        }

        if (selectedMenu <= -1)
        {
            selectedMenu = 2;
        }

        CheckMenuState();

        if (Input.GetKeyDown(KeyCode.KeypadEnter)&& selectedMenu == 1)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void CheckMenuState()
    {
        if (selectedMenu == 0)
        {
            storyButton.SetBool("isSelected", true);
            freeplayButton.SetBool("isSelected", false);
            optionsButton.SetBool("isSelected", false);
        }
        else if (selectedMenu == 1)
        {
            storyButton.SetBool("isSelected", false);
            freeplayButton.SetBool("isSelected", true);
            optionsButton.SetBool("isSelected", false);
        }
        else if (selectedMenu == 2)
        {
            storyButton.SetBool("isSelected", false);
            freeplayButton.SetBool("isSelected", false);
            optionsButton.SetBool("isSelected", true);
        }
    }
}
