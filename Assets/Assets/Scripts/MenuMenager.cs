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
    public GameObject optionsMenu;
    public GameObject freeplayMenu;

    public MainMenu mainMenuScript;

    public AudioSource selectSound;
    public AudioSource backSound;
    public AudioSource song;

    public bool IsClicked;

    private void Start()
    {
        Songdata.Initialize(102f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && currentMenu == 0 && !IsClicked || Input.GetKeyDown(KeyCode.Space) && currentMenu == 0 && !IsClicked)
        {
            StartCoroutine(LoadMainMenuFromStart());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentMenu == 2 && !IsClicked || Input.GetKeyDown(KeyCode.Escape) && currentMenu == 1 && !IsClicked)
        {
            StartCoroutine(LoadMainMenu());
        }

        Songdata.SetSongTime(song);

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
        IsClicked = true;
        selectSound.Play();
        whiteFade.SetTrigger("Start");
        startText.SetBool("Pressed", true);

        yield return new WaitForSeconds(1);

        blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1.2f);

        IsClicked = false;
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;
        
    }

    public IEnumerator LoadMainMenu()
    {
        IsClicked = true;
        backSound.Play();
        blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1.2f);

        IsClicked = false;
        optionsMenu.SetActive(false);
        freeplayMenu.SetActive(false);
        mainMenu.SetActive(true);
        currentMenu = 1;
    }

    public IEnumerator LoadFreeplayMenu()
    {
        IsClicked = true;
        selectSound.Play();
        freeplayButton.SetTrigger("Clicked");
        background.SetTrigger("Click");
        yield return new WaitForSeconds(1);
        
        blackFade.SetTrigger("Transition");
        currentMenu = 1;
        
        yield return new WaitForSeconds(1.2f);

        IsClicked = false;
        mainMenu.SetActive(false);
        freeplayMenu.SetActive(true);
        mainMenuScript.selectedMenu = 0;
    }

    public IEnumerator LoadOptionsMenu()
    {
        IsClicked = true;
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
        IsClicked = false;
    }
}
