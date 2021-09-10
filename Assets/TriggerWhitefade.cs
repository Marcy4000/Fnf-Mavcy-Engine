using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhitefade : MonoBehaviour
{
    public Animator whiteFade;
    
    public void triggerAnimation()
    {
        whiteFade.SetTrigger("Start");
    }
}
