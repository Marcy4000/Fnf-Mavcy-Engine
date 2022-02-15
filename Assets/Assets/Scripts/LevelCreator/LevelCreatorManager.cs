using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCreatorManager : MonoBehaviour
{
    public static LevelCreatorManager Instance;
    private Camera mainCamera;
    public GameObject selectedObject;
    public LevelObject selectedSelectableObject;
    public TMP_InputField proprietiesName, xCoord, yCoord, xScale, yScale, layer, exportPath;
    public GameObject prefab;
    public Vector3 oldMousePos;
    public float scrollMult = 1f;
    public bool moveObjectOnSelect = true;
    public Transform bfPos, gfPos, enemyPos, bfCamPos, enemyCamPos;

    private void Start()
    {
        Instance = this;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (selectedSelectableObject != null && selectedSelectableObject.dragging)
        {
            xCoord.text = Math.Round(selectedObject.transform.position.x, 3).ToString();
            yCoord.text = Math.Round(selectedObject.transform.position.y, 3).ToString();
        }
        if (Input.GetMouseButtonDown(2))
        {
            oldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            mainCamera.transform.position += oldMousePos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }

        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollMult;
    }

    public void SelectObject(GameObject _gameObject, LevelObject objData)
    {
        selectedObject = _gameObject;
        selectedSelectableObject = objData;
        proprietiesName.text = objData.objName;
        xCoord.text = Math.Round(selectedObject.transform.position.x, 3).ToString();
        yCoord.text = Math.Round(selectedObject.transform.position.y, 3).ToString();
        xScale.text = selectedObject.transform.localScale.x.ToString();
        yScale.text = selectedObject.transform.localScale.y.ToString();
        layer.text = objData.spriteRenderer.sortingOrder.ToString();
    }

    public void TestCameraThing()
    {
        mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        mainCamera.orthographicSize = 5;
    }

    public void SetName(string _name)
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        selectedSelectableObject.objName = _name;
    }

    public void SetPosition()
    {
        if (selectedObject == null)
            return;

        selectedObject.transform.position = new Vector3(float.Parse(xCoord.text), float.Parse(yCoord.text), 0);
    }

    public void SetScale()
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        selectedObject.transform.localScale = new Vector3(float.Parse(xScale.text), float.Parse(yScale.text), 1);
    }

    public void LoadCustomSprite(string _path)
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        if (string.IsNullOrWhiteSpace(_path))
            return;

        selectedSelectableObject.spriteRenderer.sprite = IMG2Sprite.LoadNewSprite(_path);
        selectedSelectableObject.imagePath = _path;
    }

    public void SetMoveObject(bool _value)
    {
        moveObjectOnSelect = _value;
    }

    public void SetLayer()
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        selectedSelectableObject.spriteRenderer.sortingOrder = int.Parse(layer.text);
    }

    public void AddNewObject()
    {
        Instantiate(prefab);
    }

    public void DeleteObject()
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        Destroy(selectedObject);
    }

    public void ButtonExport()
    {
        if (string.IsNullOrWhiteSpace(exportPath.text))
            return;
        
        ExportLevel(exportPath.text);
    }

    private void ExportLevel(string _path)
    {
        Level level = new Level();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LevelObject");
        LevelObject[] objectsThings = new LevelObject[objects.Length];
        for (int i = 0; i < objectsThings.Length; i++)
        {
            objectsThings[i] = objects[i].GetComponent<LevelObject>();
        }
        level.levelObjects = new List<ExportableLevelObject>();
        level.levelObjects.Clear();
        for (int i = 0; i < objectsThings.Length; i++)
        {
            ExportableLevelObject levelObject = new ExportableLevelObject();
            levelObject._position.x = objectsThings[i].transform.position.x;
            levelObject._position.y = objectsThings[i].transform.position.y;
            levelObject._scale.x = objectsThings[i].transform.localScale.x;
            levelObject._scale.y = objectsThings[i].transform.localScale.y;
            levelObject.layer = objectsThings[i].spriteRenderer.sortingOrder;
            string output = Path.GetFileName(objectsThings[i].imagePath);
            levelObject.imageName = output;
            level.levelObjects.Add(levelObject);
        }
        level.bfPos.x = bfPos.position.x;
        level.bfPos.y = bfPos.position.y;
        level.gfPos.x = gfPos.position.x;
        level.gfPos.y = gfPos.position.y;
        level.enemyPos.x = enemyPos.position.x;
        level.enemyPos.y = enemyPos.position.y;
        level.bfCamPos.x = bfCamPos.position.x;
        level.bfCamPos.y = bfCamPos.position.y;
        level.enemyCamPos.x = enemyCamPos.position.x;
        level.enemyCamPos.y = enemyCamPos.position.y;
        level.levelName = "";
        level.stagePath = "";

        string levelStuff = JsonUtility.ToJson(level);
        File.WriteAllText(_path + @"\stage.json", levelStuff);
    }

}

[Serializable]
public class Level
{
    public string levelName = "Sus Stage", stagePath = "";
    public List<ExportableLevelObject> levelObjects;
    public SerializableVector2 bfPos = new SerializableVector2();
    public SerializableVector2 gfPos = new SerializableVector2();
    public SerializableVector2 enemyPos = new SerializableVector2();
    public SerializableVector2 enemyCamPos = new SerializableVector2();
    public SerializableVector2 bfCamPos = new SerializableVector2();
}

[Serializable]
public class ExportableLevelObject
{
    public SerializableVector2 _position = new SerializableVector2();
    public SerializableVector2 _scale = new SerializableVector2();
    public int layer;
    public string imageName;
}