using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSettings : MonoBehaviour
{
    public static StageSettings instance;
    public Transform playerPos, gfPos, enemyPos;
    public Transform playerCam, enemyCam;
    public int playerLayer, gfLayer, enemyLayer;

    private void Start()
    {
        instance = this;
    }

}
