using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /**
     * <summary>
     * Prueba la clase EstadoPersonaje.
     * </summary> 
     */
    public class TestEstadoPersonaje
    {
        private EstadoPersonaje congelado;
        private EstadoPersonaje confundido;
        private EstadoPersonaje paralizado;

        /**
         * <summary>
         * Inicializa los objetos para las pruebas.
         * </summary>
         */
        [SetUp]
        protected void SetUp()
        {
            congelado = new EstadoPersonaje(EstadosPersonaje.CONGELADO);
            confundido = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);
            paralizado = new EstadoPersonaje(EstadosPersonaje.PARALIZADO);
        }

        /**
         * <summary>
         * Verifica si el método esEstadoCongelado funciona correctamente.
         * </summary>
         */
        [Test]
        public void TestEsEstadoCongelado()
        {
            Assert.IsTrue(congelado.esEstadoCongelado());
            Assert.IsFalse(congelado.esEstadoConfundido());
            Assert.IsFalse(congelado.esEstadoParalizado());
        }

        /**
         * <summary>
         * Verifica si el método esEstadoConfundido funciona correctamente.
         * </summary>
         */
        [Test]
        public void TestEsEstadoConfundido()
        {
            Assert.IsFalse(confundido.esEstadoCongelado());
            Assert.IsTrue(confundido.esEstadoConfundido());
            Assert.IsFalse(confundido.esEstadoParalizado());
        }

        /**
         * <summary>
         * Verifica si el método esEstadoParalizado funciona correctamente.
         * </summary>
         */
        [Test]
        public void TestEsEstadoParalizado()
        {
            Assert.IsFalse(paralizado.esEstadoCongelado());
            Assert.IsFalse(paralizado.esEstadoConfundido());
            Assert.IsTrue(paralizado.esEstadoParalizado());
        }
    }
}
