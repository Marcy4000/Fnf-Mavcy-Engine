using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
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
    public GameObject storymodeMenu;
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
        LoadSettings();
        if (!GlobalDataSfutt.hasLoadedMods)
        {
            LoadMods();
        }
    }

    private void LoadMods()
    {
        DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(".") + "/data" + "/Mods/");
        DirectoryInfo[] info = dir.GetDirectories();
        GlobalDataSfutt.customCharacters.Clear();

        for (int i = 0; i < info.Length; i++)
        {
            if (File.Exists(Path.GetFullPath(".") + "/data/Mods/" + info[i].Name + "/character.dat"))
            {
                ExportableFnfCharacter character;
                character = LoadFile(Path.GetFullPath(".") + "/data/Mods/" + info[i].Name + "/character.dat");
                FNFCharacter character1 = new FNFCharacter();
                character1.LoadAnimations(character, Path.GetFullPath(".") + @"\data\Mods\" + info[i].Name);
                GlobalDataSfutt.customCharacters.Add(character1);
                GlobalDataSfutt.characterActive.Add(true);
            }
        }

        GlobalDataSfutt.hasLoadedMods = true;
    }

    public ExportableFnfCharacter LoadFile(string _destination)
    {
        string destination;

        if (!string.IsNullOrWhiteSpace(_destination))
        {
            destination = _destination;
        }
        else
        {
            return null;
        }

        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        ExportableFnfCharacter data = (ExportableFnfCharacter)bf.Deserialize(file);
        file.Close();
        return data;
    }

    private void LoadSettings()
    {
        Player.leftArrowKey = (KeyCode)PlayerPrefs.GetInt("left");
        Player.downArrowKey = (KeyCode)PlayerPrefs.GetInt("down");
        Player.upArrowKey = (KeyCode)PlayerPrefs.GetInt("up");
        Player.rightArrowKey = (KeyCode)PlayerPrefs.GetInt("right");
        Player.secLeftArrowKey = (KeyCode)PlayerPrefs.GetInt("secLeft");
        Player.secDownArrowKey = (KeyCode)PlayerPrefs.GetInt("secDown");
        Player.secUpArrowKey = (KeyCode)PlayerPrefs.GetInt("secUp");
        Player.secRightArrowKey = (KeyCode)PlayerPrefs.GetInt("secRight");
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
        GlobalDataSfutt.ghostTapping = intToBool(PlayerPrefs.GetInt("ghostTapping"));
        GlobalDataSfutt.overrideStage = intToBool(PlayerPrefs.GetInt("overrideStage"));
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && currentMenu == 0 && !IsClicked || Input.GetKeyDown(KeyCode.Space) && currentMenu == 0 && !IsClicked)
        {
            StartCoroutine(LoadMainMenuFromStart());
        }
        if (Input.GetKeyDown(KeyCode.Escape) && currentMenu == 2 && !IsClicked || Input.GetKeyDown(KeyCode.Escape) && currentMenu == 1 && !IsClicked || Input.GetKeyDown(KeyCode.Escape) && currentMenu == 3 && !IsClicked)
        {
            StartCoroutine(LoadMainMenu());
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SceneManager.LoadScene(11);
        }

        Songdata.SetSongTime(song);

    }

    public void StartCorutines(int corutine)
    {
        switch (corutine)
        {
            case 0:
                StartCoroutine(LoadStoryModeMenu());
                break;
            case 1:
                StartCoroutine(LoadFreeplayMenu());
                break;
            case 2:
                StartCoroutine(LoadOptionsMenu());
                break;
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
        storymodeMenu.SetActive(false);
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
        storymodeMenu.SetActive(false);
        freeplayMenu.SetActive(true);
        mainMenuScript.selectedMenu = 0;
    }

    public IEnumerator LoadStoryModeMenu()
    {
        IsClicked = true;
        selectSound.Play();
        storyButton.SetTrigger("Clicked");
        background.SetTrigger("Click");

        yield return new WaitForSeconds(1);

        blackFade.SetTrigger("Transition");
        currentMenu = 3;

        yield return new WaitForSeconds(1.2f);

        mainMenu.SetActive(false);
        storymodeMenu.SetActive(true);
        mainMenuScript.selectedMenu = 1;
        IsClicked = false;
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
