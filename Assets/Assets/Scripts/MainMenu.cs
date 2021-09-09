using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Animator storyButton;
    public Animator freeplayButton;
    public Animator optionsButton;

    public int selectedMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
