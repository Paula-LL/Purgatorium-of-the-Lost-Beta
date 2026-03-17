using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("Ajustes de Vida")]
    [Tooltip("Vida máxima del boss.")]
    [SerializeField] private float vidaMaxima = 5f;

    private float vidaActual;

    public float VidaActual => vidaActual;
    public float VidaMaxima => vidaMaxima;
    public bool EstaMuerto => vidaActual <= 0f;

    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDanio(float cantidad)
    {
        if (EstaMuerto) return;

        vidaActual -= cantidad;
        vidaActual = Mathf.Max(vidaActual, 0f);

        Debug.Log($"[BossHealth] {gameObject.name} recibió {cantidad} de daño. Vida: {vidaActual}/{vidaMaxima}");

        if (vidaActual <= 0f)
            Morir();
    }

    private void Morir()
    {
        Debug.Log($"[BossHealth] {gameObject.name} ha muerto.");

        // Destruir el objeto del Boss
        Destroy(gameObject);
    }
}