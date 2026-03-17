using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerMenuPrincipal : MonoBehaviour
{
    [Header("Configuración de Botones")]
    [SerializeField] private Button botonInicio;
    [SerializeField] private Button botonAjustes;
    [SerializeField] private Button botonSalir;

    [Header("Botones Extra para Canvas")]
    [SerializeField] private Button botonAyuda;
    [SerializeField] private Button botonCreditos;


    [Header("Configuración de Escena")]
    [SerializeField] private string nombreEscena = "Mapa";

    [Header("Configuración de Canvas Principal")]
    [SerializeField] private Canvas canvasActivar;
    [SerializeField] private Canvas canvasDesactivar;

    [Header("Configuración de Canvas Ayuda")]
    [SerializeField] private Canvas canvasActivarAyuda;
    [SerializeField] private Canvas canvasDesactivarMenu1;

    [Header("Configuración de Canvas Creditos")]
    [SerializeField] private Canvas canvasActivarCreditos;
    [SerializeField] private Canvas canvasDesactivarMenu;


    [Header("Sonido de Botones")]
    [SerializeField] private AudioClip sonidoBoton;
    [SerializeField] private AudioSource audioSourceBoton;

    void Start()
    {
        // Asignar los métodos a los botones
        if (botonInicio != null)
        {
            botonInicio.onClick.AddListener(CambiarEscena);
            botonInicio.onClick.AddListener(ReproducirSonido);
        }

        if (botonAjustes != null)
        {
            botonAjustes.onClick.AddListener(AlternarCanvas);
            botonAjustes.onClick.AddListener(ReproducirSonido);
        }

        if (botonSalir != null)
        {
            botonSalir.onClick.AddListener(SalirDelJuego);
            botonSalir.onClick.AddListener(ReproducirSonido);
        }

        // Asignar los métodos a los botones extra
        if (botonAyuda != null)
        {
            botonAyuda.onClick.AddListener(AlternarCanvasAyuda);
            botonAyuda.onClick.AddListener(ReproducirSonido);
        }

        if (botonCreditos != null)
        {
            botonCreditos.onClick.AddListener(AlternarCanvasCreditos);
            botonCreditos.onClick.AddListener(ReproducirSonido);
        }
    }

    // Método para reproducir sonido
    private void ReproducirSonido()
    {
        if (sonidoBoton != null)
        {
            if (audioSourceBoton != null)
                audioSourceBoton.PlayOneShot(sonidoBoton);
            else
                AudioSource.PlayClipAtPoint(sonidoBoton, Camera.main != null ? Camera.main.transform.position : Vector3.zero);
        }
    }

    // Método para cambiar de escena
    public void CambiarEscena()
    {
       SceneManager.LoadScene(nombreEscena);
    }

    // Método para alternar entre canvas
    public void AlternarCanvas()
    {
        if (canvasActivar != null)
        {
            canvasActivar.gameObject.SetActive(true);
        }

        if (canvasDesactivar != null)
        {
            canvasDesactivar.gameObject.SetActive(false);
        }
    }

    public void AlternarCanvasAyuda()
    {
        if (canvasActivarAyuda != null)
            canvasActivarAyuda.gameObject.SetActive(true);

        if (canvasDesactivarMenu1 != null)
            canvasDesactivarMenu1.gameObject.SetActive(false);
    }

    public void AlternarCanvasCreditos()
    {
        if (canvasActivarCreditos != null)
            canvasActivarCreditos.gameObject.SetActive(true);

        if (canvasDesactivarMenu != null)
            canvasDesactivarMenu.gameObject.SetActive(false);
    }

    // Método para salir del juego
    public void SalirDelJuego()
    {

            Application.Quit();

    }
}
