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
    private static int CAPA_ENTIDADES = 8;
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
    private Enemigo objetivoActual;
    private Dado d20;
    

    // Métodos
    /*
    public ControladorJuego(PantallaJuego pantalla)
    {
        this.pantalla = pantalla;
    }
    */
    public PantallaJuego Pantalla { get => pantalla; set => pantalla = value; }
    public Vector3 DirecciónMovimiento { get => direcciónMovimiento; set => direcciónMovimiento = value; }
    // TODO: Hacer que el get verifique si la propiedad es nula.
    public Personaje Personaje { get => personaje; set => personaje = value; }
    public Enemigo ObjetivoActual { get => objetivoActual; set => objetivoActual = value; }
    public Dado D20 { get => d20; set => d20 = value; }

    public void mostrarAnimaciónMovimientoPersonaje(Vector3 dirección)
    {
        pantalla.mostrarAnimaciónMovimientoPersonaje(dirección);
    }

    /**
     * <summary>
     * Se encarga de realizar las acciones relacionadas con el CU Mover personaje.
     * </summary>
     * <param name="dirección">La dirección en que se quiere mover el jugador.</param>
     */
    public void moverPersonaje(Vector3 dirección)
    {
        DirecciónMovimiento = dirección;
        
        if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CONGELADO))
        {
            if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CONFUNDIDO))
            {
                if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.PARALIZADO))
                {
                    if (!verificarSiElCaminoEstáObstruido(dirección))
                    {
                        // Mover personaje.
                        Personaje.moverse(dirección);

                        // Actualizar visibilidad del mapa.
                        //actualizarVisibilidadDelMapa();
                    } else if (verificarSiObstruidoPorEnemigo(dirección))
                    {
                        obtenerEnemigoEnDirecciónMovimiento(dirección);

                        if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.MUERTO, ObjetivoActual))
                        {
                            //try
                            //{
                                atacarEnemigoCuerpoACuerpo(dirección);
                            //} catch (Exception e)
                            //{
                                
                            //}
                        }
                    }
                }
            }
        }
    }

    public void atacarEnemigoCuerpoACuerpo(Vector3 dirección)
    {
        if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.DÉBIL))
        {
            if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CONFUNDIDO))
            {
                if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CEGADO))
                {
                    if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.HAMBRIENTO))
                    {
                        if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.INVISIBLE, ObjetivoActual))
                        {
                            if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.INVISIBLE))
                            {
                                if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.VOLANDO, ObjetivoActual))
                                {
                                    realizarAtaqueEnemigoCuerpoACuerpo(ObjetivoActual);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void realizarAtaqueEnemigoCuerpoACuerpo(Enemigo enemigo)
    {
        Personaje.atacarCuerpoACuerpo(enemigo);
    }

    public bool verificarSiEnemigoEstáEnEstado(EstadosEnemigo estado, Enemigo enemigo)
    {
        return enemigo.verificarSiEstáEnEstado(estado);
    }

    public void obtenerEnemigoEnDirecciónMovimiento(Vector3 dirección)
    {
        RaycastHit2D hit = Physics2D.Raycast(Personaje.transform.position, dirección, dirección.magnitude);
        ObjetivoActual = hit.collider.gameObject.GetComponent<Enemigo>();
    }

    public bool verificarSiObstruidoPorEnemigo(Vector3 dirección)
    {
        RaycastHit2D hit = Physics2D.Raycast(Personaje.transform.position, dirección, dirección.magnitude);

        if (hit.collider.tag == "Enemigo")
        {
            return true;
        }
        return false;
    }

    public int tirarDadoImpacto(bool conVentaja, bool conDesventaja)
    {
        int tirada = D20.tirarDados(1);
        int aux = D20.tirarDados(1);

        if ((conVentaja && aux > tirada) || (conDesventaja && aux < tirada))
        {
             tirada = aux;
        }

        return tirada;
    }

    /*
    internal bool verificarSiAtaqueImpacta(int impacto)
    {
        return ObjetivoActual.verificarSiAtaqueImpacta(impacto);
    }
    */

    public void actualizarVisibilidadDelMapa()
    {
        throw new NotImplementedException();
    }

    public void mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(int dañoRealizado, bool esCrítico)
    {
        Pantalla.mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(DirecciónMovimiento, ObjetivoActual, dañoRealizado, esCrítico);
    }

    public void realizarAtaque(int impacto, int daño, bool esCrítico)
    {
        ObjetivoActual.recibirAtaque(impacto, daño, esCrítico);
    }

    /**
     * <summary>
     * Verifica si el personaje está en el estado <c>nombreEstado</c>.
     * </summary>
     * <param name="nombreEstado">El nombre del estado a controlar.</param>
     * <returns>Verdadero si <c>nombreEstado</c> está entre los estados del 
     * personaje.</returns>
     */
    public bool verificarSiPersonajeEstáEnEstado(EstadosPersonaje estado)
    {
        return personaje.verificarSiEstáEnEstado(estado);
    }

    /**
     * <summary>
     * Verifica si hay algo en la dirección en que se quiere mover 
     * el personaje que pueda obstruir el movimiento.
     * </summary>
     * <param name="dirección">Dirección del movimiento.</param>
     * <returns>Falso si el camino está libre.</returns>
     */
    public bool verificarSiElCaminoEstáObstruido(Vector3 dirección)
    {
        RaycastHit2D hit = Physics2D.Raycast(Personaje.transform.position, dirección, dirección.magnitude);
        
        if (hit.collider == null)
        {
            return false;
        } else if (hit.collider.tag == "Player")
        {
            return false;
        }
        return true;
    }

    public void Start()
    {
        // Inicializo la pantalla
        pantalla = GameObject.Find("PantallaJuego").GetComponent<PantallaJuego>();
        personaje = GameObject.Find("Personaje").GetComponent<Personaje>();
        D20 = new Dado(20);
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
