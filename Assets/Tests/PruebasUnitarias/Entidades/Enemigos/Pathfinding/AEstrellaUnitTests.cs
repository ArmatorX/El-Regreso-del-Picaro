using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AEstrellaUnitTests
    {
        Vector2 nodoInicio;
        Vector2 nodoDestino;

        Dictionary<Vector2, Vector2> tcReconstruirCamino;
        List<Vector2> tcRCRuta;
        Vector2 tcRCNodoFinal;

        AEstrella aEstrella;

        [SetUp]
        public void SetUp()
        {
            nodoInicio = new Vector2(2, 3);
            nodoDestino = new Vector2(23, 13);

            aEstrella = new AEstrella(nodoInicio, nodoDestino);

            // Caso de prueba para reconstruir camino.
            tcReconstruirCamino = new Dictionary<Vector2, Vector2>();

            Vector2 n33 = new Vector2(3, 3);
            Vector2 n24 = new Vector2(2, 4);
            Vector2 n25 = new Vector2(2, 5);
            Vector2 n34 = new Vector2(3, 4);
            Vector2 n43 = new Vector2(4, 3);
            Vector2 n44 = new Vector2(4, 4);
            Vector2 n35 = new Vector2(3, 5);
            Vector2 n45 = new Vector2(4, 5);
            Vector2 n46 = new Vector2(4, 6);
            Vector2 n36 = new Vector2(3, 6);

            tcReconstruirCamino.Add(nodoInicio, Vector2.negativeInfinity);
            tcReconstruirCamino.Add(n33, nodoInicio);
            tcReconstruirCamino.Add(n24, nodoInicio);
            tcReconstruirCamino.Add(n25, n24);
            tcReconstruirCamino.Add(n34, n24);
            tcReconstruirCamino.Add(n43, n33);
            tcReconstruirCamino.Add(n44, n34);
            tcReconstruirCamino.Add(n35, n34);
            tcReconstruirCamino.Add(n45, n35);
            tcReconstruirCamino.Add(n36, n35);
            tcReconstruirCamino.Add(n46, n45);

            tcRCNodoFinal = n36;
            tcRCRuta = new List<Vector2>();

            tcRCRuta.Add(nodoInicio);
            tcRCRuta.Add(n24);
            tcRCRuta.Add(n34);
            tcRCRuta.Add(n35);
            tcRCRuta.Add(tcRCNodoFinal);
        }

        [Test]
        public void AEstrella_heurística_DevuelveValorCorrectoSimple() 
        {
            Vector2 nodoA = Vector2.zero;
            Vector2 nodoB = new Vector2(0, 3);

            float valorEsperado = 3;
            float valorObtenido = AEstrella.heurística(nodoA, nodoB);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }

        [Test]
        public void AEstrella_heurística_DevuelveValorCorrectoComplejo()
        {
            // Se usa el típico triángulo rectángulo 3, 4, 5. Reposicionado en (4, 8).
            Vector2 nodoA = Vector2.zero + new Vector2(4, 8);
            Vector2 nodoB = new Vector2(3, 4) + new Vector2(4, 8);

            float valorEsperado = 5;
            float valorObtenido = AEstrella.heurística(nodoA, nodoB);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }

        [Test]
        public void AEstrella_heurística_DevuelveValorCorrectoCero()
        {
            Vector2 nodoA = Vector2.zero;
            Vector2 nodoB = Vector2.zero;

            float valorEsperado = 0;
            float valorObtenido = AEstrella.heurística(nodoA, nodoB);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }

        [Test]
        public void AEstrella_constructor_CreaLaListaDeNodosAbiertos()
        {
            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructor_CreaLaListaDeNodosCerrados()
        {
            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosCerrados);
        }

        [Test]
        public void AEstrella_constructor_ColocaElNodoInicioEnNodosAbiertos()
        {
            Assert.Contains(nodoInicio, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructor_NodoAnteriorDeNodoInicioEsNegativeInfinity()
        {
            Assert.AreEqual(aEstrella.NodoAnterior[aEstrella.Inicio], Vector2.negativeInfinity);
        }

        [Test]
        public void AEstrella_constructor_CreaDiccionarioPuntajeG()
        {
            Assert.IsInstanceOf<Dictionary<Vector2, float>>(aEstrella.PuntajeG);
        }

        [Test]
        public void AEstrella_constructor_AgregaValorDeNodoInicioAPuntajeG()
        {
            Assert.IsTrue(aEstrella.PuntajeG.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructor_CreaDiccionarioNodoAnterior()
        {
            Assert.IsInstanceOf<Dictionary<Vector2, Vector2>>(aEstrella.NodoAnterior);
        }

        [Test]
        public void AEstrella_constructor_AgregaNodoInicioADiccionarioNodoAnterior()
        {
            Assert.IsTrue(aEstrella.NodoAnterior.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructor_ValorInicialPuntajeGNodoInicioEsCero()
        {
            Assert.AreEqual(aEstrella.PuntajeG[aEstrella.Inicio], 0);
        }

        [Test]
        public void AEstrella_PuntajeF_DevuelveValorCorrecto()
        {
            aEstrella.PuntajeG[aEstrella.Inicio] = 15;

            float valorEsperado = (nodoDestino - nodoInicio).magnitude + 15;
            float valorReal = aEstrella.PuntajeF(nodoInicio);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void AEstrella_reconstruirCamino_ReconstruyeLaRutaCorrectamente()
        {
            aEstrella.NodoAnterior = tcReconstruirCamino;

            List<Vector2> rutaEsperada = tcRCRuta;
            List<Vector2> rutaObtenida = aEstrella.reconstruirCamino(tcRCNodoFinal);

            Assert.AreEqual(rutaEsperada, rutaObtenida);
        }

        [Test]
        public void AEstrella_reconstruirCamino_NodoInicioEsNodoFinal()
        {
            List<Vector2> rutaEsperada = new List<Vector2>();
            rutaEsperada.Add(aEstrella.Inicio);

            List<Vector2> rutaObtenida = aEstrella.reconstruirCamino(aEstrella.Inicio);

            Assert.AreEqual(rutaEsperada, rutaObtenida);
        }
    }
}
