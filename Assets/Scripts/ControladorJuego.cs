using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/**
 * <summary>
 * Coordinar los distintos objetos que intervienen a lo largo de la 
 * partida, como el movimiento de los enemigos.
 * </summary> 
 * <remarks>
 * Se encarga de mover a los enemigos, ejecutar las acciones del 
 * jugador, controlar colisiones, etc.
 * </remarks>
 */
public class ControladorJuego : MonoBehaviour {
    // CONSTANTES
    //public static string RUTA_ESTADOS_PERSONAJE = "Datos/EstadoPersonaje.bin";
    private static int CAPA_ENTIDADES = 8;

    public GameObject hitboxes;
    public GameObject prefabHitboxPersonaje;
    public GameObject prefabHitboxEnemigoTamañoNormal;
    public GameObject prefabHitboxEnemigoTamañoGrande;

    /**
    * <summary>
    * <c>FasesTurno</c> representa los diferentes periodos que pueden
    * ocurrir a lo largo de un asalto, los cuales afectan el 
    * comportamiento del <c>ControladorJuego</c>.
    * </summary>
    */
    public enum FasesTurno : byte
    {
        // El ciclo se realiza en el orden que está escrito.
        // Se realizan las tareas iniciales del juego, como la generación de 
        // mapas.
        INICIALIZACIÓN,
        // En este punto inicia el ciclo de turnos.
        // El turno del personaje. Espera a que el personaje realice su acción.
        TURNO_PERSONAJE,
        // Para evitar inconsistencias se espera que terminen las animaciones 
        // del personaje.
        ESPERANDO_ANIMACIONES_PERSONAJE,
        // El turno de los enemigos. Recorre todos los enemigos, y realiza la 
        // acción adecuada al comportamiento del mismo.
        TURNO_ENEMIGOS,
        // Para evitar inconsistencias se espera que terminen las animaciones 
        // de los enemigos.
        ESPERANDO_ANIMACIONES_ENEMIGOS,
        // El controlador aplica los efectos de los estados alterados a las 
        // entidades correspondientes.
        // EFECTOS_ESTADOS_ALTERADOS,
        // El controlador agrega enemigos al mapa si están por debajo de cierto 
        // nivel.
        // REPOSICIÓN_ENEMIGOS
    }

    // ATRIBUTOS
    /// <value>La instancia actual de la pantalla del juego.</value>
    private PantallaJuego pantalla;
    /// <value>La dirección en que se quiere mover el jugador.</value>
    //private Vector2 direcciónMovimiento;
    /// <value>La instancia actual del personaje.</value>
    private Personaje personaje;
    /// <value>La fase en la que se encuentra el controlador.</value>
    /// <remarks>
    /// Identifica a la acción que se encuentra realizando el controlador en un 
    /// momento determinado.
    /// El cicloTurno nos muestra el momento del asalto en que está el 
    /// <c>ControladorJuego</c>.
    /// </remarks>
    private FasesTurno faseActual;
    //private Entidad objetivoActual;
    /// <value>Instancia de un dado de 20 caras, para realizar las
    /// distintas tiradas de la partida.</value>
    private Dado d20;
    /// <value>Lista de niveles de la partida.</value>
    private List<GameObject> mapas;
    /// <value>Número de orden del mapa actual.</value>
    private int mapaActual;
    /// <value>Lista de enemigos que quedan con vida en el mapa 
    /// actual.</value>
    private List<Enemigo> enemigos;
    private GameObject escaleraActual;
    private bool juegoEnPausa;
    private bool modoTest = false;

    // GETTERS Y SETTERS
    public PantallaJuego Pantalla { get => pantalla == null ? pantalla = GameObject.Find("PantallaJuego").GetComponent<PantallaJuego>() : pantalla; set => pantalla = value; }
    //public Vector2 DirecciónMovimiento { get => direcciónMovimiento; set => direcciónMovimiento = value; }
    public Personaje Personaje { get => personaje == null ? personaje = GameObject.Find("Personaje").GetComponent<Personaje>() : personaje; set => personaje = value; }
    //public Entidad ObjetivoActual { get => objetivoActual; set => objetivoActual = value; }
    public Dado D20 { get => d20 == null ? new Dado(20) : d20; set => d20 = value; }
    public FasesTurno FaseActual { get => faseActual; set => faseActual = value; }
    public List<Enemigo> Enemigos { get => enemigos; set => enemigos = value; }
    public List<GameObject> Mapas { get => mapas; set => mapas = value; }
    public int MapaActual { get => mapaActual; set => mapaActual = value; }
    public GameObject EscaleraActual { get => escaleraActual == null ? escaleraActual = GameObject.Find("Escalera") : escaleraActual; set => escaleraActual = value; }
    public bool JuegoEnPausa { get => juegoEnPausa; set => juegoEnPausa = value; }
    public bool ModoTest { get => modoTest; set => modoTest = value; }


    // CASOS DE USO
    /// <summary>
    /// Se encarga de realizar las acciones relacionadas con el CU001 Mover 
    /// personaje.
    /// </summary>
    /// <param name="dirección">La dirección en que se quiere mover el jugador.
    /// </param>
    public void moverPersonaje(Vector2 dirección)
    {
        if (JuegoEnPausa)
        {
            Pantalla.moverCursorInventario(dirección);
        }
        else
        {
            if (FaseActual == FasesTurno.TURNO_PERSONAJE)
            {
                FaseActual = FasesTurno.TURNO_ENEMIGOS;

                Vector2 posiciónDestino = (Vector2)Personaje.transform.position + dirección;

                if (!personajeEnEstado(EstadosPersonaje.CONGELADO))
                {
                    if (personajeEnEstado(EstadosPersonaje.CONFUNDIDO))
                    {
                        if (!hayEnemigoEn(posiciónDestino))
                        {
                            dirección = obtenerDirecciónAleatoria(Personaje.transform.position, Personaje.Tamaño);
                        }
                    }

                    if (personajeEnEstado(EstadosPersonaje.PARALIZADO))
                    {
                        if (hayObstáculoEn(posiciónDestino))
                        {
                            Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
                            Pantalla.animaciónMovimientoPJFalla(dirección);
                        }
                    }
                    else
                    {
                        if (!hayObstáculoEn(posiciónDestino))
                        {
                            Personaje.moverse(dirección);

                            //actualizarVisibilidadDelMapa();
                        }
                        else
                        {
                            Enemigo objetivo = obtenerEnemigoEn(posiciónDestino);

                            if (objetivo != null)
                            {
                                try
                                {
                                    atacarEnemigoCuerpoACuerpo(objetivo);
                                }
                                catch (Exception e)
                                {
                                    Pantalla.mostrarExcepcion(e);
                                }
                            }
                            else
                            {
                                Pantalla.animaciónMovimientoPJFalla(dirección);
                            }
                        }
                    }
                }
                else
                {
                    Personaje.consumirComida(Personaje.COMIDA_MOVIMIENTO);
                    Pantalla.animaciónMovimientoPJFalla(dirección);
                }
            }
        }
    }

    /// <summary>
    /// Se encarga de realizar las acciones relacionadas con el CU002 
    /// Atacar enemigo cuerpo a cuerpo.
    /// </summary>
    /// <param name="objetivo">Objetivo del ataque.</param>
    public void atacarEnemigoCuerpoACuerpo(Entidad objetivo)
    {
        int modificadorAtaque = 0;

        if (!personajeEnEstado(EstadosPersonaje.DÉBIL))
        {
            if (personajeEnEstado(EstadosPersonaje.CONFUNDIDO))
            {
                if (tirarD20(false, false) <= 4)
                {
                    objetivo = Personaje;
                }
            }

            Personaje.Desventaja = pjAtacaConDesventajaMelé(objetivo);
            Personaje.Ventaja = pjAtacaConVentajaMelé(objetivo);

            if (objetivo.esEnemigo())
            {
                if (enemigoEnEstado(EstadosEnemigo.VOLANDO, (Enemigo)objetivo))
                {
                    modificadorAtaque -= 2;
                }
            }

            Personaje.atacarCuerpoACuerpo(objetivo, modificadorAtaque);
        }
        else
        {
            Pantalla.animaciónAtaqueMeléPersonaje((Enemigo)objetivo, 0, 0);
        }
    }

    /// <summary>
    /// Se encarga de realizar las acciones relacionadas con el CU018 
    /// Actualizar visibilidad del mapa.
    /// </summary>
    /// <remarks>
    /// No se va a implementar para el final.
    /// </remarks>
    public void actualizarVisibilidadDelMapa()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Se encarga de realizar las acciones relacionadas con el CU017 Subir o 
    /// bajar escaleras.
    /// </summary>
    public void bajarEscaleras()
    {
        if (JuegoEnPausa)
        {

        }
        else
        {
            if (FaseActual == FasesTurno.TURNO_PERSONAJE)
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

                        EscaleraActual = GameObject.Find("Escalera");
                    }
                    else if (MapaActual == 2)
                    {
                        Enemigos.Add(GameObject.Find("Murciélago (1)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Murciélago (2)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Murciélago (3)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Murciélago (4)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Murciélago (5)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Murciélago (6)").GetComponent<Murciélago>());
                        Enemigos.Add(GameObject.Find("Troll").GetComponent<Troll>());

                        //EscaleraActual = GameObject.Find("Escalera");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Se encarga de realizar las acciones relacionadas con el CU092 Mostrar 
    /// inventario personaje.
    /// </summary>
    public void mostrarInventario()
    {
        if (!JuegoEnPausa)
        {
            JuegoEnPausa = true;

            // Objetos del inventario.
            ObjetoAgarrable[] objetos = new ObjetoAgarrable[0];

            // Cantidad de cada objeto en el inventario.
            int[] cantidades = new int[0];
            bool[] estáEquipado = new bool[0];

            obtenerObjetosInventario(ref objetos, ref estáEquipado, ref cantidades);

            Pantalla.mostrarObjetosInventario(objetos, estáEquipado, cantidades);
        } else
        {
            JuegoEnPausa = false;

            Pantalla.ocultarInventario();
        }
    }

    // MÉTODOS
    /*
    /// <summary>
    /// Elimina el maniquí creado para evitar que los enemigos se muevan al 
    /// mismo casillero, cuando termina la animación.
    /// </summary>
    /// <param name="enemigo">Enemigo cuya animación terminó.</param>
    public void eliminarManiquí(Entidad entidad)
    {
        entidad.eliminarManiquí();
    }
    */

    /// <summary>
    /// Busca los objetos del inventario del personaje.
    /// </summary>
    public void obtenerObjetosInventario(ref ObjetoAgarrable[] objetos, ref bool[] estáEquipado, ref int[] cantidades)
    {
        //TODO: Esto es una porquería.
        int cantidadObjetos = Personaje.Inventario.Detalle.Count;
        objetos = new ObjetoAgarrable[cantidadObjetos];
        cantidades = new int[cantidadObjetos];
        estáEquipado = new bool[cantidadObjetos];

        for (int i = 0; i < cantidadObjetos; i++)
        {
            objetos[i] = Personaje.Inventario.Detalle[i].ObjetoAgarrable;
            estáEquipado[i] = Personaje.EquipoActual.esObjetoEquipado(objetos[i]);
            cantidades[i] = Personaje.Inventario.Detalle[i].Cantidad;
        }
    }

    /// <summary>
    /// Verifica si el personaje tiene desventaja para atacar a melé.
    /// </summary>
    /// <param name="objetivo">Objetivo del ataque.</param>
    /// <returns>Verdadero si el personaje ataca con desventaja.</returns>
    public bool pjAtacaConDesventajaMelé(Entidad objetivo)
    {
        bool desventaja = false;
        
        if (personajeEnEstado(EstadosPersonaje.HAMBRIENTO))
        {
            desventaja = true;
        }

        if (objetivo.esEnemigo())
        {
            if (personajeEnEstado(EstadosPersonaje.CEGADO))
            {
                desventaja = true;
            }

            if (enemigoEnEstado(EstadosEnemigo.INVISIBLE, (Enemigo) objetivo))
            {
                desventaja = true;
            }
        }

        return desventaja;
    }
    
    /// <summary>
    /// Verifica si el personaje tiene ventaja para atacar a melé.
    /// </summary>
    /// <param name="objetivo">Objetivo del ataque.</param>
    /// <returns>Verdadero si el personaje ataca con ventaja.</returns>
    public bool pjAtacaConVentajaMelé(Entidad objetivo)
    {
        bool ventaja = false;

        if (objetivo.esEnemigo())
        {
            if (personajeEnEstado(EstadosPersonaje.INVISIBLE))
            {
                ventaja = true;
            }
        }

        return ventaja;
    }

    /*
    /// <summary>
    /// El personaje se ataca a sí mismo.
    /// </summary>
    private void autoatacarPersonaje()
    {
        throw new NotImplementedException();
    }
    */

    /// <summary>
    /// Verifica si un enemigo está en un estado determinado.
    /// </summary>
    /// <param name="estado">Estado a controlar.</param>
    /// <param name="enemigo">Enemigo que se quiere verificar.</param>
    /// <returns>Verdadero si está en el estado.</returns>
    public bool enemigoEnEstado(EstadosEnemigo estado, Enemigo enemigo)
    {
        return enemigo.estáEnEstado(estado);
    }

    /// <summary>
    /// Verifica si hay un enemigo o un maniquí enemigo en la posición 
    /// <c>posición</c>.
    /// </summary>
    /// <param name="posición">Posición a controlar.</param>
    /// <returns>Verdadero si hay un enemigo en <c>posición</c></returns>
    public bool hayEnemigoEn(Vector2 posición)
    {
        Vector2 p1 = posición - new Vector2(0.25f, 0.25f);
        Vector2 p2 = posición + new Vector2(0.25f, 0.25f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2);

        if (collider != null && collider.tag == "Enemigo")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica si el personaje o su maniquí está en la posición <c>posición</c>.
    /// </summary>
    /// <param name="posición">Posición a controlar.</param>
    /// <returns>Verdadero si hay el personaje está en <c>posición</c>.</returns>
    public bool hayPersonajeEn(Vector2 posición)
    {
        Vector2 p1 = posición - new Vector2(0.25f, 0.25f);
        Vector2 p2 = posición + new Vector2(0.25f, 0.25f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2);

        if (collider != null && collider.tag == "Player")
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Verifica si hay una entidad en la posición <c>posición</c>.
    /// </summary>
    /// <param name="posición">Posición a controlar.</param>
    /// <returns>Verdadero si hay una entidad en a posición.</returns>
    public bool hayEntidadEn(Vector2 posición)
    {
        return hayEnemigoEn(posición) || hayPersonajeEn(posición);
    }

    /// <summary>
    /// Verifica si hay un obstáculo (un objeto sólido) en <c>posición</c>.
    /// </summary>
    /// <param name="posición">La posición (x, y, z) a controlar.</param>
    /// <returns>Verdadero si hay un obstáculo.</returns>
    public bool hayObstáculoEn(Vector2 posición)
    {
        Vector2 p1 = posición - new Vector2(0.25f, 0.25f);
        Vector2 p2 = posición + new Vector2(0.25f, 0.25f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2);

        // TODO: No tiene en cuenta si un enemigo está sobre la escalera.
        if (collider == null || collider.tag == "Escalera")
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Obtiene el enemigo que se encuentra en la <c>posición</c>.
    /// </summary>
    /// <param name="posición">Posición del enemigo.</param>
    public Enemigo obtenerEnemigoEn(Vector2 posición)
    {
        Vector2 p1 = posición - new Vector2(0.25f, 0.25f);
        Vector2 p2 = posición + new Vector2(0.25f, 0.25f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2);

        if (collider != null && collider.tag == "Enemigo")
        {
            Enemigo enemigo = Enemigos.Find(e => e.Hitbox.Equals(collider.gameObject));

            return enemigo;
        }

        return null;
    }
    
    /// <summary>
    /// Obtiene una dirección válida de movimiento cuando se utiliza el tamaño 
    /// <c>TamañoEntidad.NORMAL</c>. Para tamaños más grandes, devuelve todos 
    /// los vectores que parten desde la posición actual de la entidad, hasta un
    /// casillero adyacente.
    /// </summary>
    /// <remarks>
    /// Este método se usa para recorrer los casilleros adyacentes a las
    /// entidades, o las direcciones en que se pueden mover.
    /// </remarks>
    /// <param name="dirección">Entero que representa una dirección.</param>
    /// <returns>Vector con la dirección correspondiente.</returns>
    [ObsoleteAttribute("Este método es obsoleto. Usar la clase CasillerosVálidos en su lugar.", true)]
    public Vector2 obtenerDirección(int dirección, TamañoEntidad tamaño = TamañoEntidad.NORMAL)
    {
        switch(tamaño)
        {
            case TamañoEntidad.NORMAL:
                switch (dirección)
                {
                    case 0:
                        return Vector2.up;
                    case 1:
                        return Vector2.left;
                    case 2:
                        return Vector2.down;
                    case 3:
                        return Vector2.right;
                    default:
                        return Vector2.zero;
                }
            case TamañoEntidad.GRANDE:
                switch (dirección)
                {
                    case 0:
                        return new Vector2(-0.5f, 1.5f);
                    case 1:
                        return new Vector2(-1.5f, 0.5f);
                    case 2:
                        return new Vector2(-1.5f, -0.5f);
                    case 3:
                        return new Vector2(-0.5f, -1.5f);
                    case 4:
                        return new Vector2(0.5f, -1.5f);
                    case 5:
                        return new Vector2(1.5f, -0.5f);
                    case 6:
                        return new Vector2(1.5f, 0.5f);
                    case 7:
                        return new Vector2(0.5f, 1.5f);
                    default:
                        return Vector2.zero;
                }
            case TamañoEntidad.GIGANTE:
                switch (dirección)
                {
                    case 0:
                        return new Vector2(0f, 2f);
                    case 1:
                        return new Vector2(-1f, 2f);
                    case 2:
                        return new Vector2(-2f, 1f);
                    case 3:
                        return new Vector2(-2f, 0f);
                    case 4:
                        return new Vector2(-2f, -1f);
                    case 5:
                        return new Vector2(-1f, -2f);
                    case 6:
                        return new Vector2(0f, -2f);
                    case 7:
                        return new Vector2(1f, -2f);
                    case 8:
                        return new Vector2(2f, -1f);
                    case 9:
                        return new Vector2(2f, 0f);
                    case 10:
                        return new Vector2(2f, 1f);
                    case 11:
                        return new Vector2(1f, 2f);
                    default:
                        return Vector2.zero;
                }
        }

        return Vector2.zero;
    }
    
    /// <summary>
    /// Devuelve una dirección de movimiento aleatoria, que no esté 
    /// obstruida por un enemigo o por el personaje.
    /// </summary>
    /// <param name="posiciónInicio">Posición desde la que se 
    /// realiza el control.</param>
    /// <param name="controlarParedes">Si se pone en true, 
    /// también se controla la colisión con las paredes.</param>
    /// <returns>Dirección aleatoria libre.</returns>
    public Vector2 obtenerDirecciónAleatoria(Vector2 posiciónInicio, TamañoEntidad tamaño, bool controlarParedes = false)
    {
        List<Vector2> direcciones = obtenerDireccionesVálidas(posiciónInicio, tamaño, controlarParedes);
        
        if (direcciones.Count == 0)
        {
            return Vector2.zero;
        }

        return direcciones[UnityEngine.Random.Range(0, direcciones.Count)];
    }

    /// <summary>
    /// Verifica todas las direcciones válidas desde una posición, a partir del
    /// tamaño de una entidad.
    /// </summary>
    /// <param name="posiciónInicio">Posición desde la cual se controla.</param>
    /// <param name="tamaño">Tamaño de la entidad.</param>
    /// <param name="controlarParedes">Si se controlan también las paredes.</param>
    /// <returns>Lista de direcciones válidas de movimiento.</returns>
    public List<Vector2> obtenerDireccionesVálidas(Vector2 posiciónInicio, TamañoEntidad tamaño, bool controlarParedes = false)
    {
        List<Vector2> direcciones = new List<Vector2>();

        foreach (Vector2 dirección in CasillerosVálidos.DireccionesVálidas)
        {
            if (esDirecciónVálida(dirección, posiciónInicio, tamaño, controlarParedes))
            {
                direcciones.Add(dirección);
            }
        }

        return direcciones;
    }

    public bool esDirecciónVálida(Vector2 dirección, Vector2 posiciónInicio, TamañoEntidad tamaño, bool controlarParedes)
    {
        bool esDirecciónVálida = true;

        foreach(Vector2 casilleroRelativo in CasillerosVálidos.CasillerosTamañoNormal[dirección])
        {
            Vector2 posición = posiciónInicio + casilleroRelativo;
            if (!controlarParedes)
            {
                if (hayEntidadEn(posición))
                {
                    esDirecciónVálida = false;
                    break;
                }
            }
            else
            {
                if (hayObstáculoEn(posición))
                {
                    esDirecciónVálida = false;
                    break;
                }
            }
        }

        return esDirecciónVálida;
    }

    /// <summary>
    /// Realiza una tirada del dado de 20 caras, permitiendo tirar 
    /// con ventaja y desventaja.
    /// </summary>
    /// <param name="conVentaja">Indica si la tirada tiene ventaja.</param>
    /// <param name="conDesventaja">Indica si la tirada tiene desventaja.</param>
    /// <returns>El resultado de la tirada.</returns>
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

    /// <summary>
    /// Ejecuta un ataque físico. Verifica si el impacto supera la defensa del 
    /// objetivo y en caso de que sea así, aplica el daño correspondiente.
    /// </summary>
    /// <param name="objetivo">Objetivo del ataque.</param>
    /// <param name="impacto">Impacto del ataque.</param>
    /// <param name="daño">Daño que recibe el objetivo en un ataque exitoso.</param>
    /// <returns>Verdadero si el ataque impacta.</returns>
    public bool realizarAtaque(Entidad objetivo, int impacto, int daño)
    {
        return objetivo.recibirAtaque(impacto, daño);
    }

    /**
     * <summary>
     * Verifica si el personaje está en el estado <c>nombreEstado</c>.
     * </summary>
     * <param name="nombreEstado">El nombre del estado a controlar.</param>
     * <returns>Verdadero si <c>nombreEstado</c> está entre los 
     * estados del personaje.</returns>
     */
    public bool personajeEnEstado(EstadosPersonaje estado)
    {
        return Personaje.estáEnEstado(estado);
    }

    /// <summary>
    /// Verifica si el enemigo está adyacente al personaje.
    /// </summary>
    /// <param name="posición">Posición desde el cual se hace el control.</param>
    /// <returns>Verdadero si está adyacente.</returns>
    public bool estáAdyacenteAlPersonaje(Vector2 posición, TamañoEntidad tamaño)
    {
        foreach (List<Vector2> direcciones in CasillerosVálidos.ObtenerCasillerosVálidos(tamaño).Values)
        {
            foreach(Vector2 dirección in direcciones)
            {
                if (hayPersonajeEn(posición + dirección))
                {
                    return true;
                }
            }
        }

        return false;
    }
    
    /// <summary>
    /// Controla si el personaje está posicionado sobre la escalera del nivel 
    /// actual.
    /// </summary>
    /// <returns>Verdadero si está en la escalera.</returns>
    public bool personajeEstáEnEscalera()
    {
        Vector2 posiciónEscaleraFixed = EscaleraActual.transform.position;

        if ((posiciónEscaleraFixed - Personaje.RB.position).magnitude == 0)
        {
            return true;
        }

        return false;
        
        /*
        Vector2 p1 = Personaje.RB.position - new Vector2(0.25f, 0.25f);
        Vector2 p2 = Personaje.RB.position + new Vector2(0.25f, 0.25f);
        Collider2D collider = Physics2D.OverlapArea(p1, p2, 10);

        if (collider != null && collider.tag == "Escalera")
        {
            return true;
        }

        return false;
        */
    }

    public IEnumerator usarTurnosEnemigos()
    {
        foreach (Enemigo e in Enemigos)
        {
            e.usarTurno(0);

            //Debug.Log(e.gameObject.name);

            yield return null;
        }

        while (Enemigos.Last().SeEstáMoviendo)
        {
            yield return null;
        }

        FaseActual = FasesTurno.TURNO_PERSONAJE;
    }


    // ANIMACIONES
    /// <summary>
    /// Muestra la animación de movimiento del personaje.
    /// </summary>
    /// <param name="dirección">Dirección del movimiento.</param>
    public void animaciónMovimientoPersonaje(Vector2 dirección)
    {
        Pantalla.animaciónMovimientoPersonaje(dirección);
    }

    /// <summary>
    /// Muestra la animación del movimiento de un enemigo.
    /// </summary>
    /// <param name="enemigo">Enemigo que se quiere mover.</param>
    /// <param name="dirección">Dirección del movimiento.</param>
    public void animaciónMovimientoEnemigo(Enemigo enemigo, Vector2 dirección)
    {
        Pantalla.animaciónMovimientoEnemigo(enemigo, dirección);
    }

    /// <summary>
    /// Muestra la animación del ataque melé básico de un enemigo. 
    /// </summary>
    /// <remarks>
    /// No todos los enemigos tienen un ataque melé, pero la gran mayoría sí.
    /// Tipos de animación:
    /// 0 - El ataque falla.
    /// 1 - El ataque impacta. Animación por defecto.
    /// 2 - El ataque es crítico.
    /// </remarks>
    /// <param name="enemigo">Enemigo que realiza el ataque.</param>
    /// <param name="daño">Daño realizado al personaje.</param>
    /// <param name="tipo">Tipo de animación.</param>
    public void animaciónAtaqueMeléEnemigo(Enemigo enemigo, int daño, byte tipo = 1)
    {
        Pantalla.animaciónAtaqueMeléEnemigo(enemigo, daño, tipo);
    }

    /// <summary>
    /// Muestra la animación del ataque melé del personaje.
    /// </summary>
    /// <remarks>
    /// Tipos de animación:
    /// 0 - El ataque falla.
    /// 1 - El ataque impacta. Animación por defecto.
    /// 2 - El ataque es crítico.
    /// </remarks>
    /// <param name="objetivo">Objetivo del ataque.</param>
    /// <param name="dañoRealizado">Puntos de daño realizado.</param>
    /// <param name="tipo">Tipos de animación.</param>
    public void animaciónAtaqueMeléPersonaje(Entidad objetivo, int dañoRealizado, byte tipo = 1)
    {
        Pantalla.animaciónAtaqueMeléPersonaje(objetivo, dañoRealizado, tipo);
    }

    // MÉTODOS DE UNITY
    public void Start()
    {
        // Inicializo cicloTurno en el turno del personaje
        FaseActual = FasesTurno.TURNO_PERSONAJE;

        if (!ModoTest)
        {
            Mapas = new List<GameObject>
            {
                GameObject.Find("Nivel 1"),
                GameObject.Find("Nivel 2"),
                GameObject.Find("Nivel 3")
            };

            MapaActual = 0;
            if (Mapas[1] != null)
            {
                Mapas[1].SetActive(false);
            }

            if (Mapas[2] != null)
            {
                Mapas[2].SetActive(false);
            }

            // Creo los enemigos
            Enemigos = new List<Enemigo>();
            Enemigos.Add(GameObject.Find("Murciélago (1)").GetComponent<Murciélago>());
            Enemigos.Add(GameObject.Find("Serpiente (1)").GetComponent<Serpiente>());

            if (GameObject.Find("Murciélago (2)") != null)
            {
                Enemigos.Add(GameObject.Find("Murciélago (2)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (3)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (4)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (5)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Murciélago (6)").GetComponent<Murciélago>());
                Enemigos.Add(GameObject.Find("Serpiente (2)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (3)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (4)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (5)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (6)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Serpiente (7)").GetComponent<Serpiente>());
                Enemigos.Add(GameObject.Find("Troll").GetComponent<Troll>());
            }

            // Creo la hitbox del personaje.
            GameObject hitboxPersonaje = Instantiate(prefabHitboxPersonaje,
                Personaje.transform.position,
                Quaternion.identity,
                hitboxes.transform);
            Personaje.Hitbox = hitboxPersonaje;
            
            // Creo las hitbox de los enemigos.
            Enemigos.ForEach(e =>
            {
                GameObject hitbox;

                if (e.Tamaño == TamañoEntidad.NORMAL)
                {
                    hitbox = Instantiate(prefabHitboxEnemigoTamañoNormal,
                        e.transform.position,
                        Quaternion.identity,
                        hitboxes.transform);
                }
                else
                {
                    hitbox = Instantiate(prefabHitboxEnemigoTamañoGrande,
                        e.transform.position,
                        Quaternion.identity,
                        hitboxes.transform);
                }

                e.Hitbox = hitbox;
            });
        }

        D20 = new Dado(20);

        JuegoEnPausa = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ModoTest)
        {
            switch (FaseActual)
            {
                //case FasesTurno.INICIALIZACIÓN:

                //    break;
                //case FasesTurno.TURNO_PERSONAJE:

                //    break;
                //case FasesTurno.ESPERANDO_ANIMACIONES_PERSONAJE:
                //    if (!Pantalla.AnimaciónEnProgreso)
                //    {
                //        FaseActual = FasesTurno.TURNO_ENEMIGOS;
                //    }

                //    break;
                case FasesTurno.TURNO_ENEMIGOS:
                    StartCoroutine(usarTurnosEnemigos());

                    FaseActual = FasesTurno.ESPERANDO_ANIMACIONES_ENEMIGOS;

                    break;

                    //case FasesTurno.ESPERANDO_ANIMACIONES_ENEMIGOS:
                    //    if (!Enemigos.First().SeEstáMoviendo && !Enemigos.Last().SeEstáMoviendo)
                    //    {
                    //        FaseActual = FasesTurno.TURNO_PERSONAJE;
                    //    }

                    //    break;
                    //case FasesTurno.EFECTOS_ESTADOS_ALTERADOS:

                    //    break;
                    //case FasesTurno.REPOSICIÓN_ENEMIGOS:

                    //    break;
            }
        }
    }
}
