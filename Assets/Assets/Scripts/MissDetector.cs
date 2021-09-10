using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissDetector : MonoBehaviour
{

    public int misses = 0;
    public TMP_Text missCounter;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject note = collision.gameObject;
        Destroy(note);
        misses++;
        missCounter.text = "Misses: " + misses;
    }
}
