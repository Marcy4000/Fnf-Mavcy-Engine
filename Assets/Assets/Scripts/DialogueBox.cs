using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox Instance;
    public TMP_Text textObj;
    public Image portrait;
    public Animator animator;
    public List<string> senteces;
    public List<string> characters;
    public Queue<string> sentencesQueue;
    public Queue<string> charactersQueue;
    private bool inDialogue, hasTextBeenWritten;
    public Sprite[] portraitsImg;
    public string[] portraitsNames;
    public Dictionary<string, Sprite> portraitsDictionary;
    public AudioSource dialogueText, nextPhrase;

    private void Start()
    {
        Instance = this;
        sentencesQueue = new Queue<string>();
        charactersQueue = new Queue<string>();
        portraitsDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < portraitsImg.Length; i++)
        {
            portraitsDictionary.Add(portraitsNames[i], portraitsImg[i]);
        }
    }

    private void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Return) && hasTextBeenWritten == true || Input.GetKeyDown(KeyCode.Z) && hasTextBeenWritten == true)
            {
                nextPhrase.Play();
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(string[] _sentences)
    {
        senteces.Clear();
        sentencesQueue.Clear();
        inDialogue = true;

        for (int i = 1; i < _sentences.Length; i += 2)
        {
            senteces.Add(_sentences[i]);
        }
        
        for (int i = 0; i < _sentences.Length; i += 2)
        {
            characters.Add(_sentences[i]);
        }


        foreach (string sentence in senteces)
        {
            sentencesQueue.Enqueue(sentence);
        }
        
        foreach (string character in characters)
        {
            charactersQueue.Enqueue(character);
        }

        animator.Play("appear");
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string _character = charactersQueue.Dequeue();
        if (portraitsDictionary.ContainsKey(_character))
        {
            portrait.sprite = portraitsDictionary[_character];
            portrait.rectTransform.sizeDelta = new Vector2(portrait.sprite.rect.width, portrait.sprite.rect.height);
        }
        else
        {
            portrait.sprite = portraitsDictionary["bf"];
            portrait.rectTransform.sizeDelta = new Vector2(portrait.sprite.rect.width, portrait.sprite.rect.height);
        }
        string sentence = sentencesQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        textObj.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.Play();
            textObj.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        hasTextBeenWritten = true;
    }

    public void EndDialogue()
    {
        inDialogue = false;
        animator.SetTrigger("disappear");
        Debug.Log("End of dialogue.");
    }

    public void PlaySong()
    {
        LoadSong.instance.DialogueStuff();
        gameObject.SetActive(false);
    }

}
