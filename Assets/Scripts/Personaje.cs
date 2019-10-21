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
    private int COMIDA_MOVIMIENTO = 30;

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
    public int ModificadorVidaMáxima1 { get => modificadorVidaMáxima; set => modificadorVidaMáxima = value; }
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

        // Obtengo el casillero de destino.
        // Piso casilleroDestino = controlador.obtenerCasilleroDestino(Ubicación, dirección);

        // Actualizo la ubicación del personaje.
        // Ubicación = casilleroDestino;
    }

    /**
     * <summary>
     * Consume una determinada cantidad de puntos de comida del personaje.
     * </summary>
     * <param name="comimda">La cantidad de puntos de comida a consumir.</param> 
     */
    private void consumirComida(int comida)
    {
        ComidaActual -= comida;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
