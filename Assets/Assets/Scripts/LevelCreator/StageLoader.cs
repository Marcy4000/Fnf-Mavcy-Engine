using System.Collections;
using System.Collections.Generic;
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
        }
        StageSettings.instance.playerPos.position = new Vector3(stage.bfPos.x, stage.bfPos.y, 0f);
        StageSettings.instance.gfPos.position = new Vector3(stage.gfPos.x, stage.gfPos.y, 0f);
        StageSettings.instance.enemyPos.position = new Vector3(stage.enemyPos.x, stage.enemyPos.y, 0f);
        StageSettings.instance.playerCam.position = new Vector3(stage.bfCamPos.x, stage.bfCamPos.y, 0f);
        StageSettings.instance.enemyCam.position = new Vector3(stage.enemyCamPos.x, stage.enemyCamPos.y, 0f);
    }
}