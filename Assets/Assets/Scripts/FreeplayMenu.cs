using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class FreeplayMenu : MonoBehaviour
{
    public Transform contents;
    //public Button button;
    public GameObject item;
    public Animator blackFade;
    public AudioSource scrollSound;
    private VerticalLayoutGroup layoutGroup;
    public Image background;


    //public int totalHeight;
    public int selectedItem;
    private int lenght;
    private int currentPosition;
    
    void Start()
    {
        layoutGroup = contents.GetComponent<VerticalLayoutGroup>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Songs/");
        DirectoryInfo[] info = dir.GetDirectories();
        for (int i = 0; i < info.Length; i++)
        {
            TMP_Text text;
            freeplayItem itemComponent;
            //Button thing = Instantiate(button, contents.content.transform);
            GameObject lmao = Instantiate(item, contents.transform);
            itemComponent = lmao.GetComponent<freeplayItem>();
            itemComponent.id = i;
            text = lmao.GetComponent<TMP_Text>();
            itemComponent.songName = info[i].Name;
            text.text = info[i].Name;

            //totalHeight = totalHeight + 84;
        }
        lenght = info.Length;
    }

    private void Update()
    {
        //layoutGroup.padding.top = (207 * selectedItem) * -1;
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedItem--;
            scrollSound.Play();
            currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, currentPosition + 210, 0.2f));
            StartCoroutine(MoveList(0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItem++;
            scrollSound.Play();
            currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, currentPosition - 210, 0.2f));
            StartCoroutine(MoveList(1));
        }

        if (selectedItem > lenght -1)
        {
            selectedItem = 0;
            currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, 0, 0.35f));
            StartCoroutine(MoveList(3));
        }
        if (selectedItem < 0)
        {
            selectedItem = lenght - 1;
            currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, 210 * (lenght - 1) * -1, 0.35f));
            StartCoroutine(MoveList(2));
        }
    }

    IEnumerator MoveList(int Operation)
    {
        switch (Operation)
        {
            case 0:
                for (int i = 0; i < 26; i++)
                {
                    layoutGroup.padding.top = layoutGroup.padding.top + 8;
                    yield return null;
                }
                break;
            case 1:
                for (int i = 0; i < 26; i++)
                {
                    layoutGroup.padding.top = layoutGroup.padding.top - 8;
                    yield return null;
                }
                break;
            case 2:
                for (int i = 0; i < (208 * lenght) / (8 * lenght); i++)
                {
                    layoutGroup.padding.top = layoutGroup.padding.top - 8 * lenght;
                    yield return null;
                }
                break;
            case 3:
                for (int i = 0; i < (208 * lenght) / (8 * lenght); i++)
                {
                    layoutGroup.padding.top = layoutGroup.padding.top + 8 * lenght;
                    yield return null;
                }
                break;
        }
    }
    
    //Lmao copied from stack overflow
    public IEnumerator ChangeSomeValue(int oldValue, int newValue, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            layoutGroup.padding.top = (int)Mathf.Lerp(oldValue, newValue, t / duration);
            yield return null;
        }
        layoutGroup.padding.top = newValue;
    }

}
