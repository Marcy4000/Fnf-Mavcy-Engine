using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModsList : MonoBehaviour
{
    public GameObject togglePrefab;
    public Transform viewport;
    public List<GameObject> toggles;

    private void Start()
    {
        UpdateModList();
    }

    public void UpdateModList()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            Destroy(toggles[i]);
        }
        toggles.Clear();
        for (int i = 0; i < GlobalDataSfutt.customCharacters.Count; i++)
        {
            GameObject thing = Instantiate(togglePrefab, viewport);
            thing.GetComponentInChildren<TMP_Text>().text = GlobalDataSfutt.customCharacters[i].characterName;
            thing.GetComponent<Toggle>().isOn = GlobalDataSfutt.characterActive[i];
            thing.GetComponent<MenuListItem>().ID = i;
            toggles.Add(thing);
        }
    }
}
