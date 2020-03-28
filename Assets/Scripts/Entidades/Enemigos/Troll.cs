using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : Enemigo
{
    public override Vector2 elegirDirecciónMovimiento()
    {
        return Controlador.obtenerDirecciónAleatoria(this.transform.position, Tamaño, true);
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
