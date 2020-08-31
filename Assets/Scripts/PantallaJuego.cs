using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject[,] inventario = new GameObject[4, 4];
    private GameObject inventarioGUI;
    private GameObject cursorInventario;
    private GameObject contenedorTextoDaños;

    // GETTERS Y SETTERS
    public ControladorJuego Controlador { get => controlador == null ? controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>() : controlador; set => controlador = value; }
    public Personaje Personaje { get => personaje == null ? personaje = GameObject.Find("Personaje").GetComponent<Personaje>() : personaje; set => personaje = value; }
    public bool AnimaciónEnProgreso { get => animaciónEnProgreso; set => animaciónEnProgreso = value; }
    public GameObject BarraVidaPersonaje { get => barraVidaPersonaje == null ? barraVidaPersonaje = GameObject.Find("Vida") : barraVidaPersonaje; set => barraVidaPersonaje = value; }
    public GameObject[,] Inventario { get => inventario; set => inventario = value; }
    public GameObject InventarioGUI { get => inventarioGUI == null ? inventarioGUI = GameObject.Find("Inventario") : inventarioGUI; set => inventarioGUI = value; }
    public GameObject CursorInventario { get => cursorInventario == null ? cursorInventario = GameObject.Find("Cursor Inventario") : cursorInventario; set => cursorInventario = value; }
    public GameObject ContenedorTextoDaños { get => contenedorTextoDaños == null ? contenedorTextoDaños = GameObject.Find("Daños") : contenedorTextoDaños; set => contenedorTextoDaños = value; }


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

    public void actualizarVidaPersonaje()
    {
        Vector2 delta = new Vector2(128 * Personaje.VidaActual / Personaje.obtenerVidaMáxima(), 7);
        BarraVidaPersonaje.GetComponent<RectTransform>().sizeDelta = delta;
    }

    /// <summary>
    /// Pausa el juego y abre el inventario del personaje.
    /// </summary>
    public void mostrarInventario()
    {
        InventarioGUI.SetActive(true);
        Controlador.mostrarInventario();
    }

    /// <summary>
    /// Crea y muestra las visualizaciones de los objetos del inventario del 
    /// personaje.
    /// </summary>
    /// <param name="objetos">Objetos del inventario.</param>
    /// <param name="cantidades">Cantidad de cada objeto del inventario.</param>
    public void mostrarObjetosInventario(ObjetoAgarrable[] objetos, bool[] estáEquipado, int[] cantidades)
    {
        for (int i = 0; i < objetos.Length; i ++)
        {
            int j = tranformarIndex(i)[0];
            int k = tranformarIndex(i)[1];
            int posX = tranformarIndex(i)[2];
            int posY = tranformarIndex(i)[3];

            // Creo la imagen
            Inventario[j, k] = new GameObject(objetos[i].Nombre);

            Image imagen = Inventario[j, k].AddComponent<Image>(); 
            Sprite sprite = Resources.Load<Sprite>(objetos[i].RutaSprite);
            imagen.sprite = sprite;

            RectTransform rtImagen = Inventario[j, k].GetComponent<RectTransform>();
                
            rtImagen.SetParent(InventarioGUI.transform);

            rtImagen.anchorMin.Set(0.5f, 0.5f);
            rtImagen.anchorMax.Set(0.5f, 0.5f);
            rtImagen.pivot.Set(0.5f, 0.5f);

            rtImagen.anchoredPosition = new Vector3(posX, posY, 0);

            rtImagen.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 16);
            rtImagen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 16);
            rtImagen.localScale = new Vector3(7, 7, 1);

            Inventario[j, k].SetActive(true);

            // Creo el texto que indica la cantidad
            GameObject goCantidad = new GameObject("txtCantidad");

            Text txtCantidad = goCantidad.AddComponent<Text>();

            txtCantidad.text = "x" + cantidades[i];
            txtCantidad.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            txtCantidad.alignment = TextAnchor.MiddleCenter;
            txtCantidad.fontSize = 14;

            RectTransform rtTextoCantidad = goCantidad.GetComponent<RectTransform>();
            rtTextoCantidad.SetParent(rtImagen);
            
            rtTextoCantidad.anchorMin.Set(0.5f, 0.5f);
            rtTextoCantidad.anchorMax.Set(0.5f, 0.5f);
            rtTextoCantidad.pivot.Set(0.5f, 0.5f);

            rtTextoCantidad.anchoredPosition = new Vector3(4.6f, -4.6f);

            // Creo el indicador de objeto equipado
            if (estáEquipado[i])
            {
                GameObject goMarcadorEquipado = new GameObject("txtMarcadorEquipado");

                Text txtMarcadorEquipado = goMarcadorEquipado.AddComponent<Text>();

                txtMarcadorEquipado.text = "+";
                txtMarcadorEquipado.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                txtMarcadorEquipado.alignment = TextAnchor.MiddleCenter;
                txtMarcadorEquipado.fontSize = 16;
                txtMarcadorEquipado.color = Color.yellow;

                RectTransform rtTextoMarcador = goMarcadorEquipado.GetComponent<RectTransform>();
                rtTextoMarcador.SetParent(rtImagen);

                rtTextoMarcador.anchorMin.Set(0.5f, 0.5f);
                rtTextoMarcador.anchorMax.Set(0.5f, 0.5f);
                rtTextoMarcador.pivot.Set(0.5f, 0.5f);

                rtTextoMarcador.anchoredPosition = new Vector3(-4.6f, 4.6f);
            }
        }
    }

    /// <summary>
    /// Se encarga de mover el cursor dentro del inventario.
    /// </summary>
    /// <param name="dirección"></param>
    public void moverCursorInventario(Vector2 dirección)
    {
        dirección *= 112;

        Vector3 posición = CursorInventario.GetComponent<RectTransform>().anchoredPosition + dirección;

        if (!(posición.x > 168 || posición.x < -168 || posición.y > 133 || posición.y < -203))
        {
            CursorInventario.GetComponent<RectTransform>().anchoredPosition = posición;
        }
    }

    /// <summary>
    /// Sirve para adaptar el i al inventario visual de 4x4.
    /// </summary>
    /// <param name="i">i</param>
    /// <returns>(i, j)</returns>
    public int[] tranformarIndex(int i)
    {
        int[] salida = new int[4];
        if (i < 4) {
            salida[0] = 0;
            salida[1] = i;
            salida[2] = i * 112 - 168;
            salida[3] = 133;
        } else if (i < 8)
        {
            salida[0] = 1;
            salida[1] = i - 4;
            salida[2] = (i - 4) * 112 - 168;
            salida[3] = -1 * 112 + 133;
        } else if (i < 12)
        {
            salida[0] = 2;
            salida[1] = i - 8;
            salida[2] = (i - 8) * 112 - 168;
            salida[3] = -2 * 112 + 133;
        } else
        {
            salida[0] = 3;
            salida[1] = i - 12;
            salida[2] = (i - 12) * 112 - 168;
            salida[3] = -3 * 112 + 133;
        }

        return salida;
    }

    public void ocultarInventario()
    {
        foreach (GameObject objeto in Inventario) {
            Destroy(objeto);
        }

        InventarioGUI.SetActive(false);
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
        Personaje.Animaciones.SetInteger("estado", 1);

        orientarSpriteEntidad(Personaje, destino - Personaje.RB.position);

        for (int i = 1; (destino - Personaje.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(Personaje.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            Personaje.RB.position = mov;

            yield return null;
        }

        Personaje.Animaciones.SetInteger("estado", 0);

        //Controlador.eliminarManiquí(Personaje);
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
        orientarSpriteEntidad(enemigo, destino - enemigo.RB.position);

        for (int i = 1; (destino - enemigo.RB.position).magnitude > Mathf.Epsilon; i++)
        {
            Vector2 mov = Vector2.Lerp(enemigo.RB.position, destino, (Time.deltaTime / TIEMPO_ANIMACIÓN_MOVIMIENTO) * i);

            enemigo.RB.position = mov;

            yield return null;
        }

        //Controlador.eliminarManiquí(enemigo);
    }

    public void mostrarExcepcion(Exception e)
    {
        Debug.LogError(e);
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
                mostrarTextoAtaqueFalló(objetivo);
                break;
            case 1:
                mostrarTextoDañoAtaque(objetivo, dañoRealizado);
                break;
            case 2:
                mostrarTextoDañoAtaque(objetivo, dañoRealizado);
                break;
        }
    }

    private void mostrarTextoAtaqueFalló(Entidad objetivo)
    {
        GameObject goTxtFalló = new GameObject("Ataque Melé Personaje");

        Text txtFalló = goTxtFalló.AddComponent<Text>();

        txtFalló.text = "¡Falló!";
        txtFalló.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        txtFalló.alignment = TextAnchor.MiddleCenter;
        txtFalló.fontSize = 24;
        txtFalló.fontStyle = FontStyle.Bold;
        txtFalló.color = Color.white;

        RectTransform rtTxtFalló = goTxtFalló.GetComponent<RectTransform>();
        rtTxtFalló.SetParent(ContenedorTextoDaños.transform);

        rtTxtFalló.anchorMin.Set(0.5f, 0.5f);
        rtTxtFalló.anchorMax.Set(0.5f, 0.5f);
        rtTxtFalló.pivot.Set(0.5f, 0.5f);

        rtTxtFalló.position = objetivo.transform.position;
        rtTxtFalló.localScale = new Vector3(1, 1, 1);
    }

    private void mostrarTextoDañoAtaque(Entidad objetivo, int daño)
    {
        GameObject goTxtFalló = new GameObject("Ataque Melé Personaje");

        Text txtFalló = goTxtFalló.AddComponent<Text>();

        txtFalló.text = "-" + daño;
        txtFalló.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        txtFalló.alignment = TextAnchor.MiddleCenter;
        txtFalló.fontSize = 24;
        txtFalló.fontStyle = FontStyle.Bold;
        txtFalló.color = Color.red;

        RectTransform rtTxtFalló = goTxtFalló.GetComponent<RectTransform>();
        rtTxtFalló.SetParent(ContenedorTextoDaños.transform);

        rtTxtFalló.anchorMin.Set(0.5f, 0.5f);
        rtTxtFalló.anchorMax.Set(0.5f, 0.5f);
        rtTxtFalló.pivot.Set(0.5f, 0.5f);

        rtTxtFalló.position = objetivo.transform.position;
        rtTxtFalló.localScale = new Vector3(1, 1, 1);
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
        InventarioGUI.SetActive(false);
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
                else if (Input.GetButtonDown("BajarEscalera"))
                {
                    bajarEscaleras();
                }
                else if (Input.GetButtonDown("MostrarInventario"))
                {
                    mostrarInventario();
                }
            }
        }
    }
}
