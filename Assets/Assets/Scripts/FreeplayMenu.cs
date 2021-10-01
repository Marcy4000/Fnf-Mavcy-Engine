using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class FreeplayMenu : MonoBehaviour
{
    public ScrollRect contents;
    public Button button;
    public Animator blackFade;


    public int totalHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.json");
        for (int i = 0; i < info.Length; i++)
        {
            TMP_Text text;
            Button thing = Instantiate(button, contents.content.transform);
            text = thing.GetComponentInChildren<TMP_Text>();
            text.text = info[i].Name;
            totalHeight += 84;
        }
        contents.content.sizeDelta = new Vector2(0, totalHeight);
    }

}
