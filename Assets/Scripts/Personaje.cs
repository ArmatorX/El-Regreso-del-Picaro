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
    private static int COMIDA_MOVIMIENTO = 5;
    private static float TIEMPO_MOVIMIENTO = 0.1f;
    private static float TIEMPO_INVERSO_MOVIMIENTO = 1f / TIEMPO_MOVIMIENTO;

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
    //private Nivel nivel;
    /// <value>Lista de estados en los que se encuentra actualmente el personaje.</value>
    private List<EstadoPersonaje> estados;

    // Físicas
    private Rigidbody2D rigidBody2D;
    private bool seEstáMoviendo;

    // Métodos
    public Personaje(Piso ubicación, int vidaActual, string nombre, int modificadorVidaMáxima, int comidaActual, int experienciaActual, List<EstadoPersonaje> estados) : base(ubicación, vidaActual)
    {
        this.nombre = nombre;
        this.modificadorVidaMáxima = modificadorVidaMáxima;
        this.comidaActual = comidaActual;
        this.experienciaActual = experienciaActual;
        this.estados = estados;
    }
    public string Nombre { get => nombre; set => nombre = value; }
    public int ModificadorVidaMáxima { get => modificadorVidaMáxima; set => modificadorVidaMáxima = value; }
    public int ComidaActual { get => comidaActual; set => comidaActual = value; }
    public int ExperienciaActual { get => experienciaActual; set => experienciaActual = value; }
    public List<EstadoPersonaje> Estados { get => estados; set => estados = value; }

    /**
     * <summary>
     * Obtiene los estados actuales del personaje y devuelve sus respectivos nombres en
     * una lista de strings.
     * </summary>
     * <returns>Lista con los nombres de los estados actuales del personaje.</returns> 
     */
    public List<string> obtenerEstados()
    {
        // Defino una variable auxiliar para guardar los nombres de los estados.
        List<string> cadenasEstados = new List<string>();

        // Recorro los estados, buscando los nombres de cada uno.
        foreach (EstadoPersonaje estado in this.estados)
        {
            // Guardo el nombre del estado en la lista.
            cadenasEstados.Add(estado.Nombre);
        }

        // Devuelvo la lista de nombres de estados.
        return cadenasEstados;
    }

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
        // controlador.mostrarAnimaciónMovimientoPersonaje();

        // Cambio la posición del personaje en Unity.
        IEnumerator corrutina = movimientoSuavizado(dirección);

        StartCoroutine(corrutina);

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
    protected IEnumerator movimientoSuavizado(Vector3 dirección)
    {
        if (seEstáMoviendo)
        {
            // Si se está moviendo que espere hasta que termine.
            yield return new WaitForSeconds(TIEMPO_MOVIMIENTO / 4);

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

            while (distanciaRestante > float.Epsilon)
            {
                // Deshabilito el movimiento.
                seEstáMoviendo = true;

                // Calculo la nueva posición a la que se va a mover el personaje.
                Vector3 nuevaPosición = Vector3.MoveTowards(rigidBody2D.position, destino, TIEMPO_INVERSO_MOVIMIENTO * Time.deltaTime);

                // Muevo el personaje a la posición calculada.
                rigidBody2D.MovePosition(nuevaPosición);

                // Vuelvo a calcular la distancia restante.
                distanciaRestante = (this.transform.position - destino).sqrMagnitude;

                // Salta un frame, y continúa el loop hasta que está en un valor cercano a la posición de destino.
                yield return null;
            }

            // Pongo la posición del personaje en el lugar de destino.
            rigidBody2D.MovePosition(destino);

            // Habilito nuevamente el movimiento.
            seEstáMoviendo = false;
        }
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

    // Start is called before the first frame update
    void Start()
    {
        // Inicializo los atributos
        rigidBody2D = GetComponent<Rigidbody2D>();

        ModificadorVidaMáxima = 0;
        ComidaActual = 100;
        ExperienciaActual = 0;
        Estados = new List<EstadoPersonaje>();
        Estados.Add(new EstadoPersonaje("Normal"));

        Ubicación = new Piso(this.transform.position);
        VidaActual = 100;

        seEstáMoviendo = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
