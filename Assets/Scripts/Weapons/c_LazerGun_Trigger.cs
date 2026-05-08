using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class c_LazerGun_Trigger : MonoBehaviour
{
    private List<GameObject> EnemyObjects;
    private GameObject _PlayerObjectContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyObjects = new List<GameObject>();
        _PlayerObjectContainer = GameObject.Find("PlayerObjectContainer");
        transform.parent = _PlayerObjectContainer.transform;
    }

    public void InitOrbTrigger(Vector3 _pos, Vector3 _eulerAng, Vector3 _scale)
    {
        transform.position = _pos;
        transform.eulerAngles = _eulerAng;
        transform.localScale = _scale;

        GetComponent<BoxCollider>().enabled = true;
    }

    void DisableOrbTrigger()
    {
        GetComponent<BoxCollider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(Tag.CompareTag(other.gameObject.tag, TagDictionary.Enemy))
        {
            EnemyObjects.Add(other.gameObject);
            print("Added: " + other.gameObject.name);
        }


        // UnityEditorInternal.InternalEditorUtility.tags.
    }

    private void OnTriggerExit(Collider other)
    {
        if (Tag.CompareTag(other.gameObject.tag, TagDictionary.Enemy))
        {
            EnemyObjects.Remove(other.gameObject);
            print("Removed: " + other.gameObject.name);
        }
    }
}
