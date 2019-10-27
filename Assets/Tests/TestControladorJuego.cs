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
        private string congelado;
        private string confundido;
        private string paralizado;

        [SetUp]
        protected void SetUp()
        {
            // Inicialización de datos de entrada
            controlador = new ControladorJuego();

            // Inicialización de datos de salida
            congelado = "Congelado";
            confundido = "Confundido";
            paralizado = "Paralizado";
        }

        [Test]
        public void TestObtenerEstadoPersonajeCongelado()
        {
            controlador.obtenerEstadoPersonajeCongelado();

            Assert.That(controlador.EstadoPersonajeCongelado == congelado,
                "Expected: " + congelado + ", Obtained: " + controlador.EstadoPersonajeCongelado);

        }

        [Test]
        public void TestObtenerEstadoPersonajeConfundido()
        {
            controlador.obtenerEstadoPersonajeConfundido();

            Assert.That(controlador.EstadoPersonajeConfundido == confundido,
                "Expected: " + confundido + ", Obtained: " + controlador.EstadoPersonajeConfundido);

        }

        [Test]
        public void TestObtenerEstadoPersonajeParalizado()
        {
            controlador.obtenerEstadoPersonajeParalizado();

            Assert.That(controlador.EstadoPersonajeParalizado == paralizado,
                "Expected: " + paralizado + ", Obtained: " + controlador.EstadoPersonajeParalizado);

        }
    }
}
