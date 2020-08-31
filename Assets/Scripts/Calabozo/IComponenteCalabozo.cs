using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta interfaz surge de un patrón Composite, y se utiliza para 
/// comunicarse con los elementos que conforman la estructura del 
/// Calabozo. Puede representar el calabozo, un mapa, una habitación,
/// un pasillo, o un casillero.
/// </summary>
/// <remarks>
/// Si bien las interfaces no pueden tener atributos, durante el 
/// diseño se estableció que era necesario que contuviera algunos 
/// setters y getters comunes a toda la estructura.
/// </remarks>
public interface IComponenteCalabozo 
{
    Vector2 Dimensiones { get; set; }
    Vector2 Posición { get; set; }
    int NúmeroOrden { get; set; }

    /// <summary>
    /// Se usa para determinar el casillero al cual se quiere mover
    /// una entidad.
    /// </summary>
    /// <param name="casilleroOrigen">Casillero en el cual se 
    /// encuentra el personaje.</param>
    /// <param name="dirección">Dirección en que se quiere mover la 
    /// entidad</param>
    /// <returns>Casillero de destino. Es adyacente al de origen. 
    /// Devuelve null si se usa en una estructura que no contiene 
    /// casilleros, o si la estructura no contiene el casillero de
    /// destino.</returns>
    Casillero obtenerCasilleroDestino(Casillero casilleroOrigen, Vector2 dirección);
    
    /// <summary>
    /// Ilumina un elemento del calabozo, haciéndolo completamente
    /// visible para el usuario. Maximiza la visibilidad del componente.
    /// </summary>
    void iluminar();

    /// <summary>
    /// Muestra un componente del calobozo (solo la parte que es 
    /// visible al Jugador).
    /// </summary>
    void mostrar();

    /// <summary>
    /// Añade un componente como hijo al componente padre, como 
    /// puede ser una habitación a un mapa.
    /// </summary>
    /// <param name="componente">Componente a agregar.</param>
    void añadir(IComponenteCalabozo componente);

    /// <summary>
    /// Elimina un componente de la estructura.
    /// </summary>
    /// <param name="componente">Componente a eliminar.</param>
    void eliminar(IComponenteCalabozo componente);

    /// <summary>
    /// Obtiene un hijo del objeto a partir del número de órden.
    /// </summary>
    /// <param name="númeroOrden">Número de órden del hijo.</param>
    /// <returns>Componente del calabozo.</returns>
    IComponenteCalabozo obtenerHijo(int númeroOrden);

    /// <summary>
    /// Obtiene un hijo del objeto a partir de su posición.
    /// </summary>
    /// <param name="posición">Posición del objeto.</param>
    /// <returns>Componente del calabozo, o null si no encontró 
    /// resultados.</returns>
    IComponenteCalabozo obtenerHijo(Vector2 posición);
}
