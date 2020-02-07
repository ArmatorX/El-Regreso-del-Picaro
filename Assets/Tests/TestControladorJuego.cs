using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /**
     * <summary>
     * Prueba la clase ControladorJuego.
     * </summary> 
     * <remarks>
     * No se testea el método obtenerEstadosPersonaje, ni verificarSiPersonajeEstáEnEstado porque es trivial.
     * </remarks>
     */
    public class TestControladorJuego
    {
        // Datos de entrada
        private ControladorJuego controlador;

        // Datos de salida
        private EstadosPersonaje congelado;
        private EstadosPersonaje confundido;
        private EstadosPersonaje paralizado;
        
        [SetUp]
        protected void SetUp()
        {
            // Inicialización de datos de entrada
            controlador = new ControladorJuego();

            // Inicialización de datos de salida
            congelado = EstadosPersonaje.CONGELADO;
            confundido = EstadosPersonaje.CONFUNDIDO;
            paralizado = EstadosPersonaje.PARALIZADO;
        }

        
        
        
    }
}
