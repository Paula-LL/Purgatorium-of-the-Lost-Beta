using System.Collections.Generic;
using UnityEngine;

public class EnemigoBase : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f; // Distancia para empezar a seguir

    [Header("Damage Settings")]
    [SerializeField] private float timeBetweenAttacks = 3f;
    [SerializeField] private int damagePerAttack = 1;

    [Header("Health Settings")]
    public int maxHealth = 3;
    private float currentHealth;

    private Transform player;
    private bool playerInRange = false;
    private bool playerDetected = false;
    private float timeInRange = 0f;
    private float lastDamageTime = 0f;
    private Animator animator;

    public static List<EnemigoBase> enemyList = new List<EnemigoBase>();

    void Awake()
    {
        enemyList.Add(this);
    }

    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentHealth <= 0) return;

        // Verificar distancia al player
        CheckPlayerDetection();

        float speed = 0f;

        // Solo seguir si el player está detectado
        if (playerDetected)
        {
            speed = FollowPlayer();
        }

        if (playerInRange)
            ProcessDamage();

        UpdateAnimation(speed);
    }

    void CheckPlayerDetection()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        playerDetected = distance <= detectionRange;
    }

    float FollowPlayer()
    {
        if (player == null) return 0f;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        if (!playerInRange && distance > 0.1f)
        {
            Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
            transform.position += move;
            transform.forward = direction.normalized;
            return move.magnitude / Time.deltaTime;
        }

        return 0f;
    }

    void ProcessDamage()
    {
        timeInRange += Time.deltaTime;

        if (timeInRange >= lastDamageTime + timeBetweenAttacks)
        {
            DealDamageToPlayer();
            lastDamageTime = timeInRange;
        }
    }

    void DealDamageToPlayer()
    {
        if (player != null)
        {
            Player_controller playerScript = player.GetComponent<Player_controller>();
            if (playerScript != null)
                playerScript.TakeDamage(damagePerAttack);
        }
    }

    void UpdateAnimation(float speed)
    {
        if (animator == null) return;

        animator.SetFloat("Speed", speed);
        animator.SetBool("isAttacking", playerInRange);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        enemyList.Remove(this);
        if (animator != null)
            animator.SetTrigger("Die");

        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            timeInRange = 0f;
            lastDamageTime = timeBetweenAttacks;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            timeInRange = 0f;
        }
    }
}