using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer instance;
    private VideoPlayer videoPlayer;
    public GameObject renderImage;

    void Start()
    {
        instance = this;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    public void PlayCutscene(string _url)
    {
        videoPlayer.url = _url;
        videoPlayer.frame = 0;
        videoPlayer.isLooping = false;
        renderImage.SetActive(true);
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        renderImage.SetActive(false);
        LoadSong.instance.StartDialogueAfterCutscene();
        gameObject.SetActive(false);
    }
}
