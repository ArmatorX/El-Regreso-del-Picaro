using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piso : MonoBehaviour
{
    // Atributos
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
     * Verifica si el casillero es el casillero al que se está intentando mover el personaje. 
     * Se usa durante la actualización de la ubicación del personaje.
     * @param casilleroOrigen - La ubicación actual del personaje.
     * @param dirección - La dirección en la cual el personaje se quiere mover.
     * @returns - Verdadero si el casillero es el casillero destino.
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
