using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entidad : MonoBehaviour
{
    // Atributos
    private Piso ubicación;
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
     * Se encarga de mover a la entidad en una dirección determinada.
     * @param dirección - La dirección en que se quiere mover la entidad.
     */
    public abstract void moverse(Vector3 dirección);
}
