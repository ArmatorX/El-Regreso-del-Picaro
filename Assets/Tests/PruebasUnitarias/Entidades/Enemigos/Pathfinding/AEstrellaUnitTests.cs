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
        Vector2 nodoPruebas;
        Vector2 nodoPruebasVecino;
        TamañoEntidad tamaño;

        AEstrella aEstrella;

        [SetUp]
        public void SetUp()
        {
            ControladorJuego controlador = new GameObject("ControladorJuego").AddComponent<ControladorJuego>();

            nodoInicio = new Vector2(2, 3);
            nodoDestino = new Vector2(23, 13);

            nodoPruebas = new Vector2(6, 7);
            nodoPruebasVecino = new Vector2(6, 8);

            tamaño = TamañoEntidad.NORMAL;

            aEstrella = new AEstrella(nodoInicio, nodoDestino, tamaño);
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
        public void AEstrella_constructorNormal_CreaLaListaDeNodosAbiertos()
        {
            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructorNormal_ValorTamañoEntidadEsCorrecto()
        {
            Assert.AreEqual(aEstrella.TamañoEntidad, tamaño);
        }

        [Test]
        public void AEstrella_constructorNormal_CreaLaListaDeNodosCerrados()
        {
            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosCerrados);
        }

        [Test]
        public void AEstrella_constructorNormal_ColocaElNodoInicioEnNodosAbiertos()
        {
            Assert.Contains(nodoInicio, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructorNormal_NodoAnteriorDeNodoInicioEsNegativeInfinity()
        {
            Assert.AreEqual(aEstrella.NodoAnterior[aEstrella.Inicio], Vector2.negativeInfinity);
        }

        [Test]
        public void AEstrella_constructorNormal_CreaDiccionarioPuntajeG()
        {
            Assert.IsInstanceOf<Dictionary<Vector2, float>>(aEstrella.PuntajeG);
        }

        [Test]
        public void AEstrella_constructorNormal_AgregaValorDeNodoInicioAPuntajeG()
        {
            Assert.IsTrue(aEstrella.PuntajeG.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructorNormal_CreaDiccionarioNodoAnterior()
        {
            Assert.IsInstanceOf<Dictionary<Vector2, Vector2>>(aEstrella.NodoAnterior);
        }

        [Test]
        public void AEstrella_constructorNormal_AgregaNodoInicioADiccionarioNodoAnterior()
        {
            Assert.IsTrue(aEstrella.NodoAnterior.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructorNormal_ValorInicialPuntajeGNodoInicioEsCero()
        {
            Assert.AreEqual(aEstrella.PuntajeG[aEstrella.Inicio], 0);
        }

        [Test]
        public void AEstrella_constructorSinDestino_ValorTamañoEntidadEsCorrecto()
        {
            Assert.AreEqual(aEstrella.TamañoEntidad, tamaño);
        }

        [Test]
        public void AEstrella_constructorSinDestino_CreaLaListaDeNodosAbiertos()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructorSinDestino_CreaLaListaDeNodosCerrados()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.IsInstanceOf<List<Vector2>>(aEstrella.NodosCerrados);
        }

        [Test]
        public void AEstrella_constructorSinDestino_ColocaElNodoInicioEnNodosAbiertos()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.Contains(nodoInicio, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_constructorSinDestino_NodoAnteriorDeNodoInicioEsNegativeInfinity()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.AreEqual(aEstrella.NodoAnterior[aEstrella.Inicio], Vector2.negativeInfinity);
        }

        [Test]
        public void AEstrella_constructorSinDestino_CreaDiccionarioPuntajeG()
        {
            Assert.IsInstanceOf<Dictionary<Vector2, float>>(aEstrella.PuntajeG);
        }

        [Test]
        public void AEstrella_constructorSinDestino_AgregaValorDeNodoInicioAPuntajeG()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.IsTrue(aEstrella.PuntajeG.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructorSinDestino_CreaDiccionarioNodoAnterior()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.IsInstanceOf<Dictionary<Vector2, Vector2>>(aEstrella.NodoAnterior);
        }

        [Test]
        public void AEstrella_constructorSinDestino_AgregaNodoInicioADiccionarioNodoAnterior()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.IsTrue(aEstrella.NodoAnterior.ContainsKey(nodoInicio));
        }

        [Test]
        public void AEstrella_constructorSinDestino_ValorInicialPuntajeGNodoInicioEsCero()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.AreEqual(aEstrella.PuntajeG[aEstrella.Inicio], 0);
        }

        [Test]
        public void AEstrella_constructorSinDestino_ValorInicioEsInicio()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.AreEqual(aEstrella.Inicio, nodoInicio);
        }

        [Test]
        public void AEstrella_constructorSinDestino_ValorDestinoEsNegativeInfinity()
        {
            aEstrella = new AEstrella(nodoInicio, TamañoEntidad.NORMAL);

            Assert.AreEqual(aEstrella.Destino, Vector2.negativeInfinity);
        }

        [Test]
        public void AEstrella_constructorNormal_ValorInicioEsInicio()
        {
            Assert.AreEqual(aEstrella.Inicio, nodoInicio);
        }

        [Test]
        public void AEstrella_constructorNormal_ValorDestinoEsDestino()
        {
            Assert.AreEqual(aEstrella.Destino, nodoDestino);
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
            Dictionary<Vector2, Vector2> tcReconstruirCamino = new Dictionary<Vector2, Vector2>();

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

            Vector2 tcRCNodoFinal = n36;
            List<Vector2> tcRCRuta = new List<Vector2>();

            tcRCRuta.Add(nodoInicio);
            tcRCRuta.Add(n24);
            tcRCRuta.Add(n34);
            tcRCRuta.Add(n35);
            tcRCRuta.Add(tcRCNodoFinal);

            aEstrella.NodoAnterior = tcReconstruirCamino;

            List<Vector2> rutaEsperada = tcRCRuta;

            aEstrella.Actual = tcRCNodoFinal;
            List<Vector2> rutaObtenida = aEstrella.reconstruirCamino();

            Assert.AreEqual(rutaEsperada, rutaObtenida);
        }

        [Test]
        public void AEstrella_reconstruirCamino_NodoInicioEsNodoFinal()
        {
            List<Vector2> rutaEsperada = new List<Vector2>();
            rutaEsperada.Add(aEstrella.Inicio);

            aEstrella.Actual = aEstrella.Inicio;
            List<Vector2> rutaObtenida = aEstrella.reconstruirCamino();

            Assert.AreEqual(rutaEsperada, rutaObtenida);
        }

        [Test]
        public void AEstrella_esNodoDestino_DevuelveValorCorrecto()
        {
            aEstrella.Actual = nodoDestino;
            Assert.IsTrue(aEstrella.esNodoDestino());
        }

        [Test]
        public void AEstrella_getControlador_NoDevuelveNulo()
        {
            Assert.IsNotNull(aEstrella.Controlador);
        }

        [Test]
        public void AEstrella_transformarDireccionesANodos_DevuelveValorCorrecto()
        {
            List<Vector2> direcciones = new List<Vector2>();

            direcciones.Add(Vector2.up);
            direcciones.Add(Vector2.left);
            direcciones.Add(Vector2.right);

            List<Vector2> posicionesEsperadas = new List<Vector2>();

            posicionesEsperadas.Add(nodoInicio + Vector2.up);
            posicionesEsperadas.Add(nodoInicio + Vector2.left);
            posicionesEsperadas.Add(nodoInicio + Vector2.right);

            aEstrella.Actual = nodoInicio;

            List<Vector2> posicionesObtenidas = aEstrella.transformarDireccionesANodos(direcciones);

            Assert.AreEqual(posicionesEsperadas, posicionesObtenidas);
        }

        [Test]
        public void AEstrella_transformarDireccionesANodos_ListaVacíaDevuelveListaVacía()
        {
            List<Vector2> direcciones = new List<Vector2>();

            aEstrella.Actual = nodoInicio;

            List<Vector2> posicionesObtenidas = aEstrella.transformarDireccionesANodos(direcciones);

            Assert.IsEmpty(posicionesObtenidas);
        }

        [Test]
        public void AEstrella_transformarDireccionesANodos_ListaLlenaDevuelveValorCorrecto()
        {
            List<Vector2> direcciones = new List<Vector2>();

            direcciones.Add(Vector2.up);
            direcciones.Add(Vector2.down);
            direcciones.Add(Vector2.left);
            direcciones.Add(Vector2.right);

            List<Vector2> posicionesEsperadas = new List<Vector2>();

            posicionesEsperadas.Add(nodoInicio + Vector2.up);
            posicionesEsperadas.Add(nodoInicio + Vector2.down);
            posicionesEsperadas.Add(nodoInicio + Vector2.left);
            posicionesEsperadas.Add(nodoInicio + Vector2.right);

            aEstrella.Actual = nodoInicio;

            List<Vector2> posicionesObtenidas = aEstrella.transformarDireccionesANodos(direcciones);

            Assert.AreEqual(posicionesEsperadas, posicionesObtenidas);
        }

        [Test]
        public void AEstrella_transformarDireccionesANodos_ListaSobrecargadaDevuelveValorCorrecto()
        {
            List<Vector2> direcciones = new List<Vector2>();

            direcciones.Add(Vector2.up);
            direcciones.Add(Vector2.down);
            direcciones.Add(Vector2.down);
            direcciones.Add(Vector2.left);
            direcciones.Add(Vector2.right);
            direcciones.Add(Vector2.right);

            List<Vector2> posicionesEsperadas = new List<Vector2>();

            posicionesEsperadas.Add(nodoInicio + Vector2.up);
            posicionesEsperadas.Add(nodoInicio + Vector2.down);
            posicionesEsperadas.Add(nodoInicio + Vector2.down);
            posicionesEsperadas.Add(nodoInicio + Vector2.left);
            posicionesEsperadas.Add(nodoInicio + Vector2.right);
            posicionesEsperadas.Add(nodoInicio + Vector2.right);

            aEstrella.Actual = nodoInicio;

            List<Vector2> posicionesObtenidas = aEstrella.transformarDireccionesANodos(direcciones);

            Assert.AreEqual(posicionesEsperadas, posicionesObtenidas);
        }

        [Test]
        public void AEstrella_abrirNodo_AgregaNodoANodosAbiertos()
        {
            aEstrella.abrirNodo(nodoPruebas);

            Assert.Contains(nodoPruebas, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_abrirNodo_SiCerradoQuitaNodoDeNodosCerrados()
        {
            aEstrella.NodosCerrados.Add(nodoPruebas);
            aEstrella.abrirNodo(nodoPruebas);

            Assert.IsFalse(aEstrella.NodosCerrados.Contains(nodoPruebas));
        }

        [Test]
        public void AEstrella_abrirNodo_SiNodoAbiertoNoHaceNada()
        {
            aEstrella.abrirNodo(nodoInicio);

            Assert.Contains(nodoInicio, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_cerrarNodo_AgregaANodosCerrados()
        {
            aEstrella.cerrarNodo(nodoPruebas);

            Assert.Contains(nodoPruebas, aEstrella.NodosCerrados);
        }


        [Test]
        public void AEstrella_cerrarNodo_SiAbiertoQuitaDeNodosAbiertos()
        {
            aEstrella.NodosAbiertos.Add(nodoPruebas);

            aEstrella.cerrarNodo(nodoPruebas);

            Assert.False(aEstrella.NodosAbiertos.Contains(nodoPruebas));
        }

        [Test]
        public void AEstrella_cerrarNodo_SiCerradoNoHaceNada()
        {
            aEstrella.NodosCerrados.Add(nodoPruebas);

            aEstrella.cerrarNodo(nodoPruebas);

            Assert.Contains(nodoPruebas, aEstrella.NodosCerrados);
        }

        [Test]
        public void AEstrella_obtenerNodoPrioritario_SiPrimerIteraciónDevuelveInicio()
        {
            Assert.AreEqual(aEstrella.Inicio, aEstrella.obtenerNodoPrioritario());
        }

        [Test]
        public void AEstrella_obtenerNodoPrioritario_DevuelveValorCorrecto()
        {
            Vector2 n1 = new Vector2(6, 4); // Puntaje F = 7 + 3.605... = 10.61
            Vector2 n2 = new Vector2(6, 5); // Puntaje F = 7 + 3.162... = 10.16
            Vector2 n3 = new Vector2(7, 5); // Puntaje F = 8 + 2.236... = 10.24
            Vector2 n4 = new Vector2(7, 4); // Puntaje F = 8 + 2.828... = 10.83
            Vector2 destino = new Vector2(9, 6);

            aEstrella.Destino = destino;

            aEstrella.NodosAbiertos = new List<Vector2>();
            aEstrella.NodosAbiertos.Add(n3);
            aEstrella.NodosAbiertos.Add(n4);
            aEstrella.NodosAbiertos.Add(n2);
            aEstrella.NodosAbiertos.Add(n1);

            aEstrella.PuntajeG.Add(n1, 7);
            aEstrella.PuntajeG.Add(n2, 7);
            aEstrella.PuntajeG.Add(n3, 8);
            aEstrella.PuntajeG.Add(n4, 8);

            Vector2 nodoEsperado = n2;
            Vector2 nodoRecibido = aEstrella.obtenerNodoPrioritario();

            Assert.AreEqual(nodoEsperado, nodoRecibido);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NuevoNodoAgregaPuntajeG()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.Contains(nodoPruebasVecino, aEstrella.PuntajeG.Keys);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NuevoNodoAgregaNodoAnterior()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.Contains(nodoPruebasVecino, aEstrella.NodoAnterior.Keys);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NuevoNodoAbreNodo()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.Contains(nodoPruebasVecino, aEstrella.NodosAbiertos);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NodoConMejorGCambiaPuntajeG()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);
            aEstrella.PuntajeG.Add(nodoPruebasVecino, 9);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.AreEqual(8, aEstrella.PuntajeG[nodoPruebasVecino]);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NodoConMejorGCambiaNodoAnterior()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);
            aEstrella.NodoAnterior.Add(nodoPruebasVecino, new Vector2(6, 9));
            aEstrella.PuntajeG.Add(nodoPruebasVecino, 9);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.AreEqual(nodoPruebas, aEstrella.NodoAnterior[nodoPruebasVecino]);
        }

        [Test]
        public void AEstrella_agregarNodoParaExploración_NodoConMejorGAbreNodo()
        {
            aEstrella.Actual = nodoPruebas;

            aEstrella.PuntajeG.Add(nodoPruebas, 7);
            aEstrella.PuntajeG.Add(nodoPruebasVecino, 9);

            aEstrella.agregarNodoParaExploración(nodoPruebasVecino);

            Assert.Contains(nodoPruebasVecino, aEstrella.NodosAbiertos);
        }
    }
}
