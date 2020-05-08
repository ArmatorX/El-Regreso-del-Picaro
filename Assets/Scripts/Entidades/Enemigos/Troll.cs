using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemigo
{
    public override Vector2 elegirDirecciónMovimiento()
    {
        List<Vector2> posiciones = aStar(RB.position, Controlador.Personaje.RB.position);

        List<Vector2> direcciones = obtenerDirecciones(posiciones);

        for (int i = 0; i < posiciones.Count - 1; i ++)
        {
            Debug.DrawLine(posiciones[i], posiciones[i + 1], Color.green, 1000);
        }

        if (direcciones.Count == 0)
        {
            return Controlador.obtenerDirecciónAleatoria(RB.position, Tamaño, true);
        }
        Debug.Log(direcciones[0]);
        return direcciones[0];
    }

    public List<Vector2> obtenerDirecciones(List<Vector2> posiciones)
    {
        List<Vector2> direcciones = new List<Vector2>();

        for (int i = 0; i < posiciones.Count; i ++)
        {
            if (i == 0)
            {
                //direcciones.Add(posiciones[i] - RB.position);
            }
            else
            {
                direcciones.Add(posiciones[i] - posiciones[i - 1]);
            }
        }

        return direcciones;
    }

    public List<Vector2> aStar(Vector2 inicio, Vector2 objetivo)
    {
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

        // Para el nodo n, puntajeF[n] = puntajeG[n] + la función de la heurística,
        // que en este caso es la distancia euclidiana entre los puntos.
        float puntajeF(Vector3 x) => (objetivo - (Vector2) x).magnitude + puntajeG[(int) x.z];

        while (nodosAbiertos.Count > 0)
        {
            // Ordeno la lista por prioridad
            nodosAbiertos.Sort((x, y) => puntajeF(x).CompareTo(puntajeF(y)));
            Vector3 actual = nodosAbiertos[0];

            if (Controlador.estáAdyacenteAlPersonaje(actual, Tamaño))
            {
                return reconstruirCamino(nodoAnterior, actual);
            }

            nodosCerrados.Add(actual);
            nodosAbiertos.Remove(actual);

            List<Vector2> direcciones = Controlador.obtenerDireccionesVálidas(actual, Tamaño, true);

            // Como necesito los nodos vecinos, tenemos que acomodar la lista.
            List<Vector3> vecinos = new List<Vector3>();  
            foreach(Vector2 dirección in direcciones)
            {
                // Verifico si el nodo ya existe, y si es así lo busco.
                Vector2 posición = (Vector2) actual + dirección;
                Vector3 vecinoExistente = nodosAbiertos.Find(nodo => (posición - (Vector2) nodo).magnitude == 0);

                if (((Vector2) vecinoExistente - posición).magnitude == 0)
                {
                    vecinos.Add(vecinoExistente);
                }
                else
                {
                    vecinoExistente = nodosCerrados.Find(nodo => (posición - (Vector2) nodo).magnitude == 0);

                    if (((Vector2) vecinoExistente - posición).magnitude == 0)
                    {
                        vecinos.Add(vecinoExistente);
                    }
                    else
                    {
                        vecinos.Add(new Vector3(posición.x, posición.y, -1));
                    }
                }
            }

            foreach(Vector3 vecino in vecinos)
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
            */
            
        }

        return new List<Vector2>();
    }

    private List<Vector2> reconstruirCamino(List<Vector3> nodoAnterior, Vector3 actual)
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