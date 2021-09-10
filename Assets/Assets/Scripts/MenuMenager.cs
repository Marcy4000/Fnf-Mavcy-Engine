using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMenager : MonoBehaviour
{
    public int currentMenu = 0;

    public Animator startText;
    public Animator whiteFade;
    public Animator blackFade;
    public Animator background;

    public Animator storyButton;
    public Animator freeplayButton;
    public Animator optionsButton;

    public GameObject startMenu;
    public GameObject mainMenu;
    public MainMenu mainMenuScript;

    public AudioSource selectSound;
    public AudioSource backSound;

    public GameObject optionsMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && currentMenu == 0 || Input.GetKeyDown(KeyCode.Space) && currentMenu == 0)
        {
            StartCoroutine(LoadMainMenuFromStart());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentMenu == 2)
        {
            StartCoroutine(LoadMainMenu());
        }

    }

    public void StartCorutines(int corutine)
    {
        if (corutine == 1)
        {
            StartCoroutine(LoadFreeplayMenu());
        }
        else if (corutine == 2)
        {
            StartCoroutine(LoadOptionsMenu());
        }
    }

    public IEnumerator LoadMainMenuFromStart()
    {
        selectSound.Play();
        whiteFade.SetTrigger("Start");
        startText.SetBool("Pressed", true);

        yield return new WaitForSeconds(1);

        blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1);

        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;
        
    }

    public IEnumerator LoadMainMenu()
    {
        backSound.Play();
        blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1);

        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;

    }

    public IEnumerator LoadFreeplayMenu()
    {
        selectSound.Play();
        freeplayButton.SetTrigger("Clicked");
        background.SetTrigger("Click");
        yield return new WaitForSeconds(1);
        
        blackFade.SetTrigger("Transition");
        
        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(1);
    }

    public IEnumerator LoadOptionsMenu()
    {
        selectSound.Play();
        optionsButton.SetTrigger("Clicked");
        background.SetTrigger("Click");

        yield return new WaitForSeconds(1);
        
        blackFade.SetTrigger("Transition");
        currentMenu = 2;

        yield return new WaitForSeconds(1.2f);

        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        mainMenuScript.selectedMenu = 0;
    }
}
