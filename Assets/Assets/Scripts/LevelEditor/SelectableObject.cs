using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public string objName;
    public Material defaultMat, selectedMat;
    public bool dragging;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (LevelEditorManager.Instance.selectedObject == gameObject)
        {
            spriteRenderer.material = selectedMat;
        }
        else
        {
            spriteRenderer.material = defaultMat;
        }
    }

    private void OnMouseDown()
    {
        LevelEditorManager.Instance.SelectObject(gameObject, this);
    }

    private void OnMouseDrag()
    {
        dragging = true;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
