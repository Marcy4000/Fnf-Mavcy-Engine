using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuntimeLevelObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] frames;
    private Sprite[,] animations;
    public Dictionary<string, Sprite[]> sprites = new Dictionary<string, Sprite[]>();
    public int nAnims = 1;
    public int[] nFramesPerAnim;
    public int defAnim = 0;
    private List<string> anims = new List<string>();
    private Coroutine lastRoutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Songdata.OnBeat += Bop;
    }

    private void OnDisable()
    {
        Songdata.OnBeat -= Bop;
    }

    public void Bop()
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine(PlayAnimation(anims[defAnim], false));
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
        nAnims = 1;
        var digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var input = frames[0].name;
        var lastName = input.TrimEnd(digits);
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
}
