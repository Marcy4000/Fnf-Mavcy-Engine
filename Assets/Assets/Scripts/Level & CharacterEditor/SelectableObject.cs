using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    public string objName;
    public Material defaultMat, selectedMat;
    public bool dragging;
    public SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    public Sprite[,] animations;
    public int nAnims = 1;
    public int[] nFramesPerAnim;
    public float debug = 2f;
    public bool debugAnim = true;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!debugAnim)
            return;

        
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayNew("idle", 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayNew("Sing Left", 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayNew("Sing Down", 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayNew("Sing Up", 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayNew("Sing Right", 0);
        }
    }

    public void OrderAnimations()
    {
        nAnims = 1;
        var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var input = frames[0].name;
        var lastName = input.TrimEnd(digits);
        LevelEditorManager.Instance.animDropdown.ClearOptions();
        for (int i = 0; i < LevelEditorManager.Instance.exportDropdowns.Length; i++)
        {
            LevelEditorManager.Instance.exportDropdowns[i].ClearOptions();
        }
        List<string> anims = new List<string>();
        anims.Clear();
        anims.Add(lastName);
        for (int i = 0; i < frames.Length; i++)
        {
            var input1 = frames[i].name;
            var currName = input1.TrimEnd(digits);
            if (string.Compare(currName, lastName) != 0)
            {
                nAnims++;
                anims.Add(currName);
            }
            lastName = currName;
        }
        LevelEditorManager.Instance.animDropdown.AddOptions(anims);
        for (int i = 0; i < LevelEditorManager.Instance.exportDropdowns.Length; i++)
        {
            LevelEditorManager.Instance.exportDropdowns[i].AddOptions(anims);
        }
        nFramesPerAnim = new int[nAnims];
        int currentAnimation = 0;
        input = frames[0].name;
        lastName = input.TrimEnd(digits);
        for (int i = 0; i < frames.Length; i++)
        {
            var input1 = frames[i].name;
            var currName = input1.TrimEnd(digits);
            if (string.Compare(currName, lastName) != 0)
            {
                currentAnimation++;
            }
            else
            {
                nFramesPerAnim[currentAnimation]++;
            }
            lastName = currName;
        }
        for (int i = 1; i < nFramesPerAnim.Length; i++)
        {
            nFramesPerAnim[i]++;
        }
        animations = new Sprite[nAnims, nFramesPerAnim.Max()];
        var currentFrame = 0;
        for (int i = 0; i < nAnims; i++)
        {
            for (int j = 0; j < nFramesPerAnim[i]; j++)
            {
                animations[i, j] = frames[currentFrame];
                currentFrame++;
            }
        }
    }

    public void PlayNew(string animID, int charID)
    {
        AnimationSystem.instance.Play(spriteRenderer, animID, charID, false);
    }

    public void PlayNew(string animID, int charID, bool loop)
    {
        AnimationSystem.instance.Play(spriteRenderer, animID, charID, loop);
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
