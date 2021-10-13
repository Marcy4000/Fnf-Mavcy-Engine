using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class FreeplayMenu : MonoBehaviour
{
    public Transform contents;
    public Button button;
    public GameObject item;
    public Animator blackFade;
    public AudioSource scrollSound;
    private VerticalLayoutGroup layoutGroup;


    public int totalHeight;
    public int selectedItem;
    private int lenght;
    
    // Start is called before the first frame update
    void Start()
    {
        layoutGroup = contents.GetComponent<VerticalLayoutGroup>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.json");
        for (int i = 0; i < info.Length; i++)
        {
            TMP_Text text;
            freeplayItem itemComponent;
            //Button thing = Instantiate(button, contents.content.transform);
            GameObject lmao = Instantiate(item, contents.transform);
            itemComponent = lmao.GetComponent<freeplayItem>();
            itemComponent.id = i;
            text = lmao.GetComponent<TMP_Text>();
            text.text = info[i].Name;
            totalHeight = totalHeight + 84;
        }
        lenght = info.Length;
    }

    private void Update()
    {
        layoutGroup.padding.top = (207 * selectedItem) * -1;
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItem--;
            scrollSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItem++;
            scrollSound.Play();
        }

        if (selectedItem > lenght -1)
        {
            selectedItem = 0;
        }
        if (selectedItem < 0)
        {
            selectedItem = lenght - 1;
        }
    }

}
