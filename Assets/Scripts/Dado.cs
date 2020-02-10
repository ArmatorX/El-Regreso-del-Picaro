using UnityEngine;

/// <summary>
/// Representa un dado.
/// </summary>
public class Dado
{
    private int cantidadCaras;

    public Dado(int cantidadCaras)
    {
        this.CantidadCaras = cantidadCaras;
    }

    public int CantidadCaras { get => cantidadCaras; set => cantidadCaras = value; }

    /// <summary>
    /// Tira una determinada cantidad de dados.
    /// </summary>
    /// <param name="cantidad">Cantidad de dados a tirar.</param>
    /// <returns>Resultado de la tirada.</returns>
    public int tirarDados(int cantidad)
    {
        int salida = 0;

        for (int i = 0; i < cantidad; i ++)
        {
            salida += Random.Range(1, CantidadCaras + 1);
        }
        
        return salida;
    }
}