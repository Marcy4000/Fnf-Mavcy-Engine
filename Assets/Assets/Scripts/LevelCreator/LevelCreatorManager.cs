using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;

public class LevelCreatorManager : MonoBehaviour
{
    public static LevelCreatorManager Instance;
    private Camera mainCamera;
    public GameObject selectedObject;
    public LevelObject selectedSelectableObject;
    public TMP_InputField proprietiesName, xCoord, yCoord, xScale, yScale, layer, exportPath, imageName;
    public TMP_Dropdown animDropdown;
    public GameObject prefab;
    public Vector3 oldMousePos;
    public float scrollMult = 1f;
    public bool moveObjectOnSelect = true, selectingSprite = false, canSelectStaticObjects = true;
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

    public void SetCanSelectThing(bool value)
    {
        canSelectStaticObjects = value;
    }

    public void SetPosition()
    {
        if (selectedObject == null)
            return;

        selectedObject.transform.position = new Vector3(float.Parse(xCoord.text), float.Parse(yCoord.text), 0);
    }

    public void PlayAnimatioThing()
    {
        if (selectedSelectableObject == null)
            return;

        selectedSelectableObject.Play(animDropdown.value);
    }

    public void LoadXmlFile()
    {
        if (selectedObject == null)
            return;

        if (string.IsNullOrWhiteSpace(imageName.text))
            return;

        StartCoroutine(ShowLoadXmlCoroutine(imageName.text));
    }

    IEnumerator ShowLoadXmlCoroutine(string _name)
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, null, "Select Sprites Folder", "Select");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            string _path = FileBrowser.Result[0];
            Debug.Log(_path);
            FinishLoading(_path, _name);
            selectingSprite = false;
        }
        else
        {
            selectingSprite = false;
        }
    }

    private void FinishLoading(string path, string _name)
    {
        TextureAtlasXml spriteSheet = GlobalDataSfutt.ImportXml<TextureAtlasXml>(path + @"\" + _name + ".xml");
        Texture2D SpriteTexture = IMG2Sprite.LoadTexture(path + @"\" + spriteSheet.imagePath);
        selectedSelectableObject.frames = new Sprite[spriteSheet.frame.Length];
        for (int i = 0; i < spriteSheet.frame.Length; i++)
        {
            selectedSelectableObject.frames[i] = IMG2Sprite.LoadSpriteSheet(SpriteTexture, new Rect(spriteSheet.frame[i].x, SpriteTexture.height - spriteSheet.frame[i].y, spriteSheet.frame[i].width, -spriteSheet.frame[i].height), new Vector2(0.5f, 1), 100f, SpriteMeshType.Tight);
            selectedSelectableObject.frames[i].name = spriteSheet.frame[i].name;
        }
        selectedSelectableObject.OrderAnimations();
        selectedSelectableObject.imagePath = path + @"\" + spriteSheet.imagePath;
    }

    public void SetScale()
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        selectedObject.transform.localScale = new Vector3(float.Parse(xScale.text), float.Parse(yScale.text), 1);
    }

    public void LoadCustomSprite()
    {
        if (selectedObject == null || selectedSelectableObject.isStaticObject)
            return;

        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.SetDefaultFilter(".png");
        selectingSprite = true;

        StartCoroutine(ShowLoadSpriteCoroutine());
    }

    IEnumerator ShowLoadSpriteCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Select Image", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            string _path = FileBrowser.Result[0];
            Debug.Log(_path);
            selectedSelectableObject.spriteRenderer.sprite = IMG2Sprite.LoadNewSprite(_path);
            selectedSelectableObject.imagePath = _path;
            selectingSprite = false;
        }
        else
        {
            selectingSprite = false;
        }
    }

    public void SetMoveObject(bool _value)
    {
        moveObjectOnSelect = _value;
    }

    public void SetDefaultAnimation()
    {
        selectedSelectableObject.defAnim = animDropdown.value;
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

        selectingSprite = true;

        StartCoroutine(ShowSelectExportPathCoroutine());
    }

    IEnumerator ShowSelectExportPathCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, null, "Select Export Path", "Select");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            string path = FileBrowser.Result[0];
            ExportLevel(path);
            selectingSprite = false;
        }
        else
        {
            selectingSprite = false;
        }
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
            levelObject.defID = objectsThings[i].defAnim;
            levelObject.hasAnimation = objectsThings[i].hasAnimation;
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
    public int layer, defID;
    public bool hasAnimation;
    public string imageName;
}