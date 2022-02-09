using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FreeplayMenu : MonoBehaviour
{
    public Transform contents;
    //public Button button;
    public GameObject item;
    public Animator blackFade;
    public AudioSource scrollSound;
    private VerticalLayoutGroup layoutGroup;
    public Image background;
    public TMP_Text stats;
    public freeplayItem[] items;


    //public int totalHeight;
    public int selectedItem;
    private int lenght;
    //private int currentPosition;
    
    void Start()
    {
        layoutGroup = contents.GetComponent<VerticalLayoutGroup>();
        DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(".") + "/data" + "/Songs/");
        DirectoryInfo[] info = dir.GetDirectories();
        items = new freeplayItem[info.Length];
        for (int i = 0; i < info.Length; i++)
        {
            TMP_Text text;
            freeplayItem itemComponent;
            GameObject lmao = Instantiate(item, contents.transform);
            itemComponent = lmao.GetComponent<freeplayItem>();
            itemComponent.id = i;
            itemComponent.songStats = LoadFile(Path.GetFullPath(".") + "/data" + "/Songs/" + info[i].Name + "/stats.dat");
            items[i] = itemComponent;
            text = lmao.GetComponent<TMP_Text>();
            itemComponent.songName = info[i].Name;
            text.text = info[i].Name;
        }
        lenght = info.Length;
    }

    public PlayerStat LoadFile(string _destination)
    {
        string destination;

        if (!string.IsNullOrWhiteSpace(_destination))
        {
            destination = _destination;
        }
        else
        {
            destination = Path.GetFullPath(".") + "/data/Songs/" + GlobalDataSfutt.songNameToLoad + "/stats.dat";
        }
        
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        PlayerStat data = (PlayerStat)bf.Deserialize(file);
        file.Close();
        return data;

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
            //currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, currentPosition + 210, 0.2f));
            StartCoroutine(MoveList(0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedItem++;
            scrollSound.Play();
            //currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, currentPosition - 210, 0.2f));
            StartCoroutine(MoveList(1));
        }

        if (selectedItem > lenght -1)
        {
            selectedItem = 0;
            //currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, 0, 0.35f));
            StartCoroutine(MoveList(3));
        }
        if (selectedItem < 0)
        {
            selectedItem = lenght - 1;
            //currentPosition = layoutGroup.padding.top;
            //StartCoroutine(ChangeSomeValue(currentPosition, 210 * (lenght - 1) * -1, 0.35f));
            StartCoroutine(MoveList(2));
        }

        stats.text = $"Stats:\nScore:{items[selectedItem].songStats.currentScore}\nMisses:{items[selectedItem].songStats.missedHits}\nCombo:{items[selectedItem].songStats.highestSickCombo}";
    }

    IEnumerator MoveList(int Operation)
    {
        switch (Operation)
        {
            case 0:
                for (int i = 0; i < 26; i++)
                {
                    layoutGroup.padding.top += 8;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 1:
                for (int i = 0; i < 26; i++)
                {
                    layoutGroup.padding.top -= 8;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 2:
                for (int i = 0; i < (208 * lenght) / (8 * lenght); i++)
                {
                    layoutGroup.padding.top -= 8 * lenght;
                    yield return new WaitForSeconds(0.01f);
                }
                break;
            case 3:
                for (int i = 0; i < (208 * lenght) / (8 * lenght); i++)
                {
                    layoutGroup.padding.top += 8 * lenght;
                    yield return new WaitForSeconds(0.01f);
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
