using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * <summary>
 * Se encarga de coordinar las distintas funcionalidades que deben ocurrir durante una partida.
 * </summary> 
 * <remarks>
 * Se encarga de mover a los enemigos, ejecutar las acciones del jugador, controlar colisiones, etc.
 * </remarks>
 */
public class ControladorJuego : MonoBehaviour {
    // Constantes
    // TODO: Armar un archivo de configuración para las rutas de acceso.
    public static string RUTA_ESTADOS_PERSONAJE = "Datos/EstadoPersonaje.bin";
    /**
     * <summary>
     * El enum <c>CicloTurno</c> representa el ciclo que reliza el juego cada vez que pasa una ronda completa de turnos (asalto).
     * </summary>
     * <remarks>
     * 
     * </remarks>
     */
    public enum CicloTurno : byte
    {
        // El ciclo se realiza en el orden que está escrito.
        INICIALIZACIÓN, // Se realizan las tareas iniciales del juego, como la generación de mapas.
        CARGA_NIVEL, // Se realiza una vez al comienzo de cada nivel, y se encarga de cargar los datos del nivel actual.
        // En este punto inicia el ciclo de turnos.
        TURNO_PERSONAJE, // El turno del personaje. Espera a que el personaje realice su acción.
        TURNO_ENEMIGOS, // El turno de los enemigos. Recorre todos los enemigos, y realiza la acción adecuada al comportamiento del mismo.
        ACTUALIZAR_ESTADOS, // Finalizado el turno de los enemigos, el controlador verifica si es necesario realizar un cambio en los estados de las entidades (vida<0, comida<60, pasaron 5 turnos).
        EFECTOS_ESTADOS_ALTERADOS, // El controlador aplica los efectos de los estados alterados a las entidades correspondientes.
        REPOSICIÓN_ENEMIGOS // El controlador agrega enemigos al mapa si están por debajo de cierto nivel.
    }

    // Atributos
    /// <value>La pantalla del juego.</value>
    private PantallaJuego pantalla;
    /// <value>El nombre del estado "Congelado".</value>
    private string estadoPersonajeCongelado;
    /// <value>El nombre del estado "Confundido".</value>
    private string estadoPersonajeConfundido;
    /// <value>El nombre del estado "Paralizado".</value>
    private string estadoPersonajeParalizado;
    /// <value>Los estados actuales del personaje.</value>
    private List<string> estadosPersonaje;
    /// <value>La dirección en que se quiere mover el jugador.</value>
    private Vector3 direcciónMovimiento;
    /// <value>El personaje que está jugando.</value>
    private Personaje personaje;
    /// <value>El punto del ciclo en el que se encuentra el controlador.</value>
    /// <remarks>
    /// Identifica a la acción que se encuentra realizando el controlador en un momento determinado.
    /// El cicloTurno nos muestra el momento del asalto en que está el ControladorJuego.
    /// </remarks>
    private CicloTurno cicloTurno;
    

    // Métodos
    /*
    public ControladorJuego(PantallaJuego pantalla)
    {
        this.pantalla = pantalla;
    }
    */
    public PantallaJuego Pantalla { get => pantalla; set => pantalla = value; }
    public string EstadoPersonajeCongelado { get => estadoPersonajeCongelado; set => estadoPersonajeCongelado = value; }
    public string EstadoPersonajeConfundido { get => estadoPersonajeConfundido; set => estadoPersonajeConfundido = value; }
    public string EstadoPersonajeParalizado { get => estadoPersonajeParalizado; set => estadoPersonajeParalizado = value; }
    public List<string> EstadosPersonaje { get => estadosPersonaje; set => estadosPersonaje = value; }
    public Vector3 DirecciónMovimiento { get => direcciónMovimiento; set => direcciónMovimiento = value; }
    // TODO: Hacer que el get verifique si la propiedad es nula.
    public Personaje Personaje { get => personaje; set => personaje = value; }

    /**
     * <summary>
     * Se encarga de realizar las acciones relacionadas con el CU Mover personaje.
     * </summary>
     * <param name="dirección">La dirección en que se quiere mover el jugador.</param>
     */
    public void moverPersonaje(Vector3 dirección)
    {
        // Guardo la dirección del movimiento en la propiedad del controlador.
        DirecciónMovimiento = dirección;

        // Obtener estado congelado.
        obtenerEstadoPersonajeCongelado();

        // Obtener los estados en los que se encuentra el personaje.
        obtenerEstadosPersonaje();

        // Verificar si el personaje está en estado congelado.
        if (!verificarSiPersonajeEstáEnEstado(EstadoPersonajeCongelado))
        {
            // Obtener estado confundido.
            obtenerEstadoPersonajeConfundido();

            // Verificar si el personaje está en estado confundido.
            if (!verificarSiPersonajeEstáEnEstado(EstadoPersonajeConfundido))
            {
                // Verificar si el camino está obstruido.
                if (!verificarSiElCaminoEstáObstruido(dirección))
                {
                    // Obtener estado paralizado.
                    obtenerEstadoPersonajeParalizado();

                    // Verificar si el personaje está en estado paralizado.
                    if (!verificarSiPersonajeEstáEnEstado(EstadoPersonajeParalizado))
                    {
                        // Mover personaje.
                        Personaje.moverse(dirección);

                        // Actualizar visibilidad del mapa.
                        actualizarVisibilidadDelMapa();
                    }
                }
            }
        }
    }

    private void actualizarVisibilidadDelMapa()
    {
        throw new NotImplementedException();
    }

    /**
     * <summary>
     * Obtiene el nombre del estado "Congelado" y lo guarda en una propiedad del controlador.
     * </summary>
     */
    public void obtenerEstadoPersonajeCongelado()
    {
        // Busco en los archivos locales todos los estados del personaje.
        List<EstadoPersonaje> todosLosEstados = Persistencia.ReadFromBinaryFile<List<EstadoPersonaje>>(RUTA_ESTADOS_PERSONAJE);

        // Recorro todos los estados hasta encontrar el estado "Congelado".
        foreach (EstadoPersonaje estado in todosLosEstados)
        {
            if (estado.esEstadoCongelado())
            {
                // Obtengo y guardo el nombre del estado.
                EstadoPersonajeCongelado = estado.Nombre;
                break;
            }
        }
    }
    
    /**
     * <summary>
     * Obtiene el nombre del estado "Confundido" y lo guarda en una propiedad del controlador.
     * </summary>
     */
    public void obtenerEstadoPersonajeConfundido()
    {
        // Busco en los archivos locales todos los estados del personaje.
        List<EstadoPersonaje> todosLosEstados = Persistencia.ReadFromBinaryFile<List<EstadoPersonaje>>(RUTA_ESTADOS_PERSONAJE);

        // Recorro todos los estados hasta encontrar el estado "Confundido".
        foreach (EstadoPersonaje estado in todosLosEstados)
        {
            if (estado.esEstadoConfundido())
            {
                // Obtengo y guardo el nombre del estado.
                EstadoPersonajeConfundido = estado.Nombre;
                break;
            }
        }
    }

    /**
     * <summary>
     * Obtiene el nombre del estado "Paralizado" y lo guarda en una propiedad del controlador.
     * </summary>
     */
    public void obtenerEstadoPersonajeParalizado()
    {
        // Busco en los archivos locales todos los estados del personaje.
        List<EstadoPersonaje> todosLosEstados = Persistencia.ReadFromBinaryFile<List<EstadoPersonaje>>(RUTA_ESTADOS_PERSONAJE);

        // Recorro todos los estados hasta encontrar el estado "Paralizado".
        foreach (EstadoPersonaje estado in todosLosEstados)
        {
            if (estado.esEstadoParalizado())
            {
                // Obtengo y guardo el nombre del estado.
                EstadoPersonajeParalizado = estado.Nombre;
                break;
            }
        }
    }

    /**
     * <summary>
     * Obtiene los nombres de los estados actuales del personaje y los guarda en una propiedad.
     * </summary>
     */
    public void obtenerEstadosPersonaje()
    {
        EstadosPersonaje = Personaje.obtenerEstados();
    }

    /**
     * <summary>
     * Verifica si el personaje está en el estado <c>nombreEstado</c>.
     * </summary>
     * <param name="nombreEstado">El nombre del estado a controlar.</param>
     * <returns>Verdadero si <c>nombreEstado</c> está entre los estados del personaje.</returns>
     */
    public bool verificarSiPersonajeEstáEnEstado(string nombreEstado)
    {
        return EstadosPersonaje.Contains(nombreEstado);
    }

    /**
     * <summary>
     * Verifica si hay algo en la dirección en que se quiere mover el personaje que pueda obstruir el movimiento.
     * </summary>
     * <param name="dirección">Dirección del movimiento.</param>
     * <returns>Falso si el camino está libre.</returns>
     */
    public bool verificarSiElCaminoEstáObstruido(Vector3 dirección)
    {
        RaycastHit2D hit = Physics2D.Raycast(Personaje.transform.position, dirección);
        if (hit.collider == null)
        {
            return false;
        }
        return true;
    }

    /*
    void Start()
    {
        Persistencia.CrearArchivoConTodosLosEstados();        
    }
    /*
    // Use this for initialization
    void Start () {
        // Inicializo cicloTurno en el turno del personaje.
        cicloTurno = CicloTurno.TURNO_PERSONAJE;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (cicloTurno)
        {
            case CicloTurno.INICIALIZACIÓN:

                break;
            case CicloTurno.CARGA_NIVEL:

                break;
            case CicloTurno.TURNO_PERSONAJE:
                // TURNO DEL PERSONAJE
                // Se espera que el personaje realice una acción.

                break;
            case CicloTurno.TURNO_ENEMIGOS:

                break;
            case CicloTurno.ACTUALIZAR_ESTADOS:

                break;
            case CicloTurno.EFECTOS_ESTADOS_ALTERADOS:

                break;
            case CicloTurno.REPOSICIÓN_ENEMIGOS:

                break;
        }
	}
    */
}
