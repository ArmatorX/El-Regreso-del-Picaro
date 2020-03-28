using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase es una abstracción de todas las entidades del juego.
 * </summary>
 * <remarks>
 * Contiene métodos y atributos que son comunes tanto a los enemigos como al personaje.
 * </remarks>
 */
public abstract class Entidad : MonoBehaviour
{
    // Atributos
    /// <value>El casillero en el que se encuentra ubicada la entidad.</value>
    private Piso ubicación;
    /// <value>Los puntos de vida restantes de la entidad.</value>
    private int vidaActual;
    private ControladorJuego controlador;
    private bool ventaja;
    private bool desventaja;
    private TamañoEntidad tamaño;

    public Piso Ubicación { get => ubicación; set => ubicación = value; }
    public int VidaActual { get => vidaActual; set => vidaActual = value; }
    public ControladorJuego Controlador { get => controlador == null ? GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public bool Ventaja
    {
        get => ventaja;
        set => ventaja = value && !desventaja;
    }
    public bool Desventaja
    {
        get => desventaja;
        set => desventaja = value && !ventaja;
    }
    public TamañoEntidad Tamaño { get => tamaño; set => tamaño = value; }

    public int calcularImpacto(int modificadorMisceláneo)
    {
        int impacto = 0;

        impacto += Controlador.tirarD20(Ventaja, Desventaja);

        if (impacto == 20)
        {
            impacto = -1;
        }
        else
        {
            impacto += obtenerModificadorFuerza(modificadorMisceláneo);
        }

        return impacto;
    }

    public bool recibirAtaque(int impacto, int daño)
    {
        if (verificarSiAtaqueImpacta(impacto) || impacto == -1)
        {
            recibirDaño(daño);
            return true;
        }

        return false;
    }

    public virtual void recibirDaño(int daño)
    {
        if (VidaActual <= daño)
        {
            VidaActual = 0;
        }
        else
        {
            VidaActual -= daño;
        }
    }

    /**
     * <summary>
     * Se encarga de mover a la entidad en una dirección determinada.
     * </summary>
     * <param name="dirección">La dirección en que se quiere mover la entidad.</param> 
     */
    public abstract void moverse(Vector2 dirección);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="impacto">El impacto del ataque. Si es -1 el ataque impacta 
    /// siempre.</param>
    /// <returns></returns>
    public abstract bool verificarSiAtaqueImpacta(int impacto);
    public abstract int obtenerModificadorFuerza(int modificadorMisceláneo);
    public abstract bool esEnemigo();
}
