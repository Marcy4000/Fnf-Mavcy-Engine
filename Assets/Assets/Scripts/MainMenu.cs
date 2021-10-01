using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public MenuMenager menuMenager;
    
    public Animator storyButton;
    public Animator freeplayButton;
    public Animator optionsButton;

    public AudioSource scrollMenu;
    public AudioSource selectMenu;

    public int selectedMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedMenu++;
            scrollMenu.Play();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedMenu--;
            scrollMenu.Play();
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

        if (Input.GetKeyDown(KeyCode.Return) && selectedMenu == 1 && !menuMenager.IsClicked)
        {
            menuMenager.StartCorutines(selectedMenu);
        }
        if (Input.GetKeyDown(KeyCode.Return) && selectedMenu == 2 && !menuMenager.IsClicked)
        {
            menuMenager.StartCorutines(selectedMenu);
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
