using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LootPool : MonoBehaviour
{
    public GameObject spawnedCardPrefab;
    public List<CardsLoot> lootCards = new List<CardsLoot>();

    public CardsLoot GetRandomCard()
    {
        float randomNum = Random.Range(0.0f, 100.0f);
        float counter = 0;
        CardsLoot returnCard = null;
        int i = 0;
        while (returnCard == null)
        {
            if (i != lootCards.Count)
            {
                if (randomNum < counter + lootCards[i].dropChance)
                {
                    returnCard = lootCards[i];
                }
                else
                {
                    counter += lootCards[i].dropChance;
                    i++;
                }
            }
            else
            {
                returnCard = lootCards[i];
            }
        }
        return returnCard;
    }

    public void InstantiateCardLoot(Vector3 spawnPosition)
    {
        CardsLoot droppedCard = GetRandomCard();
        Debug.Log(droppedCard);

    }

#if UNITY_EDITOR
    [ContextMenu("InstantiateDebug")]
#endif
    public void InstantiateDebug()
    {
        for (int i = 0; i < 1000; i++)
        {

            Debug.Log(GetRandomCard().name);
        }

    }

}

//public CardsLoot dropChanceLoot; 

    /*List<CardsLoot> GetDroppedCard() {
        float randomNumber = Random.Range(1f, 100.01f);
        List<CardsLoot> possibleCards = new List<CardsLoot>();

        foreach (CardsLoot cards in lootCards) {
            if (randomNumber <= cards.dropChance) { 
                possibleCards.Add(cards);
                return possibleCards; 
            }
        }

        if (possibleCards.Count > 0) { 
            CardsLoot droppedCard = possibleCards[Random.Range(0,possibleCards.Count)];
        }

       // Mathf.Max(dropChanceLoot.dropChance);

        return null;//placeholder, return card with highest drop rate.  
    }*/