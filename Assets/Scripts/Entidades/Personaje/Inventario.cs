using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Estructura de datos que encapsula el inventario del personaje.
/// </summary>
public class Inventario
{
    private List<DetalleInventario> detalle;

    public List<DetalleInventario> Detalle { get => detalle == null ? detalle = new List<DetalleInventario>() : detalle; set => detalle = value; }
}
