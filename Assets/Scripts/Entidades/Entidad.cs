using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase es una abstracción de todas las entidades del juego.
 * </summary>
 * <remarks>
 * Contiene métodos y atributos que son comunes tanto a los enemigos como al 
 * personaje.
 * </remarks>
 */
public abstract class Entidad : MonoBehaviour
{
    // Atributos
    /// <value>El casillero en el que se encuentra ubicada la entidad.</value>
    private Piso ubicación;
    /// <value>Los puntos de vida restantes de la entidad.</value>
    private int vidaActual;
    /// <value>Controlador.</value>
    private ControladorJuego controlador;
    /// <value>Determina si la entidad tiene ventaja para sus ataques.</value>
    private bool ventaja;
    /// <value>Determina si la entidad tiene desventaja para sus ataques.</value>
    private bool desventaja;
    /// <value>Determina el tamaño que ocupa una entidad.</value>
    private TamañoEntidad tamaño;
    /// <value>Es un objeto falso que determina la hitbox de la entidad.
    /// El movimiento de los maniquíes es síncrono, de forma que las animaciones 
    /// se pueden hacer de forma asíncrona sin afectar el juego.</value>
    private GameObject maniquí;

    public Piso Ubicación { get => ubicación; set => ubicación = value; }
    public int VidaActual { get => vidaActual; set => vidaActual = value; }
    public ControladorJuego Controlador { get => controlador == null ? GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public bool Ventaja
    {
        get => ventaja;
        // Estoy seguro que para este set armé una tabla de verdad.
        set => ventaja = value && !desventaja;
    }
    public bool Desventaja
    {
        get => desventaja;
        // Estoy seguro que para este set armé una tabla de verdad.
        set => desventaja = value && !ventaja;
    }
    public TamañoEntidad Tamaño { get => tamaño; set => tamaño = value; }
    public GameObject Maniquí { get => maniquí; set => maniquí = value; }

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
    /*
    /// <summary>
    /// Elimina la instancia del GameObject de maniquí.
    /// </summary>
    public void eliminarManiquí()
    {
        Destroy(Maniquí);
    }
    */
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
