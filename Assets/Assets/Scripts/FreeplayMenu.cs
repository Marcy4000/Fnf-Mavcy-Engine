using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

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
    public List<freeplayItem> items;
    public int selectedDifficulty = 0;
    private Diffuculty diffuculty;


    //public int totalHeight;
    public int selectedItem;
    private int lenght;
    //private int currentPosition;
    
    void Start()
    {
        layoutGroup = contents.GetComponent<VerticalLayoutGroup>();
        DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(".") + "/data" + "/Songs/");
        DirectoryInfo[] info = dir.GetDirectories();
        items = new List<freeplayItem>();
        items.Clear();
        int oldI = 0;
        for (int i = 0; i < info.Length; i++)
        {
            TMP_Text text;
            freeplayItem itemComponent;
            GameObject lmao = Instantiate(item, contents.transform);
            itemComponent = lmao.GetComponent<freeplayItem>();
            itemComponent.id = i;
            itemComponent.songStats = LoadFile(Path.GetFullPath(".") + "/data" + "/Songs/" + info[i].Name + "/stats.dat");
            items.Add(itemComponent);
            text = lmao.GetComponent<TMP_Text>();
            itemComponent.songName = info[i].Name;
            itemComponent.songPath = Path.GetFullPath(".") + @"\data\Songs\";
            text.text = info[i].Name;
            oldI = i;
        }
        lenght = info.Length;
        oldI++;
        for (int i = 0; i < GlobalDataSfutt.mods.Count; i++)
        {
            Mod mod = GlobalDataSfutt.mods[i];
            for (int j = 0; j < mod.songNames.Count; j++)
            {
                TMP_Text text;
                freeplayItem itemComponent;
                GameObject lmao = Instantiate(item, contents.transform);
                itemComponent = lmao.GetComponent<freeplayItem>();
                itemComponent.id = oldI;
                itemComponent.songStats = LoadFile(mod.modPath + @"\Songs\" + mod.songNames[j] + @"\stats.dat");
                items.Add(itemComponent);
                text = lmao.GetComponent<TMP_Text>();
                itemComponent.songName = mod.songNames[j];
                itemComponent.songPath = mod.modPath + @"\Songs\";
                text.text = mod.songNames[j];
                lenght++;
                oldI++;
            }
        }
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedDifficulty--;
            if (selectedDifficulty < 0)
            {
                selectedDifficulty = 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedDifficulty++;
            if (selectedDifficulty > 2)
            {
                selectedDifficulty = 0;
            }
        }

        diffuculty = (Diffuculty)selectedDifficulty;

        stats.text = $"Stats:\nScore:{items[selectedItem].songStats.currentScore}\nMisses:{items[selectedItem].songStats.missedHits}\nCombo:{items[selectedItem].songStats.highestSickCombo}\nSelected Difficulty:\n{diffuculty}";
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
