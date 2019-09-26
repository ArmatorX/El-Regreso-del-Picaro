using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * <summary La clase <c>ControladorJuego</c> se encarga de coordinar las distintas funcionalidades de la pantalla principal del juego.
 */
public class ControladorJuego : MonoBehaviour {
     /// <summary>El enum <c>CicloTurno</c> representa el ciclo que reliza el juego cada vez que pasa una ronda completa de turnos (asalto).</summary>
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

    /// <summary>
    /// Identifica a la acción que se encuentra realizando el controlador en un momento determinado. El juego se divide en asaltos, y cada asalto
    /// requiere realizar una serie de acciones. El <c>cicloTurno</c> nos muestra el momento del asalto en que está el <c>ControladorJuego</c>.
    /// </summary>
    private CicloTurno cicloTurno;

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
}
