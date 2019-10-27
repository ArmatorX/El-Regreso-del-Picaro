using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase representa a un casillero del mapa sobre el cual se puede posicionar el personaje.
 * </summary>
 * <remarks>
 * Se utiliza de forma lógica para la creación de los diferentes mapas que se utilizan durante una partida.
 * </remarks>
 */
public class Piso : MonoBehaviour
{
    // Atributos
    /// <value>La ubicación x, y, z del casillero.</value>
    private Vector3 posición;

    // private Visibilidad visibilidad;
    // private ObjetoAgarrable objeto;

    // Métodos
    public Piso(Vector3 posición)
    {
        this.posición = posición;
    }
    public Vector3 Posición { get => posición; set => posición = value; }

    /**
     * <summary>
     * Verifica si esta instancia de casillero, es el casillero adyacente al personaje en la dirección del movimiento. 
     * </summary>
     * <remarks>
     * Se usa durante la actualización de la ubicación del personaje, para obtener el casillero de destino.
     * </remarks>
     * <param name="casilleroOrigen">La ubicación actual del personaje.</param>
     * <param name="dirección">La dirección en la cual el personaje se quiere mover.</param>
     * <returns>Verdadero si el casillero es el casillero destino.</returns>
     */
    public bool esCasilleroDestino(Piso casilleroOrigen, Vector3 dirección)
    {
        if (casilleroOrigen.Posición + dirección == this.posición)
        {
            return true;
        }

        return false;
    }
}
