using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entidad
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

    //private Recompensa recompensa;

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

    public override void moverse(Vector3 dirección)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        Estados = new List<EstadoEnemigo>();
        Estados.Add(new EstadoEnemigo(EstadosEnemigo.NORMAL));

        VidaMáxima = 1;
        VidaActual = VidaMáxima;
        Defensa = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

    internal bool verificarSiAtaqueImpacta(int impacto)
    {
        return impacto >= Defensa;
    }
}
