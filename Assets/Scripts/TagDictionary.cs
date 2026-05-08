using UnityEngine;
using System.Collections.Generic;

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

    public static int GetLayerMask(TagDictionary[] tagDictionary)
    {
        int num = 0;

        foreach (var tag in tagDictionary)
        {
            num += LayerMask.GetMask(tag.ToString());
        }

        return num;
    }

    public static int GetLayerMask( List<TagDictionary> tagDictionary )
    {
        int num = 0;

        foreach (var tag in tagDictionary)
        {
            num += LayerMask.GetMask(tag.ToString());
        }

        return num;
    }
}

