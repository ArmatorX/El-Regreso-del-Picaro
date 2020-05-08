using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murciélago : Enemigo
{
    public override Vector2 elegirDirecciónMovimiento()
    {
        return Controlador.obtenerDirecciónAleatoria(this.transform.position, Tamaño, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Estados = new List<EstadoEnemigo>();
        Estados.Add(new EstadoEnemigo(EstadosEnemigo.VOLANDO));

        Tamaño = TamañoEntidad.NORMAL;
        VidaMáxima = 10;
        VidaActual = VidaMáxima;
        Defensa = 5;
        Fuerza = 0;
        Destreza = 0;
        Magia = 0;
        DadoDañoAtaqueBase = new Dado(4);
        CantidadDadosDañoAtaqueBase = 1;

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
