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

    /// <summary>
    /// Muestra la animación de movimiento del personaje.
    /// </summary>
    /// <param name="dirección">Dirección del movimiento.</param>
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
            if (verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CONFUNDIDO))
            {
                if(!verificarSiObstruidoPorEnemigo(DirecciónMovimiento))
                {
                    cambiarDirecciónMovimiento();
                }
            }

            if (verificarSiPersonajeEstáEnEstado(EstadosPersonaje.PARALIZADO))
            {
                if (verificarSiElCaminoEstáObstruido(DirecciónMovimiento))
                {
                    Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
                    Pantalla.mostrarMovimientoPersonajeFalla();
                }
            } 
            else
            {
                if (!verificarSiElCaminoEstáObstruido(DirecciónMovimiento))
                {
                    // Mover personaje.
                    Personaje.moverse(DirecciónMovimiento);

                    // Actualizar visibilidad del mapa.
                    //actualizarVisibilidadDelMapa();
                }
                else if (verificarSiObstruidoPorEnemigo(DirecciónMovimiento))
                {
                    obtenerEnemigoEnDirecciónMovimiento(DirecciónMovimiento);

                    if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.MUERTO, ObjetivoActual))
                    {
                        try
                        {
                            atacarEnemigoCuerpoACuerpo(DirecciónMovimiento);
                        }
                        catch (Exception e)
                        {
                            Pantalla.mostrarExcepcion(e);
                        }
                    }
                }
            }
        } 
        else
        {
            Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
            Pantalla.mostrarMovimientoPersonajeFalla();
        }
    }

    /// <summary>
    /// Cambia la dirección de movimiento actual por una aleatoria,
    /// que no esté obstruida por un enemigo.
    /// </summary>
    public void cambiarDirecciónMovimiento()
    {
        List<Vector3> direcciones = new List<Vector3>();
        direcciones.Add(Vector3.up);
        direcciones.Add(Vector3.left);
        direcciones.Add(Vector3.down);
        direcciones.Add(Vector3.right);

        for (int i = 0; i < 4; i++)
        {
            if (verificarSiObstruidoPorEnemigo(obtenerDirección(i)))
            {
                direcciones.Remove(obtenerDirección(i));
            }
        }

        DirecciónMovimiento = direcciones[UnityEngine.Random.Range(0, direcciones.Count)];
    }

    /// <summary>
    /// Obtiene una dirección válida de movimiento en función de un 
    /// número. Se usa para loopear las direcciones.
    /// </summary>
    /// <param name="dirección">Entero que representa una dirección.</param>
    /// <returns></returns>
    private Vector3 obtenerDirección(int dirección)
    {
        switch (dirección)
        {
            case 0:
                return Vector3.up;
                break;
            case 1:
                return Vector3.left;
                break;
            case 2:
                return Vector3.down;
                break;
            case 3:
                return Vector3.right;
                break;
            default:
                return Vector3.zero;
                break;
        }
    }

    /// <summary>
    /// Realiza el ataque del personaje a un enemigo cuerpo a cuerpo.
    /// </summary>
    /// <param name="dirección">Dirección en que se realiza el ataque.</param>
    public void atacarEnemigoCuerpoACuerpo(Vector3 dirección)
    {
        if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.DÉBIL))
        {
            if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CONFUNDIDO))
            {
                if (verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CEGADO) || verificarSiPersonajeEstáEnEstado(EstadosPersonaje.HAMBRIENTO) || verificarSiEnemigoEstáEnEstado(EstadosEnemigo.INVISIBLE, ObjetivoActual))
                {
                    Personaje.Desventaja = true;
                }

                if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.INVISIBLE))
                {
                    Personaje.Ventaja = true;
                }

                if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.VOLANDO, ObjetivoActual))
                {
                    Personaje.atacarCuerpoACuerpo(ObjetivoActual, 0);
                } 
                else
                {
                    Personaje.atacarCuerpoACuerpo(ObjetivoActual, -5);
                }
            }
            else if (tirarD20(false, false) <= 4)
            {
                autoatacarPersonaje();
            }
        }
        else
        {
            Pantalla.mostrarAtaquePersonajeFalla();
        }
    }

    /// <summary>
    /// El personaje se ataca a sí mismo.
    /// </summary>
    private void autoatacarPersonaje()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica si un enemigo está en un estado determinado.
    /// </summary>
    /// <param name="estado">Estado a controlar.</param>
    /// <param name="enemigo">Enemigo que se quiere verificar.</param>
    /// <returns>Verdadero si está en el estado.</returns>
    public bool verificarSiEnemigoEstáEnEstado(EstadosEnemigo estado, Enemigo enemigo)
    {
        return enemigo.verificarSiEstáEnEstado(estado);
    }

    /// <summary>
    /// Obtiene el enemigo que se encuentra en la dirección.
    /// </summary>
    /// <param name="dirección">Dirección.</param>
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

    public int tirarD20(bool conVentaja, bool conDesventaja)
    {
        int tirada = D20.tirarDados(1);
        int aux = D20.tirarDados(1);
        
        if (conVentaja != conDesventaja)
        {
            if ((conVentaja && aux > tirada) || (conDesventaja && aux < tirada))
            {
                tirada = aux;
            }
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
