using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMenager : MonoBehaviour
{
    public int currentMenu = 0;

    public Animator startText;
    public Animator whiteFade;

    public GameObject startMenu;
    public GameObject mainMenu;

    public AudioSource selectSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && currentMenu == 0 || Input.GetKeyDown(KeyCode.Space)&&currentMenu == 0)
        {
            StartCoroutine(LoadMainMenu());
        }
    }

    IEnumerator LoadMainMenu()
    {
        selectSound.Play();
        whiteFade.SetTrigger("Start");
        startText.SetBool("Pressed", true);
        

        yield return new WaitForSeconds(2);

        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;
        
    }
}
