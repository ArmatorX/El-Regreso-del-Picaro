using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadísticasNivel
{
    private int fuerza;
    private int destreza;
    private int defensa;
    private int magia;
    private int vidaMáxima;
    private int comidaMáxima;
    private int cantidadDadosDaño;

    public EstadísticasNivel(int fuerza, int destreza, int defensa, int magia, int vidaMáxima, int comidaMáxima, int cantidadDadosDaño)
    {
        this.Fuerza = fuerza;
        this.Destreza = destreza;
        this.Defensa = defensa;
        this.Magia = magia;
        this.VidaMáxima = vidaMáxima;
        this.ComidaMáxima = comidaMáxima;
        this.CantidadDadosDaño = cantidadDadosDaño;
    }

    public int Fuerza { get => fuerza; set => fuerza = value; }
    public int Destreza { get => destreza; set => destreza = value; }
    public int Defensa { get => defensa; set => defensa = value; }
    public int Magia { get => magia; set => magia = value; }
    public int VidaMáxima { get => vidaMáxima; set => vidaMáxima = value; }
    public int ComidaMáxima { get => comidaMáxima; set => comidaMáxima = value; }
    public int CantidadDadosDaño { get => cantidadDadosDaño; set => cantidadDadosDaño = value; }

    /// <summary>
    /// Calcula el modificador a partir de la fuerza base. Se usa para
    /// calcular el impacto.
    /// </summary>
    /// <returns>Modificador fuerza base.</returns>
    public int calcularModificadorFuerza()
    {
        return Mathf.FloorToInt((Fuerza - 10) / 2);
    }

    /// <summary>
    /// Calcula el modificador a partir de la destreza base. Se usa para
    /// calcular el impacto.
    /// </summary>
    /// <returns>Modificador destreza base.</returns>
    public int calcularModificadorDestreza()
    {
        return Mathf.FloorToInt((Destreza - 10) / 2);
    }

    /// <summary>
    /// Calcula el modificador a partir de la magia base. Se usa para
    /// calcular el impacto.
    /// </summary>
    /// <returns>Modificador magia base.</returns>
    public int calcularModificadorMagia()
    {
        return Mathf.FloorToInt((Magia - 10) / 2);
    }

    /*
    /// <summary>
    /// Calcula la dificultad de la salvación a partir de la magia.
    /// Se usa para los ataques mágicos.
    /// </summary>
    /// <returns>Dificultad salvación.</returns>
    public int calcularDificultadDeSalvaciónBase()
    {
        return 10 + calcularModificadorMagiaBase();
    }
    */
}
