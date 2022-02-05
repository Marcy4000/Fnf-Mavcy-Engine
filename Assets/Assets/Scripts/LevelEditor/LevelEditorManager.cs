using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager Instance;
    public GameObject selectedObject;
    public SelectableObject selectedSelectableObject;
    public TMP_InputField proprietiesName, xCoord, yCoord, xScale, yScale, layer;
    public GameObject prefab;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (selectedSelectableObject != null && selectedSelectableObject.dragging)
        {
            xCoord.text = selectedObject.transform.position.x.ToString();
            yCoord.text = selectedObject.transform.position.y.ToString();
        }
    }

    public void SelectObject(GameObject _gameObject, SelectableObject objData)
    {
        selectedObject = _gameObject;
        selectedSelectableObject = objData;
        proprietiesName.text = objData.objName;
        xCoord.text = selectedObject.transform.position.x.ToString();
        yCoord.text = selectedObject.transform.position.y.ToString();
        xScale.text = selectedObject.transform.localScale.x.ToString();
        yScale.text = selectedObject.transform.localScale.y.ToString();
        layer.text = objData.spriteRenderer.sortingOrder.ToString();
    }

    public void SetName(string _name)
    {
        if (selectedObject == null)
            return;

        selectedObject.GetComponent<SelectableObject>().objName = _name;
    }

    public void SetPosition()
    {
        if (selectedObject == null)
            return;

        selectedObject.transform.position = new Vector3(float.Parse(xCoord.text), float.Parse(yCoord.text), 0);
    }

    public void SetScale()
    {
        if (selectedObject == null)
            return;

        selectedObject.transform.localScale = new Vector3(float.Parse(xScale.text), float.Parse(yScale.text), 1);
    }

    public void LoadCustomSprite(string _path)
    {
        if (selectedObject == null)
            return;

        selectedSelectableObject.spriteRenderer.sprite = IMG2Sprite.LoadNewSprite(_path);
    }

    public void SetLayer()
    {
        if (selectedObject == null)
            return;

        selectedSelectableObject.spriteRenderer.sortingOrder = int.Parse(layer.text);
    }

    public void AddNewObject()
    {
        Instantiate(prefab);
    }

    public void DeleteObject()
    {
        if (selectedObject == null)
            return;

        Destroy(selectedObject);
    }
}
