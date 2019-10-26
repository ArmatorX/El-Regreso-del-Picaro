using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /**
     * <summary>
     * Prueba la clase Personaje.
     * </summary>
     */
    public class TestPersonaje
    {
        // Datos de entrada
        private Personaje personajeNormal;
        private Personaje personajeConfundido;
        private Personaje personajeConfundidoYParalizado;

        // Datos necesarios para la creación de personajes.
        private Piso casillero;
        private int vida;
        private string nombre;
        private int modificadorVidaMáxima;

        private int comidaActual1;
        private int comidaActual2;
        private int comidaActual3;

        private int comidaAConsumir1;
        private int comidaAConsumir2;
        private int comidaAConsumir3;

        private int experienciaActual;

        private List<EstadoPersonaje> estadosPersonaje1;
        private List<EstadoPersonaje> estadosPersonaje2;
        private List<EstadoPersonaje> estadosPersonaje3;

        private EstadoPersonaje confundido;
        private EstadoPersonaje paralizado;
        private EstadoPersonaje normal;

        // Datos de salida
        private string nombreNormal;
        private string nombreConfundido;
        private string nombreParalizado;

        private int cantidadEstadosPersonaje1;
        private int cantidadEstadosPersonaje2;
        private int cantidadEstadosPersonaje3;

        private int comidaP1C1;
        private int comidaP2C1;
        private int comidaP3C1;
        private int comidaP1C2;
        private int comidaP2C2;
        private int comidaP3C2;
        private int comidaP1C3;
        private int comidaP2C3;
        private int comidaP3C3;

        /**
         * <summary>
         * Inicializa los objetos para las pruebas.
         * </summary>
         */
        [SetUp]
        protected void SetUp()
        {
            // Inicializo los datos de entrada.
            nombreNormal = "Normal";
            nombreConfundido = "Confundido";
            nombreParalizado = "Paralizado";

            casillero = new Piso(Vector3.zero);
            vida = 100;
            nombre = "Bonifacia";
            modificadorVidaMáxima = 0;

            comidaActual1 = 100;
            comidaActual2 = 40;
            comidaActual3 = 3;

            comidaAConsumir1 = 10;
            comidaAConsumir2 = 4;
            comidaAConsumir3 = -20;

            experienciaActual = 0;

            confundido = new EstadoPersonaje(nombreConfundido);
            paralizado = new EstadoPersonaje(nombreParalizado);
            normal = new EstadoPersonaje(nombreNormal);

            estadosPersonaje1 = new List<EstadoPersonaje>();
            estadosPersonaje2 = new List<EstadoPersonaje>();
            estadosPersonaje3 = new List<EstadoPersonaje>();

            estadosPersonaje1.Add(normal);
            estadosPersonaje2.Add(confundido);
            estadosPersonaje3.Add(confundido);
            estadosPersonaje3.Add(paralizado);

            // Creo los personajes.
            personajeNormal = new Personaje(casillero, vida, nombre, modificadorVidaMáxima, comidaActual1, experienciaActual, estadosPersonaje1);
            personajeConfundido = new Personaje(casillero, vida, nombre, modificadorVidaMáxima, comidaActual2, experienciaActual, estadosPersonaje2);
            personajeConfundidoYParalizado = new Personaje(casillero, vida, nombre, modificadorVidaMáxima, comidaActual3, experienciaActual, estadosPersonaje3);

            // Inicializo los datos de salida.
            cantidadEstadosPersonaje1 = 1;
            cantidadEstadosPersonaje2 = 1;
            cantidadEstadosPersonaje3 = 2;

            comidaP1C1 = 90;
            comidaP2C1 = 30;
            comidaP3C1 = 0;
            comidaP1C2 = 86;
            comidaP2C2 = 26;
            comidaP3C2 = 0;
            comidaP1C3 = 86;
            comidaP2C3 = 26;
            comidaP3C3 = 0;
        }

        /**
         * <summary>
         * Prueba el método obtenerEstados.
         * </summary>
         */
        [Test]
        public void TestObtenerEstados()
        {
            List<string> estadosPersonaje1 = personajeNormal.obtenerEstados();
            List<string> estadosPersonaje2 = personajeConfundido.obtenerEstados();
            List<string> estadosPersonaje3 = personajeConfundidoYParalizado.obtenerEstados();

            // Verifico que la cantidad de estados sea correcta.
            Assert.That(estadosPersonaje1.Count == cantidadEstadosPersonaje1);
            Assert.That(estadosPersonaje2.Count == cantidadEstadosPersonaje2);
            Assert.That(estadosPersonaje3.Count == cantidadEstadosPersonaje3);

            // Verifico que los estados sean correctos.
            Assert.That(estadosPersonaje1.Contains(nombreNormal));
            Assert.That(estadosPersonaje2.Contains(nombreConfundido));
            Assert.That(estadosPersonaje3.Contains(nombreConfundido));
            Assert.That(estadosPersonaje3.Contains(nombreParalizado));
        }

        /**
         * <summary>
         * Prueba el método consumirComida.
         * </summary>
         */
        [Test]
        public void TestConsumirComida()
        {
            personajeNormal.consumirComida(comidaAConsumir1);
            personajeConfundido.consumirComida(comidaAConsumir1);
            // Intenta consumir más comida de la que tiene actualmente.
            personajeConfundidoYParalizado.consumirComida(comidaAConsumir1);

            Assert.AreEqual(comidaP1C1, personajeNormal.ComidaActual);
            Assert.AreEqual(comidaP2C1, personajeConfundido.ComidaActual);
            Assert.AreEqual(comidaP3C1, personajeConfundidoYParalizado.ComidaActual);

            personajeNormal.consumirComida(comidaAConsumir2);
            personajeConfundido.consumirComida(comidaAConsumir2);
            personajeConfundidoYParalizado.consumirComida(comidaAConsumir2);

            Assert.AreEqual(comidaP1C2, personajeNormal.ComidaActual);
            Assert.AreEqual(comidaP2C2, personajeConfundido.ComidaActual);
            Assert.AreEqual(comidaP3C2, personajeConfundidoYParalizado.ComidaActual);

            // Prueba consumir una cantidad negativa de comida
            personajeNormal.consumirComida(comidaAConsumir3);
            personajeConfundido.consumirComida(comidaAConsumir3);
            personajeConfundidoYParalizado.consumirComida(comidaAConsumir3);

            Assert.AreEqual(comidaP1C3, personajeNormal.ComidaActual);
            Assert.AreEqual(comidaP2C3, personajeConfundido.ComidaActual);
            Assert.AreEqual(comidaP3C3, personajeConfundidoYParalizado.ComidaActual);
        }
    }
}
