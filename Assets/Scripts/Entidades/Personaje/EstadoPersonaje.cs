using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/**
 * <summary>
 * Representa un estado del personaje.
 * </summary>
 * <remarks>
 * El personaje puede estar en varios estados simultáneos a la vez.
 * Identifica principalmente los estados alterados en los que puede estar el personaje, 
 * dado que el resto está resuelto en el editor de Unity.
 * </remarks>
 */

[Serializable()]
public class EstadoPersonaje
{
    // Atributos
    /// <value>El nombre del estado del personaje.</value>
    EstadosPersonaje nombre;
    
    // Métodos
    public EstadoPersonaje(EstadosPersonaje nombre)
    {
        this.nombre = nombre;
    }
    public EstadosPersonaje Nombre { get => nombre; set => nombre = value; }

    /**
     * <summary>
     * Verifica si el estado es el estado "Congelado".
     * </summary>
     * <returns>Verdadero si el estado es "Congelado".</returns>
     */
    public bool esEstadoCongelado()
    {
        if (Nombre == EstadosPersonaje.CONGELADO)
        {
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Verifica si el estado es el estado "Confundido".
     * </summary>
     * <returns>Verdadero si el estado es "Confundido".</returns>
     */
    public bool esEstadoConfundido()
    {
        if (Nombre == EstadosPersonaje.CONFUNDIDO)
        {
            return true;
        }
        return false;
    }

    /**
     * <summary>
     * Verifica si el estado es el estado "Paralizado".
     * </summary>
     * <returns>Verdadero si el estado es "Paralizado".</returns>
     */
    public bool esEstadoParalizado()
    {
        if (Nombre == EstadosPersonaje.PARALIZADO)
        {
            return true;
        }
        return false;
    }
}
