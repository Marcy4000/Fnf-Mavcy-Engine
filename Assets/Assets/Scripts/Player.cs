using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float safeFrames = 10;

    public static KeyCode leftArrowKey = KeyCode.LeftArrow;
    public static KeyCode downArrowKey = KeyCode.DownArrow;
    public static KeyCode upArrowKey = KeyCode.UpArrow;
    public static KeyCode rightArrowKey = KeyCode.RightArrow;

    public static KeyCode secLeftArrowKey = KeyCode.A;
    public static KeyCode secDownArrowKey = KeyCode.S;
    public static KeyCode secUpArrowKey = KeyCode.W;
    public static KeyCode secRightArrowKey = KeyCode.D;

    public static KeyCode pauseKey = KeyCode.Return;
    public static KeyCode resetKey = KeyCode.R;

    public static bool ghostTapping = false;

    public NoteObject leftNote;
    public NoteObject downNote;
    public NoteObject upNote;
    public NoteObject rightNote;

    public NoteObject secLeftNote;
    public NoteObject secDownNote;
    public NoteObject secUpNote;
    public NoteObject secRightNote;

    public static bool demoMode = false;
    public static bool twoPlayers = false;
    public static bool playAsEnemy = false;

    public static float maxHitRoom;
    public static float safeZoneOffset;
    public static Player instance;
    public static float inputOffset;
    public static float visualOffset;

    private void Start()
    {
        instance = this;
        maxHitRoom = -135 * Time.timeScale;
        safeZoneOffset = safeFrames / 60 * 1000;

        inputOffset = 0f;
        visualOffset = 0f;
    }

    public static void SaveKeySet()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (!LoadSong.instance.songStarted || demoMode)
            return;


        if (!playAsEnemy)
        {
            if (LoadSong.instance.player1NotesObjects[0].Count != 0)
                leftNote = LoadSong.instance.player1NotesObjects[0][0];
            else if (!leftNote.dummyNote || leftNote == null)
                leftNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player1NotesObjects[1].Count != 0)
                downNote = LoadSong.instance.player1NotesObjects[1][0];
            else if (!downNote.dummyNote || downNote == null)
                downNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player1NotesObjects[2].Count != 0)
                upNote = LoadSong.instance.player1NotesObjects[2][0];
            else if (!upNote.dummyNote || upNote == null)
                upNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player1NotesObjects[3].Count != 0)
                rightNote = LoadSong.instance.player1NotesObjects[3][0];
            else if (!rightNote.dummyNote || rightNote == null)
                rightNote = new GameObject().AddComponent<NoteObject>();
        }

        if (twoPlayers || playAsEnemy)
        {
            if (LoadSong.instance.player2NotesObjects[0].Count != 0)
                secLeftNote = LoadSong.instance.player2NotesObjects[0][0];
            else if (!secLeftNote.dummyNote || secLeftNote == null)
                secLeftNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player2NotesObjects[1].Count != 0)
                secDownNote = LoadSong.instance.player2NotesObjects[1][0];
            else if (!secDownNote.dummyNote || secDownNote == null)
                secDownNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player2NotesObjects[2].Count != 0)
                secUpNote = LoadSong.instance.player2NotesObjects[2][0];
            else if (!secUpNote.dummyNote || secUpNote == null)
                secUpNote = new GameObject().AddComponent<NoteObject>();

            if (LoadSong.instance.player2NotesObjects[3].Count != 0)
                secRightNote = LoadSong.instance.player2NotesObjects[3][0];
            else if (!secRightNote.dummyNote || secRightNote == null)
                secRightNote = new GameObject().AddComponent<NoteObject>();

        }

        #region Player 1 Inputs

        if (!playAsEnemy)
        {
            if (Input.GetKey(leftArrowKey))
            {
                if (leftNote.susNote && !leftNote.dummyNote)
                {
                    if (leftNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 0, "Hit");
                        LoadSong.instance.NoteHit(leftNote);
                    }
                }
            }
            if (Input.GetKey(downArrowKey))
            {
                if (downNote.susNote && !downNote.dummyNote)
                {
                    if (downNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 1, "Hit");
                        LoadSong.instance.NoteHit(downNote);
                    }
                }
            }
            if (Input.GetKey(upArrowKey))
            {
                if (upNote.susNote && !upNote.dummyNote)
                {
                    if (upNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 2, "Hit");
                        LoadSong.instance.NoteHit(upNote);
                    }
                }
            }
            if (Input.GetKey(rightArrowKey))
            {
                if (rightNote.susNote && !rightNote.dummyNote)
                {
                    if (rightNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 3, "Hit");
                        LoadSong.instance.NoteHit(rightNote);
                    }
                }
            }

            if (Input.GetKeyDown(leftArrowKey))
            {
                if (CanHitNote(leftNote))
                {
                    LoadSong.instance.AnimateNote(1, 0, "Hit");
                    LoadSong.instance.NoteHit(leftNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 0, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(leftNote);
                    }
                }
            }
            if (Input.GetKeyDown(downArrowKey))
            {
                if (CanHitNote(downNote))
                {
                    LoadSong.instance.AnimateNote(1, 1, "Hit");
                    LoadSong.instance.NoteHit(downNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 1, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(downNote);
                    }
                }
            }
            if (Input.GetKeyDown(upArrowKey))
            {
                if (CanHitNote(upNote))
                {
                    LoadSong.instance.AnimateNote(1, 2, "Hit");
                    LoadSong.instance.NoteHit(upNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 2, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(upNote);
                    }
                }
            }
            if (Input.GetKeyDown(rightArrowKey))
            {
                if (CanHitNote(rightNote))
                {
                    LoadSong.instance.AnimateNote(1, 3, "Hit");
                    LoadSong.instance.NoteHit(rightNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 3, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(rightNote);
                    }
                }
            }

            if (Input.GetKeyUp(leftArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 0, "Normal");
            }
            if (Input.GetKeyUp(downArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 1, "Normal");
            }
            if (Input.GetKeyUp(upArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 2, "Normal");
            }
            if (Input.GetKeyUp(rightArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 3, "Normal");
            }
        }
        #endregion

        #region Player 2 Inputs & Player 1 Sub-Inputs

        if (twoPlayers || playAsEnemy)
        {
            if (Input.GetKey(secLeftArrowKey))
            {
                if (secLeftNote.susNote && !secLeftNote.dummyNote)
                {
                    if (secLeftNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 0, "Hit");
                        LoadSong.instance.NoteHit(secLeftNote);
                    }
                }
            }

            if (Input.GetKey(secDownArrowKey))
            {
                if (secDownNote.susNote && !secDownNote.dummyNote)
                {
                    if (secDownNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 1, "Hit");
                        LoadSong.instance.NoteHit(secDownNote);
                    }
                }
            }

            if (Input.GetKey(secUpArrowKey))
            {
                if (secUpNote.susNote && !secUpNote.dummyNote)
                {
                    if (secUpNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 2, "Hit");
                        LoadSong.instance.NoteHit(secUpNote);
                    }
                }
            }

            if (Input.GetKey(secRightArrowKey))
            {
                if (secRightNote.susNote && !secRightNote.dummyNote)
                {
                    if (secRightNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 3, "Hit");
                        LoadSong.instance.NoteHit(secRightNote);
                    }
                }
            }

            if (Input.GetKeyDown(secLeftArrowKey))
            {
                if (CanHitNote(secLeftNote))
                {
                    LoadSong.instance.AnimateNote(1, 0, "Hit");
                    LoadSong.instance.NoteHit(secLeftNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(2, 0, "Pressed");
                    LoadSong.instance.NoteMiss(secLeftNote);
                }
            }

            if (Input.GetKeyDown(secDownArrowKey))
            {
                if (CanHitNote(secDownNote))
                {
                    LoadSong.instance.AnimateNote(1, 1, "Hit");
                    LoadSong.instance.NoteHit(secDownNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(2, 1, "Pressed");
                    LoadSong.instance.NoteMiss(secDownNote);
                }
            }

            if (Input.GetKeyDown(secUpArrowKey))
            {
                if (CanHitNote(secUpNote))
                {
                    LoadSong.instance.AnimateNote(1, 2, "Hit");
                    LoadSong.instance.NoteHit(secUpNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(2, 2, "Pressed");
                    LoadSong.instance.NoteMiss(secUpNote);
                }
            }

            if (Input.GetKeyDown(secRightArrowKey))
            {
                if (CanHitNote(secRightNote))
                {
                    LoadSong.instance.AnimateNote(1, 3, "Hit");
                    LoadSong.instance.NoteHit(secRightNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(2, 3, "Pressed");
                    LoadSong.instance.NoteMiss(secRightNote);
                }
            }

            if (Input.GetKeyUp(secLeftArrowKey))
            {
                LoadSong.instance.AnimateNote(2, 0, "Normal");
            }

            if (Input.GetKeyUp(secDownArrowKey))
            {
                LoadSong.instance.AnimateNote(2, 1, "Normal");
            }

            if (Input.GetKeyUp(secUpArrowKey))
            {
                LoadSong.instance.AnimateNote(2, 2, "Normal");
            }

            if (Input.GetKeyUp(secRightArrowKey))
            {
                LoadSong.instance.AnimateNote(2, 3, "Normal");
            }
        }
        else
        {
            if (Input.GetKey(secLeftArrowKey))
            {
                if (leftNote.susNote && !leftNote.dummyNote)
                {
                    if (leftNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 0, "Hit");
                        LoadSong.instance.NoteHit(leftNote);
                    }
                }
            }
            if (Input.GetKey(secDownArrowKey))
            {
                if (downNote.susNote && !downNote.dummyNote)
                {
                    if (downNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 1, "Hit");
                        LoadSong.instance.NoteHit(downNote);
                    }
                }
            }
            if (Input.GetKey(secUpArrowKey))
            {
                if (upNote.susNote && !upNote.dummyNote)
                {
                    if (upNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 2, "Hit");
                        LoadSong.instance.NoteHit(upNote);
                    }
                }
            }
            if (Input.GetKey(secRightArrowKey))
            {
                if (rightNote.susNote && !rightNote.dummyNote)
                {
                    if (rightNote.strumTime + visualOffset <= LoadSong.instance.stopwatch.ElapsedMilliseconds)
                    {
                        LoadSong.instance.AnimateNote(1, 3, "Hit");
                        LoadSong.instance.NoteHit(rightNote);
                    }
                }
            }

            if (Input.GetKeyDown(secLeftArrowKey))
            {
                if (CanHitNote(leftNote))
                {
                    LoadSong.instance.AnimateNote(1, 0, "Hit");
                    LoadSong.instance.NoteHit(leftNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 0, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(leftNote);
                    }
                }
            }
            if (Input.GetKeyDown(secDownArrowKey))
            {
                if (CanHitNote(downNote))
                {
                    LoadSong.instance.AnimateNote(1, 1, "Hit");
                    LoadSong.instance.NoteHit(downNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 1, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(downNote);
                    }
                }
            }
            if (Input.GetKeyDown(secUpArrowKey))
            {
                if (CanHitNote(upNote))
                {
                    LoadSong.instance.AnimateNote(1, 2, "Hit");
                    LoadSong.instance.NoteHit(upNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 2, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(upNote);
                    }
                }
            }
            if (Input.GetKeyDown(secRightArrowKey))
            {
                if (CanHitNote(rightNote))
                {
                    LoadSong.instance.AnimateNote(1, 3, "Hit");
                    LoadSong.instance.NoteHit(rightNote);
                }
                else
                {
                    LoadSong.instance.AnimateNote(1, 3, "Pressed");
                    if (!GlobalDataSfutt.ghostTapping)
                    {
                        LoadSong.instance.NoteMiss(rightNote);
                    }
                }
            }

            if (Input.GetKeyUp(secLeftArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 0, "Normal");
            }
            if (Input.GetKeyUp(secDownArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 1, "Normal");
            }
            if (Input.GetKeyUp(secUpArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 2, "Normal");
            }
            if (Input.GetKeyUp(secRightArrowKey))
            {
                LoadSong.instance.AnimateNote(1, 3, "Normal");
            }
        }
        #endregion

    }


    public bool CanHitNote(NoteObject noteObject)
    {
        /*
        var position = noteObject.transform.position;
        return position.y <= 4.55 + LoadSong.instance.topSafeWindow & position.y >= 4.55 - LoadSong.instance.bottomSafeWindow & !noteObject.dummyNote;
    */
        float noteDiff = noteObject.strumTime + visualOffset - LoadSong.instance.stopwatch.ElapsedMilliseconds + inputOffset;

        return noteDiff <= 135 * Time.timeScale & noteDiff >= -135 * Time.timeScale & !noteObject.dummyNote;
    }
}
