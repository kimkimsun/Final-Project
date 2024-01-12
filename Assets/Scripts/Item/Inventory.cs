using CustomInterface;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public abstract class Inventory : MonoBehaviour
{
    public abstract void AddItem(Item item);
}