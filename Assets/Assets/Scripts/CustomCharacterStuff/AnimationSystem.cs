using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{
    public static AnimationSystem instance;
    private Coroutine lastRoutine;
    
    private void Start()
    {
        instance = this;
    }

    public void Play(SpriteRenderer renderer, string animName, int character, bool shouldLoop)
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine(PlayAnimation(renderer, character, animName, shouldLoop));
    }

    public void Play(SpriteRenderer renderer, Sprite[] frames, bool shouldLoop)
    {
        if (lastRoutine != null)
            StopCoroutine(lastRoutine);

        lastRoutine = StartCoroutine(PlayAnimation(renderer, frames, shouldLoop));
    }

    public static void PlayFrame(SpriteRenderer renderer, string animName, int character, int frame)
    {
        List<Sprite> _frames;
        GlobalDataSfutt.customCharacters[character].animations.TryGetValue(animName, out _frames);
        renderer.sprite = _frames[frame];
    }

    public static int GetNumberOfFrames(string animName, int character)
    {
        List<Sprite> _frames;
        GlobalDataSfutt.customCharacters[character].animations.TryGetValue(animName, out _frames);
        return _frames.Count - 1;
    }

    private IEnumerator PlayAnimation(SpriteRenderer _renderer, int _character, string _animName, bool _shouldLoop)
    {
        List<Sprite> _frames;
        //List<SerializableVector2> _piviots;
        GlobalDataSfutt.customCharacters[_character].animations.TryGetValue(_animName, out _frames);
        //GlobalDataSfutt.customCharacters[_character].piviots.TryGetValue(_animName, out _piviots);

        switch (_shouldLoop)
        {
            case true:
                while (_shouldLoop)
                {
                    for (int i = 0; i < _frames.Count; i++)
                    {
                        _renderer.sprite = _frames[i];
                        //_renderer.transform.localPosition = new Vector3(_piviots[i].x, _piviots[i].y);
                        yield return new WaitForSeconds(0.025f);
                    }
                }
                break;
            case false:
                for (int i = 0; i < _frames.Count; i++)
                {
                    _renderer.sprite = _frames[i];
                    //_renderer.transform.localPosition = new Vector3(_piviots[i].x, _piviots[i].y);
                    yield return new WaitForSeconds(0.025f);
                }
                break;
        }
    }

    private IEnumerator PlayAnimation(SpriteRenderer _renderer, Sprite[] frames, bool _shouldLoop)
    {
        switch (_shouldLoop)
        {
            case true:
                while (_shouldLoop)
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        _renderer.sprite = frames[i];
                        yield return new WaitForSeconds(0.025f);
                    }
                }
                break;
            case false:
                for (int i = 0; i < frames.Length; i++)
                {
                    _renderer.sprite = frames[i];
                    yield return new WaitForSeconds(0.025f);
                }
                break;
        }
    }
}
