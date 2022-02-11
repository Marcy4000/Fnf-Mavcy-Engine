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
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;

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
    public Sprite[] holdNoteEnd;
    public Sprite[] holdNotes;
    public bool hasVoicesLoaded;

    public string[] characterNames;
    public Character[] characters;
    public Dictionary<string, Character> charactersDictionary;

    [Space]
    public GameObject bf, enemy, gf;

    public bool songStarted;
    private bool exiting;
    public Animator blackFade;
    public GameObject ratingObject;

    [Space]
    public GameObject pauseScreen;
    public bool paused = false;

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
        if (!GlobalDataSfutt.overrideStage)
        {
            if (File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\stage.txt"))
            {
                string[] lines = File.ReadAllLines(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\stage.txt");
                SceneManager.LoadScene(lines[0], LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(GlobalDataSfutt.stages[GlobalDataSfutt.selectedStage], LoadSceneMode.Additive);
            }
        }
        else
        {
            SceneManager.LoadScene(GlobalDataSfutt.stages[GlobalDataSfutt.selectedStage], LoadSceneMode.Additive);
        }

        yield return null;
        
        gf.transform.position = StageSettings.instance.gfPos.position;
        gf.GetComponent<SpriteRenderer>().sortingOrder = StageSettings.instance.gfLayer;
        bf.transform.position = StageSettings.instance.playerPos.position;
        bf.GetComponent<SpriteRenderer>().sortingOrder = StageSettings.instance.playerLayer;
        enemy.transform.position = StageSettings.instance.enemyPos.position;
        enemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = StageSettings.instance.enemyLayer;
        PlaySong(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad);
    }

    private void Update()
    {
        if (songStarted)
        {
            Songdata.SetSongTime(inst);
        }

        if (songStarted && Songdata.songPosition >= inst.clip.length && !exiting && !paused || !inst.isPlaying && songStarted && !exiting && !paused)
        {
            SaveStats();
            if (GlobalDataSfutt.isStoryMode)
            {
                GlobalDataSfutt.LoadNextStoryModeSong();
            }
            else
            {
                StartCoroutine(GoToMainMenu());
            }
        }

        if (Input.GetKeyDown(Player.pauseKey) && !exiting)
        {
            switch (paused)
            {
                case true:
                    ContinueSong();
                    break;
                case false:
                    PauseSong();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(GoToMainMenu());
        }
    }

    private void SaveStats()
    {
        string destination = Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\stats.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        PlayerStat data = HealthBar.instance.playerOneStats;
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    IEnumerator GoToMainMenu()
    {
        exiting = true;
        blackFade.SetTrigger("Change");

        yield return new WaitForSeconds(1.2f);

        SceneManager.LoadScene(0);
    }

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

        jsonDir = selectedSongDir + @"\Chart.json";

        StartCoroutine(SetupSong());
    }

    IEnumerator SetupSong()
    {
        string url = UnityWebRequest.EscapeURL(selectedSongDir);
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + url + @"\Inst.ogg", AudioType.OGGVORBIS);
        //UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("https://cdn.discordapp.com/attachments/859868869609783336/939510294340841492/Inst.ogg", AudioType.OGGVORBIS);
        yield return req.SendWebRequest();
        musicClip = DownloadHandlerAudioClip.GetContent(req);
        while (musicClip.loadState != AudioDataLoadState.Loaded)
            yield return new WaitForSeconds(0.1f);

        if (File.Exists(selectedSongDir + @"\Voices.ogg"))
        {
            //UnityWebRequest req2 = UnityWebRequestMultimedia.GetAudioClip("https://cdn.discordapp.com/attachments/859868869609783336/939510270290722877/Voices.ogg", AudioType.OGGVORBIS);
            UnityWebRequest req2 = UnityWebRequestMultimedia.GetAudioClip("file:///" + url + @"\Voices.ogg", AudioType.OGGVORBIS);
            yield return req2.SendWebRequest();
            vocalClip = DownloadHandlerAudioClip.GetContent(req2);
            while (vocalClip.loadState != AudioDataLoadState.Loaded)
                yield return new WaitForSeconds(0.1f);
            hasVoicesLoaded = true;
            GenerateSong();
        }
        else
        {
            hasVoicesLoaded = false;
            GenerateSong();
        }
    }

    /*IEnumerator SetupSong()
    {
        WWW www1 = new WWW(selectedSongDir + @"\Inst.ogg");
        if (www1.error != null)
        {
            UnityEngine.Debug.LogError(www1.error);
            UnityEngine.Debug.LogError(selectedSongDir + @"\Inst.ogg");
            UnityEngine.Debug.LogError(File.Exists(selectedSongDir + @"\Inst.ogg"));
        }
        else
        {
            musicClip = www1.GetAudioClip();
            while (musicClip.loadState != AudioDataLoadState.Loaded)
                yield return new WaitForSeconds(0.1f);
            if (File.Exists(selectedSongDir + @"\Voices.ogg"))
            {

                WWW www2 = new WWW(selectedSongDir + @"\Voices.ogg");
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
    }*/

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

                    switch (noteType)
                    {
                        case 0:
                            newSusNoteObj = Instantiate(holdNote);
                            newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNotes[0];
                            if ((i + 1) == Math.Floor(susLength))
                            {
                                newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd[0];
                                setAsLastSus = true;
                            }
                            break;
                        case 1:
                            newSusNoteObj = Instantiate(holdNote);
                            newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNotes[1];
                            if ((i + 1) == Math.Floor(susLength))
                            {
                                newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd[1];
                                setAsLastSus = true;
                            }
                            break;
                        case 2:
                            newSusNoteObj = Instantiate(holdNote);
                            newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNotes[2];
                            if ((i + 1) == Math.Floor(susLength))
                            {
                                newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd[2];
                                setAsLastSus = true;
                            }
                            break;
                        case 3:
                            newSusNoteObj = Instantiate(holdNote);
                            newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNotes[3];
                            if ((i + 1) == Math.Floor(susLength))
                            {
                                newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd[3];
                                setAsLastSus = true;
                            }
                            break;
                        default:
                            newSusNoteObj = Instantiate(holdNote);
                            newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNotes[0];
                            if ((i + 1) == Math.Floor(susLength))
                            {
                                newSusNoteObj.GetComponent<SpriteRenderer>().sprite = holdNoteEnd[0];
                                setAsLastSus = true;
                            }
                            break;
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
            enemyAnimation.Initialize(false, 0);
            enemyAnimation.enemyAnimator.runtimeAnimatorController = enemyThing.overrideAnimator;
            enemyAnimation.gameObject.GetComponent<SpriteRenderer>().flipX = enemyThing.flipX;

            HealthBar.instance.opponent = enemyThing.icon;
            HealthBar.instance.opponentDed = enemyThing.deadIcon;
            HealthBar.instance.opponentIcon.GetComponent<RectTransform>().sizeDelta = enemyThing.iconSize;
            HealthBar.instance.Initialize();
        }
        else
        {
            bool thingExists = false;
            int character = 0;
            for (int i = 0; i < GlobalDataSfutt.customCharacters.Count; i++)
            {
                if (GlobalDataSfutt.customCharacters[i].characterName == _song.Player2)
                {
                    thingExists = true;
                    character = i;
                }
            }

            if (thingExists && GlobalDataSfutt.characterActive[character])
            {
                enemyAnimation.Initialize(true, character);
                HealthBar.instance.opponent = GlobalDataSfutt.customCharacters[character].icons[0];
                HealthBar.instance.opponentDed = GlobalDataSfutt.customCharacters[character].icons[1];
            }
            else
            {
                Character enemyThing = charactersDictionary["dad"];
                enemyAnimation.Initialize(false, 0);
                enemyAnimation.enemyAnimator.runtimeAnimatorController = enemyThing.overrideAnimator;
                enemyAnimation.gameObject.GetComponent<SpriteRenderer>().flipX = enemyThing.flipX;

                HealthBar.instance.opponent = enemyThing.icon;
                HealthBar.instance.opponentDed = enemyThing.deadIcon;
                HealthBar.instance.opponentIcon.GetComponent<RectTransform>().sizeDelta = enemyThing.iconSize;
                HealthBar.instance.Initialize();
            }
        }

        if (!File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + "/cutscene.mp4") && GlobalDataSfutt.isStoryMode)
        {
            if (File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + "/dialogue.txt") && GlobalDataSfutt.isStoryMode)
            {
                string[] lines = File.ReadAllLines(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + "/dialogue.txt");
                DialogueBox.Instance.StartDialogue(lines);
            }
            else
            {
                StartCoroutine(StartSong());
            }
        }
        else if (File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\cutscene.mp4") && GlobalDataSfutt.isStoryMode)
        {
            CutscenePlayer.instance.PlayCutscene(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\cutscene.mp4");
        }
        else
        {
            if (File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\dialogue.txt") && GlobalDataSfutt.isStoryMode)
            {
                string[] lines = File.ReadAllLines(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\dialogue.txt");
                DialogueBox.Instance.StartDialogue(lines);
            }
            else
            {
                StartCoroutine(StartSong());
            }
        }
        
    }

    public void StartDialogueAfterCutscene()
    {
        if (File.Exists(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\dialogue.txt") && GlobalDataSfutt.isStoryMode)
        {
            string[] lines = File.ReadAllLines(Path.GetFullPath(".") + @"\data\Songs\" + GlobalDataSfutt.songNameToLoad + @"\dialogue.txt");
            DialogueBox.Instance.StartDialogue(lines);
        }
        else
        {
            StartCoroutine(StartSong());
        }
    }

    public void DialogueStuff()
    {
        StartCoroutine(StartSong());
    }

    public IEnumerator StartSong()
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

    public enum Rating
    {
        Sick = 1,
        Good = 2,
        Bad = 3,
        Shit = 4
    }

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

        Rating rating;

        float noteDiff = Math.Abs(note.strumTime - stopwatch.ElapsedMilliseconds + Player.visualOffset + Player.inputOffset);

        if (noteDiff > 0.75 * Player.safeZoneOffset) // way early or late
            rating = Rating.Shit;
        else if (noteDiff > .55 * Player.safeZoneOffset) // early or late
            rating = Rating.Bad;
        else if (noteDiff > .35 * Player.safeZoneOffset) // your kinda there
            rating = Rating.Good;
        else
            rating = Rating.Sick;

        if (!note.susNote)
        {
            if (player == 1)
            {
                HealthBar.instance.playerOneStats.totalNoteHits++;
                RateObject ratingObj = Instantiate(ratingObject).GetComponent<RateObject>();
                ratingObj.SetSprite((int)rating);
            }

            switch (rating)
            {
                case Rating.Sick:
                    {
                        if (player == 1)
                        {
                            HealthBar.instance.AddHp(5);
                            HealthBar.instance.playerOneStats.currentSickCombo++;
                            HealthBar.instance.playerOneStats.totalSickHits++;
                            HealthBar.instance.playerOneStats.currentScore += 10;
                        }
                        break;
                    }
                case Rating.Good:
                    {
                        if (player == 1)
                        {
                            HealthBar.instance.AddHp(2);
                            HealthBar.instance.playerOneStats.currentSickCombo++;
                            HealthBar.instance.playerOneStats.currentScore += 5;
                        }
                        break;
                    }
                case Rating.Bad:
                    {
                        if (player == 1)
                        {
                            HealthBar.instance.AddHp(1);
                            HealthBar.instance.playerOneStats.currentSickCombo++;
                            HealthBar.instance.playerOneStats.currentScore += 1;
                        }
                        break;
                    }
                case Rating.Shit:
                    {
                        if (player == 1)
                        {
                            HealthBar.instance.SubtractHp(2);
                            HealthBar.instance.playerOneStats.currentSickCombo = 0;
                        }
                        break;
                    }
            }

            if (player == 1)
            {
                if (HealthBar.instance.playerOneStats.highestSickCombo < HealthBar.instance.playerOneStats.currentSickCombo)
                {
                    HealthBar.instance.playerOneStats.highestSickCombo = HealthBar.instance.playerOneStats.currentSickCombo;
                }
                HealthBar.instance.playerOneStats.hitNotes++;
            }

            HealthBar.instance.UpdateScoringInfo();
        }

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
                HealthBar.instance.SubtractHp(8);
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

        if (player == 1)
        {
            RateObject ratingObj = Instantiate(ratingObject).GetComponent<RateObject>();
            ratingObj.SetSprite(4);
        }

        HealthBar.instance.playerOneStats.currentScore -= 5;
        HealthBar.instance.playerOneStats.currentSickCombo = 0;
        HealthBar.instance.playerOneStats.missedHits++;
        HealthBar.instance.playerOneStats.totalNoteHits++;

        HealthBar.instance.UpdateScoringInfo();
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

    public void PauseSong()
    {
        paused = true;
        stopwatch.Stop();
        beatStopwatch.Stop();

        inst.Pause();
        if (hasVoicesLoaded)
            voices.Pause();

        pauseScreen.SetActive(true);
    }

    public void ContinueSong()
    {
        paused = false;
        stopwatch.Start();
        beatStopwatch.Start();

        inst.UnPause();

        if (hasVoicesLoaded)
            voices.UnPause();

        pauseScreen.SetActive(false);
    }

    public void RestartSong()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitSong()
    {
        StartCoroutine(GoToMainMenu());
    }

    #endregion
}
