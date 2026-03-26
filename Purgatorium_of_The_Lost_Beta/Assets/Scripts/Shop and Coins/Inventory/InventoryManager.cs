using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;
public class InventoryManager : MonoBehaviour
{
    public int goldQuantity;
    public TMP_Text goldTextQuantity;
    public GameObject lootPrefab;
    public Transform player; 

    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem; 
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    public void AddItem(ItemScriptableObject itemScriptable, int quantity) {// Check if the object is gold
        if (itemScriptable.isGold)
        {
            goldQuantity += quantity;
            goldTextQuantity.text = goldQuantity.ToString();
            return;
        }
    }
}
