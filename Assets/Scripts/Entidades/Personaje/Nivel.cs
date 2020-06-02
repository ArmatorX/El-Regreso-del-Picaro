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

    public int obtenerModificadorFuerza()
    {
        return Estadísticas.calcularModificadorFuerza();
    }

    public int obtenerModificadorDestreza()
    {
        return Estadísticas.calcularModificadorDestreza();
    }

    public int obtenerModificadorMagia()
    {
        return Estadísticas.calcularModificadorMagia();
    }
}
