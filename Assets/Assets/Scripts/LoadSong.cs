using System;
using System.Collections.Generic;
using UnityEngine;
using FridayNightFunkin;
using System.IO;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class LoadSong : MonoBehaviour
{
    #region Variables
    public Stopwatch stopwatch;
    public Stopwatch beatStopwatch;
    public Countdown countdown;

    public static LoadSong instance;
    public AudioSource inst, voices;

    [Space] public Transform player1Notes;
    public SpriteRenderer[] player1NoteSprites;
    public string selectedSongDir;
    public AudioClip musicClip;
    public AudioClip vocalClip;
    public AudioClip startSound;
    public FNFSong _song;
    public List<List<NoteObject>> player1NotesObjects;
    public List<List<NoteObject>> player2NotesObjects;
    public Animator[] player1NotesAnimators;
    public BfAnimations boyfriendAnimation;
    public opponentAnimations enemyAnimation;

    public NoteObject lastNote;

    public float notesOffset;
    public float noteDelay;
    public Transform player1Left;
    public Transform player1Down;
    public Transform player1Up;
    public Transform player1Right;
    public Transform player2Left;
    public Transform player2Down;
    public Transform player2Up;
    public Transform player2Right;

    public GameObject leftArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public GameObject rightArrow;

    [Space] public GameObject holdNote;
    public Sprite holdNoteEnd;
    public bool hasVoicesLoaded;

    public string[] characterNames;
    public Character[] characters;
    public Dictionary<string, Character> charactersDictionary;

    [Space]
    public GameObject bf, enemy, gf;

    public bool songStarted;
    private bool exiting;
    public Animator blackFade;

    private string jsonDir;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Songdata.songPosition = 0f;
        songStarted = false;
        charactersDictionary = new Dictionary<string, Character>();
        for (int i = 0; i < characters.Length; i++)
        {
            charactersDictionary.Add(characterNames[i], characters[i]);
        }
        StartCoroutine(LoadShit());
    }

    IEnumerator LoadShit()
    {
        SceneManager.LoadScene(GlobalDataSfutt.stages[GlobalDataSfutt.selectedStage], LoadSceneMode.Additive);

        yield return null;
        
        gf.transform.position = StageSettings.instance.gfPos.position;
        gf.GetComponent<SpriteRenderer>().sortingOrder = StageSettings.instance.gfLayer;
        bf.transform.position = StageSettings.instance.playerPos.position;
        bf.GetComponent<SpriteRenderer>().sortingOrder = StageSettings.instance.playerLayer;
        enemy.transform.position = StageSettings.instance.enemyPos.position;
        enemy.GetComponent<SpriteRenderer>().sortingOrder = StageSettings.instance.enemyLayer;
        PlaySong(Application.persistentDataPath + "/Songs/" + GlobalDataSfutt.songNameToLoad);
    }

    private void Update()
    {
        if (songStarted)
        {
            Songdata.SetSongTime(inst);
        }

        if (songStarted && Songdata.songPosition >= inst.clip.length && !exiting)
        {
            StartCoroutine(GoToMainMenu());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(GoToMainMenu());
        }
    }

    IEnumerator GoToMainMenu()
    {
        exiting = true;
        blackFade.SetTrigger("Change");

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(0);
    }

    [Button]
    public void PlaySong(string path = "C:/Users/Marce/AppData/LocalLow/DefaultCompany/Fnf Dumbass engine/Songs/roses")
    {
        if (!string.IsNullOrWhiteSpace(path))
        {
            selectedSongDir = path;
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid song path");
        }

        jsonDir = selectedSongDir + "/Chart.json";

        StartCoroutine(SetupSong());
    }

    IEnumerator SetupSong()
    {
        WWW www1 = new WWW(selectedSongDir + "/Inst.ogg");
        if (www1.error != null)
        {
            UnityEngine.Debug.LogError(www1.error);
        }
        else
        {
            musicClip = www1.GetAudioClip();
            while (musicClip.loadState != AudioDataLoadState.Loaded)
                yield return new WaitForSeconds(0.1f);
            if (File.Exists(selectedSongDir + "/Voices.ogg"))
            {

                WWW www2 = new WWW(selectedSongDir + "/Voices.ogg");
                if (www2.error != null)
                {
                    UnityEngine.Debug.LogError(www2.error);
                }
                else
                {
                    vocalClip = www2.GetAudioClip();
                    while (vocalClip.loadState != AudioDataLoadState.Loaded)
                        yield return new WaitForSeconds(0.1f);
                    print("Sounds loaded, generating song.");
                    hasVoicesLoaded = true;
                    GenerateSong();
                }
            }
            else
            {
                print("Sounds loaded, generating song.");
                hasVoicesLoaded = false;
                GenerateSong();
            }
        }
    }

    void GenerateSong()
    {
        _song = new FNFSong(jsonDir);

        Songdata.Initialize(_song.Bpm);

        if (player1NotesObjects != null)
        {
            foreach (List<NoteObject> list in player1NotesObjects)
            {
                foreach (var t in list)
                {
                    Destroy(t.gameObject);
                }

                list.Clear();
            }

            player1NotesObjects.Clear();
        }

        if (player2NotesObjects != null)
        {
            foreach (List<NoteObject> list in player2NotesObjects)
            {
                foreach (var t in list)
                {
                    Destroy(t.gameObject);
                }

                list.Clear();
            }

            player2NotesObjects.Clear();
        }

        player1NotesObjects = new List<List<NoteObject>>
        {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
        };

        player2NotesObjects = new List<List<NoteObject>>
        {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
        };

        if (_song == null)
        {
            UnityEngine.Debug.LogError("Error with song data");
            return;
        }

        foreach (FNFSong.FNFSection section in _song.Sections)
        {
            foreach (var noteData in section.Notes)
            {
                GameObject newNoteObj;
                List<decimal> data = noteData.ConvertToNote();

                bool mustHitNote = section.MustHitSection;
                if (data[1] > 3)
                    mustHitNote = !section.MustHitSection;
                int noteType = Convert.ToInt32(data[1] % 4);

                Vector3 spawnPos;

                float susLength = (float)data[2];

                susLength = susLength / Songdata.susStepCrotchet;

                switch (noteType)
                {
                    case 0: //Left
                        newNoteObj = Instantiate(leftArrow);
                        spawnPos = mustHitNote ? player1Left.position : player2Left.position;
                        break;
                    case 1: //Down
                        newNoteObj = Instantiate(downArrow);
                        spawnPos = mustHitNote ? player1Down.position : player2Down.position;
                        break;
                    case 2: //Up
                        newNoteObj = Instantiate(upArrow);
                        spawnPos = mustHitNote ? player1Up.position : player2Up.position;
                        break;
                    case 3: //Right
                        newNoteObj = Instantiate(rightArrow);
                        spawnPos = mustHitNote ? player1Right.position : player2Right.position;
                        break;
                    default:
                        UnityEngine.Debug.LogError("Invalid note data.");
                        return;
                }

                spawnPos += Vector3.down *
                            (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
                spawnPos.y -= (_song.Bpm / 60) * startSound.length * _song.Speed;
                newNoteObj.transform.position = spawnPos;

                NoteObject nObj = newNoteObj.GetComponent<NoteObject>();
                
                nObj.ScrollSpeed = -_song.Speed;
                nObj.strumTime = (float)data[0];
                nObj.type = noteType;
                nObj.mustHit = mustHitNote;
                nObj.dummyNote = false;
                nObj.layer = section.MustHitSection ? 1 : 2;

                if (mustHitNote)
                    player1NotesObjects[noteType].Add(nObj);
                else
                    player2NotesObjects[noteType].Add(nObj);

                lastNote = nObj;

                //newNoteObj.transform.parent = Camera.main.transform;

                for (int i = 0; i < Math.Floor(susLength); i++)
                {
                    GameObject newSusNoteObj;
                    Vector3 susSpawnPos;

                    bool setAsLastSus = false;

                    /*
                     * Math.floor returns the largest integer less than or equal to a given number.
                     *
                     * I uh... have no clue why this is needed or what it does but we need this
                     * in or else it won't do hold notes right so...
                     */
                    newSusNoteObj = Instantiate(holdNote);
                    if ((i + 1) == Math.Floor(susLength))
                    {
                        newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd;
                        setAsLastSus = true;
                    }

                    switch (noteType)
                    {
                        case 0: //Left
                            susSpawnPos = mustHitNote ? player1Left.position : player2Left.position;
                            break;
                        case 1: //Down
                            susSpawnPos = mustHitNote ? player1Down.position : player2Down.position;
                            break;
                        case 2: //Up
                            susSpawnPos = mustHitNote ? player1Up.position : player2Up.position;
                            break;
                        case 3: //Right
                            susSpawnPos = mustHitNote ? player1Right.position : player2Right.position;
                            break;
                        default:
                            susSpawnPos = mustHitNote ? player1Left.position : player2Left.position;
                            break;
                    }


                    susSpawnPos += Vector3.down *
                                   (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
                    susSpawnPos.y -= (_song.Bpm / 60) * startSound.length * _song.Speed;
                    newSusNoteObj.transform.position = susSpawnPos;
                    NoteObject susObj = newSusNoteObj.GetComponent<NoteObject>();
                    susObj.type = noteType;
                    susObj.ScrollSpeed = -_song.Speed;
                    susObj.mustHit = mustHitNote;
                    susObj.strumTime = (float)data[0] + (Songdata.susStepCrotchet * i) + Songdata.susStepCrotchet;
                    susObj.susNote = true;
                    susObj.dummyNote = false;
                    susObj.lastSusNote = setAsLastSus;
                    susObj.layer = section.MustHitSection ? 1 : 2;
                    susObj.GenerateHold(lastNote);
                    if (mustHitNote)
                        player1NotesObjects[noteType].Add(susObj);
                    else
                        player2NotesObjects[noteType].Add(susObj);
                    lastNote = susObj;

                    //newSusNoteObj.transform.parent = Camera.main.transform;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                player1NotesObjects[i] = player1NotesObjects[i].OrderBy(s => s.strumTime).ToList();
                player2NotesObjects[i] = player2NotesObjects[i].OrderBy(s => s.strumTime).ToList();
            }
        }

        print("Checking for and applying " + _song.Player2 + ". Result is " + charactersDictionary.ContainsKey(_song.Player2));
        if (charactersDictionary.ContainsKey(_song.Player2))
        {
            Character enemyThing = charactersDictionary[_song.Player2];
            enemyAnimation.enemyAnimator.runtimeAnimatorController = enemyThing.overrideAnimator;
            enemyAnimation.gameObject.GetComponent<SpriteRenderer>().flipX = enemyThing.flipX;

            HealthBar.instance.opponent = enemyThing.icon;
            HealthBar.instance.opponentDed = enemyThing.deadIcon;
            HealthBar.instance.opponentIcon.GetComponent<RectTransform>().sizeDelta = enemyThing.iconSize;
            HealthBar.instance.Initialize();
        }
        else
        {
            Character enemyThing = charactersDictionary["dad"];
            enemyAnimation.enemyAnimator.runtimeAnimatorController = enemyThing.overrideAnimator;
            enemyAnimation.gameObject.GetComponent<SpriteRenderer>().flipX = enemyThing.flipX;

            HealthBar.instance.opponent = enemyThing.icon;
            HealthBar.instance.opponentDed = enemyThing.deadIcon;
            HealthBar.instance.opponentIcon.GetComponent<RectTransform>().sizeDelta = enemyThing.iconSize;
            HealthBar.instance.Initialize();
        }

        StartCoroutine(StartSong());
    }

    IEnumerator StartSong()
    {
        countdown.CountdownStart();
        
        yield return new WaitForSeconds(Songdata.crotchet * 4);

        stopwatch = new Stopwatch();
        stopwatch.Start();

        inst.clip = musicClip;
        if (hasVoicesLoaded)
            voices.clip = vocalClip;

        inst.Play();
        if (hasVoicesLoaded)
            voices.Play();

        songStarted = true;

        beatStopwatch = new Stopwatch();
        beatStopwatch.Start();
        stopwatch.Restart();

    }

    #region nonLoadSongCode
    
    public void NoteHit(NoteObject note)
    {
        if (note == null) return;


        int player;

        player = note.mustHit ? 1 : 2;


        if (hasVoicesLoaded)
            voices.mute = false;

        //bool invertHealth = false;
        

        int noteType = note.type;
        switch (player)
        {
            case 1:
                HealthBar.instance.AddHp();
                switch (noteType)
                {
                    case 0:
                        //Left
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Left");
                        break;
                    case 1:
                        //Down
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Down");
                        break;
                    case 2:
                        //Up
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Up");
                        break;
                    case 3:
                        //Right
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Right");
                        break;
                }
                //AnimateNote(1, noteType, "Activated");
                break;
            case 2:
                //if (Player.playAsEnemy || Player.demoMode || Player.twoPlayers)
                    //invertHealth = true;
                switch (noteType)
                {
                    case 0:
                        //Left
                        enemyAnimation.PlayAnimation("Sing Left");
                        break;
                    case 1:
                        //Down
                        enemyAnimation.PlayAnimation("Sing Down");
                        break;
                    case 2:
                        //Up
                        enemyAnimation.PlayAnimation("Sing Up");
                        break;
                    case 3:
                        //Right
                        enemyAnimation.PlayAnimation("Sing Right");
                        break;
                }
                //AnimateNote(2, noteType, "Activated");
                break;
        }
        /*
        bool modifyScore = true;

        if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
            modifyScore = false;
        else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
            modifyScore = false;

        if (Player.demoMode) modifyScore = true;

        CameraMovement.instance.focusOnPlayerOne = note.layer == 1;

        Rating rating;
        if (!note.susNote & modifyScore)
        {
            if (player == 1)
            {
                playerOneStats.totalNoteHits++;
            }
            else
            {
                playerTwoStats.totalNoteHits++;
            }

            float yPos = note.transform.position.y;

            GameObject newRatingObject = Instantiate(ratingObject);

            Vector3 ratingPos = newRatingObject.transform.position;
            if (player == 2)
            {
                ratingPos.x = -ratingPos.x;
                newRatingObject.transform.position = ratingPos;
            }


            var ratingObjectScript = newRatingObject.GetComponent<RatingObject>();

            // Rating and difference calulations from FNF Week 6 update

            float noteDiff = Math.Abs(note.strumTime - stopwatch.ElapsedMilliseconds + Player.visualOffset + Player.inputOffset);

            if (noteDiff > 0.9 * Player.safeZoneOffset) // way early or late
                rating = Rating.Shit;
            else if (noteDiff > .75 * Player.safeZoneOffset) // early or late
                rating = Rating.Bad;
            else if (noteDiff > .35 * Player.safeZoneOffset) // your kinda there
                rating = Rating.Good;
            else
                rating = Rating.Sick;

            switch (rating)
            {
                case Rating.Sick:
                    {
                        ratingObjectScript.sprite.sprite = sickSprite;

                        if (!invertHealth)
                            health += 5;
                        else
                            health -= 5;
                        if (player == 1)
                        {
                            playerOneStats.currentSickCombo++;
                            playerOneStats.currentScore += 10;
                        }
                        else
                        {
                            playerTwoStats.currentSickCombo++;
                            playerTwoStats.currentScore += 10;
                        }
                        break;
                    }
                case Rating.Good:
                    {
                        ratingObjectScript.sprite.sprite = goodSprite;

                        if (!invertHealth)
                            health += 2;
                        else
                            health -= 2;

                        if (player == 1)
                        {
                            playerOneStats.currentSickCombo++;
                            playerOneStats.currentScore += 5;
                        }
                        else
                        {
                            playerTwoStats.currentSickCombo++;
                            playerTwoStats.currentScore += 5;
                        }
                        break;
                    }
                case Rating.Bad:
                    {
                        ratingObjectScript.sprite.sprite = badSprite;

                        if (!invertHealth)
                            health += 1;
                        else
                            health -= 1;

                        if (player == 1)
                        {
                            playerOneStats.currentSickCombo++;
                            playerOneStats.currentScore += 1;
                        }
                        else
                        {
                            playerTwoStats.currentSickCombo++;
                            playerTwoStats.currentScore += 1;
                        }
                        break;
                    }
                case Rating.Shit:
                    ratingObjectScript.sprite.sprite = shitSprite;

                    if (player == 1)
                    {
                        playerOneStats.currentSickCombo = 0;
                    }
                    else
                    {
                        playerTwoStats.currentSickCombo = 0;
                    }
                    break;
            }

            if (player == 1)
            {
                if (playerOneStats.highestSickCombo < playerOneStats.currentSickCombo)
                {
                    playerOneStats.highestSickCombo = playerOneStats.currentSickCombo;
                }
                playerOneStats.hitNotes++;
            }
            else
            {
                if (playerTwoStats.highestSickCombo < playerTwoStats.currentSickCombo)
                {
                    playerTwoStats.highestSickCombo = playerTwoStats.currentSickCombo;
                }
                playerTwoStats.hitNotes++;
            }




            _currentRatingLayer++;
            ratingObjectScript.sprite.sortingOrder = _currentRatingLayer;
            ratingLayerTimer = _ratingLayerDefaultTime;
        }

        UpdateScoringInfo();*/
        if (player == 1)
        {
            player1NotesObjects[noteType].Remove(note);
        }
        else
        {
            player2NotesObjects[noteType].Remove(note);
        }

        Destroy(note.gameObject);

    }

    public void NoteMiss(NoteObject note)
    {

        if (hasVoicesLoaded)
            voices.mute = true;
        //oopsSource.clip = noteMissClip[Random.Range(0, noteMissClip.Length)];
        //oopsSource.Play();

        var player = note.mustHit ? 1 : 2;


        //bool invertHealth = player == 2;


        int noteType = note.type;
        switch (player)
        {
            case 1:
                HealthBar.instance.SubtractHp();
                switch (noteType)
                {
                    case 0:
                        //Left
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Left Miss");
                        break;
                    case 1:
                        //Down
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Down Miss");
                        break;
                    case 2:
                        //Up
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Up Miss");
                        break;
                    case 3:
                        //Right
                        boyfriendAnimation.BoyfriendPlayAnimation("Sing Right Miss");
                        break;
                }
                break;
            default:
                switch (noteType)
                {
                    case 0:
                        //Left
                        //EnemyPlayAnimation("Sing Left");
                        break;
                    case 1:
                        //Down
                        //EnemyPlayAnimation("Sing Down");
                        break;
                    case 2:
                        //Up
                        //EnemyPlayAnimation("Sing Up");
                        break;
                    case 3:
                        //Right
                        //EnemyPlayAnimation("Sing Right");
                        break;
                }
                break;
        }

        /*bool modifyHealth = true;

        if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
            modifyHealth = false;
        else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
            modifyHealth = false;

        if (modifyHealth)
        {
            if (!invertHealth)
                health -= 8;
            else
                health += 8;
        }

        if (player == 1)
        {
            playerOneStats.currentScore -= 5;
            playerOneStats.currentSickCombo = 0;
            playerOneStats.missedHits++;
            playerOneStats.totalNoteHits++;
        }
        else
        {
            playerTwoStats.currentScore -= 5;
            playerTwoStats.currentSickCombo = 0;
            playerTwoStats.missedHits++;
            playerTwoStats.totalNoteHits++;
        }

        UpdateScoringInfo();*/

    }

    public void AnimateNote(int player, int type, string animName)
    {
        switch (player)
        {
            case 1: //Boyfriend

                player1NotesAnimators[type].Play(animName, 0, 0);
                player1NotesAnimators[type].speed = 0;

                player1NotesAnimators[type].Play(animName);
                player1NotesAnimators[type].speed = 1;

                break;
            /*case 2:

                player2NotesAnimators[type].Play(animName, 0, 0);
                player2NotesAnimators[type].speed = 0;

                player2NotesAnimators[type].Play(animName);
                player2NotesAnimators[type].speed = 1;

                if (animName == "Activated" & !Player.twoPlayers)
                {
                    if (!Player.playAsEnemy)
                        _currentEnemyNoteTimers[type] = enemyNoteTimer;
                }
                break;*/
        }
    }

    #endregion
}
