using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardsLoot : ScriptableObject
{
    public GameObject cardPrefabs;
    public int dropChance;

    public CardsLoot(GameObject cardPrefabs, int dropChance) { 
        this.cardPrefabs = cardPrefabs;
        this.dropChance = dropChance;
    }
    //call to intances of ScriptableObjects
    /*
     Scriptable Nesting (dont forget casting):
        SO with Call to dropChance and public ScriptableObject variable
     */
}
