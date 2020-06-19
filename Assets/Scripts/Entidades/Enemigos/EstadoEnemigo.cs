public class EstadoEnemigo
{
    private EstadosEnemigo nombre;

    public EstadoEnemigo(EstadosEnemigo nombre)
    {
        this.nombre = nombre;
    }

    public EstadosEnemigo Nombre { get => nombre; set => nombre = value; }

    /// <summary>
    /// Verifica si dos estados son iguales.
    /// </summary>
    /// <param name="estado">Un objeto de tipo <c>EstadoEnemigo</c>.</param>
    /// <returns>Verdadero, si ambos estados son iguales.</returns>
    public override bool Equals(object estado)
    {
        if (Nombre == ((EstadoEnemigo)estado).Nombre)
        {
            return true;
        }

        return false;
    }
}