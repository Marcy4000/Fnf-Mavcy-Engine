using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoUp : MonoBehaviour
{
    [HideInInspector]public LoadSong song;
    [HideInInspector]public RectTransform transform;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject songLoader;
        songLoader = GameObject.Find("SongLoader");
        song = songLoader.GetComponent<LoadSong>();
        transform = this.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(0, ((Vector3.up.y * Time.deltaTime) * song.scrollSpeed) * 1000, 0));
    }
}
