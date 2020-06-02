using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EstadísticasNivelUnitTests
    {
        private EstadísticasNivel estadísticas;

        [SetUp]
        public void SetUp()
        {
            estadísticas = new EstadísticasNivel(15, 14, 13, 8, 10, 20, 1);
        }

        [Test]
        public void EstadísticasNivel_calcularModificadorFuerza_devuelveValorCorrecto()
        {
            int valorEsperado = 2;
            int valorReal = estadísticas.calcularModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void EstadísticasNivel_calcularModificadorDestreza_devuelveValorCorrecto()
        {
            int valorEsperado = 2;
            int valorReal = estadísticas.calcularModificadorDestreza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void EstadísticasNivel_calcularModificadorMagia_devuelveValorCorrecto()
        {
            int valorEsperado = -1;
            int valorReal = estadísticas.calcularModificadorMagia();

            Assert.AreEqual(valorEsperado, valorReal);
        }
    }
}
