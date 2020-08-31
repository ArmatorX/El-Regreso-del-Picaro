using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Representa al personaje.
 * </summary>
 * <remarks>
 * Se encarga de realizar todas las acciones del personaje, y gestionar todos los atributos relacionados al mismo.
 * </remarks>
 */
public class Personaje : Entidad
{
    // Constantes
    public static int COMIDA_MOVIMIENTO = 5;
    public static int COMIDA_ATAQUE_MELÉ = 10;
    private static float TIEMPO_MOVIMIENTO = 0.05f;
    //private static float MAX_DELTA_MOVIMIENTO = PantallaJuego.UNIDAD_MOVIMIENTO / TIEMPO_MOVIMIENTO;

    // Atributos
    /// <value>El nombre que se le colocó al personaje.</value>
    private string nombre;
    /// <value>Se usa para gestionar cambios en la vida máxima dinámicamente.</value>
    /// <remarks>
    /// De momento, se usa solamente para cuando el vampiro reduce la vida del personaje. 
    /// Esto evita tener que modificar las estadísticas de un nivel determinado, y garantiza
    /// que el efecto perdure incluso después de cambios de nivel.
    /// </remarks>
    private int modificadorVidaMáxima;
    /// <value>Puntos de comida actuales del personaje.</value>
    private int comidaActual;
    /// <value>Puntos de experiencia actuales del personaje.</value>
    private int experienciaActual;
    private Nivel nivelActual;
    /// <value>Lista de estados en los que se encuentra actualmente el personaje.</value>
    private List<EstadoPersonaje> estados;
    private Equipo equipoActual;
    private Inventario inventario;
    private Rigidbody2D rb;
    private Animator animaciones;

    // Métodos
    /*
    public Personaje(Piso ubicación, int vidaActual, string nombre, int modificadorVidaMáxima, int comidaActual, int experienciaActual, List<EstadoPersonaje> estados, bool ventaja, bool desventaja)
        : base(ubicación, vidaActual)
    {
        this.nombre = nombre;
        this.modificadorVidaMáxima = modificadorVidaMáxima;
        this.comidaActual = comidaActual;
        this.experienciaActual = experienciaActual;
        this.estados = estados;
        this.ventaja = ventaja;
        this.desventaja = desventaja;
    }
    */
    public string Nombre { get => nombre; set => nombre = value; }
    public int ModificadorVidaMáxima { get => modificadorVidaMáxima; set => modificadorVidaMáxima = value; }
    public int ComidaActual { get => comidaActual; set => comidaActual = value; }
    public int ExperienciaActual { get => experienciaActual; set => experienciaActual = value; }
    public List<EstadoPersonaje> Estados { get => estados; set => estados = value; }
    public Equipo EquipoActual { get => equipoActual; set => equipoActual = value; }
    public Nivel NivelActual { get => nivelActual; set => nivelActual = value; }
    public Rigidbody2D RB { get => rb == null ? rb = GetComponent<Rigidbody2D>() : rb; set => rb = value; }
    public Animator Animaciones { get => animaciones == null ? animaciones = GetComponent<Animator>() : animaciones; set => animaciones = value; }
    public Inventario Inventario { get => inventario == null ? inventario = new Inventario() : inventario; set => inventario = value; }

    /**
     * <summary>
     * Mueve al personaje en una dirección determinada.
     * Se encarga de gestionar todas las tareas del movimientod del personaje,
     * como restar puntos de comida, mostrar la animación, cambiar la ubicación,
     * etc.
     * </summary>
     * <param name="dirección">La dirección en la que se quiere mover el 
     * personaje.</param>
     */
    public override void moverse(Vector2 dirección)
    {
        // Consumo los puntos de comida del movimiento.
        consumirComida(COMIDA_MOVIMIENTO);

        moverManiquí(dirección);

        // Muestro la animación del movimiento del personaje.
        Controlador.animaciónMovimientoPersonaje(dirección);
    }

    /// <summary>
    /// Crea un maniquí. Evita que los enemigos elijan el mismo casillero de 
    /// destino que el personaje.
    /// </summary>
    public void crearManiquí()
    {
        Maniquí = new GameObject("Maniquí");

        Maniquí.tag = "Player";

        Maniquí.transform.localScale = new Vector3(6.25f, 6.25f, 1);

        Maniquí.transform.position = this.transform.position;

        BoxCollider2D hitbox = Maniquí.AddComponent<BoxCollider2D>();

        hitbox.size = new Vector2(0.16f, 0.16f);
    }

    /// <summary>
    /// Mueve el maniquí en la dirección de movimiento. Evita que los enemigos 
    /// elijan el mismo casillero de destino que el personaje.
    /// </summary>
    /// <param name="dirección">Dirección de movimiento.</param>
    public void moverManiquí(Vector2 dirección)
    {
        Maniquí.transform.position += (Vector3) dirección;
    }

    /**
     * <summary>
     * Consume una determinada cantidad de puntos de comida del personaje.
     * </summary>
     * <param name="comimda">La cantidad de puntos de comida a consumir.</param> 
     */
    public void consumirComida(int comida)
    {
        // Controla que la comida a consumir no sea negativa.
        if (comida > 0)
        {
            // Controla que no consuma más comida de la que tiene el personaje.
            if (ComidaActual >= comida)
            {
                ComidaActual -= comida;
            }
            else
            {
                ComidaActual = 0;
            }
        }
    }

    /// <summary>
    /// Ataca a una entidad que se encuentra adyacente al personaje con el arma 
    /// equipada.
    /// </summary>
    /// <param name="objetivo">Entidad que recibe el ataque.</param>
    /// <param name="modificadorMisceláneo">Modificador que se aplica en casos 
    /// específicos, como a enemigos voladores.</param>
    public void atacarCuerpoACuerpo(Entidad objetivo, int modificadorMisceláneo)
    {
        int impacto = calcularImpacto(modificadorMisceláneo);
        byte tipo = 1;
        bool esCrítico = false;

        if (impacto == -1)
        {
            tipo = 2;
            esCrítico = true;
        }

        int daño = calcularDaño(esCrítico, false);

        if (tieneArmaVorpalizada())
        {
            
        }

        if (!realizarAtaque(objetivo, impacto, daño))
        {
            tipo = 0;
        }

        consumirComida(COMIDA_ATAQUE_MELÉ);

        Controlador.animaciónAtaqueMeléPersonaje(objetivo, daño, tipo);
    }

    /// <summary>
    /// Realiza el ataque del personaje a una entidad.
    /// Es decir, concreta el ataque. Todos los cálculos ya fueron hechos en 
    /// este punto. Sólo verifica si el ataque impacta.
    /// </summary>
    /// <param name="objetivo">Entidad que recibe el ataque.</param>
    /// <param name="impacto">Impacto del ataque. -1 si es crítico.</param>
    /// <param name="daño">Daño del ataque. siempre es mayor o igual a 1. No 
    /// hay daño negativo.</param>
    /// <returns>Verdadero si el ataque impacta.</returns>
    public bool realizarAtaque(Entidad objetivo, int impacto, int daño)
    {
        return Controlador.realizarAtaque(objetivo, impacto, daño);
    }

    /// <summary>
    /// Calcula el daño base (sin modificadores) que realiza el personaje.
    /// </summary>
    /// <returns>Puntos de daño base del ataque.</returns>
    public override int calcularDañoBase()
    {
        return EquipoActual.calcularDañoBase();
    }

    /// <summary>
    /// Verifica si el arma equipada está vorpalizada.
    /// </summary>
    /// <returns>Devuelve verdadero si hay arma equipada vorpalizada.</returns>
    public bool tieneArmaVorpalizada()
    {
        return EquipoActual.armaEstáVorpalizada();
    }

    /// <summary>
    /// Obtiene el modificador de fuerza del personaje, teniendo en cuenta los modificadores 
    /// del equipo y estados alterados.
    /// </summary>
    /// <returns>Modificador de fuerza.</returns>
    public override int obtenerModificadorFuerza()
    {
        int modificador = 0;
        modificador += obtenerModificadorFuerzaEstadísticas();
        modificador += obtenerModificadorFuerzaEquipo();
        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores de fuerza otorgados por los objetos equipados.
    /// </summary>
    /// <returns>Modificador de fuerza.</returns>
    public int obtenerModificadorFuerzaEquipo()
    {
        return EquipoActual.obtenerModificadorFuerza();
    }

    /// <summary>
    /// Devuelve el modificador de fuerza otorgado por el nivel actual.
    /// </summary>
    /// <returns>Modificador de fuerza.</returns>
    public int obtenerModificadorFuerzaEstadísticas()
    {
        return NivelActual.obtenerModificadorFuerza();
    }

    /// <summary>
    /// Verifica si el personaje está en un estado alterado a partir
    /// de su nombre.
    /// </summary>
    /// <param name="estado">Nombre del estado alterado.</param>
    /// <returns>Verdadero si el personaje está en el estado
    /// alterado.</returns>
    public bool estáEnEstado(EstadosPersonaje estado)
    {
        foreach (EstadoPersonaje estadoAux in Estados)
        {
            if (estadoAux.Nombre == estado)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Resta puntos de vida al personaje, y si llega a 0, setea el estado en 
    /// "Muerto".
    /// </summary>
    /// <param name="daño">Daño recibido.</param>
    public override void recibirDaño(int daño)
    {
        base.recibirDaño(daño);

        if (VidaActual == 0)
        {
            Estados.Clear();
            Estados.Add(new EstadoPersonaje(EstadosPersonaje.MUERTO));
        }
    }
    
    /// <summary>
    /// Obtiene la defensa del personaje, teniendo en cuenta el equipo que lleva.
    /// </summary>
    /// <returns>Defensa.</returns>
    public override int obtenerDefensa()
    {
        int defensa = 0;
        defensa += obtenerDefensaEstadísticas();
        defensa += obtenerModificadorDefensaEquipo();
        return defensa;
    }

    /// <summary>
    /// Obtiene los modificadores de defensa que provienen del equipo.
    /// </summary>
    /// <returns>Modificador de defensa.</returns>
    public int obtenerModificadorDefensaEquipo()
    {
        return EquipoActual.obtenerModificadorDefensa();
    }

    /// <summary>
    /// Obtiene la defensa que determina el nivel.
    /// </summary>
    /// <returns>Defensa.</returns>
    public int obtenerDefensaEstadísticas()
    {
        return NivelActual.Estadísticas.Defensa;
    }
    
    /// <summary>
    /// Obtiene la cantidad máxima de puntos de vida del personaje.
    /// </summary>
    /// <returns>Vida máxima.</returns>
    public int obtenerVidaMáxima()
    {
        return NivelActual.Estadísticas.VidaMáxima;
    }

    /// <summary>
    /// Verifica si el personaje es un enemigo.
    /// </summary>
    /// <returns>Siempre devuelve false.</returns>
    public override bool esEnemigo()
    {
        return false;
    }

    /// <summary>
    /// Obtiene el modificador de destreza del personaje, teniendo en cuenta los 
    /// modificadores del equipo y estados alterados.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public override int obtenerModificadorDestreza()
    {
        int destreza = 0;
        destreza += obtenerModificadorDestrezaEstadísticas();
        destreza += obtenerModificadorDestrezaEquipo();
        return destreza;
    }

    /// <summary>
    /// Devuelve el modificador de destreza otorgado por el nivel actual.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public int obtenerModificadorDestrezaEstadísticas()
    {
        return NivelActual.obtenerModificadorDestreza(); 
    }

    /// <summary>
    /// Obtiene los modificadores de destreza otorgados por los objetos equipados.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public int obtenerModificadorDestrezaEquipo()
    {
        return EquipoActual.obtenerModificadorDestreza();
    }

    /// <summary>
    /// Obtiene los modificadores de magia, incluyendo los modificadores
    /// otorgados por los objetos equipados y estados alterados.
    /// </summary>
    /// <returns>Modificador de magia.</returns>
    public override int obtenerModificadorMagia()
    {
        int modificador = 0;
        modificador += obtenerModificadorMagiaEstadísticas();
        modificador += obtenerModificadorMagiaEquipo();
        return modificador;
    }

    /// <summary>
    /// Devuelve el modificador de magia otorgado por el nivel actual.
    /// </summary>
    /// <returns>Modificador de magia.</returns>
    public int obtenerModificadorMagiaEstadísticas()
    {
        return NivelActual.obtenerModificadorMagia();
    }

    /// <summary>
    /// Obtiene los modificadores de magia otorgados por los objetos equipados.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public int obtenerModificadorMagiaEquipo()
    {
        return EquipoActual.obtenerModificadorMagia();
    }

    /// <summary>
    /// Obtiene el modificador que aplica al daño que realiza el personaje, 
    /// teniendo en cuenta objetos equipados y estados alterados.
    /// </summary>
    /// <param name="ataqueADistancia">Indica si el ataque a realizar es
    /// un ataque a distancia.</param>
    /// <returns>Modificador de daño.</returns>
    public override int obtenerModificadorDaño(bool ataqueADistancia = false)
    {
        int modificador = 0;
        modificador += EquipoActual.obtenerModificadorDaño();

        if (ataqueADistancia)
        {
            modificador += obtenerModificadorDestreza();
        }
        else
        {
            modificador += obtenerModificadorFuerza();
        }

        return modificador;
    }

    // TODO: ataqueMelé quiero que sea un byte, pero esto es más rápido. Por el tema de que magia va a usar el mismo.
    /// <summary>
    /// Obtiene el modificador de impacto del personaje, sumando la destreza o 
    /// la fuerza dependiendo si el ataque es a distancia, o melé.
    /// </summary>
    /// <param name="ataqueADistancia">Verdadero indica que se trata de un 
    /// ataque a distancia.</param>
    /// <returns>Modificador impacto.</returns>
    public override int obtenerModificadorImpacto(bool ataqueADistancia = false)
    {
        int modificador = 0;
        modificador += EquipoActual.obtenerModificadorImpacto();

        if (ataqueADistancia)
        {
            modificador += obtenerModificadorDestreza();
        }
        else
        {
            modificador += obtenerModificadorFuerza();
        }

        return modificador;
    }

    // MÉTODOS DE UNITY
    void Start()
    {
        // Inicializo los atributos
        //rigidBody2D = GetComponent<Rigidbody2D>();

        Tamaño = TamañoEntidad.NORMAL;
        ModificadorVidaMáxima = 0;
        ComidaActual = 100;
        ExperienciaActual = 0;
        Estados = new List<EstadoPersonaje>();
        Estados.Add(new EstadoPersonaje(EstadosPersonaje.NORMAL));

        Ubicación = new Piso(this.transform.position);

        //seEstáMoviendo = false;

        EquipoActual = new Equipo();
        Arma arma = new EspadaLarga();
        EquipoActual.ArmaEquipada = arma;

        EstadísticasNivel en1 = new EstadísticasNivel(14, 14, 16, 12, 25, 100, 1);
        NivelActual = new Nivel(1, 0, 150, en1);

        VidaActual = obtenerVidaMáxima();

        ObjetoAgarrable pociónVida = new ObjetoAgarrable("Poción de Vida",
            "Una poción que restaura la salud.",
            "Una poción que al ser utilizada restaura 1d8 de salud.",
            "Gráficos/items/poción_vida");

        Inventario.Detalle.Add(new DetalleInventario(1, arma));
        Inventario.Detalle.Add(new DetalleInventario(3, pociónVida));
        /*
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        //Inventario.Detalle.Add(new DetalleInventario(1, pociónVida));
        */

        crearManiquí();
    }

    // Update is called once per frame
    void Update()
    {
        if (Estados[0].Nombre == EstadosPersonaje.MUERTO)
        {
            Destroy(gameObject);
            Controlador.Personaje = null;
        }
    }
}
