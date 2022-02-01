using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Create New Character", order = 0), Serializable]
public class Character : ScriptableObject
{
    public string Name;
    public AnimatorOverrideController overrideAnimator;
    public bool flipX;
    public Sprite icon;
    public Sprite deadIcon;
    public Vector2 iconSize;
}
