using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrella
{
    private Vector2 inicio;
    private Vector2 destino;
    private List<Vector2> nodosAbiertos;
    private List<Vector2> nodosCerrados;
    private Dictionary<Vector2, Vector2> nodoAnterior;
    private Dictionary<Vector2, float> puntajeG;


    public AEstrella(Vector2 inicio, Vector2 destino)
    {
        this.Inicio = inicio;
        this.Destino = destino;

        this.NodosAbiertos = new List<Vector2>();
        this.NodosCerrados = new List<Vector2>();

        this.NodosAbiertos.Add(Inicio);

        this.NodoAnterior = new Dictionary<Vector2, Vector2>();
        NodoAnterior.Add(Inicio, Vector2.negativeInfinity);

        this.PuntajeG = new Dictionary<Vector2, float>();
        PuntajeG.Add(Inicio, 0);
    }

    public Vector2 Inicio { get => inicio; set => inicio = value; }
    public Vector2 Destino { get => destino; set => destino = value; }
    public List<Vector2> NodosAbiertos { get => nodosAbiertos; set => nodosAbiertos = value; }
    public List<Vector2> NodosCerrados { get => nodosCerrados; set => nodosCerrados = value; }
    public Dictionary<Vector2, Vector2> NodoAnterior { get => nodoAnterior; set => nodoAnterior = value; }
    public Dictionary<Vector2, float> PuntajeG { get => puntajeG; set => puntajeG = value; }

    /// <summary>
    /// Función heurítisca del algoritmo.
    /// Toma como entrada dos nodos y devuelve su costo en función de
    /// lo que se use como heurística. 
    /// Se usa la distancia euclidiana.
    /// </summary>
    /// <param name="a">Nodo a.</param>
    /// <param name="b">Nodo b.</param>
    /// <returns>Distancia euclidiana entre los nodos.</returns>
    static public float heurística(Vector2 a, Vector2 b)
    {
        return (b - a).magnitude;
    }

    /// <summary>
    /// Es la función que se usa en el algoritmo A* para determinar el costo 
    /// ajustado de moverse a un nodo determinado.
    /// Se usa como función eurística, la distancia euclidiana entre el 
    /// nodo a y el de destino.
    /// </summary>
    /// <param name="a">Casillero del cual se quiere calcular el valor de F.</param>
    /// <returns>Costo del nodo a.</returns>
    public float PuntajeF(Vector2 a)
    {
        return heurística(a, destino) + PuntajeG[a];
    }

    /// <summary>
    /// Reconstruye el camino desde el nodo <c>actual</c> hasta el inicial.
    /// </summary>
    /// <param name="actual">Nodo desde el cual se reconstruye el camino.</param>
    /// <returns>Ruta desde el nodo de inicio al actual.</returns>
    public List<Vector2> reconstruirCamino(Vector2 actual)
    {
        List<Vector2> camino = new List<Vector2>();
        camino.Add(actual);

        while (!NodoAnterior[actual].Equals(Vector2.negativeInfinity))
        {
            actual = NodoAnterior[actual];
            camino.Add(actual);
        }

        camino.Reverse();

        return camino;
    }

    /// <summary>
    /// Ejecuta el algoritmo A* para encontrar el camino más corto desde <c>inicio</c>
    /// hasta <c>destino</c>.
    /// </summary>
    /// <param name="tamaño">Tamaño de la entidad para la cual se busca la ruta.</param>
    /// <returns>Lista de casilleros de la ruta óptima.</returns>
    public List<Vector2> calcularRutaÓptima(TamañoEntidad tamaño)
    {
        Vector2 actual;

        while (NodosAbiertos.Count > 0)
        {
            // Ordeno los nodos de acuerdo a su puntaje F.
            NodosAbiertos.Sort((x, y) => PuntajeF(x).CompareTo(PuntajeF(y)));

            actual = NodosAbiertos[0];

            if (ControladorJuego.estáAdyacenteAlPersonaje(actual, tamaño))
            {
                NodoAnterior.Add(Destino, actual);
                break;
            }

            NodosCerrados.Add(actual);
            NodosAbiertos.Remove(actual);

            List<Vector2> nodosVecinos = encontrarNodosVecinos();

            foreach (Vector2 vecino in nodosVecinos)
            {
                float puntajeGTentativo = PuntajeG[actual] + 1;

                if (puntajeGTentativo < PuntajeG[vecino] || !PuntajeG.ContainsKey(vecino))
                {
                    NodoAnterior.Add(vecino, actual);
                    PuntajeG.Add(vecino, puntajeGTentativo);

                    if (!NodosAbiertos.Contains(vecino))
                    {
                        NodosAbiertos.Add(vecino);
                    }
                }
            }
        }

        return reconstruirCamino(Destino);
    }

    public List<Vector2> encontrarNodosVecinos()
    {
        throw new NotImplementedException();
    }
}
