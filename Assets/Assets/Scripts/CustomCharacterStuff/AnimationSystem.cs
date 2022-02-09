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

    private IEnumerator PlayAnimation(SpriteRenderer _renderer, int _character, string _animName, bool _shouldLoop)
    {
        List<Sprite> _frames;

        switch (_shouldLoop)
        {
            case true:
                GlobalDataSfutt.customCharacters[_character].animations.TryGetValue(_animName, out _frames);
                while (_shouldLoop)
                {
                    for (int i = 0; i < _frames.Count; i++)
                    {
                        _renderer.sprite = _frames[i];
                        yield return new WaitForSeconds(0.025f);
                    }
                }
                break;
            case false:
                GlobalDataSfutt.customCharacters[_character].animations.TryGetValue(_animName, out _frames);
                for (int i = 0; i < _frames.Count; i++)
                {
                    _renderer.sprite = _frames[i];
                    yield return new WaitForSeconds(0.025f);
                }
                break;
        }
    }
}
