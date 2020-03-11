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

    private List<Enemigo> enemigos;


    /**
* <summary>
* El enum <c>CicloTurno</c> representa el ciclo que reliza el juego cada vez que pasa una ronda completa de turnos (asalto).
* </summary>
*/
    public enum CicloTurno : byte
    {
        // El ciclo se realiza en el orden que está escrito.
        INICIALIZACIÓN, // Se realizan las tareas iniciales del juego, como la generación de mapas.
        CARGA_NIVEL, // Se realiza una vez al comienzo de cada nivel, y se encarga de cargar los datos del nivel actual.
        // En este punto inicia el ciclo de turnos.
        TURNO_PERSONAJE, // El turno del personaje. Espera a que el personaje realice su acción.
        ESPERANDO_ANIMACIONES_PERSONAJE, // Para evitar inconsistencias se espera que terminen las animaciones del personaje.
        TURNO_ENEMIGOS, // El turno de los enemigos. Recorre todos los enemigos, y realiza la acción adecuada al comportamiento del mismo.
        ESPERANDO_ANIMACIONES_ENEMIGOS, // Para evitar inconsistencias se espera que terminen las animaciones de los enemigos.
        ACTUALIZAR_ESTADOS, // Finalizado el turno de los enemigos, el controlador verifica si es necesario realizar un cambio en los estados de las entidades (vida<0, comida<60, pasaron 5 turnos).
        EFECTOS_ESTADOS_ALTERADOS, // El controlador aplica los efectos de los estados alterados a las entidades correspondientes.
        REPOSICIÓN_ENEMIGOS // El controlador agrega enemigos al mapa si están por debajo de cierto nivel.
    }

    public void mostrarAnimaciónMovimientoEnemigo(Vector3 dirección, Enemigo enemigo)
    {
        Pantalla.mostrarAnimaciónMovimientoEnemigo(dirección, enemigo);
    }

    public void mostrarAnimaciónAtaqueMurciélago(bool impacta, int daño, bool esCrítico)
    {
        Pantalla.mostrarAnimaciónAtaqueMurciélago(impacta, (Personaje) ObjetivoActual, daño, esCrítico);
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
    private CicloTurno cicloActual;
    private Entidad objetivoActual;
    private Dado d20;
    private List<GameObject> mapas;
    private int mapaActual;
    //private bool animaciónEnProgreso;
    

    // Métodos
    /*
    public ControladorJuego(PantallaJuego pantalla)
    {
        this.pantalla = pantalla;
    }
    */
    public PantallaJuego Pantalla { get => pantalla == null ? pantalla = GameObject.Find("PantallaJuego").GetComponent<PantallaJuego>() : pantalla; set => pantalla = value; }
    public Vector3 DirecciónMovimiento { get => direcciónMovimiento; set => direcciónMovimiento = value; }
    // TODO: Hacer que el get verifique si la propiedad es nula.
    public Personaje Personaje { get => personaje == null ? personaje = GameObject.Find("Personaje").GetComponent<Personaje>() : personaje; set => personaje = value; }
    public Entidad ObjetivoActual { get => objetivoActual; set => objetivoActual = value; }
    public Dado D20 { get => d20; set => d20 = value; }
    public CicloTurno CicloActual { get => cicloActual; set => cicloActual = value; }
    public List<Enemigo> Enemigos { get => enemigos; set => enemigos = value; }
    public List<GameObject> Mapas { get => mapas; set => mapas = value; }
    public int MapaActual { get => mapaActual; set => mapaActual = value; }

    //public bool AnimaciónEnProgreso { get => animaciónEnProgreso; set => animaciónEnProgreso = value; }

    /// <summary>
    /// Muestra la animación de movimiento del personaje.
    /// </summary>
    /// <param name="dirección">Dirección del movimiento.</param>
    public void mostrarAnimaciónMovimientoPersonaje(Vector3 dirección)
    {
        Pantalla.mostrarAnimaciónMovimientoPersonaje(dirección);
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
                if (verificarSiElCaminoEstáObstruido(DirecciónMovimiento, Personaje))
                {
                    Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
                    Pantalla.mostrarMovimientoPersonajeFalla(DirecciónMovimiento);
                }
            } 
            else
            {
                if (!verificarSiElCaminoEstáObstruido(DirecciónMovimiento, Personaje))
                {
                    // Mover personaje.
                    Personaje.moverse(DirecciónMovimiento);

                    // Actualizar visibilidad del mapa.
                    //actualizarVisibilidadDelMapa();
                }
                else if (verificarSiObstruidoPorEnemigo(DirecciónMovimiento))
                {
                    obtenerEnemigoEnDirecciónMovimiento(DirecciónMovimiento);

                    if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.MUERTO, (Enemigo) ObjetivoActual))
                    {
                        Pantalla.mostrarMovimientoPersonajeFalla(DirecciónMovimiento);

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
                else
                {
                    Pantalla.mostrarMovimientoPersonajeFalla(DirecciónMovimiento);
                }
            }
        } 
        else
        {
            Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
            Pantalla.mostrarMovimientoPersonajeFalla(DirecciónMovimiento);
        }

        CicloActual = CicloTurno.ESPERANDO_ANIMACIONES_PERSONAJE;
    }

    /// <summary>
    /// Cambia la dirección de movimiento actual por una aleatoria,
    /// que no esté obstruida por un enemigo.
    /// </summary>
    public void cambiarDirecciónMovimiento()
    {
        List<Vector3> direcciones = new List<Vector3>();

        for (int i = 0; i < 4; i++)
        {
            if (!verificarSiObstruidoPorEnemigo(obtenerDirección(i)))
            {
                direcciones.Add(obtenerDirección(i));
            }
        }

        DirecciónMovimiento = direcciones[UnityEngine.Random.Range(0, direcciones.Count)];
    }

    public Vector3 obtenerDirecciónAleatoria(Enemigo enemigo)
    {
        List<Vector3> direcciones = new List<Vector3>();

        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemigo.transform.position, obtenerDirección(i), 1);

            if (hit.collider == null)
            {
                direcciones.Add(obtenerDirección(i));
                //Debug.DrawRay(enemigo.transform.position, obtenerDirección(i), Color.red, 3000);
            }
        }

        if (direcciones.Count == 0)
        {
            return Vector3.back;
        }

        return direcciones[UnityEngine.Random.Range(0, direcciones.Count)];
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
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.down;
            case 3:
                return Vector3.right;
            default:
                return Vector3.zero;
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
                if (verificarSiPersonajeEstáEnEstado(EstadosPersonaje.CEGADO) || verificarSiPersonajeEstáEnEstado(EstadosPersonaje.HAMBRIENTO) || verificarSiEnemigoEstáEnEstado(EstadosEnemigo.INVISIBLE, (Enemigo) ObjetivoActual))
                {
                    Personaje.Desventaja = true;
                }

                if (!verificarSiPersonajeEstáEnEstado(EstadosPersonaje.INVISIBLE))
                {
                    Personaje.Ventaja = true;
                }

                if (!verificarSiEnemigoEstáEnEstado(EstadosEnemigo.VOLANDO, (Enemigo) ObjetivoActual))
                {
                    Personaje.atacarCuerpoACuerpo((Enemigo) ObjetivoActual, 0);
                } 
                else
                {
                    Personaje.atacarCuerpoACuerpo((Enemigo) ObjetivoActual, -3);
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

    public void mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(bool impacta, int dañoRealizado, bool esCrítico)
    {
        Pantalla.mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(impacta, DirecciónMovimiento, (Enemigo) ObjetivoActual, dañoRealizado, esCrítico);
    }

    public bool realizarAtaque(int impacto, int daño, bool esCrítico)
    {
        return ObjetivoActual.recibirAtaque(impacto, daño, esCrítico);
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
        return Personaje.verificarSiEstáEnEstado(estado);
    }

    /**
     * <summary>
     * Verifica si hay algo en la dirección en que se quiere mover 
     * el personaje que pueda obstruir el movimiento.
     * </summary>
     * <param name="dirección">Dirección del movimiento.</param>
     * <returns>Falso si el camino está libre.</returns>
     */
    public bool verificarSiElCaminoEstáObstruido(Vector3 dirección, Entidad origen)
    {
        RaycastHit2D hit = Physics2D.Raycast(origen.transform.position, dirección, dirección.magnitude);
        
        if (hit.collider == null)
        {
            return false;
        } else if (hit.collider.tag == "Escalera")
        {
            return false;
        }

        return true;
    }

    public void Start()
    {
        // Inicializo cicloTurno en el turno del personaje
        CicloActual = CicloTurno.TURNO_PERSONAJE;

        Mapas = new List<GameObject>();
        Mapas.Add(GameObject.Find("Nivel 1"));
        Mapas.Add(GameObject.Find("Nivel 2"));
        Mapas.Add(GameObject.Find("Nivel 3"));

        MapaActual = 0;
        Mapas[1].SetActive(false);
        Mapas[2].SetActive(false);

        // Creo los enemigos
        Enemigos = new List<Enemigo>();
        Enemigos.Add(GameObject.Find("Murciélago (1)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Murciélago (2)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Murciélago (3)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Murciélago (4)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Murciélago (5)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Murciélago (6)").GetComponent<Murciélago>());
        Enemigos.Add(GameObject.Find("Serpiente (1)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (2)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (3)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (4)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (5)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (6)").GetComponent<Serpiente>());
        Enemigos.Add(GameObject.Find("Serpiente (7)").GetComponent<Serpiente>());

        D20 = new Dado(20);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		switch (CicloActual)
        {
            case CicloTurno.INICIALIZACIÓN:

                break;
            case CicloTurno.CARGA_NIVEL:

                break;
            case CicloTurno.TURNO_PERSONAJE:

                break;
            case CicloTurno.ESPERANDO_ANIMACIONES_PERSONAJE:
                if (!Pantalla.AnimaciónEnProgreso)
                {
                    CicloActual = CicloTurno.TURNO_ENEMIGOS;
                }

                break;
            case CicloTurno.TURNO_ENEMIGOS:
                ObjetivoActual = Personaje;
                Enemigos.ForEach((enemigo) => enemigo.usarTurno());

                CicloActual = CicloTurno.ESPERANDO_ANIMACIONES_ENEMIGOS;

                break;
            case CicloTurno.ESPERANDO_ANIMACIONES_ENEMIGOS:
                if (!Pantalla.AnimaciónEnProgreso)
                {
                    CicloActual = CicloTurno.TURNO_PERSONAJE;
                }

                break;
            case CicloTurno.ACTUALIZAR_ESTADOS:

                break;
            case CicloTurno.EFECTOS_ESTADOS_ALTERADOS:

                break;
            case CicloTurno.REPOSICIÓN_ENEMIGOS:

                break;
        }
	}

    public bool verificarSiEnemigoEstáAdyacenteAPersonaje(Enemigo enemigo)
    {
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(enemigo.transform.position, obtenerDirección(i), 1);
            //Debug.DrawRay(enemigo.transform.position, obtenerDirección(i), Color.red, 3000);

            if (hit.collider != null)
            {
                //Debug.DrawRay(enemigo.transform.position, obtenerDirección(i), Color.white, 3000);
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void bajarEscaleras()
    {
        if (personajeEstáEnEscalera())
        {
            Mapas[MapaActual].SetActive(false);

            if (MapaActual < 3)
            {
                MapaActual++;
                Mapas[MapaActual].SetActive(true);
            }

            Enemigos = new List<Enemigo>();

            if (MapaActual == 1)
            {
                Enemigos.Add(GameObject.Find("Murciélago (1)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (2)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (3)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (4)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (5)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (6)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (7)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (8)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (9)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (10)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (11)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Serpiente (1)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (2)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (3)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (4)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (5)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (6)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (7)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (8)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (9)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (10)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (11)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (12)").GetComponent<Serpiente>());
            } 
            else if (MapaActual == 2)
            {
                Enemigos.Add(GameObject.Find("Murciélago (1)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (2)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (3)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (4)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (5)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (6)").GetComponent<Murciélago>());
            }
        }
    }

    public bool personajeEstáEnEscalera()
    {
        /*
        RaycastHit2D[] hits = Physics2D.RaycastAll(Personaje.transform.position, Vector3.up, 0.1f);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Escalera")
            {
                return true;
            }
        }

        return false;
        */
        BoxCollider2D escalera = GameObject.Find("Escalera").GetComponent<BoxCollider2D>();

        if (escalera.ClosestPoint((Vector2) Personaje.transform.position) == (Vector2) Personaje.transform.position)
        {
            return true;
        }

        return false;
    }
}
