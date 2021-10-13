using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongMenager : MonoBehaviour
{
    public AudioSource inst;
    public float songTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        songTime = inst.time;
    }
}
