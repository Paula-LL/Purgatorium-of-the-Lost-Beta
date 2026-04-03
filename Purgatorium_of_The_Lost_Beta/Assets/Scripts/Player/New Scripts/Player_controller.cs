using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_controller;
using static PlayerStats;

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

    public PlayerStats currentPlayerStats;
    public PlayerStats.Movement currentMovement;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;

    public List<LoversNormalModifier> loversBaseModifierList = new List<LoversNormalModifier>();
    public List<LoversInvertedModifier> loversInvertedModifierList = new List<LoversInvertedModifier>();
    public List<ChariotNormalModifier> modifierMovementList = new List<ChariotNormalModifier>();
    public List<ChariotInvertedModifier> charriotInvertedModifier = new List<ChariotInvertedModifier>();

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
        currentPlayerStats = new PlayerStats();
        currentMovement = currentPlayerStats.movement;
        controller = GetComponent<CharacterController>();
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
        }

        if (!isDashing && moveDirection.magnitude > 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton3))
            {
                StartDash();
            }
        }
        else if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
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
        currentMovement = new PlayerStats.Movement(currentMovement);
        dashTimeLeft = currentMovement.dashDuration;
    }

    public void TakeDamage(float amount)
    {
        float finalDamage = Mathf.Max(amount - currentPlayerStats.defense, 0);
        currentPlayerStats.currentHealth -= finalDamage;
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
        Destroy(gameObject);
    }


    //THE CHARIOT BASE MODIFIERS
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

    //THE CHARIOT INVERTED MODIFIERS
    void ApplyChariotInvertedModifier(Movement m)
    {
        foreach (ChariotInvertedModifier modifier in charriotInvertedModifier)
        {
            modifier.ApplyChariotInvertedCardModifier(m);
        }
    }

    internal void AddModifier(ChariotInvertedModifier cardsBuff)
    {
        charriotInvertedModifier.Add(cardsBuff);
        ApplyChariotInvertedModifier(currentMovement);
    }


    //THE LOVERS INVERTED MODIFIERS
    internal void AddModifier(LoversInvertedModifier cardsBuff, bool updateUI = true)
    {
        loversInvertedModifierList.Add(cardsBuff);
        ApplyLoversInvertedModifiers(currentPlayerStats, updateUI);
    }

    void ApplyLoversInvertedModifiers(PlayerStats p, bool updateUI = true)
    {
        p.maxHealth = p.baseHealth;
        foreach (LoversInvertedModifier modifier in loversInvertedModifierList)
        {
            modifier.ApplyLoversInvertedCardModifier(p);
        }

        if (updateUI)
            healthBar.UpdateHealthBar();
    }


    //THE LOVERS BASE MODIFIERS
    internal void AddModifier(LoversNormalModifier cardsBuff, bool updateUI = true)
    {
        loversBaseModifierList.Add(cardsBuff);
        ApplyLoversNormalModifiers(currentPlayerStats, updateUI);
    }

    void ApplyLoversNormalModifiers(PlayerStats p, bool updateUI = true)
    {
        p.maxHealth = p.baseHealth;
        foreach (LoversNormalModifier modifier in loversBaseModifierList)
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