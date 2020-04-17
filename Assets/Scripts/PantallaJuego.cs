using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Esta clase se encarga de realizar las operaciones relacionadas con la GUI durante una partida.
 * </summary>
 * <remarks>
 * Algunas operaciones que realiza la pantalla son mostrar las animaciones de los enemigos, y del
 * personaje, mostrar las barras de vida y hambre, mostrar el daño realizado, obtener las inputs del jugador, etc.
 * </remarks>
 */
public class PantallaJuego : MonoBehaviour
{
    // CONSTANTES
    private static float TIEMPO_ANIMACIÓN_MOVIMIENTO = 0.2f;

    // ATRIBUTOS
    /// <value>El controlador del juego.</value>
    private ControladorJuego controlador;
    /// <value>El objeto personaje de Unity.</value>
    /// <remarks>Necesario para activar las animaciones.</remarks>
    private Personaje personaje;
    private bool animaciónEnProgreso;
    private GameObject barraVidaPersonaje;

    // GETTERS Y SETTERS
    public ControladorJuego Controlador { get => controlador == null ? controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public Personaje Personaje { get => personaje == null ? personaje = GameObject.Find("Personaje").GetComponent<Personaje>() : personaje; set => personaje = value; }
    public bool AnimaciónEnProgreso { get => animaciónEnProgreso; set => animaciónEnProgreso = value; }
    public GameObject BarraVidaPersonaje { get => barraVidaPersonaje == null ? barraVidaPersonaje = GameObject.Find("Vida") : barraVidaPersonaje; set => barraVidaPersonaje = value; }

    // MÉTODOS
    /**
     * <summary>
     * Obtiene la dirección del movimiento a partir de la tecla que haya 
     * presionado el jugador en este frame.
     * </summary>
     * <returns>Un vector unitario con dirección horizontal o vertical.</returns>
     */
    public Vector2 obtenerDirecciónMovimiento()
    {
        Vector2 dirección;

        if (Input.GetButtonDown("Up"))
        {
            dirección = Vector2.up;
        }
        else if (Input.GetButtonDown("Down"))
        {
            dirección = Vector2.down;
        }
        else if (Input.GetButtonDown("Right"))
        {
            dirección = Vector2.right;
        }
        else if (Input.GetButtonDown("Left"))
        {
            dirección = Vector2.left;
        }
        else
        {
            dirección = Vector2.zero;
        }

        return dirección;
    }

    /**
     * <summary>
     * Inicia el movimiento del personaje.
     * </summary>
     * <param name="dirección">Dirección en la que se quiere mover el personaje.</param>
     */
    public void moverPersonaje(Vector2 dirección)
    {
        Controlador.moverPersonaje(dirección);
    }

    /// <summary>
    /// Inicia la bajada de las escaleras.
    /// </summary>
    public void bajarEscaleras()
    {
        Controlador.bajarEscaleras();
    }

    private void actualizarVidaPersonaje()
    {
        Vector2 delta = new Vector2(128 * Personaje.VidaActual / Personaje.obtenerVidaMáxima(), 7);
        BarraVidaPersonaje.GetComponent<RectTransform>().sizeDelta = delta;
    }

    // ANIMACIONES
    /// <summary>
    /// Inicia la corrutina que anima el movimiento del personaje.
    /// </summary>
    /// <param name="dirección">Dirección en que se realiza el movimiento.</param>
    public void animaciónMovimientoPersonaje(Vector2 dirección)
    {
        IEnumerator corrutina = movimientoSuavizadoPersonaje(Personaje.RB.position + dirección);

        StartCoroutine(corrutina);
    }

    /// <summary>
    /// Corrutina que se encarga de realizar la animación suavizada del 
    /// movimiento del personaje.
    /// </summary>
    /// <param name="destino">Casillero de destino.</param>
    /// <returns>Corrutina de movimiento.</returns>
    public IEnumerator movimientoSuavizadoPersonaje(Vector2 destino)
    {
        AnimaciónEnProgreso = true;
        Personaje.Animaciones.SetInteger("estado", 1);

        orientarSpriteEntidad(Personaje, destino - Personaje.RB.position);

        for (int i = 1; (destino - Personaje.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(Personaje.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            Personaje.RB.position = mov;

            yield return null;
        }

        Personaje.Animaciones.SetInteger("estado", 0);
        AnimaciónEnProgreso = false;
    }

    /// <summary>
    /// Muestra la animación del movimiento del personaje cuando este falla.
    /// </summary>
    /// <param name="dirección">Dirección de movimiento.</param>
    public void animaciónMovimientoPJFalla(Vector2 dirección)
    {
        orientarSpriteEntidad(Personaje, dirección);
    }

    /// <summary>
    /// Orienta correctamente el sprite de una entidad en función de la 
    /// dirección del movimiento.
    /// </summary>
    /// <param name="dirección">Dirección de movimiento.</param>
    /// <param name="entidad">Entidad a orientar.</param>
    public void orientarSpriteEntidad(Entidad entidad, Vector2 dirección)
    {
        int facing = (int) dirección.x;

        if (facing != 0 && facing != Mathf.Sign(entidad.transform.localScale.x))
        {
            Vector3 escala = entidad.transform.localScale;
            escala.x *= -1;
            entidad.transform.localScale = escala;
            //entidad.transform.localScale = new Vector3(facing * entidad.transform.localScale.x, entidad.transform.localScale.y, entidad.transform.localScale.z);
        }
    }

    /// <summary>
    /// Muestra la animación del movimiento de un enemigo.
    /// </summary>
    /// <param name="enemigo">Enemigo que se quiere mover.</param>
    /// <param name="dirección">Dirección del movimiento.</param>
    public void animaciónMovimientoEnemigo(Enemigo enemigo, Vector2 dirección)
    {
        IEnumerator corrutinaAnimación = movimientoSuavizadoEnemigo(enemigo.RB.position + dirección, enemigo);

        StartCoroutine(corrutinaAnimación);
    }

    /// <summary>
    /// Corrutina de movimiento suavizado para los enemigos.
    /// </summary>
    /// <param name="destino">Posición de destino.</param>
    /// <param name="enemigo">Enemigo a mover.</param>
    /// <returns>Corrutina de movimiento.</returns>
    public IEnumerator movimientoSuavizadoEnemigo(Vector2 destino, Enemigo enemigo)
    {
        AnimaciónEnProgreso = true;

        orientarSpriteEntidad(enemigo, destino - enemigo.RB.position);

        for (int i = 1; (destino - enemigo.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(enemigo.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            enemigo.RB.position = mov;

            yield return null;
        }

        AnimaciónEnProgreso = false;
    }

    public void mostrarExcepcion(Exception e)
    {
        throw new NotImplementedException();
    }

    // ANIMACIONES ATAQUES
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
    public void animaciónAtaqueMeléPersonaje(Entidad objetivo, int dañoRealizado, byte tipo)
    {
        orientarSpriteEntidad(Personaje, (Vector2) objetivo.transform.position - Personaje.RB.position);

        switch (tipo)
        {
            case 0:
                Debug.Log("El ataque falló.");
                break;
            case 1:
                Debug.Log("El ataque impacta por " + dañoRealizado + " puntos de daño y dejó al enemigo con " + objetivo.VidaActual + " puntos de vida.");
                break;
            case 2:
                Debug.Log("El ataque fue CRÍTICO, realizando " + dañoRealizado + " puntos de daño y dejó al enemigo con " + objetivo.VidaActual + " puntos de vida.");
                break;
        }
    }

    /// <summary>
    /// Muestra la animación del ataque del enemigo.
    /// </summary>
    /// <remarks>
    /// Tipos de animación:
    /// 0 - El ataque falla.
    /// 1 - El ataque impacta. Animación por defecto.
    /// 2 - El ataque es crítico.
    /// </remarks>
    /// <param name="enemigo">Enemigo que realiza el ataque.</param>
    /// <param name="dañoRealizado">Cantidad de daño realizado.</param>
    /// <param name="tipo">Tipo de animación.</param>
    public void animaciónAtaqueMeléEnemigo(Enemigo enemigo, int dañoRealizado, byte tipo = 1)
    {
        orientarSpriteEntidad(enemigo, Personaje.RB.position - enemigo.RB.position);

        switch (tipo)
        {
            case 0:
                Debug.Log("El ataque falló.");
                break;
            case 1:
                actualizarVidaPersonaje();
                Debug.Log("El ataque impacta por " + dañoRealizado + " puntos de daño y dejó al personaje con " + Personaje.VidaActual + " puntos de vida.");
                break;
            case 2:
                actualizarVidaPersonaje();
                Debug.Log("El ataque fue CRÍTICO, realizando " + dañoRealizado + " puntos de daño y dejó al personaje con " + Personaje.VidaActual + " puntos de vida.");
                break;
        }
    }

    // MÉTODOS DE UNITY
    void Start()
    {
        AnimaciónEnProgreso = false;
    }

    void Update()
    {
        if (!AnimaciónEnProgreso)
        {
            if (Input.anyKeyDown)
            {
                Vector2 dirección = obtenerDirecciónMovimiento();

                if (dirección != Vector2.zero)
                {
                    moverPersonaje(dirección);
                }
                else if (Input.GetButtonDown("GoDown"))
                {
                    bajarEscaleras();
                }
            }
        }
    }
}
