using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Major Arcana/The Lovers (Inverted)")]

public class LoversInvertedValueBuffModifier : LoversNormalModifier
{
    public int healthDrop;
    public override void ApplyLoversNormalCardModifier(PlayerStats health)
    {
        health.maxHealth -= (healthDrop * 50)/100;
    }

}