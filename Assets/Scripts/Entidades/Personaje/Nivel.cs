using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel
{
    private int número;
    private int experienciaMínima;
    private int experienciaMáxima;
    private EstadísticasNivel estadísticas;

    public Nivel(int nivel, int experienciaMínima, int experienciaMáxima, EstadísticasNivel estadísticas)
    {
        this.Número = nivel;
        this.ExperienciaMínima = experienciaMínima;
        this.ExperienciaMáxima = experienciaMáxima;
        this.Estadísticas = estadísticas;
    }

    public int Número { get => número; set => número = value; }
    public int ExperienciaMínima { get => experienciaMínima; set => experienciaMínima = value; }
    public int ExperienciaMáxima { get => experienciaMáxima; set => experienciaMáxima = value; }
    public EstadísticasNivel Estadísticas { get => estadísticas; set => estadísticas = value; }

    public int obtenerModificadorFuerzaBase()
    {
        return Estadísticas.calcularModificadorFuerzaBase();
    }

    public int obtenerModificadorDestrezaBase()
    {
        return Estadísticas.calcularModificadorDestrezaBase();
    }

    public int obtenerModificadorMagiaBase()
    {
        return Estadísticas.calcularModificadorMagiaBase();
    }

    public int obtenerCantidadDadosDaño()
    {
        return Estadísticas.CantidadDadosDaño;
    }
}
