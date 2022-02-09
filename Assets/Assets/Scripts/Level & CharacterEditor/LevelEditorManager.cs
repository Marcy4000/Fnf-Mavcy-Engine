using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelEditorManager : MonoBehaviour
{
    public static LevelEditorManager Instance;
    private Camera mainCamera;
    public GameObject selectedObject;
    public SelectableObject selectedSelectableObject;
    public TMP_InputField proprietiesName, xCoord, yCoord, xScale, yScale, layer;
    public GameObject prefab;
    public Vector3 oldMousePos;
    public TMP_Dropdown animDropdown, charList;
    public TMP_Dropdown[] exportDropdowns;
    public float scrollMult = 1f;
    public string defPath, ThingName, exportPath;
    public GameObject animationMenu, debugAnimMenu, warningSign;
    private bool hasSetAnimations = false;

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

        warningSign.SetActive(!hasSetAnimations);

        mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollMult;
    }

    public void LoadXmlFile(string name)
    {
        if (selectedObject == null)
            return;
        
        string path = defPath;
        exportPath = path + @"\" + name + ".xml";
        TextureAtlasXml spriteSheet = GlobalDataSfutt.ImportXml<TextureAtlasXml>(path + @"\" + name + ".xml");
        Texture2D SpriteTexture = IMG2Sprite.LoadTexture(path + @"\" + spriteSheet.imagePath);
        selectedSelectableObject.frames = new Sprite[spriteSheet.frame.Length];
        for (int i = 0; i < spriteSheet.frame.Length; i++)
        {
            selectedSelectableObject.frames[i] = IMG2Sprite.LoadSpriteSheet(SpriteTexture, new Rect(spriteSheet.frame[i].x, SpriteTexture.height - spriteSheet.frame[i].y, spriteSheet.frame[i].width, -spriteSheet.frame[i].height), new Vector2(0.5f, 1), 100f, SpriteMeshType.Tight);
            selectedSelectableObject.frames[i].name = spriteSheet.frame[i].name;
        }
        selectedSelectableObject.OrderAnimations();
    }

    public void SelectObject(GameObject _gameObject, SelectableObject objData)
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

        defPath = _path;
        //selectedSelectableObject.spriteRenderer.sprite = IMG2Sprite.LoadNewSprite(_path);
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

    public void SetThingName(string _name)
    {
        if (selectedObject == null)
            return;

        ThingName = _name;
    }

    public void SetThingVisibility()
    {
        animationMenu.SetActive(!animationMenu.activeInHierarchy);
    }

    public void SetAnimDebugMenuVisibility()
    {
        debugAnimMenu.SetActive(!debugAnimMenu.activeInHierarchy);
    }

    public void ExportFnfCharacter()
    {
        if (selectedObject == null)
            return;

        FNFCharacter character = new FNFCharacter();
        character.characterName = ThingName;
        character.xmlPath = exportPath;
        character.animations = new Dictionary<string, List<Sprite>>();
        List<Sprite> placeHolder = new List<Sprite>();
        List<Sprite> placeHolder1 = new List<Sprite>();
        List<Sprite> placeHolder2 = new List<Sprite>();
        List<Sprite> placeHolder3 = new List<Sprite>();
        List<Sprite> placeHolder4 = new List<Sprite>();
        placeHolder.Clear();
        for (int i = 0; i < selectedSelectableObject.nFramesPerAnim[exportDropdowns[0].value]; i++)
        {
            placeHolder.Add(selectedSelectableObject.animations[exportDropdowns[0].value, i]);
        }
        character.animations.Add("idle", placeHolder);
        
        placeHolder1.Clear();
        for (int i = 0; i < selectedSelectableObject.nFramesPerAnim[exportDropdowns[1].value]; i++)
        {
            placeHolder1.Add(selectedSelectableObject.animations[exportDropdowns[1].value, i]);
        }
        character.animations.Add("Sing Left", placeHolder1);

        placeHolder2.Clear();
        for (int i = 0; i < selectedSelectableObject.nFramesPerAnim[exportDropdowns[2].value]; i++)
        {
            placeHolder2.Add(selectedSelectableObject.animations[exportDropdowns[2].value, i]);
        }
        character.animations.Add("Sing Down", placeHolder2);

        placeHolder3.Clear();
        for (int i = 0; i < selectedSelectableObject.nFramesPerAnim[exportDropdowns[3].value]; i++)
        {
            placeHolder3.Add(selectedSelectableObject.animations[exportDropdowns[3].value, i]);
        }
        character.animations.Add("Sing Up", placeHolder3);

        placeHolder4.Clear();
        for (int i = 0; i < selectedSelectableObject.nFramesPerAnim[exportDropdowns[4].value]; i++)
        {
            placeHolder4.Add(selectedSelectableObject.animations[exportDropdowns[4].value, i]);
        }
        character.animations.Add("Sing Right", placeHolder4);
        character.icons[0] = IMG2Sprite.LoadNewSprite(defPath + @"\icon.png");
        character.icons[1] = IMG2Sprite.LoadNewSprite(defPath + @"\icon-ded.png");

        GlobalDataSfutt.customCharacters.Add(character);
        hasSetAnimations = true;
    }

    public void SaveFnfCharacter()
    {
        string destination = defPath + @"\character.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        ExportableFnfCharacter data = new ExportableFnfCharacter();
        data.animations = new Dictionary<string, List<string>>();
        data.animations.Clear();
        data.xmlPath = GlobalDataSfutt.customCharacters[charList.value].xmlPath;
        data.characterName = GlobalDataSfutt.customCharacters[charList.value].characterName;
        List<Sprite> anims;
        List<string> animsNames = new List<string>();
        List<string> animsNames1 = new List<string>();
        List<string> animsNames2 = new List<string>();
        List<string> animsNames3 = new List<string>();
        List<string> animsNames4 = new List<string>();
        animsNames.Clear();
        GlobalDataSfutt.customCharacters[charList.value].animations.TryGetValue("idle", out anims);
        for (int i = 0; i < anims.Count; i++)
        {
            animsNames.Add(anims[i].name);
        }
        data.animations.Add("idle", animsNames);
        
        animsNames1.Clear();
        GlobalDataSfutt.customCharacters[charList.value].animations.TryGetValue("Sing Left", out anims);
        for (int i = 0; i < anims.Count; i++)
        {
            animsNames1.Add(anims[i].name);
        }
        data.animations.Add("Sing Left", animsNames1);

        animsNames2.Clear();
        GlobalDataSfutt.customCharacters[charList.value].animations.TryGetValue("Sing Down", out anims);
        for (int i = 0; i < anims.Count; i++)
        {
            animsNames2.Add(anims[i].name);
        }
        data.animations.Add("Sing Down", animsNames2);

        animsNames3.Clear();
        GlobalDataSfutt.customCharacters[charList.value].animations.TryGetValue("Sing Up", out anims);
        for (int i = 0; i < anims.Count; i++)
        {
            animsNames3.Add(anims[i].name);
        }
        data.animations.Add("Sing Up", animsNames3);

        animsNames4.Clear();
        GlobalDataSfutt.customCharacters[charList.value].animations.TryGetValue("Sing Right", out anims);
        for (int i = 0; i < anims.Count; i++)
        {
            animsNames4.Add(anims[i].name);
        }
        data.animations.Add("Sing Right", animsNames4);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }
}
