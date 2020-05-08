using UnityEngine;

/// <summary>
/// Representa un dado de n caras.
/// </summary>
public class Dado
{
    /// <summary>
    /// Cantidad de caras del dado.
    /// </summary>
    private int cantidadCaras;

    public Dado(int cantidadCaras)
    {
        this.CantidadCaras = cantidadCaras;
    }

    public int CantidadCaras { get => cantidadCaras; set => cantidadCaras = value; }

    /// <summary>
    /// Realiza una tirada de dados n dados, y devuelve la suma del resultado.
    /// La cantidad de dados que se tiran está determinada por <c>cantidad</c>.
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