using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casillero : MonoBehaviour, IComponenteCalabozo
{
    private int númeroOrden;
    private Vector2 posición;
    private Visibilidad visibilidad;
    //private ObjetoAgarrable objeto;

    public Casillero(Vector2 posición)
    {
        this.posición = posición;
    }

    /// <summary>
    /// No se guardan las dimensiones del Casillero.
    /// </summary>
    public Vector2 Dimensiones { 
        get => throw new System.InvalidOperationException("No se guardan las dimensiones del Casillero.");
        set => throw new System.InvalidOperationException("No se guardan las dimensiones del Casillero."); 
    }
    public int NúmeroOrden { get => númeroOrden; set => númeroOrden = value; }
    public Vector2 Posición { get => posición; set => posición = value; }
    public Visibilidad Visibilidad { get => visibilidad; set => visibilidad = value; }

    /// <summary>
    /// No se utiliza en esta clase, dado que no tiene hijos.
    /// </summary>
    /// <param name="componente">-</param>
    public void añadir(IComponenteCalabozo componente)
    {
        // No tiene hijos, por lo que no se implementa.
        throw new System.InvalidOperationException("La clase Casillero no tiene hijos. No se pueden añadir hijos.");
    }

    /// <summary>
    /// No se utiliza en esta clase, dado que no tiene hijos.
    /// </summary>
    /// <param name="componente">-</param>
    public void eliminar(IComponenteCalabozo componente)
    {
        // No tiene hijos, por lo que no se implementa.
        throw new System.InvalidOperationException("La clase Casillero no tiene hijos. No se pueden eliminar hijos.");
    }

    public void iluminar()
    {
        this.visibilidad = new Visibilidad(Visibilidad.VISIBILIDAD.Total, this.GetType().Name);
    }

    public void mostrar()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// No se utiliza en esta clase, dado que los casilleros no se
    /// pueden ver entre sí.
    /// </summary>
    /// <param name="casilleroOrigen">-</param>
    /// <param name="dirección">-</param>
    /// <returns>-</returns>
    public Casillero obtenerCasilleroDestino(Casillero casilleroOrigen, Vector2 dirección)
    {
        throw new System.InvalidOperationException("Los casilleros no se conocen entre sí.");
    }

    /// <summary>
    /// No se utiliza en esta clase, dado que no tiene hijos.
    /// </summary>
    /// <param name="númeroOrden">-</param>
    /// <returns>-</returns>
    public IComponenteCalabozo obtenerHijo(int númeroOrden)
    {
        throw new System.InvalidOperationException("La clase Casillero no tiene hijos. No se pueden obtener hijos.");
    }

    /// <summary>
    /// No se utiliza en esta clase, dado que no tiene hijos.
    /// </summary>
    /// <param name="posición">-</param>
    /// <returns>-</returns>
    public IComponenteCalabozo obtenerHijo(Vector2 posición)
    {
        throw new System.InvalidOperationException("La clase Casillero no tiene hijos. No se pueden obtener hijos.");
    }

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
    public bool esCasilleroDestino(Casillero casilleroOrigen, Vector2 dirección)
    {
        return (casilleroOrigen.Posición + dirección == this.posición);
    }
}
