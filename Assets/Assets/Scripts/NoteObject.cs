using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private float _scrollSpeed;
    private SpriteRenderer _sprite;
    private Transform cameraTransform;
    private float oldX = 0f;

    public float strumTime;
    private LoadSong _song;
    public bool mustHit;
    public bool susNote;
    public int type;
    public bool dummyNote = true;
    public bool lastSusNote = false;
    public int layer;
    public float currentStrumTime;
    public float currentStopwatch;

    public float susLength;
    public float ScrollSpeed
    {
        get => _scrollSpeed * 100;
        set => _scrollSpeed = value / 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        _song = LoadSong.instance;
        _sprite = GetComponentInChildren<SpriteRenderer>();
        Color color = _song.player1NoteSprites[type].color;
        if (susNote)
            color.a = 1f;
        _sprite.color = color;
        oldX = transform.position.x;
    }

    public void GenerateHold(NoteObject prevNote)
    {
        var noteTransform = transform;

        if (lastSusNote)
        {
            Vector3 oldPos = noteTransform.position;
            oldPos.y += -((float)(Songdata.susStepCrotchet / 100 * 1.8 * ScrollSpeed) / 1.76f) + 1.3f;
            return;
        }
        Vector3 oldScale = noteTransform.localScale;
        oldScale.y *= -((float)(Songdata.susStepCrotchet / 100 * 1.8 * ScrollSpeed) / 1.76f) * 2;
        noteTransform.localScale = oldScale;

    }

    void Update()
    {
        if (dummyNote)
            return;

        Vector3 oldPos;
        if (_song.songStarted)
        {
            oldPos = transform.position;
            oldPos.y = (float)(4.45f -
                                (_song.stopwatch.ElapsedMilliseconds - (strumTime + Player.visualOffset + 1964f)) *
                                (0.45f * (_scrollSpeed)));
            if (lastSusNote)
                oldPos.y += ((float)(Songdata.susStepCrotchet / 100 * 1.8 * ScrollSpeed) / 1.76f) * (_scrollSpeed);
            transform.position = oldPos;
        }

        if (!_song.inst.isPlaying) return;


        oldPos = transform.position;
        oldPos.y = (float)(4.45f - (_song.stopwatch.ElapsedMilliseconds - (strumTime + Player.visualOffset)) * (0.45f * (_scrollSpeed)));
        if (lastSusNote)
            oldPos.y += ((float)(Songdata.susStepCrotchet / 100 * 1.8 * ScrollSpeed) / 1.76f) * (_scrollSpeed);

        oldPos = new Vector3(oldX + cameraTransform.position.x, oldPos.y + cameraTransform.position.y, 0);
        transform.position = oldPos;

        if (!mustHit)
        {
            //return;
            if (Player.twoPlayers || Player.playAsEnemy)
            {
                if (!(strumTime + Player.visualOffset - _song.stopwatch.ElapsedMilliseconds < Player.maxHitRoom)) return;
                _song.NoteMiss(this);
                CameraController.instance.Transition(true);
                _song.player2NotesObjects[type].Remove(this);
                Destroy(gameObject);
            }
            else
            {

                if (strumTime + Player.visualOffset >= _song.stopwatch.ElapsedMilliseconds) return;
                _song.NoteHit(this);
                CameraController.instance.Transition(true);

            }
        }
        else
        {
            //return;
            if (!Player.demoMode & !Player.playAsEnemy)
            {
                if (!(strumTime + Player.visualOffset - _song.stopwatch.ElapsedMilliseconds < Player.maxHitRoom)) return;
                _song.NoteMiss(this);
                CameraController.instance.Transition(false);
                _song.player1NotesObjects[type].Remove(this);
                Destroy(gameObject);
            }
            else
            {
                if (strumTime + Player.visualOffset >= _song.stopwatch.ElapsedMilliseconds) return;
                _song.NoteHit(this);
                CameraController.instance.Transition(false);
            }
        }
    }
}
