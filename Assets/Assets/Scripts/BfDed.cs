using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BfDed : MonoBehaviour
{
    public AudioSource dedMusic;
    public AudioClip selectSound;
    public GameObject transition;
    private Animator transitionAnim;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transitionAnim = transition.GetComponent<Animator>();
    }

    void PlayMusic()
    {
        dedMusic.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Retry());
        }
    }

    IEnumerator Retry()
    {
        animator.Play("Bf-Ded-Press");
        dedMusic.Stop();
        dedMusic.clip = selectSound;
        dedMusic.Play();

        yield return new WaitForSeconds(5.1f);
        transition.SetActive(true);
        transitionAnim.SetTrigger("Change");
        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(1);
    }
}
