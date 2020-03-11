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
    // Atributos
    /// <value>El controlador del juego.</value>
    private ControladorJuego controlador;
    /// <value>El objeto personaje de Unity.</value>
    /// <remarks>Necesario para activar las animaciones.</remarks>
    private Personaje personaje;
    private bool animaciónEnProgreso;

    private static float TIEMPO_ANIMACIÓN_MOVIMIENTO = 0.2f;

    public ControladorJuego Controlador { get => controlador == null ? controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public Personaje Personaje { get => personaje == null ? personaje = GameObject.Find("Personaje").GetComponent<Personaje>() : personaje; set => personaje = value; }
    public bool AnimaciónEnProgreso { get => animaciónEnProgreso; set => animaciónEnProgreso = value; }

    /**
     * <summary>
     * Se encarga de realizar todo el movimiento del personaje.
     * </summary>
     * <param name="dirección">Dirección en la que se quiere mover el personaje.</param>
     */
    public void moverPersonaje(Vector3 dirección)
    {
        Controlador.moverPersonaje(dirección);
    }

    /**
     * <summary>
     * Obtiene la dirección del movimiento a partir de la tecla que haya presionado el jugador en este frame.
     * </summary>
     * <returns>Un vector unitario con dirección horizontal o vertical.</returns>
     */
    public Vector3 obtenerDirecciónMovimiento()
    {
        // Obtengo la dirección del movimiento.
        Vector3 dirección;

        if (Input.GetButtonDown("Up"))
        {
            dirección = Vector3.up;
        }
        else if (Input.GetButtonDown("Down"))
        {
            dirección = Vector3.down;
        }
        else if (Input.GetButtonDown("Right"))
        {
            dirección = Vector3.right;
        }
        else if (Input.GetButtonDown("Left"))
        {
            dirección = Vector3.left;
        }
        else
        {
            dirección = Vector3.zero;
        }

        return dirección;
    }

    public void mostrarAnimaciónMovimientoEnemigo(Vector3 dirección, Enemigo enemigo)
    {
        IEnumerator corrutinaAnimación = animaciónMovimientoEnemigo(enemigo.RB.position + (Vector2)dirección, enemigo);

        StartCoroutine(corrutinaAnimación);
    }

    public IEnumerator animaciónMovimientoEnemigo(Vector2 destino, Enemigo enemigo)
    {
        AnimaciónEnProgreso = true;


        for (int i = 1; (destino - enemigo.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(enemigo.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            enemigo.RB.position = mov;

            //Debug.Log(i);

            yield return null;
        }

        AnimaciónEnProgreso = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        AnimaciónEnProgreso = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!AnimaciónEnProgreso)
        {
            // Controlar si el jugador quiere moverse.
            if (Input.anyKeyDown)
            {
                // Obtengo la dirección movimiento.
                Vector3 dirección = obtenerDirecciónMovimiento();

                if (dirección != Vector3.zero)
                {
                    // Muevo al personaje en la dirección correspondiente.
                    moverPersonaje(dirección);
                }
                else if (Input.GetButtonDown("GoDown"))
                {
                    bajarEscaleras();
                }
            }
        }
    }

    public void bajarEscaleras()
    {
        Controlador.bajarEscaleras();
    }

    public void mostrarAnimaciónMovimientoPersonaje(Vector3 dirección)
    {
        IEnumerator corrutinaAnimación = animaciónMovimientoPersonaje(Personaje.RB.position + (Vector2)dirección);

        StartCoroutine(corrutinaAnimación);
    }

    public IEnumerator animaciónMovimientoPersonaje(Vector2 destino)
    {
        //if (AnimaciónPersonaje)
        //{
        //    yield return new WaitForSeconds(TIEMPO_ANIMACIÓN / 2);

        //    IEnumerator corrutinaAnimación = animaciónMovimientoPersonaje(destino);

        //    StartCoroutine(corrutinaAnimación);
        //}
        //else
        //{
        AnimaciónEnProgreso = true;
        int facing = (int) (destino - Personaje.RB.position).x;

        if (facing != 0)
        {
            Personaje.transform.localScale = new Vector3(facing * 6.25f, 6.25f, 1);
        }

        Personaje.Animaciones.SetInteger("estado", 1);

        for (int i = 1; (destino - Personaje.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(Personaje.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            Personaje.RB.position = mov;

            //Debug.Log(i);

            yield return null;
        }

        Personaje.Animaciones.SetInteger("estado", 0);
        AnimaciónEnProgreso = false;
        //}
    }

    public void mostrarAnimaciónAtaqueCuerpoACuerpoPersonaje(bool impacta, Vector3 dirección, Enemigo objetivo, int dañoRealizado, bool esCrítico)
    {
        if (impacta)
        {
            if (esCrítico)
            {
                Debug.Log("El ataque fue CRÍTICO, realizando " + dañoRealizado + " puntos de daño y dejó al enemigo con " + objetivo.VidaActual + " puntos de vida.");
            }
            else
            {
                Debug.Log("El ataque impacta por " + dañoRealizado + " puntos de daño y dejó al enemigo con " + objetivo.VidaActual + " puntos de vida.");
            }
        }
        else
        {
            Debug.Log("El ataque falló.");
        }
       
    }

    public void mostrarExcepcion(Exception e)
    {
        throw new NotImplementedException();
    }

    public void mostrarAtaquePersonajeFalla()
    {
        throw new NotImplementedException();
    }

    public void mostrarMovimientoPersonajeFalla(Vector3 dirección)
    {
        AnimaciónEnProgreso = true;
        int facing = (int) dirección.x;

        if (facing != 0)
        {
            Personaje.transform.localScale = new Vector3(facing * 6.25f, 6.25f, 1);
        }

        Personaje.Animaciones.SetInteger("estado", 1);
        /*
        for (int i = 1; (destino - Personaje.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(Personaje.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            Personaje.RB.position = mov;

            //Debug.Log(i);

            yield return null;
        }
        */
        Personaje.Animaciones.SetInteger("estado", 0);
        AnimaciónEnProgreso = false;
    }

    public void mostrarAnimaciónAtaqueMurciélago(bool impacta, Personaje objetivo, int dañoRealizado, bool esCrítico)
    {
        if (impacta)
        {
            if (esCrítico)
            {
                Debug.Log("El ataque fue CRÍTICO, realizando " + dañoRealizado + " puntos de daño y dejó al personaje con " + objetivo.VidaActual + " puntos de vida.");
            }
            else
            {
                Debug.Log("El ataque impacta por " + dañoRealizado + " puntos de daño y dejó al personaje con " + objetivo.VidaActual + " puntos de vida.");
            }
        }
        else
        {
            Debug.Log("El ataque falló.");
        }
    }
}
