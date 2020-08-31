using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase representa a un casillero del mapa sobre el cual 
 * se puede posicionar el personaje.
 * </summary>
 * <remarks>
 * Se utiliza de forma lógica para la creación de los diferentes 
 * mapas que se utilizan durante una partida.
 * </remarks>
 */
public class Piso : Casillero
{
    public Piso(Vector2 posición)
        : base(posición) { }
}
