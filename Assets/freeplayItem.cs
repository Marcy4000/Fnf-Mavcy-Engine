using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class freeplayItem : MonoBehaviour
{
    public FreeplayMenu freeplay;
    public int id;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && freeplay.selectedItem == id)
        {
            StartCoroutine(ActuallyLoadSong(this.GetComponent<TMP_Text>().text));
        }
    }

    public IEnumerator ActuallyLoadSong(string name)
    {
        freeplay.blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1.2f);

        GlobalDataSfutt.songNameToLoad = name;
        SceneManager.LoadScene(1);
    }
}
