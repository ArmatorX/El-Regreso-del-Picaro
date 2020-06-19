using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /// <summary>
    /// Realiza las pruebas unitarias de la clase <c>Dado</c>.
    /// </summary>
    public class DadoUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            Random.InitState(42);

            // Los primeros 10 valores de la seed 42 son:
            // Range(1, 21) = {4, 3, 16, 8, 15, 13, 16, 17, 15, 15, 15, ...}
            // Range(1, 7) = {2, 1, 6, 2, 3, 1, 6, 3, 3, 5, ...}
            // Range(1, 5) = {4, 3, 4, 4, 3, 1, 4, 1, 3, 3, ...}
        }

        [Test]
        public void Dado_tirarDados_ValoresEnRango()
        {
            Dado d20 = new Dado(20);

            int valorMáximo = d20.CantidadCaras;
            int valorMínimo = 1;

            int tirada = d20.tirarDados(1);

            Assert.LessOrEqual(tirada, valorMáximo);
            Assert.GreaterOrEqual(tirada, valorMínimo);
        }

        [Test]
        public void Dado_tirarDados_ValorCorrecto()
        {
            Dado d6 = new Dado(6);

            int tiradaEsperada = 2;
            int tirada = d6.tirarDados(1);

            Assert.AreEqual(tirada, tiradaEsperada);
        }
        
        [Test]
        public void Dado_tirarDados_SumaEsCorrecta()
        {
            Dado d4 = new Dado(4);
            
            int tiradaEsperada = 11;
            int tirada = d4.tirarDados(3);

            Assert.AreEqual(tirada, tiradaEsperada);
        }
    }
}
