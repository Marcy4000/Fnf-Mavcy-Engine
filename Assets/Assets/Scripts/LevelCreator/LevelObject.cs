using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public string objName;
    public bool dragging, isStaticObject = false;
    public SpriteRenderer spriteRenderer;
    public string imagePath = "";
    public Sprite[] frames;
    private Sprite[,] animations;
    public Dictionary<string, Sprite[]> sprites = new Dictionary<string, Sprite[]>();
    public int nAnims = 1;
    public int[] nFramesPerAnim;
    public int defAnim = 0;
    public bool hasAnimation = false;
    private List<string> anims = new List<string>();
    private Coroutine lastRoutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Play(int anim)
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine(PlayAnimation(anims[anim], false));
    }

    private IEnumerator PlayAnimation(string animName, bool _shouldLoop)
    {
        Sprite[] frames;
        Debug.Log(animName + " " + sprites.ContainsKey(animName));
        sprites.TryGetValue(animName, out frames);
        switch (_shouldLoop)
        {
            case true:
                while (_shouldLoop)
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        spriteRenderer.sprite = frames[i];
                        yield return new WaitForSeconds(0.025f);
                    }
                }
                break;
            case false:
                for (int i = 0; i < frames.Length; i++)
                {
                    spriteRenderer.sprite = frames[i];
                    yield return new WaitForSeconds(0.025f);
                }
                break;
        }
    }

    public void OrderAnimations()
    {
        hasAnimation = true;
        nAnims = 1;
        var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var input = frames[0].name;
        var lastName = input.TrimEnd(digits);
        LevelCreatorManager.Instance.animDropdown.ClearOptions();
        /*for (int i = 0; i < LevelCreatorManager.Instance.exportDropdowns.Length; i++)
        {
            LevelCreatorManager.Instance.exportDropdowns[i].ClearOptions();
        }*/
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
        LevelCreatorManager.Instance.animDropdown.AddOptions(anims);
        /*for (int i = 0; i < LevelCreatorManager.Instance.exportDropdowns.Length; i++)
        {
            LevelCreatorManager.Instance.exportDropdowns[i].AddOptions(anims);
        }*/
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
        frames = null;
        TurnToDictionary();
    }

    private void TurnToDictionary()
    {
        sprites.Clear();
        for (int i = 0; i < animations.GetLength(0); i++)
        {
            Sprite[] _frames = new Sprite[nFramesPerAnim[i]];
            for (int j = 0; j < animations.GetLength(1); j++)
            {
                _frames[j] = animations[i, j];
            }
            char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string input = _frames[0].name;
            string lastName = input.TrimEnd(digits);
            sprites.Add(lastName, _frames);
        }
    }

    private void OnMouseDown()
    {
        if (LevelCreatorManager.Instance.selectingSprite || !LevelCreatorManager.Instance.canSelectStaticObjects && isStaticObject)
            return;
        
        LevelCreatorManager.Instance.SelectObject(gameObject, this);
        LevelCreatorManager.Instance.animDropdown.ClearOptions();
        LevelCreatorManager.Instance.animDropdown.AddOptions(anims);
    }

    private void OnMouseDrag()
    {
        if (!LevelCreatorManager.Instance.moveObjectOnSelect || LevelCreatorManager.Instance.selectingSprite || !LevelCreatorManager.Instance.canSelectStaticObjects && isStaticObject)
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
