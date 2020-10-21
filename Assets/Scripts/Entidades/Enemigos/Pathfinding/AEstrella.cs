using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEstrella
{
    private Vector2 inicio;
    private Vector2 destino;
    private Vector2 actual;
    private List<Vector2> nodosAbiertos;
    private List<Vector2> nodosCerrados;
    private Dictionary<Vector2, Vector2> nodoAnterior;
    private Dictionary<Vector2, float> puntajeG;

    ControladorJuego controlador;
    TamañoEntidad tamañoEntidad;

    /// <summary>
    /// Este constructor se usa para ejecutar un algoritmo normal de A*.
    /// </summary>
    /// <param name="inicio">Posición de inicio.</param>
    /// <param name="destino">Posición final.</param>
    public AEstrella(Vector2 inicio, Vector2 destino, TamañoEntidad tamañoEntidad)
    {
        this.Inicio = inicio;
        this.Destino = destino;
        this.TamañoEntidad = tamañoEntidad;

        this.NodosAbiertos = new List<Vector2>();
        this.NodosCerrados = new List<Vector2>();

        this.NodosAbiertos.Add(Inicio);

        this.NodoAnterior = new Dictionary<Vector2, Vector2>();
        NodoAnterior.Add(Inicio, Vector2.negativeInfinity);

        this.PuntajeG = new Dictionary<Vector2, float>();
        PuntajeG.Add(Inicio, 0);
    }

    /// <summary>
    /// Este constructor asume que el destino es cualquier casillero que
    /// esté adyacente al personaje.
    /// </summary>
    /// <param name="inicio">Posición de inicio.</param>
    public AEstrella(Vector2 inicio, TamañoEntidad tamañoEntidad)
    {
        this.Inicio = inicio;
        this.Destino = Vector2.negativeInfinity;
        this.TamañoEntidad = tamañoEntidad;
        
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
    /// <summary>
    /// Nodos que se deben explorar y expandir.
    /// </summary>
    public List<Vector2> NodosAbiertos { get => nodosAbiertos; set => nodosAbiertos = value; }
    /// <summary>
    /// Nodos que ya se exploraron y expandieron.
    /// </summary>
    public List<Vector2> NodosCerrados { get => nodosCerrados; set => nodosCerrados = value; }
    public Dictionary<Vector2, Vector2> NodoAnterior { get => nodoAnterior; set => nodoAnterior = value; }
    public Dictionary<Vector2, float> PuntajeG { get => puntajeG; set => puntajeG = value; }
    public TamañoEntidad TamañoEntidad { get => tamañoEntidad; set => tamañoEntidad = value; }
    public ControladorJuego Controlador { get => controlador == null ? GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public Vector2 Actual { get => actual; set => actual = value; }

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
    /// <returns>Ruta desde el nodo de inicio al <c>actual</c>.</returns>
    public List<Vector2> reconstruirCamino()
    {
        List<Vector2> camino = new List<Vector2>
        {
            Actual
        };

        while (!NodoAnterior[Actual].Equals(Vector2.negativeInfinity))
        {
            Actual = NodoAnterior[Actual];
            camino.Add(actual);
        }

        camino.Reverse();

        return camino;
    }

    /// <summary>
    /// Ejecuta el algoritmo A* para encontrar el camino más corto desde 
    /// <c>inicio</c> hasta <c>destino</c>.
    /// Si destino no se definió, entonces se calcula la ruta más corta desde 
    /// <c>inicio</c> a un casillero adyacente al <c>Personaje</c>.
    /// </summary>
    /// <param name="tamaño">Tamaño de la entidad para la cual se busca la 
    /// ruta.</param>
    /// <returns>Lista de casilleros de la ruta óptima. Lista vacía si el 
    /// destino no es accesible.</returns>
    public List<Vector2> calcularRutaÓptima()
    {
        while (NodosAbiertos.Count > 0)
        {
            Actual = obtenerNodoPrioritario();

            if (llegamosADestino())
            {
                return reconstruirCamino();
            }

            cerrarNodo(Actual);

            foreach (Vector2 vecino in encontrarNodosVecinos())
            {
                agregarNodoParaExploración(vecino);
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// Verifica si se cumple la condición de terminación del algoritmo.
    /// </summary>
    /// <returns>Verdadero si el algoritmo debe terminar.</returns>
    private bool llegamosADestino()
    {
        if (Destino.Equals(Vector2.negativeInfinity))
        {
            return esAdyacenteAlPersonaje();
        }
        else
        {
            return esNodoDestino();
        }
    }

    /// <summary>
    /// Verifica si el nodo <c>actual</c> se encuentra adyacente al 
    /// <c>destino</c>.
    /// </summary>
    /// <returns>Verdadero si está adyacente al destino.</returns>
    private bool esAdyacenteAlPersonaje()
    {
        return Controlador.estáAdyacenteAlPersonaje(Actual, TamañoEntidad);
    }

    /// <summary>
    /// Verifica si el nodo <c>actual</c> es igual al nodo de destino.
    /// </summary>
    /// <returns>Verdadero si es el nodo de destino.</returns>
    public bool esNodoDestino()
    {
        return Actual.Equals(Destino);
    }

    /// <summary>
    /// Obtiene todas las direcciones válidas en las que se puede mover una 
    /// desde el nodo <c>actual</c>.
    /// </summary>
    /// <returns>Lista de posiciones válidas.</returns>
    public List<Vector2> encontrarNodosVecinos()
    {
        List<Vector2> direccionesVálidas = Controlador
            .obtenerDireccionesVálidas(Actual, TamañoEntidad, true);

        return transformarDireccionesANodos(direccionesVálidas);
    }

    /// <summary>
    /// Transforma las direcciones en que se puede mover una entidad desde 
    /// el nodo <c>actual</c> a posiciones absolutas.
    /// </summary>
    /// <param name="direccionesVálidas">Direcciones válidas de 
    /// movimiento.</param>
    /// <returns>Lista de posiciones (nodos vecinos a <c>actual</c>).</returns>
    public List<Vector2> transformarDireccionesANodos(List<Vector2> direccionesVálidas)
    {
        List<Vector2> nodosVecinos = new List<Vector2>();

        direccionesVálidas.ForEach(d => nodosVecinos.Add(Actual + d));

        return nodosVecinos;
    }

    /// <summary>
    /// Se encarga de abrir un nodo. Es decir, lo quita de la lista de 
    /// <c>nodosCerrados</c>, y lo agrega a la lista de <c>nodosAbiertos</c>.
    /// </summary>
    /// <param name="nodo">Nodo a abrir.</param>
    public void abrirNodo(Vector2 nodo)
    {
        if (NodosCerrados.Contains(nodo))
        {
            NodosCerrados.Remove(nodo);
        }

        if (!NodosAbiertos.Contains(nodo))
        {
            NodosAbiertos.Add(nodo);
        }
    }

    /// <summary>
    /// Cierra el nodo. Es decir, lo quita de la lista <c>nodosAbiertos</c>, y lo 
    /// agrega a la lista <c>nodosCerrados</c>.
    /// </summary>
    /// <param name="nodo">Nodo a cerrar.</param>
    public void cerrarNodo(Vector2 nodo)
    {
        if (NodosAbiertos.Contains(nodo))
        {
            NodosAbiertos.Remove(nodo);
        }

        if (!NodosCerrados.Contains(nodo))
        {
            NodosCerrados.Add(nodo);
        }
    }

    /// <summary>
    /// Ordena la lista <c>nodosAbiertos</c> en función del puntaje F de cada 
    /// nodo, y luego devuelve el prioritario.
    /// </summary>
    /// <remarks>Esta función es inútil si se usa un <c>Binary Heap</c>.</remarks>
    /// <returns>Nodo con menor puntaje F (nodo más cercano al destino).</returns>
    public Vector2 obtenerNodoPrioritario()
    {
        NodosAbiertos.Sort((x, y) => PuntajeF(x).CompareTo(PuntajeF(y)));

        return NodosAbiertos[0];
    }

    /// <summary>
    /// Agrega el nodo para su exploración siempre que se cumpla alguna de las 
    /// siguientes condiciones:
    /// - No fue explorado.
    /// - Fue explorado, pero se encontró una ruta más corta.
    /// </summary>
    /// <param name="vecino">Nodo que se quiere agregar (nodo vecino de 
    /// <c>actual</c>.</param>
    public void agregarNodoParaExploración(Vector2 vecino)
    {
        float puntajeGTentativo = PuntajeG[Actual] + 1;

        if (!PuntajeG.ContainsKey(vecino))
        {
            NodoAnterior.Add(vecino, Actual);
            PuntajeG.Add(vecino, puntajeGTentativo);

            abrirNodo(vecino);
        }
        else if (puntajeGTentativo < PuntajeG[vecino])
        {
            NodoAnterior[vecino] = Actual;
            PuntajeG[vecino] = puntajeGTentativo;

            abrirNodo(vecino);
        }
    }
}
