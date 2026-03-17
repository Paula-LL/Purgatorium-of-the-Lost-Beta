using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Major Arcana/The Hanged Man (Normal)")]

public class AttackValueBuffModifier : AttackModifier
{
    public int dropChance; 
    public float hangedManAttackUp;
    public override void ApplyAttackModifier(Attack attack)
    {
        hangedManAttackUp = Random.Range(1.1f, 2f); 
        attack.attackDamage *= hangedManAttackUp;
    }
}
