using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase representa una pared del calabozo.
/// Limitan el movimiento del personaje.
/// </summary>
/// <remarks>
/// Se utiliza de forma lógica para la creación de los diferentes
/// mapas que se utilizan durante una partida.
/// </remarks>
public class Pared : Casillero
{
    public Pared(Vector3 posición)
       : base(posición) { }
}
