using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase se encarga de realizar las operaciones relacionadas con la GUI durante una partida.
 * </summary>
 * <remarks>
 * Algunas operaciones que realiza la pantalla son mostrar las animaciones de los enemigos, y del
 * personaje, mostrar las barras de vida y hambre, mostrar el daño realizado, etc.
 * </remarks>
 */
public class PantallaJuego : MonoBehaviour
{
    // Atributos
    /// <value>El controlador del juego.</value>
    private ControladorJuego controlador;
    /// <value>El objeto personaje de Unity.</value>
    /// <remarks>Necesario para activar las animaciones.</remarks>
    private GameObject personaje;

    // Método
    public PantallaJuego(ControladorJuego controlador, GameObject personaje)
    {
        this.controlador = controlador;
        this.personaje = personaje;
    }

    public ControladorJuego Controlador { get => controlador; set => controlador = value; }
    public GameObject Personaje { get => personaje; set => personaje = value; }

    /**
     * <summary>
     * Se encarga de realizar todo el movimiento del personaje.
     * </summary>
     * <param name="dirección">Dirección en la que se quiere mover el personaje.</param>
     */
    public void moverPerosonaje(Vector3 dirección)
    {
        controlador.moverPersonaje(dirección);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
