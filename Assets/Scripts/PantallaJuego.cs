using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase se encarga de realizar las operaciones relacionadas con la GUI durante una partida.
 * </summary>
 * <remarks>
 * Algunas operaciones que realiza la pantalla son mostrar las animaciones de los enemigos, y del
 * personaje, mostrar las barras de vida y hambre, mostrar el daño realizado, obtener las inputs del jugador, etc.
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
    public void moverPersonaje(Vector3 dirección)
    {
        controlador.moverPersonaje(dirección);
    }

    /**
     * <summary>
     * Obtiene la dirección del movimiento a partir de la tecla que haya presionado el jugador en este frame.
     * </summary>
     * <returns>Un vector unitario con dirección horizontal o vertical.</returns>
     */
    public Vector3 obtenerDirecciónMovimiento()
    {
        // Obtengo la dirección del movimiento.
        Vector3 dirección;

        if (Input.GetButtonDown("Up"))
        {
            dirección = Vector3.up;
        }
        else if (Input.GetButtonDown("Down"))
        {
            dirección = Vector3.down;
        }
        else if (Input.GetButtonDown("Right"))
        {
            dirección = Vector3.right;
        }
        else if (Input.GetButtonDown("Left"))
        {
            dirección = Vector3.left;
        }
        else
        {
            dirección = Vector3.zero;
        }

        return dirección;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Inicializo el controlador
        controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>();

    }

    // Update is called once per frame
    void Update()
    {
        // Controlar si el jugador quiere moverse.
        if (Input.anyKeyDown)
        {
            // Obtengo la dirección movimiento.
            Vector3 dirección = obtenerDirecciónMovimiento();

            if (dirección != Vector3.zero)
            {
                // Muevo al personaje en la dirección correspondiente.
                moverPersonaje(dirección);
            }
            
        }
    }
}
