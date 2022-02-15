using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class freeplayItem : MonoBehaviour
{
    public FreeplayMenu freeplay;
    public string songName;
    public string songPath;
    public PlayerStat songStats;
    public int id;
    private TMP_Text text;
    private float fontSize;

    private void Start()
    {
        GameObject gameObject1;
        gameObject1 = GameObject.Find("FreeplayMenu");
        freeplay = gameObject1.GetComponent<FreeplayMenu>();
        text = GetComponent<TMP_Text>();
        fontSize = text.fontSize;
    }

    public void LoadSong(TMP_Text songName)
    {
        StartCoroutine(ActuallyLoadSong(songName.text));
    }

    private void Update()
    {
        if (freeplay.selectedItem == id)
        {
            text.fontSize = fontSize * 1.3f;
        }
        else
        {
            text.fontSize = fontSize;
        }

        if (Input.GetKeyDown(KeyCode.Return) && freeplay.selectedItem == id)
        {
            StartCoroutine(ActuallyLoadSong(songName));
        }
    }

    public IEnumerator ActuallyLoadSong(string name)
    {
        freeplay.blackFade.SetTrigger("Transition");
        GlobalDataSfutt.isStoryMode = false;
        GlobalDataSfutt.selectedDifficulty = (Diffuculty)freeplay.selectedDifficulty;
        GlobalDataSfutt.songPath = songPath;

        yield return new WaitForSeconds(1.2f);

        Songdata.ResetThings();
        GlobalDataSfutt.songNameToLoad = name;
        SceneManager.LoadScene(1);
    }
}
