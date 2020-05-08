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
     * como restar puntos de comida, mostrar la animación, cambiar la ubicación, etc.
     * </summary>
     * <param name="dirección">La dirección en la que se quiere mover el personaje.</param>
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
    /// Crea un maniquí. Evita que los enemigos 
    /// elijan el mismo casillero de destino que el personaje.
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

    public void atacarCuerpoACuerpo(Entidad objetivo, int modificadorMisceláneo)
    {
        int impacto = calcularImpacto(modificadorMisceláneo);
        int daño = 0;
        byte tipo = 1;

        if (!armaEquipadaEstáVorpalizada())
        {
            daño = calcularDaño(obtenerModificadorFuerza(modificadorMisceláneo));

            if (daño <= 0)
            {
                daño = 1;
            }

            if (impacto == -1)
            {
                daño *= 2;
                tipo = 2;
            }
        }


        if (!realizarAtaque(objetivo, impacto, daño))
        {
            tipo = 0;
        }

        consumirComida(COMIDA_ATAQUE_MELÉ);

        Controlador.animaciónAtaqueMeléPersonaje(objetivo, daño, tipo);
    }

    public bool realizarAtaque(Entidad objetivo, int impacto, int daño)
    {
        return Controlador.realizarAtaque(objetivo, impacto, daño);
    }

    public int calcularDaño(int modificador)
    {
        int cantidadDadosDaño = NivelActual.obtenerCantidadDadosDaño();

        int dañoBase = EquipoActual.calcularDañoBase(cantidadDadosDaño);

        return dañoBase + modificador;
    }

    public bool armaEquipadaEstáVorpalizada()
    {
        return EquipoActual.armaEquipadaEstáVorpalizada();
    }

    public override int obtenerModificadorFuerza(int modificadorMisceláneo)
    {
        int modificador = 0;
        modificador += obtenerModificadorFuerzaBase();
        modificador += obtenerModificadoresEquipoParaFuerza();
        modificador += modificadorMisceláneo;
        return modificador;
    }

    public int obtenerModificadoresEquipoParaFuerza()
    {
        return EquipoActual.obtenerModificadoresEquipoParaFuerza();
    }

    public int obtenerModificadorFuerzaBase()
    {
        return NivelActual.obtenerModificadorFuerzaBase();
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

    public override void recibirDaño(int daño)
    {
        base.recibirDaño(daño);

        if (VidaActual == 0)
        {
            Estados.Clear();
            Estados.Add(new EstadoPersonaje(EstadosPersonaje.MUERTO));
        }
    }

    public override bool verificarSiAtaqueImpacta(int impacto)
    {
        return impacto >= obtenerDefensa() || impacto == -1;
    }

    public int obtenerDefensa()
    {
        int defensa = 0;
        defensa += obtenerDefensaBase();
        defensa += obtenerModificadoresEquipoParaDefensa();
        return defensa;
    }

    public int obtenerModificadoresEquipoParaDefensa()
    {
        return EquipoActual.obtenerModificadoresEquipoParaDefensa();
    }

    public int obtenerDefensaBase()
    {
        return NivelActual.obtenerDefensaBase();
    }
    
    public int obtenerVidaMáxima()
    {
        return NivelActual.obtenerVidaMáxima();
    }

    public override bool esEnemigo()
    {
        return false;
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
