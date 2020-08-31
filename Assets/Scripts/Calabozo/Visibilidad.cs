using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibilidad
{
    /// <summary>
    /// Se usa para crear las instancias de la clase.
    /// </summary>
    public enum VISIBILIDAD {
        Total,
        Parcial,
        Nula
    }

    private string nombre;
    private string ámbito;

    public Visibilidad(VISIBILIDAD visibilidad, string ámbito)
    {
        switch (visibilidad)
        {
            case VISIBILIDAD.Total:
                this.nombre = "Total";
                break;
            case VISIBILIDAD.Parcial:
                this.nombre = "Parcial";
                break;
            case VISIBILIDAD.Nula:
                this.nombre = "Nula";
                break;
        }

        this.ámbito = ámbito;
    }

    public string Nombre { get => nombre; set => nombre = value; }
    public string Ámbito { get => ámbito; set => ámbito = value; }
}
