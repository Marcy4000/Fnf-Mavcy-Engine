using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCameraController : MonoBehaviour
{
    public float speed = .5f;

    public Transform bfCam, enemyCam;

    private Vector3 start;
    private Vector3 des;

    private float fraction = 1;

    void Update()
    {
        if (fraction < 1)
        {
            fraction += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, des, fraction);
        }

        if (transform.position.z > -10f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
        }
    }

    public void Transition(bool lookAtEnemy)
    {
        switch (lookAtEnemy)
        {
            case false:
                start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                des = bfCam.transform.position;
                fraction = 0;
                break;
            case true:
                start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                des = enemyCam.transform.position;
                fraction = 0;
                break;
        }
    }
}
