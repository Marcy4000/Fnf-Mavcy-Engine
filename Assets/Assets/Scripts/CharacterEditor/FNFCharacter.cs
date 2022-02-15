using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FNFCharacter
{
    public string characterName;
    public string xmlPath;

    public Sprite[] icons = new Sprite[2];
    public Dictionary<string, List<Sprite>> animations;

    public Dictionary<string, List<SerializableVector2>> piviots;

    public void LoadAnimations(ExportableFnfCharacter loadedThing, string filePath)
    {
        characterName = loadedThing.characterName;
        xmlPath = loadedThing.xmlPath;
        piviots = loadedThing.piviots;
        animations = new Dictionary<string, List<Sprite>>();

        Sprite[] frames;
        TextureAtlasXml spriteSheet = GlobalDataSfutt.ImportXml<TextureAtlasXml>(filePath + @"\character" + ".xml");
        Texture2D SpriteTexture = IMG2Sprite.LoadTexture(filePath + @"\" + spriteSheet.imagePath);
        frames = new Sprite[spriteSheet.frame.Length];
        for (int i = 0; i < spriteSheet.frame.Length; i++)
        {
            frames[i] = IMG2Sprite.LoadSpriteSheet(SpriteTexture, new Rect(spriteSheet.frame[i].x, SpriteTexture.height - spriteSheet.frame[i].y, spriteSheet.frame[i].width, -spriteSheet.frame[i].height), new Vector2(0.5f, 1), 100f, SpriteMeshType.Tight);
            frames[i].name = spriteSheet.frame[i].name;
        }

        List<string> framesNames;
        List<string> framesNames1;
        List<string> framesNames2;
        List<string> framesNames3;
        List<string> framesNames4;
        List<Sprite> newFrames = new List<Sprite>();
        List<Sprite> newFrames1 = new List<Sprite>();
        List<Sprite> newFrames2 = new List<Sprite>();
        List<Sprite> newFrames3 = new List<Sprite>();
        List<Sprite> newFrames4 = new List<Sprite>();
        newFrames.Clear();
        loadedThing.animations.TryGetValue("idle", out framesNames);
        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < framesNames.Count; j++)
            {
                if (frames[i].name == framesNames[j])
                {
                    newFrames.Add(frames[i]);
                }
            }
        }
        animations.Add("idle", newFrames);
        framesNames.Clear();

        newFrames1.Clear();
        loadedThing.animations.TryGetValue("Sing Left", out framesNames1);
        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < framesNames1.Count; j++)
            {
                if (frames[i].name == framesNames1[j])
                {
                    newFrames1.Add(frames[i]);
                }
            }
        }
        animations.Add("Sing Left", newFrames1);
        framesNames1.Clear();

        newFrames2.Clear();
        loadedThing.animations.TryGetValue("Sing Down", out framesNames2);
        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < framesNames2.Count; j++)
            {
                if (frames[i].name == framesNames2[j])
                {
                    newFrames2.Add(frames[i]);
                }
            }
        }
        animations.Add("Sing Down", newFrames2);
        framesNames.Clear();

        newFrames3.Clear();
        loadedThing.animations.TryGetValue("Sing Up", out framesNames3);
        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < framesNames3.Count; j++)
            {
                if (frames[i].name == framesNames3[j])
                {
                    newFrames3.Add(frames[i]);
                }
            }
        }
        animations.Add("Sing Up", newFrames3);
        framesNames.Clear();

        newFrames4.Clear();
        loadedThing.animations.TryGetValue("Sing Right", out framesNames4);
        for (int i = 0; i < frames.Length; i++)
        {
            for (int j = 0; j < framesNames4.Count; j++)
            {
                if (frames[i].name == framesNames4[j])
                {
                    newFrames4.Add(frames[i]);
                }
            }
        }
        animations.Add("Sing Right", newFrames4);
        framesNames.Clear();

        icons[0] = IMG2Sprite.LoadNewSprite(filePath + @"\icon.png");
        icons[1] = IMG2Sprite.LoadNewSprite(filePath + @"\icon-ded.png");
    }
}

[Serializable]
public class ExportableFnfCharacter
{
    public string characterName;
    public string xmlPath;

    public Dictionary<string, List<string>> animations;

    public Dictionary<string, List<SerializableVector2>> piviots;
}
