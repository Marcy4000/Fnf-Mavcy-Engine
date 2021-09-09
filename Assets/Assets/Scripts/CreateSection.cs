using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateSection : MonoBehaviour
{

    public List<GameObject> TogglesObject;
    public List<bool> Values;

    public int SectionId = 1;

    //create a list of the toggles, kinda useless. just needed fot the update list method
    void Start()
    {
        foreach (Transform child in transform)
        {
            TogglesObject.Add(child.gameObject);
        }
        UpdateList();
        
    }
    //updates chart list with booleans, gonna change this soon
    public void UpdateList()
    {
        Values.Clear();
        
        foreach (GameObject Value in TogglesObject)
        {
            
            Toggle thing;

            thing = Value.GetComponent<Toggle>();
            Values.Add(thing.isOn);
            
        }
    }

    

}
