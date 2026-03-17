using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerAttack : MonoBehaviour
{
    Attack baseAttack = new Attack();

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Renderer rend;
    private Color originalColor;
    private LineRenderer lineRenderer;
    public List<AttackModifier> modifierAttackList = new List<AttackModifier>();
    public EnemigoBase enemigoBase;
    public EnemigoDist enemigoDist;
    public BossHealth bossHealth;


    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = baseAttack.circleSegments + 1;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.enabled = false;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = baseAttack.attackDuration;
            rend.material.color = Color.red;

            Attack attack = new Attack(baseAttack);
            ApplyAttackModifiers(attack);


            AttackEnemies(attack);
            DrawAttackCircle(attack);
            lineRenderer.enabled = true;
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
                rend.material.color = originalColor;
                lineRenderer.enabled = false;
            }
        }
    }

    void AttackEnemies(Attack attack)
    {
        Vector3 center = transform.position + transform.forward * attack.attackDistance;
        Collider[] hitColliders = Physics.OverlapSphere(center, attack.attackRadius);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                // SOLO ESTA LÍNEA CAMBIÓ:
                enemigoDist = col.GetComponent<EnemigoDist>();
                enemigoBase = col.GetComponent<EnemigoBase>();    
                if (enemigoDist != null)
                {
                    enemigoDist.TakeDamage(attack.attackDamage);

                }
                else if (enemigoBase!= null)
                {
                   enemigoBase.TakeDamage(attack.attackDamage);
                }
            }
            if (col.CompareTag("Boss"))
            {
                bossHealth.RecibirDanio(attack.attackDamage);
            }
        }
    }

    void DrawAttackCircle(Attack attack)
    {
        Vector3 center = transform.position + transform.forward * attack.attackDistance;
        for (int i = 0; i <= attack.circleSegments; i++)
        {
            float angle = i * Mathf.PI * 2 / attack.circleSegments;
            float x = Mathf.Cos(angle) * attack.attackRadius;
            float z = Mathf.Sin(angle) * attack.attackRadius;
            Vector3 pos = center + new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
        }
    }

    internal void AddModifier(AttackModifier cardsBuff)
    {
        modifierAttackList.Add(cardsBuff);
    }

    void ApplyAttackModifiers(Attack a)
    {
        foreach (AttackModifier modifier in modifierAttackList)
        {
            modifier.ApplyAttackModifier(a);
        }
    }
}

[System.Serializable]
public class Attack
{
    public float attackDistance = 1.2f;
    public float attackRadius = 0.4f;
    public float attackDuration = 0.2f;
    public float attackDamage = 1;
    public int circleSegments = 30;


    public Attack()
    {
        this.attackDistance = 2.5f;
        this.attackRadius = 2.5f;
        this.attackDuration = 0.8f;
        this.attackDamage = 1;
        this.circleSegments = 30;
    }
    public Attack(float attackDistance, float attackRadius, float attackDuration, float attackDamage, int circleSegments)
    {
        this.attackDistance = attackDistance;
        this.attackRadius = attackRadius;
        this.attackDuration = attackDuration;
        this.attackDamage = attackDamage;
        this.circleSegments = circleSegments;
    }
    public Attack(Attack attack)
    {
        attackDistance = attack.attackDistance;
        attackRadius = attack.attackRadius;
        attackDuration = attack.attackDuration;
        attackDamage = attack.attackDamage;
        circleSegments = attack.circleSegments;
    }

}