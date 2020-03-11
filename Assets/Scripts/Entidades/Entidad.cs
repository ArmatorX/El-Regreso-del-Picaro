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
    private bool esAtaqueCrítico;

    // Métodos
    /*
    protected Entidad(Piso ubicación, int vidaActual)
    {
        this.ubicación = ubicación;
        this.vidaActual = vidaActual;
    }
    */
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
    public bool EsAtaqueCrítico { get => esAtaqueCrítico; set => esAtaqueCrítico = value; }

    public int calcularImpacto(int modificadorMisceláneo)
    {
        int impacto = 0;

        impacto += Controlador.tirarD20(Ventaja, Desventaja);

        if (impacto == 20)
        {
            EsAtaqueCrítico = true;
        }
        else
        {
            EsAtaqueCrítico = false;
        }

        impacto += obtenerModificadorFuerza(modificadorMisceláneo);

        return impacto;
    }

    public bool recibirAtaque(int impacto, int daño, bool esCrítico)
    {
        if (verificarSiAtaqueImpacta(impacto, esCrítico))
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
    public abstract void moverse(Vector3 dirección);
    public abstract bool verificarSiAtaqueImpacta(int impacto, bool esCrítico);
    public abstract int obtenerModificadorFuerza(int modificadorMisceláneo);
}
