using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EntidadUnitTests
    {
        private Entidad entidad;

        [SetUp]
        public void SetUp()
        {
            entidad = new GameObject().AddComponent<EntidadPruebas>();
            entidad.VidaActual = 20;
            
            Random.InitState(42);

            // Los primeros 10 valores de la seed 42 son:
            // Range(1, 21) = {4, 3, 16, 8, 15, 13, 16, 17, 15, 15, 15, ...}
        }

        /*
         * TODO: Es un test de integración.
        [Test]
        public void Entidad_getControlador_NoDevuelveNulo()
        {
            Assert.IsNotNull(entidad.Controlador);
        }
        */

        [Test]
        public void Entidad_setVentaja_AplicaBienVentaja()
        {
            entidad.Ventaja = true;

            Assert.IsTrue(entidad.Ventaja);
            Assert.IsFalse(entidad.Desventaja);
        }

        [Test]
        public void Entidad_setDesventaja_AplicaBienDesventaja()
        {
            entidad.Desventaja = true;

            Assert.IsTrue(entidad.Desventaja);
            Assert.IsFalse(entidad.Ventaja);
        }

        [Test]
        public void Entidad_setVentaja_VentajaYDesventajaSeCancelan()
        {
            entidad.Ventaja = true;
            entidad.Desventaja = true;

            Assert.IsFalse(entidad.Ventaja);
            Assert.IsFalse(entidad.Desventaja);
        }

        [Test]
        public void Entidad_setDesventaja_VentajaYDesventajaSeCancelan()
        {
            entidad.Desventaja = true;
            entidad.Ventaja = true;

            Assert.IsFalse(entidad.Ventaja);
            Assert.IsFalse(entidad.Desventaja);
        }

        [Test]
        public void Entidad_calcularImpacto_CalculaBienElImpactoAtaqueMelé()
        {
            int impactoEsperado = 7;
            int impacto = entidad.calcularImpacto(2);

            Assert.AreEqual(impactoEsperado, impacto);
        }
        
        [Test]
        public void Entidad_calcularImpacto_CalculaBienElImpactoAtaqueADistancia()
        {
            int impactoEsperado = 9;
            int impacto = entidad.calcularImpacto(2, true);

            Assert.AreEqual(impactoEsperado, impacto);
        }

        [Test]
        public void Entidad_calcularImpacto_AplicaVentaja()
        {
            entidad.Ventaja = true;

            int impactoEsperado = 7;
            int impacto = entidad.calcularImpacto(2);

            Assert.AreEqual(impactoEsperado, impacto);
        }

        [Test]
        public void Entidad_calcularImpacto_AplicaDesventaja()
        {
            entidad.Desventaja = true;

            int impactoEsperado = 6;
            int impacto = entidad.calcularImpacto(2);

            Assert.AreEqual(impactoEsperado, impacto);
        }

        [Test]
        public void Entidad_calcularImpacto_CríticoDevuelveNegativo()
        {
            Random.InitState(44);

            // Los primeros 10 valores de la seed 44 son:
            // Range(1, 21) = {11, 20, 16, 19, 4, 7, 20, 18, 9, 2, ...}

            EntidadPruebas.generarNNúmerosAleatorios(1);

            int impactoEsperado = -1;
            int impacto = entidad.calcularImpacto(2);

            Assert.AreEqual(impactoEsperado, impacto);
        }

        [Test]
        public void Entidad_recibirAtaque_AtaqueNormalImpactaYRestaVida()
        {
            int daño = 7;
            int impacto = 12;
            int vidaEsperada = entidad.VidaActual - daño;

            entidad.recibirAtaque(impacto, daño);

            Assert.AreEqual(vidaEsperada, entidad.VidaActual);
        }

        [Test]
        public void Entidad_recibirAtaque_AtaqueNormalNoImpacta()
        {
            int daño = 7;
            int impacto = 8;
            int vidaEsperada = entidad.VidaActual;

            entidad.recibirAtaque(impacto, daño);

            Assert.AreEqual(vidaEsperada, entidad.VidaActual);
        }

        [Test]
        public void Entidad_recibirAtaque_AtaqueCríticoImpactaYRestaVida()
        {
            int daño = 14;
            int impacto = -1;
            // Si el ataque es crítico, el método simplemente verifica si impacta.
            // El daño se duplica en el ataque.
            int vidaEsperada = entidad.VidaActual - daño;

            entidad.recibirAtaque(impacto, daño);

            Assert.AreEqual(vidaEsperada, entidad.VidaActual);
        }

        [Test]
        public void Entidad_recibirDaño_RestaVidaCorrectamente()
        {
            int daño = 7;
            int vidaEsperada = entidad.VidaActual - daño;

            entidad.recibirDaño(daño);

            Assert.AreEqual(vidaEsperada, entidad.VidaActual);
        }

        [Test]
        public void Entidad_recibirDaño_NoSeteaVidaNegativa()
        {
            int daño = 22;
            int vidaEsperada = 0;

            entidad.recibirDaño(daño);

            Assert.AreEqual(vidaEsperada, entidad.VidaActual);
        }

        [Test]
        public void Entidad_obtenerModificadorImpacto_AtaqueMeléDevuelveModificadorCorrecto()
        {
            int modificadorEsperado = 1;
            int modificadorReal = entidad.obtenerModificadorImpacto(false);

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Entidad_obtenerModificadorImpacto_AtaqueADistanciaDevuelveModificadorCorrecto()
        {
            int modificadorEsperado = 3;
            int modificadorReal = entidad.obtenerModificadorImpacto(true);

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Entidad_verificarSiAtaqueImpacta_NoImpacta()
        {
            Assert.IsFalse(entidad.verificarSiAtaqueImpacta(9));
        }

        [Test]
        public void Entidad_verificarSiAtaqueImpacta_ImpactaSiSuperaDefensa()
        {
            Assert.IsTrue(entidad.verificarSiAtaqueImpacta(11));
        }

        [Test]
        public void Entidad_verificarSiAtaqueImpacta_ImpactaSiImpactoIgualaDefensa()
        {
            Assert.IsTrue(entidad.verificarSiAtaqueImpacta(10));
        }

        [Test]
        public void Entidad_verificarSiAtaqueImpacta_ImpactaSiCrítico()
        {
            Assert.IsTrue(entidad.verificarSiAtaqueImpacta(-1));
        }
    }

    public class EntidadPruebas : Entidad
    {
        public override bool esEnemigo()
        {
            return false;
        }

        public override void moverse(Vector2 dirección)
        {
            throw new System.NotImplementedException();
        }

        public override int obtenerModificadorFuerza()
        {
            return 1;
        }
        
        public override int obtenerModificadorDestreza()
        {
            return 3;
        }

        /// <summary>
        /// Esta función se usa para descartar números de la seed.
        /// Es decir, para llegar a la posición <c>N + 1</c> de la secuencia de 
        /// la seed de <c>UnityEngine.Random</c>.
        /// </summary>
        /// <param name="cantidad">Cantidad de números a generar.</param>
        public static void generarNNúmerosAleatorios(int cantidad)
        {
            float valor = 0;
            for (int i = 0; i < cantidad; i++)
            {
                valor = Random.value;
            }
        }

        public override int obtenerModificadorImpacto(bool ataqueADistancia = false)
        {
            if (ataqueADistancia)
            {
                return obtenerModificadorDestreza();
            }
            else
            {
                return obtenerModificadorFuerza();
            }
        }

        public override int obtenerModificadorMagia()
        {
            throw new System.NotImplementedException();
        }

        public override int obtenerDefensa()
        {
            return 10;
        }

        public override int obtenerModificadorDaño(bool ataqueADistancia)
        {
            throw new System.NotImplementedException();
        }

        public override int calcularDaño(bool esCrítico = false, bool ataqueADistancia = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
