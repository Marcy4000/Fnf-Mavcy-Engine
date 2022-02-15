using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public string objName;
    public bool dragging, isStaticObject = false;
    public SpriteRenderer spriteRenderer;
    public string imagePath = "";

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        LevelCreatorManager.Instance.SelectObject(gameObject, this);
    }

    private void OnMouseDrag()
    {
        if (!LevelCreatorManager.Instance.moveObjectOnSelect)
        {
            return;
        }
        
        dragging = true;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
