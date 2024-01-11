using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpawnList : ScriptableObject
{
    [SerializeField] private List<GameObject> itemList;
    public GameObject this[int i] { get => itemList[i]; }
    public int Count => itemList.Count;
}