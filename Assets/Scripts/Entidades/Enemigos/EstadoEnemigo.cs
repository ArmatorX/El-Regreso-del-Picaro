public class EstadoEnemigo
{
    private EstadosEnemigo nombre;

    public EstadoEnemigo(EstadosEnemigo nombre)
    {
        this.nombre = nombre;
    }

    public EstadosEnemigo Nombre { get => nombre; set => nombre = value; }

    
}