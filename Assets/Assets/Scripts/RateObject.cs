using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateObject : MonoBehaviour
{
    public Sprite shit, bad, good, sick;
    public SpriteRenderer spriteRenderer;

    public void SetSprite(int rating)
    {
        switch (rating)
        {
            case 1:
                spriteRenderer.sprite = sick;
                break;
            case 2:
                spriteRenderer.sprite = good;
                break;
            case 3:
                spriteRenderer.sprite = bad;
                break;
            case 4:
                spriteRenderer.sprite = shit;
                break;
        }
    }

    public void DestroyThing()
    {
        Destroy(gameObject);
    }
}
