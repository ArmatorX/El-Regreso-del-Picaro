using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoPersonaje : MonoBehaviour
{
    // Atributos
    private string nombre;
    
    // Métodos
    public EstadoPersonaje(string nombre)
    {
        this.nombre = nombre;
    }
    public string Nombre { get => nombre; set => nombre = value; }
}
