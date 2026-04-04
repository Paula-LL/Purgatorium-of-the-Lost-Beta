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
            playerController = FindObjectOfType<Player_controller>();
        return playerController;
    }

    public HealthBar healthBar;

    public PlayerStats currentPlayerStats;
    public PlayerStats.Movement currentMovement;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isDashing = false;
    private float dashTimeLeft = 0f;

    public List<LoversNormalModifier> stats = new List<LoversNormalModifier>();
    public List<ChariotNormalModifier> modifierMovementList = new List<ChariotNormalModifier>();

    [Header("Particles")]
    [SerializeField]
    private ParticleSystem characterDamageParticles;
    private ParticleSystem characterDamageParticlesInstance;

    private void Awake()
    {
        if (!playerController)
            playerController = this;

        // Inicializar stats en Awake para que HealthBar pueda acceder en su Start
        currentPlayerStats = new PlayerStats();
        currentMovement = currentPlayerStats.movement;
    }

    void Start()
    {
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
                StartDash();
        }
        else if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
                isDashing = false;
        }

        float speed = isDashing ? currentMovement.dashSpeed : currentMovement.moveSpeed;
        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    void HandleAttack()
    {
        bool attackKeyboard = Input.GetKeyDown(KeyCode.F);
        bool attackGamepad  = Input.GetKeyDown(KeyCode.JoystickButton0);

        if (attackKeyboard || attackGamepad)
            PerformAttack();
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
        float finalDamage = Mathf.Max(amount - currentPlayerStats.defense, 0f);
        currentPlayerStats.currentHealth -= finalDamage;
        EstadisticasJuego.RegistrarDanoRecibido(finalDamage);
        healthBar.UpdateHealthBar();

        if (currentPlayerStats.currentHealth <= 0)
            Die();

        SpawnDamageParticles();
    }

    private void SpawnDamageParticles()
    {
        if (characterDamageParticles != null)
            characterDamageParticlesInstance = Instantiate(characterDamageParticles, transform.position, Quaternion.identity);
    }

    public void HealHealth(int amount)
    {
        currentPlayerStats.currentHealth = Mathf.Min(currentPlayerStats.currentHealth + amount, currentPlayerStats.maxHealth);
        healthBar.UpdateHealthBar();
    }

    void Die()
    {
        EstadisticasJuego.RegistrarMuerteJugador();
        Destroy(gameObject);
    }

    internal void AddModifier(ChariotNormalModifier cardsBuff)
    {
        modifierMovementList.Add(cardsBuff);
        ApplyChariotModifier(currentMovement);
    }

    void ApplyChariotModifier(PlayerStats.Movement m)
    {
        foreach (ChariotNormalModifier modifier in modifierMovementList)
            modifier.ApplyChariotNormalCardModifier(m);
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
            modifier.ApplyLoversNormalCardModifier(p);
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