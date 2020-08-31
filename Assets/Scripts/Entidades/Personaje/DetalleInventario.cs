using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetalleInventario
{
    private int cantidad;
    private ObjetoAgarrable objetoAgarrable;

    public DetalleInventario(int cantidad, ObjetoAgarrable objetoAgarrable)
    {
        this.cantidad = cantidad;
        this.objetoAgarrable = objetoAgarrable;
    }

    public int Cantidad { get => cantidad; set => cantidad = value; }
    public ObjetoAgarrable ObjetoAgarrable { get => objetoAgarrable; set => objetoAgarrable = value; }
}
