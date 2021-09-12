using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using NAudio;

public class PlaySong : MonoBehaviour
{
    //stupid name, this script is for the buttons in freeplay
    
    public FreeplayMenu freeplay;

    private void Start()
    {
        GameObject gameObject1;
        gameObject1 = GameObject.Find("FreeplayMenu");
        freeplay = gameObject1.GetComponent<FreeplayMenu>();

    }

    public void LoadSong(TMP_Text songName)
    {
        StartCoroutine(ActuallyLoadSong(songName.text));
    }

    public IEnumerator ActuallyLoadSong(string name)
    {
        freeplay.blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1.2f);

        GlobalDataSfutt.songNameToLoad = name;
        SceneManager.LoadScene(1);
    }
}
