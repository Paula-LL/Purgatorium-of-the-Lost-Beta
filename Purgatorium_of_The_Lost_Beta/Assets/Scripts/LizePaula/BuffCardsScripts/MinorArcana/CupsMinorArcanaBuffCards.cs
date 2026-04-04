using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupsMinorArcanaBuffCards : BuffCards
{
    [SerializeField]
    CupsModifier cupsHealing;

    public override void PickUpCard(Collider collision)
    {
        base.PickUpCard(collision);

        if (Player_controller.instance.currentPlayerStats.currentHealth < Player_controller.instance.currentPlayerStats.maxHealth)
        {
            cupsHealing.ApplyCupsCardModifier(Player_controller.instance);
            //collision.GetComponent<Player_controll                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          er>().AddModifier(cupsHealing);
            //Player_controller.instance.currentPlayerStats.currentHealth += cupsHealing; 
        }
        Debug.Log("Picked up: Cups");
    }
}
