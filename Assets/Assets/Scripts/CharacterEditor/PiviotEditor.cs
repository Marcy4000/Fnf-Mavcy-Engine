using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PiviotEditor : MonoBehaviour
{
    public static PiviotEditor instance;
    public SpriteRenderer spriteRenderer;
    public Dictionary<string, List<SerializableVector2>> piviots = new Dictionary<string, List<SerializableVector2>>();
    public List<SerializableVector2> reusableList = new List<SerializableVector2>();
    public int selectedFrame, maxFrames;
    public TMP_Text text;
    public static bool editingPiviot;
    public GameObject piviotMenu;
    private string selectedAnim = "idle";
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            editingPiviot = !editingPiviot;
            ChangeMode(editingPiviot);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(piviots.ContainsKey(selectedAnim));
        }
        text.text = $"Frames:{selectedFrame + 1}/{maxFrames}";
    }

    private void ChangeMode(bool value)
    {
        switch (value)
        {
            case false:
                LevelEditorManager.Instance.selectedObject.transform.parent = null;
                spriteRenderer.enabled = false;
                piviotMenu.SetActive(false);
                break;
            case true:
                LevelEditorManager.Instance.selectedObject.transform.parent = transform;
                spriteRenderer.enabled = true;
                piviotMenu.SetActive(true);
                break;
        }
    }

    public void ChangeFrame(bool subtract)
    {
        if (!subtract)
        {
            if (selectedFrame < maxFrames)
            {
                selectedFrame++;
                AnimationSystem.PlayFrame(LevelEditorManager.Instance.selectedSelectableObject.spriteRenderer, selectedAnim, 0, selectedFrame);
            }
        }
        else
        {
            if (selectedFrame > 0)
            {
                selectedFrame--;
                AnimationSystem.PlayFrame(LevelEditorManager.Instance.selectedSelectableObject.spriteRenderer, selectedAnim, 0, selectedFrame);
            }
        }
    }

    public void ChangeAnimation(int value)
    {
        selectedFrame = 0;
        switch (value)
        {
            case 0:
                selectedAnim = "idle";
                break;
            case 1:
                selectedAnim = "Sing Left";
                break;
            case 2:
                selectedAnim = "Sing Down";
                break;
            case 3:
                selectedAnim = "Sing Up";
                break;
            case 4:
                selectedAnim = "Sing Right";
                break;
        }
        maxFrames = AnimationSystem.GetNumberOfFrames(selectedAnim, 0);
        reusableList.Clear();
        for (int i = 0; i < maxFrames; i++)
        {
            reusableList.Add(new SerializableVector2());
        }
    }

    public void SavePiviot()
    {
        if (!piviots.ContainsKey(selectedAnim))
        {
            piviots.Add(selectedAnim, reusableList);
        }
    }

    public void AddPiviot()
    {
        reusableList[selectedFrame].x = LevelEditorManager.Instance.selectedObject.transform.localPosition.x;
        reusableList[selectedFrame].y = LevelEditorManager.Instance.selectedObject.transform.localPosition.y;
    }
}

[Serializable]
public class SerializableVector2
{
    public float x;
    public float y;
}
