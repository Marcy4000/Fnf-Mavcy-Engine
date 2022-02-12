using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModsList : MonoBehaviour
{
    public GameObject thingPrefab;
    public RectTransform viewport;
    public List<GameObject> things;

    private void Start()
    {
        UpdateModList();
    }

    public void UpdateModList()
    {
        if (things.Count != 0)
        {
            foreach (GameObject gameObject in things)
            {
                Destroy(gameObject);
            }
        }
        things.Clear();
        for (int i = 0; i < GlobalDataSfutt.mods.Count; i++)
        {
            ModListItem item = Instantiate(thingPrefab, viewport).GetComponent<ModListItem>();
            item.title.text = GlobalDataSfutt.mods[i].data.title;
            item.description.text = GlobalDataSfutt.mods[i].data.description;
            item.icon.sprite = GlobalDataSfutt.mods[i].modIcon;
            item.hasCustomCharacter.isOn = GlobalDataSfutt.mods[i].characters.Count > 0;
            item.hasCustomSongs.isOn = GlobalDataSfutt.mods[i].songNames.Count > 0;
            item.hasCustomWeeks.isOn = GlobalDataSfutt.mods[i].weeks.Count > 0;
            things.Add(item.gameObject);
        }
        viewport.sizeDelta = new Vector2(viewport.sizeDelta.x, 300 * things.Count + 10 * things.Count - 1);
    }
}
