using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ModLoader
{
    public static Mod LoadMod(string path)
    {
        Mod _mod = new Mod();
        _mod.modPath = path;

        StreamReader r = new StreamReader(path + @"\meta.json");
        string jsonString = r.ReadToEnd();
        _mod.data = JsonConvert.DeserializeObject<ModData>(jsonString);

        if (File.Exists(path + @"\icon.png"))
        {
            _mod.modIcon = IMG2Sprite.LoadNewSprite(path + @"\icon.png");
        }
        _mod.characters.Clear();
        _mod.weeks.Clear();
        _mod.songNames.Clear();
        _mod.weeks.Clear();

        if (Directory.Exists(path + @"\Characters"))
        {
            DirectoryInfo dir = new DirectoryInfo(path + @"\Characters");
            DirectoryInfo[] info = dir.GetDirectories();
            foreach (DirectoryInfo info2 in info)
            {
                ExportableFnfCharacter character;
                character = GlobalDataSfutt.LoadFile<ExportableFnfCharacter>(path + @"\Characters\" + info2.Name + @"\character.dat");
                FNFCharacter character1 = new FNFCharacter();
                character1.LoadAnimations(character, path + @"\Characters\" + info2.Name);
                _mod.characters.Add(character1);
            }
        }
        if (Directory.Exists(path + @"\Weeks"))
        {
            DirectoryInfo dir = new DirectoryInfo(path + @"\Weeks");
            DirectoryInfo[] info = dir.GetDirectories();
            foreach (DirectoryInfo info2 in info)
            {
                WeekStuff week = new WeekStuff();
                string[] lines = File.ReadAllLines(path + @"\Weeks\" + info2.Name + @"\weekData.txt");
                week.weekIcon = IMG2Sprite.LoadNewSprite(path + @"\Weeks\" + info2.Name + @"\img.png");
                week.weekName = lines[0];
                week.tracks = new string[lines.Length - 1];
                for (int j = 1; j < lines.Length; j++)
                {
                    week.tracks[j - 1] = lines[j];
                }
                _mod.weeks.Add(week);
            }
        }
        if (Directory.Exists(path + @"\Songs"))
        {
            DirectoryInfo dir = new DirectoryInfo(path + @"\Songs");
            DirectoryInfo[] info = dir.GetDirectories();
            foreach (DirectoryInfo info2 in info)
            {
                _mod.songNames.Add(info2.Name);
            }
        }
        if (Directory.Exists(path + @"\Stages"))
        {
            DirectoryInfo dir = new DirectoryInfo(path + @"\Stages");
            DirectoryInfo[] info = dir.GetDirectories();
            foreach (DirectoryInfo info2 in info)
            {
                Level stage = new Level();
                StreamReader idk = new StreamReader(path + @"\Stages\" + info2.Name + @"\stage.json");
                string jsonStage = idk.ReadToEnd();
                stage = JsonConvert.DeserializeObject<Level>(jsonStage);
                stage.levelName = info2.Name;
                stage.stagePath = path + @"\Stages\" + info2.Name;
                _mod.stages.Add(stage);
            }
        }

        return _mod;
    }
}

public class WeekStuff
{
    public Sprite weekIcon;
    public string weekName;
    public string[] tracks;
}

public class Mod
{
    public ModData data;
    public string modPath;
    public Sprite modIcon;
    public List<FNFCharacter> characters = new List<FNFCharacter>();
    public List<WeekStuff> weeks = new List<WeekStuff>();
    public List<string> songNames = new List<string>();
    public List<Level> stages = new List<Level>();
}

public class ModData
{
    public string title;
    public string description;
    public string homepage;
    public Contributor[] contributors;
    public string mod_version;
}

public class Contributor
{
    public string name;
    public string role;
}