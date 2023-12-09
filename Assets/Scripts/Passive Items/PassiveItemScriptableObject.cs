using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int passiveItemLevel;
    public int PassiveItemLevel { get => passiveItemLevel; private set => passiveItemLevel = value; }

    [SerializeField]
    GameObject nextLevelPrefab; //next level of this passive item
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    Sprite passiveItemSprite;
    public Sprite PassiveItemSprite { get => passiveItemSprite; private set => passiveItemSprite = value; }

    [SerializeField]
    string name;
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

}
