using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider hpBar;
    public static HealthBar instance;
    public Sprite bf, bfDed, opponent, opponentDed;
    public Image playerIcon, opponentIcon;
    private RectTransform playerRec, opponentRec;
    public int hp = 50;

    private void Start()
    {
        instance = this;
        playerRec = playerIcon.GetComponent<RectTransform>();
        opponentRec = opponentIcon.GetComponent<RectTransform>();
    }

    public void Initialize()
    {
        playerIcon.sprite = bf;
        opponentIcon.sprite = opponent;
    }

    private void Update()
    {
        if (hp > 100)
        {
            hp = 100;
        }

        if (hp < 0)
        {
            hp = 0;
        }

        hpBar.value = hp;

        if (hp < 30 && bf != null)
        {
            playerIcon.sprite = bfDed;
            playerRec.sizeDelta = new Vector2(bfDed.rect.width, bfDed.rect.height);
        }
        else
        {
            playerIcon.sprite = bf;
            playerRec.sizeDelta = new Vector2(bf.rect.width, bf.rect.height);
        }

        if (hp > 70 && opponent != null)
        {
            opponentIcon.sprite = opponentDed;
            opponentRec.sizeDelta = new Vector2(opponentDed.rect.width, opponentDed.rect.height);
        }
        else
        {
            opponentIcon.sprite = opponent;
            opponentRec.sizeDelta = new Vector2(opponent.rect.width, opponent.rect.height);
        }

        if (hp <= 0)
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }

    }

    public void AddHp()
    {
        hp += 2;
    }

    public void SubtractHp()
    {
        hp -= 5;
    }
}
