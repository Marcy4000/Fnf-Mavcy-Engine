using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationPlayer : MonoBehaviour
{
    public TMP_Dropdown charList;
    public TMP_Text selectedChar;
    
    public void UpdateCharacterList()
    {
        List<string> characters = new List<string>();
        characters.Clear();
        for (int i = 0; i < GlobalDataSfutt.customCharacters.Count; i++)
        {
            characters.Add(GlobalDataSfutt.customCharacters[i].characterName);
        }

        charList.ClearOptions();
        charList.AddOptions(characters);
    }

    public void PlayAnimation(string animName)
    {
        if (LevelEditorManager.Instance.selectedSelectableObject == null)
            return;
        
        LevelEditorManager.Instance.selectedSelectableObject.PlayNew(animName, charList.value);
    }
}
