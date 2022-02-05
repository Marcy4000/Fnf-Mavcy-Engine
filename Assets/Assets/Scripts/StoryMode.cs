using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class StoryMode : MonoBehaviour
{
    public VerticalLayoutGroup weeksLayout;
    public TMP_Text weekScore, weekName, weekTracks;
    public Image difficultyImage;
    public Sprite[] difficulties;
    public Vector2[] spriteSizes;
    public int selectedWeek = 0, selectedDifficulty = 0;
    public WeekData[] weeks;
    public GameObject weekObject;
    public Animator blackFade;
    public AudioSource scrollSound;

    private void Start()
    {
        selectedWeek = 0;
        weeksLayout.padding.top = 0;
        selectedDifficulty = 0;
        difficultyImage.sprite = difficulties[selectedDifficulty];
        difficultyImage.rectTransform.sizeDelta = spriteSizes[selectedDifficulty];
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Weeks/");
        DirectoryInfo[] info = dir.GetDirectories();
        weeks = new WeekData[info.Length];
        for (int i = 0; i < info.Length; i++)
        {
            Image image = Instantiate(weekObject, weeksLayout.transform).GetComponent<Image>();
            image.sprite = IMG2Sprite.LoadNewSprite(Application.persistentDataPath + "/Weeks/" + info[i].Name + "/img.png");
            image.rectTransform.sizeDelta = new Vector2(539, 134);
            weeks[i] = image.gameObject.GetComponent<WeekData>();
            WeekData week = image.gameObject.GetComponent<WeekData>();
            string[] lines = File.ReadAllLines(Application.persistentDataPath + "/Weeks/" + info[i].Name + "/weekData.txt");
            week.weekName = lines[0];
            week.tracks = new string[lines.Length - 1];
            for (int j = 1; j < lines.Length; j++)
            {
                week.tracks[j - 1] = lines[j];
            }
        }
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedDifficulty--;
            if (selectedDifficulty < 0)
            {
                selectedDifficulty = 2;
            }
            difficultyImage.sprite = difficulties[selectedDifficulty];
            difficultyImage.rectTransform.sizeDelta = spriteSizes[selectedDifficulty];
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedDifficulty++;
            if (selectedDifficulty > 2)
            {
                selectedDifficulty = 0;
            }
            difficultyImage.sprite = difficulties[selectedDifficulty];
            difficultyImage.rectTransform.sizeDelta = spriteSizes[selectedDifficulty];
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && selectedWeek < weeks.Length - 1 && weeks.Length != 0)
        {
            selectedWeek++;
            scrollSound.Play();
            UpdateUI();
            StartCoroutine(Scroll(1));
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedWeek > 0)
        {
            selectedWeek--;
            scrollSound.Play();
            UpdateUI();
            StartCoroutine(Scroll(0));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadWeek());
        }

    }

    private IEnumerator LoadWeek()
    {
        GlobalDataSfutt.isStoryMode = true;
        GlobalDataSfutt.weekSongs = weeks[selectedWeek].tracks;
        GlobalDataSfutt.currentWeekSong = 0;
        GlobalDataSfutt.songNameToLoad = GlobalDataSfutt.weekSongs[0];
        blackFade.SetTrigger("Transition");

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(1);
    }

    private void UpdateUI()
    {
        weekName.text = weeks[selectedWeek].weekName;
        string s = string.Join(".", weeks[selectedWeek].tracks);
        s = s.Replace(".", "\n");
        weekTracks.text = "Tracks\n\n" + s;
    }

    private IEnumerator Scroll(int mode = 0)
    {
        switch (mode)
        {
            case 0:
                for (int i = 0; i < 38; i++)
                {
                    weeksLayout.padding.top += 5;
                    weeksLayout.enabled = false;
                    weeksLayout.enabled = true;
                    yield return null;
                }
                break;
            case 1:
                for (int i = 0; i < 38; i++)
                {
                    weeksLayout.padding.top -= 5;
                    weeksLayout.enabled = false;
                    weeksLayout.enabled = true;
                    yield return null;
                }
                break;
        }
    }

}
