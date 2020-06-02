using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoAgarrable
{
    private string nombre;
    private string descripciónParcial;
    private string descripciónCompleta;
    private string rutaSprite;

    public ObjetoAgarrable()
    {

    }

    public ObjetoAgarrable(string nombre, string descripciónParcial, string descripciónCompleta, string rutaSprite)
    {
        this.nombre = nombre;
        this.descripciónParcial = descripciónParcial;
        this.descripciónCompleta = descripciónCompleta;
        this.rutaSprite = rutaSprite;
    }

    public string Nombre { get => nombre; set => nombre = value; }
    public string DescripciónParcial { get => descripciónParcial; set => descripciónParcial = value; }
    public string DescripciónCompleta { get => descripciónCompleta; set => descripciónCompleta = value; }
    public string RutaSprite { get => rutaSprite; set => rutaSprite = value; }
}
