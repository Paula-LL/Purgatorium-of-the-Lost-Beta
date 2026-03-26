using UnityEngine;

[CreateAssetMenu(menuName = "Minor Arcana / Pentacles")]

public class ItemScriptableObject : ScriptableObject
{    

    public GameObject itemPrefab;
    public bool isGold;

    public int stackSize = 9999; 
}