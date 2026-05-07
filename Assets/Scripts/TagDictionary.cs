using UnityEngine;

public enum TagDictionary
{
    Default,
    TransparentFX,
    IgnoreRaycast,
    Player,
    Water,
    UI,
    Enemy,
    Ground,
    Wall,
    ElevatorButton,
    WeaponCamera,
    LazerGunBall
}

public class Tag
{
    public static bool CompareTag(string tag, TagDictionary tagDictionary)
    {
        return tag == tagDictionary.ToString();
    }
}

