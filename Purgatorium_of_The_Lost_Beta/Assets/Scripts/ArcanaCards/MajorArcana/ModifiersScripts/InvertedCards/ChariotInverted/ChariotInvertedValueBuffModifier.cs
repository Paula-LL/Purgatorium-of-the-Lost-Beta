using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/The Chariot (Inverted)")]

public class ChariotInvertedlValueBuffModifier : ChariotInvertedModifier
{
    public int dropChance;
    public float charriotInvertedSpeedDown;

    public override void ApplyChariotInvertedCardModifier(Movement movement)
    {
        movement.moveSpeed -= (movement.moveSpeed * charriotInvertedSpeedDown) / 100;
    }

}
