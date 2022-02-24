using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance { get; private set; }
    
    void Start()
    {
        Instance = this;
    }

    public void LoadStage(Level stage, string path)
    {
        foreach (ExportableLevelObject lObject in stage.levelObjects)
        {
            GameObject stageObject = new GameObject("No Name Sad");
            SpriteRenderer renderer = stageObject.AddComponent<SpriteRenderer>();
            renderer.sprite = IMG2Sprite.LoadNewSprite(path + @"\" + lObject.imageName);
            renderer.sortingOrder = lObject.layer;
            stageObject.transform.position = new Vector3(lObject._position.x, lObject._position.y, 0f);
            stageObject.transform.localScale = new Vector3(lObject._scale.x, lObject._scale.y, 1f);
            if (lObject.hasAnimation)
            {
                RuntimeLevelObject rLevelObject = stageObject.AddComponent<RuntimeLevelObject>();
                rLevelObject.defAnim = lObject.defID;
                TextureAtlasXml spriteSheet = GlobalDataSfutt.ImportXml<TextureAtlasXml>(Path.ChangeExtension(path + @"\" + lObject.imageName, null) + ".xml");
                Debug.Log(Path.ChangeExtension(path + @"\" + lObject.imageName, null) + ".xml");
                Texture2D SpriteTexture = IMG2Sprite.LoadTexture(path + @"\" + spriteSheet.imagePath);
                rLevelObject.frames = new Sprite[spriteSheet.frame.Length];
                for (int i = 0; i < spriteSheet.frame.Length; i++)
                {
                    rLevelObject.frames[i] = IMG2Sprite.LoadSpriteSheet(SpriteTexture, new Rect(spriteSheet.frame[i].x, SpriteTexture.height - spriteSheet.frame[i].y, spriteSheet.frame[i].width, -spriteSheet.frame[i].height), new Vector2(0.5f, 1), 100f, SpriteMeshType.Tight);
                    rLevelObject.frames[i].name = spriteSheet.frame[i].name;
                }
                rLevelObject.OrderAnimations();
            }
        }
        StageSettings.instance.playerPos.position = new Vector3(stage.bfPos.x, stage.bfPos.y, 0f);
        StageSettings.instance.gfPos.position = new Vector3(stage.gfPos.x, stage.gfPos.y, 0f);
        StageSettings.instance.enemyPos.position = new Vector3(stage.enemyPos.x, stage.enemyPos.y, 0f);
        StageSettings.instance.playerCam.position = new Vector3(stage.bfCamPos.x, stage.bfCamPos.y, 0f);
        StageSettings.instance.enemyCam.position = new Vector3(stage.enemyCamPos.x, stage.enemyCamPos.y, 0f);
    }
}