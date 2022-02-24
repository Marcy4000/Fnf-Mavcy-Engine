using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SimpleFileBrowser;
using System.IO;
using Newtonsoft.Json;

public class DialogueEditor : MonoBehaviour
{
    public DialogueThing dialogue;
    public TMP_InputField characterName, phrase;
    public Toggle isLeft, altAnim;
    public int selectedLine;
    public TMP_Text selectedLineText;

    private void Start()
    {
        CreateNewDialogue();
    }

    public void ChangeSelectedLine(bool subtract)
    {
        if (dialogue == null)
        {
            return;
        }
        
        switch (subtract)
        {
            case true:
                if (selectedLine > 0)
                {
                    selectedLine--;
                }
                break;
            case false:
                if (selectedLine < dialogue.dialogues.Count - 1)
                {
                    selectedLine++;
                }
                break;
        }

        UpdateThing();
    }

    public void UpdateThing()
    {
        characterName.text = dialogue.dialogues[selectedLine].character;
        phrase.text = dialogue.dialogues[selectedLine].sentece;
        isLeft.isOn = dialogue.dialogues[selectedLine].isRight;
        altAnim.isOn = dialogue.dialogues[selectedLine].altAnim;
        selectedLineText.text = $"Line {selectedLine + 1}/{dialogue.dialogues.Count}";
    }

    public void LoadDialogue()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("DialogueFile", ".json"));
        FileBrowser.SetDefaultFilter(".png");

        StartCoroutine(ShowSelectImportPathCoroutine());
    }

    IEnumerator ShowSelectImportPathCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select Dialogue File", "Select");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            ImportDialogue(FileBrowser.Result[0]);
        }
    }

    private void ImportDialogue(string path)
    {
        StreamReader r = new StreamReader(path);
        string jsonString = r.ReadToEnd();
        dialogue = JsonConvert.DeserializeObject<DialogueThing>(jsonString);
        selectedLine = 0;
        UpdateThing();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalDataSfutt.GoToMainMenu();
        }
    }

    public void ExportDialogue()
    {
        if (dialogue == null)
        {
            return;
        }

        StartCoroutine(ShowSelectExportPathCoroutine());
    }

    IEnumerator ShowSelectExportPathCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, null, "Select Dialogue File", "Select");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            ActuallyExportDialogue(FileBrowser.Result[0]);
        }
    }

    private void ActuallyExportDialogue(string path)
    {
        string dialogueStuff = JsonUtility.ToJson(dialogue);
        File.WriteAllText(path + @"\dialogue.json", dialogueStuff);
    }

    public void CreateNewDialogue()
    {
        dialogue = new DialogueThing();
        dialogue.dialogues.Clear();
        CreateNewLine();
    }

    public void CreateNewLine()
    {
        dialogue.dialogues.Add(new Dialogue());
        selectedLine = dialogue.dialogues.Count - 1;
        dialogue.dialogues[selectedLine].character = "bf";
        dialogue.dialogues[selectedLine].sentece = "beep bop";
        dialogue.dialogues[selectedLine].isRight = false;
        dialogue.dialogues[selectedLine].altAnim = false;
        UpdateThing();
    }

    public void SaveLine()
    {
        dialogue.dialogues[selectedLine].sentece = phrase.text;
        dialogue.dialogues[selectedLine].character = characterName.text;
        dialogue.dialogues[selectedLine].isRight = isLeft.isOn;
        dialogue.dialogues[selectedLine].altAnim = altAnim.isOn;
    }

    public void TestDialogue()
    {
        if (dialogue == null)
            return;
        
        DialogueBox.Instance.StartDialogue(dialogue.dialogues);
    }
}
