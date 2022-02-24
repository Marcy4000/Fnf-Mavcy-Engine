using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.IO;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox Instance;
    public TMP_Text textObj;
    public Image portrait;
    public Animator animator;
    public List<Dialogue> senteces;
    public Queue<Dialogue> sentencesQueue;
    public bool inDialogue, hasTextBeenWritten;
    public Sprite[] portraitsImg;
    public string[] portraitsNames;
    public Dictionary<string, Sprite> portraitsDictionary;
    public AudioSource dialogueText, nextPhrase, song;
    public AudioClip bgSong;
    public bool testMode = false;

    private void Start()
    {
        Instance = this;
        sentencesQueue = new Queue<Dialogue>();
        portraitsDictionary = new Dictionary<string, Sprite>();
        for (int i = 0; i < portraitsImg.Length; i++)
        {
            portraitsDictionary.Add(portraitsNames[i], portraitsImg[i]);
        }
        if (Directory.Exists(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad + @"\portraits"))
        {
            DirectoryInfo info = new DirectoryInfo(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad + @"\portraits");
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo file in files)
            {
                if (string.Compare(file.Extension, ".png", System.StringComparison.OrdinalIgnoreCase) == 0)
                {
                    string _name = Path.ChangeExtension(file.Name, null);
                    Debug.Log(_name);
                    Sprite _portrait = IMG2Sprite.LoadNewSprite(file.FullName);
                    portraitsDictionary.Add(_name, _portrait);
                }
            }
        }
        if (File.Exists(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad + @"\dialogueTheme.ogg"))
        {
            StartCoroutine(LoadSongFile(GlobalDataSfutt.songPath + GlobalDataSfutt.songNameToLoad));
        }
        else
        {
            song.Stop();
        }
    }

    private IEnumerator LoadSongFile(string selectedSongDir)
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + selectedSongDir + @"\dialogueTheme.ogg", AudioType.OGGVORBIS);
        yield return req.SendWebRequest();
        bgSong = DownloadHandlerAudioClip.GetContent(req);
        while (bgSong.loadState != AudioDataLoadState.Loaded)
            yield return new WaitForSeconds(0.1f);
        song.clip = bgSong;
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

    public void StartDialogue(List<Dialogue> _sentences)
    {
        senteces.Clear();
        sentencesQueue.Clear();
        song.Play();
        inDialogue = true;

        for (int i = 0; i < _sentences.Count; i ++)
        {
            senteces.Add(_sentences[i]);
        }


        foreach (Dialogue sentence in senteces)
        {
            sentencesQueue.Enqueue(sentence);
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

        Dialogue sentence = sentencesQueue.Dequeue();
        if (portraitsDictionary.ContainsKey(sentence.character))
        {
            portrait.sprite = portraitsDictionary[sentence.character];
            portrait.rectTransform.sizeDelta = new Vector2(portrait.sprite.rect.width, portrait.sprite.rect.height);
        }
        else
        {
            portrait.sprite = portraitsDictionary["bf"];
            portrait.rectTransform.sizeDelta = new Vector2(portrait.sprite.rect.width, portrait.sprite.rect.height);
        }

        switch (sentence.isRight)
        {
            case true:
                if (sentence.altAnim)
                {
                    animator.Play("Right Alt");
                }
                else
                {
                    animator.Play("Right");
                }
                break;
            case false:
                if (sentence.altAnim)
                {
                    animator.Play("Left Alt");
                }
                else
                {
                    animator.Play("Left");
                }
                break;
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence.sentece));
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
        song.Stop();
    }

    public void PlaySong()
    {
        if (testMode)
        {
            return;
        }
        LoadSong.instance.DialogueStuff();
        
        gameObject.SetActive(false);
    }

}

[System.Serializable]
public class DialogueThing
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}

[System.Serializable]
public class Dialogue
{
    public string character = "bf";
    public bool isRight = false;
    public bool altAnim = false;
    public string sentece = "beep";
}