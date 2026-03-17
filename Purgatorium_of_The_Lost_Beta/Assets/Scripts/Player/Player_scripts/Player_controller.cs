using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_controller;

[RequireComponent(typeof(CharacterController))]
public class Player_controller : MonoBehaviour
{
    public static Player_controller playerController;
    public static Player_controller instance
    {
        get { return RequestPlayerControllerReference(); }
    }

    private static Player_controller RequestPlayerControllerReference()
    {
        if (!playerController)
        {
            playerController = FindObjectOfType<Player_controller>();
        }
        return playerController;
    }

    public HealthBar healthBar;
    public Animator animator;
    public Movement currentMovement;
    public PlayerStats currentPlayerStats;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;

    public List<LoversNormalModifier> stats = new List<LoversNormalModifier>();
    public List<ChariotNormalModifier> modifierMovementList = new List<ChariotNormalModifier>();
    public AudioSource playerAudio;

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem characterDamageParticles;
    private ParticleSystem characterDamageParticlesInstance;

    private void Awake()
    {
        if (!playerController)
            playerController = this;
    }

    void Start()
    {
        currentMovement = new Movement();
        currentPlayerStats = new PlayerStats();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ApplyLoversNormalModifiers(currentPlayerStats);
        healthBar.UpdateHealthBar();
      
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        moveDirection = new Vector3(x, 0, z).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentMovement.rotationSpeed * Time.deltaTime);
            animator.SetFloat("Speed", 1);
            animator.SetBool("IsAttacking", false);
            playerAudio.Play();
        }
        else
        {
            animator.SetFloat("Speed", 0);
            playerAudio.Pause();
        }

        if (!isDashing && moveDirection.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton3))
            {
                StartDash();
                animator.SetBool("IsDashing", true);
            }
        }
        else if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                animator.SetBool("IsDashing", false);
            }
        }

        float speed = isDashing ? currentMovement.dashSpeed : currentMovement.moveSpeed;
        
        controller.Move(moveDirection * speed * Time.deltaTime);

       
    }

    void HandleAttack()
    {
        bool attackKeyboard = Input.GetKeyDown(KeyCode.F);
        bool attackGamepad = Input.GetKeyDown(KeyCode.JoystickButton0);

        if (attackKeyboard || attackGamepad)
        {
            animator.SetBool("IsAttacking", true);
            PerformAttack();
        }
        
    }

    void PerformAttack()
    {
        
        Debug.Log("Ataque realizado");
       
    }

    void StartDash()
    {
        isDashing = true;
        currentMovement = new Movement(currentMovement);
        dashTimeLeft = currentMovement.dashDuration;
    }

    public void TakeDamage(int amount)
    {
        currentPlayerStats.currentHealth -= amount;
        Debug.Log($"Jugador recibe {amount} de daño. Vida actual: {currentPlayerStats.currentHealth}/{currentPlayerStats.maxHealth}");
        healthBar.UpdateHealthBar();

        if (currentPlayerStats.currentHealth <= 0)
        {
            Die();
        }

        SpawnDamageParticles();
    }

    private void SpawnDamageParticles()
    {
        characterDamageParticlesInstance = Instantiate(characterDamageParticles, transform.position, Quaternion.identity);
    }

    public void HealHealth(int amount)
    {
        currentPlayerStats.currentHealth = Mathf.Min(currentPlayerStats.currentHealth + amount, currentPlayerStats.maxHealth);
        healthBar.UpdateHealthBar();
    }

    void Die()
    {
        Debug.Log("Jugador ha muerto");
        animator.SetBool("IsDead", true );
        Destroy(gameObject);
    }

    internal void AddModifier(ChariotNormalModifier cardsBuff)
    {
        modifierMovementList.Add(cardsBuff);
        ApplyChariotModifier(currentMovement);
    }

    void ApplyChariotModifier(Movement m)
    {
        foreach (ChariotNormalModifier modifier in modifierMovementList)
        {
            modifier.ApplyChariotNormalCardModifier(m);
        }
    }

    internal void AddModifier(LoversNormalModifier cardsBuff, bool updateUI = true)
    {
        stats.Add(cardsBuff);
        ApplyLoversNormalModifiers(currentPlayerStats, updateUI);
    }

    void ApplyLoversNormalModifiers(PlayerStats p, bool updateUI = true)
    {
        p.maxHealth = p.baseHealth;
        foreach (LoversNormalModifier modifier in stats)
        {
            modifier.ApplyLoversNormalCardModifier(p);
        }
        if (updateUI)
            healthBar.UpdateHealthBar();
    }

    internal void SetCurrentHealthToMax()
    {
        currentPlayerStats.currentHealth = currentPlayerStats.maxHealth;
        healthBar.UpdateHealthBar();
    }

    internal void SetCurrentHealth(float value)
    {
        currentPlayerStats.currentHealth = value;
        healthBar.UpdateHealthBar();
    }
}

[System.Serializable]
public class PlayerStats
{
    public float baseHealth;
    public float maxHealth = 5;
    public float currentHealth;

    public PlayerStats()
    {
        this.maxHealth = 5;
        this.baseHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public PlayerStats(float maxHealth)
    {
        this.maxHealth = maxHealth;
        this.baseHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public PlayerStats(PlayerStats playerStats)
    {
        maxHealth = playerStats.maxHealth;
        baseHealth = playerStats.baseHealth;
        currentHealth = playerStats.currentHealth;
    }
}

[System.Serializable]
public class Movement
{
    public float moveSpeed = 5f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float rotationSpeed = 10f;

    public Movement() { }

    public Movement(Movement movement)
    {
        moveSpeed = movement.moveSpeed;
        dashSpeed = movement.dashSpeed;
        dashDuration = movement.dashDuration;
        rotationSpeed = movement.rotationSpeed;
    }
}