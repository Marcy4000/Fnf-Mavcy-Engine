using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TriggerWhitefade : MonoBehaviour
{
    public Animator whiteFade;
    public TMPro.TMP_Text topText, BottomText;
    public Animator animator;
    public AudioSource song;

    private void Start()
    {
        if (File.Exists(Path.GetFullPath(".") + @"\data\introText.txt"))
        {
            string[] lines = File.ReadAllLines(Path.GetFullPath(".") + @"\data\introText.txt");
            int randomString = Random.Range(0, lines.Length);
            string[] sus = lines[randomString].Split('-');
            topText.text = sus[0];
            BottomText.text = sus[1];
            StartCoroutine(StartIntro());
        }
    }

    IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(1f);
        song.Play();
        animator.Play("Intro");
    }

    public void triggerAnimation()
    {
        whiteFade.SetTrigger("Start");
    }
}
