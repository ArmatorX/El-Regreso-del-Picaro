using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemigo
{
    /// <summary>
    /// Elige una dirección de movimiento para el troll. Si puede llegar hasta 
    /// el personaje, entonces elige el primer casillero del camino más corto 
    /// al personaje. De otro modo, se mueve aleatoriamente.
    /// </summary>
    /// <returns>Dirección del movimiento.</returns>
    public override Vector2 elegirDirecciónMovimiento()
    {
        Vector2 inicio = RB.position;
        Vector2 destino = Controlador.Personaje.RB.position;

        List<Vector2> posiciones = encontrarRutaÓptima(inicio, destino);

        List<Vector2> direcciones = transformarPosicionesRuta(posiciones);

        if (direcciones.Count == 0)
        {
            return Controlador.obtenerDirecciónAleatoria(RB.position, Tamaño, true);
        }

        return direcciones[0];
    }

    /// <summary>
    /// Transforma los vetores de posición de una ruta que inicia en la posición
    /// actual del <c>Troll</c>, y la transforma en una secuencia de direcciones
    /// que debe tomar para llegar al destino.
    /// </summary>
    /// <param name="posiciones">Ruta a transformar. Debe contener el casillero
    /// de inicio.</param>
    /// <returns>Lista de direcciones.</returns>
    public List<Vector2> transformarPosicionesRuta(List<Vector2> posiciones)
    {
        List<Vector2> direcciones = new List<Vector2>();

        for (int i = 1; i < posiciones.Count; i ++)
        {
            direcciones.Add(posiciones[i] - posiciones[i - 1]);
        }

        return direcciones;
    }

    /// <summary>
    /// Encuentra la ruta más corta entre dos casilleros.
    /// Se usa para generar hacer pathfinding. El algoritmo utilizado es A*.
    /// Si no es alcanzable, se devuelve una lista vacía.
    /// </summary>
    /// <param name="inicio">Casillero de inicio.</param>
    /// <param name="destino">Casillero de destino.</param>
    /// <returns>Lista de casilleros que forman la ruta óptima, incluyendo 
    /// inicio y destino.</returns>
    public List<Vector2> encontrarRutaÓptima(Vector2 inicio, Vector2 destino)
    {
        return null;
    }
        /*
        // Lista de nodos que pueden requerir ser expandidos, ordenados por 
        // prioridad. Estoy usando la coordenada z como identificador.
        List<Vector3> nodosAbiertos = new List<Vector3>();
        nodosAbiertos.Add(new Vector3(inicio.x, inicio.y, 0));

        // Lista de nodos ya descubiertos y explorados.
        List<Vector3> nodosCerrados = new List<Vector3>();

        // Para el nodo n, nodoAnterior[n] es el nodo más barato que lo precede.
        List<Vector3> nodoAnterior = new List<Vector3>();
        nodoAnterior.Add(Vector3.back);

        // Para el nodo n, puntajeG[n] es el costo del camino más barato, 
        // desde el comienzo.
        List<float> puntajeG = new List<float>();
        puntajeG.Add(0);

        // Para el nodo n, puntajeF[n] = puntajeG[n] + función eurística, 
        // que en este caso es la distancia euclidiana al nodo destino.
        List<float> puntajeF = new List<float>();
        puntajeG.Add(0 + (destino - inicio).magnitude);

        while (nodosAbiertos.Count > 0)
        {
            explorarNodo(nodosAbiertos, nodosCerrados, nodoAnterior, destino);

            List<Vector2> direcciones = Controlador.obtenerDireccionesVálidas(actual, Tamaño, true);

            // Como necesito los nodos vecinos, tenemos que acomodar la lista.
            List<Vector3> vecinos = new List<Vector3>();
            /*
            foreach (Vector2 dirección in direcciones)
            {
                // Verifico si el nodo ya existe, y si es así lo busco.
                Vector2 posición = (Vector2)actual + dirección;
                Vector3 vecinoExistente = nodosAbiertos.Find(nodo => (posición - (Vector2)nodo).magnitude == 0);

                if (((Vector2)vecinoExistente - posición).magnitude == 0)
                {
                    vecinos.Add(vecinoExistente);
                }
                else
                {
                    vecinoExistente = nodosCerrados.Find(nodo => (posición - (Vector2)nodo).magnitude == 0);

                    if (((Vector2)vecinoExistente - posición).magnitude == 0)
                    {
                        vecinos.Add(vecinoExistente);
                    }
                    else
                    {
                        vecinos.Add(new Vector3(posición.x, posición.y, -1));
                    }
                }
            }
            
            foreach (Vector3 vecino in vecinos)
            {
                float puntajeGTentativo = puntajeG[(int) actual.z] + 1;

                // Controlo si es la primera vez que recorro el nodo.
                if (vecino.z == -1)
                {
                    int index = puntajeG.Count;
                    Vector3 vecinoNuevo = new Vector3(vecino.x, vecino.y, index);

                    nodoAnterior.Add(actual);
                    puntajeG.Add(puntajeGTentativo);

                    Vector3 aux = nodosCerrados.Find(nodo => (vecinoNuevo - nodo).magnitude == 0);

                    if ((aux - vecinoNuevo).magnitude != 0)
                    {
                        nodosAbiertos.Add(vecinoNuevo);
                    } 
                }
                else if (puntajeGTentativo < puntajeG[(int) vecino.z])
                {
                    nodoAnterior.Add(actual);
                    puntajeG.Add(puntajeGTentativo);

                    Vector3 aux = nodosCerrados.Find(nodo => (vecino - nodo).magnitude == 0);

                    if ((aux - vecino).magnitude != 0)
                    {
                        nodosAbiertos.Add(vecino);
                    }
                }
            }
            /*
            string strNA = "Nodos abiertos: { ";
            nodosAbiertos.ForEach(nodo => strNA += nodo + ", ");
            strNA += " }";

            string strNC = "Nodos cerrados: { ";
            nodosCerrados.ForEach(nodo => strNC += nodo + ", ");
            strNC += " }";

            string strPG = "Puntaje G: { ";
            puntajeG.ForEach(p => strPG += p + ", ");
            strPG += " }";

            string strNAnt = "Nodo anterior: { ";
            nodoAnterior.ForEach(p => strNAnt += p + ", ");
            strNAnt += " }";


            Debug.Log(strNAnt);
            Debug.Log(strNA);
            Debug.Log("Cantidad Nodos Abiertos: " + nodosAbiertos.Count);
            Debug.Log(strNC);
            Debug.Log("Cantidad Nodos Cerrados: " + nodosCerrados.Count);
            Debug.Log(strPG);
            
            Debug.Log(actual);
            Debug.Log(puntajeF(actual));
            Debug.Log(objetivo);
            
            
        }

        return new List<Vector2>();
    }

    public void explorarNodo(List<Vector3> nodosAbiertos, List<Vector3> nodosCerrados, Vector3 nodoAnterior, Vector3 destino)
    {
        ordenarNodosPorPuntajeF(nodosAbiertos, destino, puntajeG);

        Vector3 actual = nodosAbiertos[0];

        if (Controlador.estáAdyacenteAlPersonaje(actual, Tamaño))
        {
            return reconstruirCamino(nodoAnterior, actual);
        }

        nodosCerrados.Add(actual);
        nodosAbiertos.Remove(actual);
    }

    public void ordenarNodosPorPuntajeF(ref List<Vector3> nodosAbiertos)
    {
        nodosAbiertos.Sort((x, y) => puntajeF((int)x.z).CompareTo(puntajeF((int)y.z)));
    }

    /// <summary>
    /// Es la función que se usa en el algoritmo A* para determinar el costo 
    /// ajustado de moverse a un nodo determinado.
    /// Se usa como función eurística, la distancia euclidiana entre el 
    /// casillero x y el de destino.
    /// </summary>
    /// <param name="x">Casillero del cual se quiere calcular el valor de F.</param>
    /// <param name="destino">Casillero de destino.</param>
    /// <param name="puntajeG">Valor de la función G para x.</param>
    /// <returns>Costo del nodo x.</returns>
    public float puntajeF(Vector2 x, Vector2 destino, float puntajeG)
    {
        return (destino - x).magnitude + puntajeG;
    }
    
    public List<Vector3> buscarCasillerosVecinos(List<Vector2> direcciones, List<Vector2> nodosAbiertos, List<Vector2> nodosCerrados, Vector2 actual)
    {
        List<Vector3> vecinos = new List<Vector3>();

        foreach (Vector2 dirección in direcciones)
        {
            // Verifico si el nodo ya existe, y si es así lo busco.
            Vector2 posición = (Vector2)actual + dirección;
            Vector3 vecinoExistente = nodosAbiertos.Find(nodo => (posición - (Vector2)nodo).magnitude == 0);

            if (((Vector2)vecinoExistente - posición).magnitude == 0)
            {
                vecinos.Add(vecinoExistente);
            }
            else
            {
                vecinoExistente = nodosCerrados.Find(nodo => (posición - (Vector2)nodo).magnitude == 0);

                if (((Vector2)vecinoExistente - posición).magnitude == 0)
                {
                    vecinos.Add(vecinoExistente);
                }
                else
                {
                    vecinos.Add(new Vector3(posición.x, posición.y, -1));
                }
            }
        }

        return vecinos;
    }

    public List<Vector2> reconstruirCamino(List<Vector3> nodoAnterior, Vector3 actual)
    {
        List<Vector2> camino = new List<Vector2>();

        while (nodoAnterior[(int) actual.z] != Vector3.back)
        {
            actual = nodoAnterior[(int) actual.z];
            camino.Add(actual);
        }

        camino.Reverse();

        return camino;
    }
    */
    void Start()
    {
        Estados = new List<EstadoEnemigo>();
        Estados.Add(new EstadoEnemigo(EstadosEnemigo.NORMAL));

        Tamaño = TamañoEntidad.GRANDE;
        VidaMáxima = 80;
        VidaActual = VidaMáxima;
        Defensa = 10;
        Fuerza = 3;
        Destreza = 0;
        Magia = 0;
        DadoDañoAtaqueBase = new Dado(8);
        CantidadDadosDañoAtaqueBase = 2;

        Ventaja = false;
        Desventaja = false;

        crearManiquí();
    }

    // Update is called once per frame
    void Update()
    {
        if (Estados[0].Nombre == EstadosEnemigo.MUERTO)
        {
            Destroy(gameObject);
            Controlador.Enemigos.Remove(this);
        }
    }
}