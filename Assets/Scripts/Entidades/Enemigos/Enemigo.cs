using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemigo : Entidad
{
    private List<EstadoEnemigo> estados;
    private int experienciaQueSuelta;
    private int dificultad;
    private int vidaMáxima;
    private int fuerza;
    private int destreza;
    private int defensa;
    private int magia;
    private Dado dadoDañoAtaqueBase;
    private int cantidadDadosDañoAtaqueBase;
    private Rigidbody2D rb;

    public List<EstadoEnemigo> Estados { get => estados; set => estados = value; }
    public int ExperienciaQueSuelta { get => experienciaQueSuelta; set => experienciaQueSuelta = value; }
    public int Dificultad { get => dificultad; set => dificultad = value; }
    public int VidaMáxima { get => vidaMáxima; set => vidaMáxima = value; }
    public int Fuerza { get => fuerza; set => fuerza = value; }
    public int Destreza { get => destreza; set => destreza = value; }
    public int Defensa { get => defensa; set => defensa = value; }
    public int Magia { get => magia; set => magia = value; }
    public Dado DadoDañoAtaqueBase { get => dadoDañoAtaqueBase; set => dadoDañoAtaqueBase = value; }
    public int CantidadDadosDañoAtaqueBase { get => cantidadDadosDañoAtaqueBase; set => cantidadDadosDañoAtaqueBase = value; }
    public Rigidbody2D RB { get => rb == null ? rb = GetComponent<Rigidbody2D>() : rb; set => rb = value; }

    //private Recompensa recompensa;
    /*
    public Enemigo(Piso ubicación, int vidaActual, List<EstadoEnemigo> estados, int experienciaQueSuelta, int dificultad, int vidaMáxima, int fuerza, int destreza, int defenza, int magia, Dado dadoDañoAtaqueBase, int cantidadDadosDañoAtaqueBase)
        : base(ubicación, vidaActual)
    {
        this.Estados = estados;
        this.ExperienciaQueSuelta = experienciaQueSuelta;
        this.Dificultad = dificultad;
        this.VidaMáxima = vidaMáxima;
        this.Fuerza = fuerza;
        this.Destreza = destreza;
        this.Defensa = defenza;
        this.Magia = magia;
        this.DadoDañoAtaqueBase = dadoDañoAtaqueBase;
        this.CantidadDadosDañoAtaqueBase = cantidadDadosDañoAtaqueBase;
    }
    */



    public bool verificarSiEstáEnEstado(EstadosEnemigo estado)
    {
        foreach (EstadoEnemigo estadoAux in Estados)
        {
            if (estadoAux.Nombre == estado)
            {
                return true;
            }
        }

        return false;
    }

    public override bool verificarSiAtaqueImpacta(int impacto, bool esCrítico)
    {
        return impacto >= Defensa || esCrítico;
    }

    public override void recibirDaño(int daño)
    {
        base.recibirDaño(daño);

        if (VidaActual == 0)
        {
            Estados.Clear();
            Estados.Add(new EstadoEnemigo(EstadosEnemigo.MUERTO));
        }
    }

    
    public abstract void usarTurno();
    public abstract void atacar();
    public override void moverse(Vector3 dirección)
    {
        Controlador.mostrarAnimaciónMovimientoEnemigo(dirección, this);
    }
}
