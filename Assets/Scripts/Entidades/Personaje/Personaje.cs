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
    private static float MAX_DELTA_MOVIMIENTO = PantallaJuego.UNIDAD_MOVIMIENTO / TIEMPO_MOVIMIENTO;

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
    private bool ventaja;
    private bool desventaja;
    private Equipo equipoActual;
    private bool esAtaqueCrítico;

    // Físicas
    private Rigidbody2D rigidBody2D;
    private bool seEstáMoviendo;
    private ControladorJuego controlador;

    // Métodos
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
    public string Nombre { get => nombre; set => nombre = value; }
    public int ModificadorVidaMáxima { get => modificadorVidaMáxima; set => modificadorVidaMáxima = value; }
    public int ComidaActual { get => comidaActual; set => comidaActual = value; }
    public int ExperienciaActual { get => experienciaActual; set => experienciaActual = value; }
    public List<EstadoPersonaje> Estados { get => estados; set => estados = value; }
    public bool Ventaja {
        get => ventaja;
        set => ventaja = value && !desventaja;
    }
    public bool Desventaja {
        get => desventaja;
        set => desventaja = value && !ventaja; }
    public ControladorJuego Controlador { get => controlador; set => controlador = value; }
    public Equipo EquipoActual { get => equipoActual; set => equipoActual = value; }
    public Nivel NivelActual { get => nivelActual; set => nivelActual = value; }
    public bool EsAtaqueCrítico { get => esAtaqueCrítico; set => esAtaqueCrítico = value; }

    /**
     * <summary>
     * Mueve al personaje en una dirección determinada.
     * Se encarga de gestionar todas las tareas del movimientod del personaje,
     * como restar puntos de comida, mostrar la animación, cambiar la ubicación, etc.
     * </summary>
     * <param name="dirección">La dirección en la que se quiere mover el personaje.</param>
     */
    public override void moverse(Vector3 dirección)
    {
        // Consumo los puntos de comida del movimiento.
        consumirComida(COMIDA_MOVIMIENTO);

        // Muestro la animación del movimiento del personaje.
        Controlador.mostrarAnimaciónMovimientoPersonaje(dirección);
        /*
        // Cambio la posición del personaje en Unity.
        IEnumerator corrutina = movimientoSuavizado(dirección);

        StartCoroutine(corrutina);
        */
        //rigidBody2D.position += (Vector2) dirección;
        // Obtengo el casillero de destino.
        // Piso casilleroDestino = controlador.obtenerCasilleroDestino(Ubicación, dirección);

        // Actualizo la ubicación del personaje.
        // Ubicación = casilleroDestino;
    }

    /**
     * <summary>
     * Cambia la posición del personaje poco a poco, hasta que llega al destino.
     * </summary>
     * <param name="dirección">La dirección en que se quiere mover el personaje.</param>
     * <returns>Es un IEnumerator porque es una corrutina.</returns>
     */
    /*
   protected IEnumerator movimientoSuavizado(Vector3 dirección)
   {
       if (seEstáMoviendo)
       {
           // Si se está moviendo que espere hasta que termine.
           yield return new WaitForSeconds(TIEMPO_MOVIMIENTO / 4f);

           // Intenta moverte nuevamente.
           IEnumerator corrutina = movimientoSuavizado(dirección);

           StartCoroutine(corrutina);
       }
       else
       {
           // Obtengo la posición de destino.
           Vector3 destino = this.transform.position + dirección;

           // Calculo la distancia restante.
           float distanciaRestante = (this.transform.position - destino).sqrMagnitude;

           while (distanciaRestante > 0.001)
           {
               // Deshabilito el movimiento.
               seEstáMoviendo = true;

               // Calculo la nueva posición a la que se va a mover el personaje.
               Vector3 nuevaPosición = Vector3.MoveTowards(rigidBody2D.position, destino, MAX_DELTA_MOVIMIENTO * Time.fixedDeltaTime);

               // Muevo el personaje a la posición calculada.
               rigidBody2D.MovePosition(nuevaPosición);

               // Vuelvo a calcular la distancia restante.
               distanciaRestante = (this.transform.position - destino).sqrMagnitude;

               // Salta un frame, y continúa el loop hasta que está en un valor cercano a la posición de destino.
               yield return null;
           }

           // Pongo la posición del personaje en el lugar de destino.
           rigidBody2D.position = destino;

           // Habilito nuevamente el movimiento.
           seEstáMoviendo = false;
       }
   }
   */
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

    public void atacarCuerpoACuerpo(Enemigo enemigo, int modificadorMisceláneo)
    {
        int impacto = calcularImpacto(modificadorMisceláneo);
        int daño = 0;

        if (!armaEquipadaEstáVorpalizada())
        {
            daño = calcularDaño(obtenerModificadorFuerza(modificadorMisceláneo));

            if (EsAtaqueCrítico)
            {
                daño *= 2;
            }
        }

        realizarAtaque(impacto, daño, EsAtaqueCrítico);

        consumirComida(COMIDA_ATAQUE_MELÉ);

        Controlador.mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(daño, EsAtaqueCrítico);

        EsAtaqueCrítico = false;
    }

    public void realizarAtaque(int impacto, int daño, bool esCrítico)
    {
        Controlador.realizarAtaque(impacto, daño, esCrítico);
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

    public int obtenerModificadorFuerza(int modificadorMisceláneo)
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

    // Start is called before the first frame update
    void Start()
    {
        // Inicializo los atributos
        rigidBody2D = GetComponent<Rigidbody2D>();
        Controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>();

        ModificadorVidaMáxima = 0;
        ComidaActual = 100;
        ExperienciaActual = 0;
        Estados = new List<EstadoPersonaje>();
        Estados.Add(new EstadoPersonaje(EstadosPersonaje.NORMAL));

        Ubicación = new Piso(this.transform.position);
        VidaActual = 100;

        seEstáMoviendo = false;

        EquipoActual = new Equipo();
        Arma arma = new EspadaLarga();
        EquipoActual.ArmaEquipada = arma;

        EstadísticasNivel en1 = new EstadísticasNivel(14, 14, 16, 12, 25, 100, 1);
        NivelActual = new Nivel(1, 0, 150, en1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Verifica si el personaje está en un estado alterado a partir
    /// de su nombre.
    /// </summary>
    /// <param name="estado">Nombre del estado alterado.</param>
    /// <returns>Verdadero si el personaje está en el estado
    /// alterado.</returns>
    public bool verificarSiEstáEnEstado(EstadosPersonaje estado)
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
}
