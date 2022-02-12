using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModListItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text title;
    public TMP_Text description;

    public Toggle hasCustomCharacter, hasCustomSongs, hasCustomWeeks;
}
