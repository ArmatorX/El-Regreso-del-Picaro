using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        //Vector2 destino = Controlador.Personaje.RB.position;

        List<Vector2> posiciones = encontrarRutaÓptima(inicio);

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
    /// Se usa para hacer pathfinding. El algoritmo utilizado es A*.
    /// Si no es alcanzable, se devuelve una lista vacía.
    /// </summary>
    /// <param name="inicio">Casillero de inicio.</param>
    /// <param name="destino">Casillero de destino.</param>
    /// <returns>Lista de casilleros que forman la ruta óptima, incluyendo 
    /// inicio y destino.</returns>
    public List<Vector2> encontrarRutaÓptima(Vector2 inicio, Vector2 destino)
    {
        AEstrella aEstrella = new AEstrella(inicio, destino, Tamaño);

        return aEstrella.calcularRutaÓptima();
    }

    /// <summary>
    /// Encuentra la ruta más corta desde el casillero de inicio, a un casillero
    /// adyacente al personaje.
    /// Se usa para hacer pathfinding. El algoritmo utilizado es A*.
    /// Si no es alcanzable, se devuelve una lista vacía.
    /// </summary>
    /// <param name="inicio">Casillero de inicio.</param>
    /// <returns>Lista de casilleros que forman la ruta óptima, incluyendo 
    /// inicio y destino.</returns>
    public List<Vector2> encontrarRutaÓptima(Vector2 inicio)
    {
        AEstrella aEstrella = new AEstrella(inicio, Tamaño);

        return aEstrella.calcularRutaÓptima();
    }

    void Start()
    {
        Estados = new List<EstadoEnemigo>
        {
            new EstadoEnemigo(EstadosEnemigo.NORMAL)
        };

        Tamaño = TamañoEntidad.GRANDE;
        VidaMáxima = 80;
        VidaActual = VidaMáxima;
        Defensa = 10;
        Fuerza = 3;
        Destreza = 0;
        Magia = 0;
        DadoDañoAtaqueBase = new Dado(8);
        CantidadDadosDañoAtaqueBase = 2;
        SeEstáMoviendo = false;

        Ventaja = false;
        Desventaja = false;

        //crearManiquí();
    }

    // Update is called once per frame
    void Update()
    {
        if (Estados[0].Nombre == EstadosEnemigo.MUERTO)
        {
            Controlador.Enemigos.Remove(this);
            Destroy(this.Hitbox);
            Destroy(gameObject);
        }
    }
}