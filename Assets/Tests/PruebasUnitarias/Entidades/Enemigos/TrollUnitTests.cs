using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TrollUnitTests
    {
        Troll troll;

        [SetUp]
        public void SetUp()
        {
            troll = new GameObject().AddComponent<Troll>();
        }

        [Test]
        public void Troll_transformarPosicionesRuta_HaceLaTransformaciónCorrectamente()
        {
            // Creamos la ruta de test.
            List<Vector2> ruta = new List<Vector2>();

            ruta.Add(new Vector2(0, 0));
            ruta.Add(new Vector2(0, 1));
            ruta.Add(new Vector2(1, 1));
            ruta.Add(new Vector2(2, 1));
            ruta.Add(new Vector2(2, 0));
            ruta.Add(new Vector2(2, -1));

            // Creamos la secuencia de movimientos esperada.
            List<Vector2> secuenciaEsperada = new List<Vector2>();

            secuenciaEsperada.Add(Vector2.up);
            secuenciaEsperada.Add(Vector2.right);
            secuenciaEsperada.Add(Vector2.right);
            secuenciaEsperada.Add(Vector2.down);
            secuenciaEsperada.Add(Vector2.down);

            List<Vector2> secuenciaObtenida = troll.transformarPosicionesRuta(ruta);

            Assert.AreEqual(secuenciaEsperada, secuenciaObtenida);
        }

        [Test]
        public void Troll_transformarPosicionesRuta_DevuelveUnaListaVacíaSiRecibeUnaListaVacía()
        {
            List<Vector2> ruta = new List<Vector2>();

            List<Vector2> secuenciaEsperada = new List<Vector2>();
            List<Vector2> secuenciaObtenida = troll.transformarPosicionesRuta(ruta);

            Assert.AreEqual(secuenciaEsperada, secuenciaObtenida);
        }

        [Test]
        public void Troll_transformarPosicionesRuta_LePasoUnSoloCasillero()
        {
            List<Vector2> ruta = new List<Vector2>();

            ruta.Add(new Vector2(0, 0));

            List<Vector2> secuenciaEsperada = new List<Vector2>();
            List<Vector2> secuenciaObtenida = troll.transformarPosicionesRuta(ruta);

            Assert.AreEqual(secuenciaEsperada, secuenciaObtenida);
        }

        [Test]
        public void Troll_transformarPosicionesRuta_LePasoDosCasilleros()
        {
            // Creamos la ruta de test.
            List<Vector2> ruta = new List<Vector2>();

            ruta.Add(new Vector2(0, 0));
            ruta.Add(new Vector2(0, 1));

            // Creamos la secuencia de movimientos esperada.
            List<Vector2> secuenciaEsperada = new List<Vector2>();

            secuenciaEsperada.Add(Vector2.up);

            List<Vector2> secuenciaObtenida = troll.transformarPosicionesRuta(ruta);

            Assert.AreEqual(secuenciaEsperada, secuenciaObtenida);
        }
        /*
        public void Troll_puntajeF_DevuelveValorCorrecto()
        {

            float valorEsperado = ;
            float valorObtenido = puntajeF(x, destino, )
        }
        */
    }
}
