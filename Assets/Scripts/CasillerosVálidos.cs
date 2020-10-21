using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase contiene todas las posiciones relativas que puede ocupar una 
/// entidad, para cada tamaño específico.
/// Las mismas se encuentran ordenadas en diccionarios, de forma tal que se 
/// al pasar una dirección, el diccionario devuelve la lista de posiciones 
/// relativas que va a ocupar la entidad si se mueve hacia allí.
/// </summary>
public static class CasillerosVálidos
{
    private static readonly List<Vector2> direccionesVálidas = new List<Vector2>
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };
    private static readonly Dictionary<Vector2, List<Vector2>> casillerosTamañoNormal = new Dictionary<Vector2, List<Vector2>>
    {
        { Vector2.up, new List<Vector2>{ Vector2.up } },
        { Vector2.right, new List<Vector2>{ Vector2.right } },
        { Vector2.down, new List<Vector2>{ Vector2.down } },
        { Vector2.left, new List<Vector2>{ Vector2.left } }
    };
    private static readonly Dictionary<Vector2, List<Vector2>> casillerosTamañoGrande = new Dictionary<Vector2, List<Vector2>>
    {
        { Vector2.up, new List<Vector2>{
            new Vector2(-0.5f, 1.5f),
            new Vector2(0.5f, 1.5f) 
        } },
        { Vector2.right, new List<Vector2>{
            new Vector2(1.5f, 0.5f),
            new Vector2(1.5f, -0.5f) 
        } },
        { Vector2.down, new List<Vector2>{
            new Vector2(0.5f, -1.5f), 
            new Vector2(-0.5f, -1.5f) 
        } },
        { Vector2.left, new List<Vector2>{
            new Vector2(-1.5f, -0.5f),
            new Vector2(-1.5f, 0.5f) 
        } }        
    };
    private static readonly Dictionary<Vector2, List<Vector2>> casillerosTamañoGigante = new Dictionary<Vector2, List<Vector2>>
    {
        { Vector2.up, new List<Vector2>{
            new Vector2(-1.0f, 2.0f),
            new Vector2(0.0f, 2.0f),
            new Vector2(1.0f, 2.0f) 
        } },
        { Vector2.right, new List<Vector2>{
            new Vector2(2.0f, 1.0f),
            new Vector2(2.0f, 0.0f),
            new Vector2(2.0f, -1.0f) 
        } },
        { Vector2.down, new List<Vector2>{
            new Vector2(1.0f, -2.0f),
            new Vector2(0.0f, -2.0f),
            new Vector2(-1.0f, -2.0f) 
        } },
        { Vector2.left, new List<Vector2>{
            new Vector2(-2.0f, -1.0f),
            new Vector2(-2.0f, 0.0f),
            new Vector2(-2.0f, 1.0f) 
        } }
    };

    public static List<Vector2> DireccionesVálidas => direccionesVálidas;

    public static Dictionary<Vector2, List<Vector2>> CasillerosTamañoNormal => casillerosTamañoNormal;

    public static Dictionary<Vector2, List<Vector2>> CasillerosTamañoGrande => casillerosTamañoGrande;

    public static Dictionary<Vector2, List<Vector2>> CasillerosTamañoGigante => casillerosTamañoGigante;
    
    /// <summary>
    /// Devuelve el diccionario de posiciones correspondiente al tamaño de la 
    /// entidad.
    /// </summary>
    /// <param name="tamaño">Tamaño de la entidad</param>
    /// <returns>Diccionario de posiciones relativas posibles que puede ocupar  
    /// la entidad.</returns>
    public static Dictionary<Vector2, List<Vector2>> ObtenerCasillerosVálidos(TamañoEntidad tamaño)
    {
        switch (tamaño)
        {
            case TamañoEntidad.NORMAL:
                return CasillerosTamañoNormal;
            case TamañoEntidad.GRANDE:
                return CasillerosTamañoGrande;
            case TamañoEntidad.GIGANTE:
                return CasillerosTamañoGigante;
            default:
                return CasillerosTamañoNormal;  
        }
    }
}
