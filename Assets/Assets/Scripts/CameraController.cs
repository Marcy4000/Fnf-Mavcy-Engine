using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    
    public float speed = .5f;

    private Vector3 start;
    private Vector3 des;

    private float fraction = 1;


    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (fraction < 1)
        {
            fraction += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, des, fraction);
        }
    }

    public void Transition(bool lookAtEnemy)
    {
        switch (lookAtEnemy)
        {
            case false:
                start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                des = StageSettings.instance.playerCam.position;
                fraction = 0;
                break;
            case true:
                start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                des = StageSettings.instance.enemyCam.position;
                fraction = 0;
                break;
        }
    }
}
