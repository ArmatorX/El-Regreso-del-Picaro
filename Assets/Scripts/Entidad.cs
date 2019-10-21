using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase es una abstracción de todas las entidades del juego.
 * </summary>
 * <remarks>
 * Contiene métodos y atributos que son comunes tanto a los enemigos como al personaje.
 * </remarks>
 */
public abstract class Entidad : MonoBehaviour
{
    // Atributos
    /// <value>El casillero en el que se encuentra ubicada la entidad.</value>
    private Piso ubicación;
    /// <value>Los puntos de vida restantes de la entidad.</value>
    private int vidaActual;

    // Métodos
    protected Entidad(Piso ubicación, int vidaActual)
    {
        this.ubicación = ubicación;
        this.vidaActual = vidaActual;
    }
    public Piso Ubicación { get => ubicación; set => ubicación = value; }
    public int VidaActual { get => vidaActual; set => vidaActual = value; }

    /**
     * <summary>
     * Se encarga de mover a la entidad en una dirección determinada.
     * </summary>
     * <param name="dirección">La dirección en que se quiere mover la entidad.</param> 
     */
    public abstract void moverse(Vector3 dirección);
}
