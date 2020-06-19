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
        set
        {
            if (value && desventaja)
            {
                desventaja = false;
            } 
            else
            {
                ventaja = value;
            }
        }
    }
    public bool Desventaja
    {
        get => desventaja;
        set
        {
            if (value && ventaja)
            {
                ventaja = false;
            }
            else
            {
                desventaja = value;
            }
        }
    }
    public TamañoEntidad Tamaño { get => tamaño; set => tamaño = value; }
    public GameObject Maniquí { get => maniquí; set => maniquí = value; }

    // TODO: ataqueMelé quiero que sea un byte, pero esto es más rápido. Por el tema de que magia va a usar el mismo.
    /// <summary>
    /// Calcula el impacto de un ataque.
    /// </summary>
    /// <param name="modificadorMisceláneo">Modificador que se aplica sobre
    /// la tirada, que depende del contexto en que se realizó.</param>
    /// <param name="ataqueADistancia">Determina si se usa el modificador de 
    /// fuerza o de destreza.</param>
    /// <returns>Impacto del ataque.</returns>
    public int calcularImpacto(int modificadorMisceláneo, bool ataqueADistancia = false)
    {
        int impacto = 0;
        impacto += Controlador.tirarD20(Ventaja, Desventaja);

        if (impacto == 20)
        {
            impacto = -1;
        } 
        else
        {
            impacto += obtenerModificadorImpacto(ataqueADistancia) + modificadorMisceláneo;
        }

        return impacto;
    }

    /// <summary>
    /// Si el ataque impacta, entonces resta los puntos de vida correspondientes
    /// al daño.
    /// </summary>
    /// <remarks>
    /// Si el ataque es crítico, el método simplemente verifica si impacta.
    /// El daño se duplica en el ataque.
    /// </remarks>
    /// <param name="impacto">Impacto.</param>
    /// <param name="daño">Puntos de daño.</param>
    /// <returns>Verdadero si el ataque impacta.</returns>
    public bool recibirAtaque(int impacto, int daño)
    {
        if (verificarSiAtaqueImpacta(impacto) || impacto == -1)
        {
            recibirDaño(daño);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resta puntos de vida a la entidad.
    /// </summary>
    /// <param name="daño">Cantidad de puntos de vida a restar.</param>
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

    /// <summary>
    /// Verifica si el ataque impacta.
    /// Verifica si el impacto es mayor que la defensa de la entidad.
    /// </summary>
    /// <param name="impacto">El impacto del ataque. Si es -1 el ataque impacta 
    /// siempre.</param>
    /// <returns>Devuelve verdadero si el ataque es efectivo.</returns>
    public bool verificarSiAtaqueImpacta(int impacto)
    {
        return impacto >= obtenerDefensa() || impacto == -1;
    }

    /// <summary>
    /// Calcula el daño de un ataque de la entidad.
    /// </summary>
    /// <param name="esCrítico">Verdadero indica que el ataque es crítico.</param>
    /// <param name="ataqueADistancia">Verdadero indica que se trata de un 
    /// ataque a distancia.</param>
    /// <returns>Cantidad de puntos de daño del ataque.</returns>
    public int calcularDaño(bool esCrítico = false, bool ataqueADistancia = false)
    {
        int daño = calcularDañoBase();

        if (obtenerModificadorDaño(ataqueADistancia) > 0)
        {
            daño += obtenerModificadorDaño(ataqueADistancia);
        }

        if (esCrítico)
        {
            daño *= 2;
        }

        return daño;
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

    // MÉTODOS ABSTRACTOS.
    /// <summary>
    /// Mueve la entidad en una dirección.
    /// </summary>
    /// <param name="dirección">Dirección en la que se quiere mover.</param>
    public abstract void moverse(Vector2 dirección);

    /// <summary>
    /// Calcula el daño base (tirada de dados) de la entidadd.
    /// </summary>
    /// <returns>Cantidad de puntos de daño base (sin modificadores) del 
    /// ataque.</returns>
    public abstract int calcularDañoBase();

    // TODO: Los enemigos guardan el modificador. Por ahí habría que cambiarlo.
    /// <summary>
    /// Obtiene el modificador de fuerza de la entidad.
    /// </summary>
    /// <returns>Modificador de fuerza.</returns>
    public abstract int obtenerModificadorFuerza();

    /// <summary>
    /// Obtiene el modificador de destreza de la entidad.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public abstract int obtenerModificadorDestreza();

    /// <summary>
    /// Obtiene el modificador de magia de la entidad.
    /// </summary>
    /// <returns>Modificador de magia.</returns>
    public abstract int obtenerModificadorMagia();

    /// <summary>
    /// Obtiene la defensa de la entidad.
    /// </summary>
    /// <returns>Defensa.</returns>
    public abstract int obtenerDefensa();

    // TODO: ataqueMelé quiero que sea un byte, pero esto es más rápido. Por el tema de que magia va a usar el mismo.
    /// <summary>
    /// Obtiene el modificador de daño de la entidad, sumando la destreza o la 
    /// fuerza dependiendo si el ataque es a distancia, o a melé.
    /// </summary>
    /// <param name="ataqueADistancia">Verdadero indica que se trata de un 
    /// ataque a distancia.</param>
    /// <returns>Modificador daño.</returns>
    public abstract int obtenerModificadorDaño(bool ataqueADistancia = false);

    /// <summary>
    /// Verifica si esta entidad es un enemigo.
    /// </summary>
    /// <returns>Verdadero si es un enemigo.</returns>
    public abstract bool esEnemigo();

    // TODO: ataqueMelé quiero que sea un byte, pero esto es más rápido. Por el tema de que magia va a usar el mismo.
    /// <summary>
    /// Obtiene el modificador de impacto de la entidad, sumando la destreza o 
    /// la fuerza dependiendo si el ataque es a distancia, o melé.
    /// </summary>
    /// <param name="ataqueADistancia">Verdadero indica que se trata de un 
    /// ataque a distancia.</param>
    /// <returns>Modificador impacto.</returns>
    public abstract int obtenerModificadorImpacto(bool ataqueADistancia = false);
}
