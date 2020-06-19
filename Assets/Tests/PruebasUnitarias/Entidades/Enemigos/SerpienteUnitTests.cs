using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SerpienteUnitTests
    {
        Serpiente serpiente;

        [SetUp]
        public void SetUp()
        {
            serpiente = new GameObject().AddComponent<Serpiente>();
            serpiente.DirecciónAnterior = Vector2.zero;

            serpiente.Controlador = new GameObject().AddComponent<ControladorJuego>();
            //serpiente.Controlador.D20 = new Dado(20);

            Random.InitState(42);

            // Los primeros 10 valores de la seed 42 son:
            // Range(1, 21) = {4, 3, 16, 8, 15, 13, 16, 17, 15, 15, 15, ...}
            // Range(1, 5) = {4, 3, 4, 4, 3, 1, 4, 1, 3, 3, ...}
            // DirAleatoria = {R, D, R, R, D, U, R, U, D, D, ...}
        }

        [Test]
        public void Serpiente_elegirDirecciónMovimiento_DevuelveUnVector2()
        {
            Assert.IsInstanceOf<Vector2>(serpiente.elegirDirecciónMovimiento());
        }

        [Test]
        public void Serpiente_elegirDirecciónMovimiento_DevuelveUnaDirecciónVálida()
        {
            Vector2 dirección = serpiente.elegirDirecciónMovimiento();

            if (dirección == Vector2.up)
            {
                Assert.Pass();
            } 
            else if (dirección == Vector2.left)
            {
                Assert.Pass();
            }
            else if (dirección == Vector2.down)
            {
                Assert.Pass();
            }
            else if (dirección == Vector2.right)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        /*
        [Test]
        public void Serpiente_elegirDirecciónMovimiento_SiNoSeMovióEsAleatorio()
        {
            Vector2 valorEsperado = Vector2.down;
            Vector2 valorReal = serpiente.elegirDirecciónMovimiento();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Serpiente_elegirDirecciónMovimiento_SiSeMovióPuedeRepetirDirecciónAnterior()
        {
            serpiente.DirecciónAnterior = Vector2.up;

            Vector2 valorEsperado = serpiente.DirecciónAnterior;
            Vector2 valorReal = serpiente.elegirDirecciónMovimiento();

            Assert.AreEqual(valorEsperado, valorReal);
        }
        
        [Test]
        public void Serpiente_elegirDirecciónMovimiento_SiSeMovióPuedeSerAleatorio()
        {
            EntidadPruebas.generarNNúmerosAleatorios(2);
            serpiente.DirecciónAnterior = Vector2.up;

            Vector2 valorEsperado = serpiente.DirecciónAnterior;
            Vector2 valorReal = serpiente.elegirDirecciónMovimiento();

            Assert.AreEqual(valorEsperado, valorReal);
        }
        
        [Test]
        public void Serpiente_elegirDirecciónMovimiento_SiHayObstáculoEnDirecciónAnteriorAleatorio()
        {
            // Creo el obstáculo.
            GameObject pared = new GameObject();
            pared.AddComponent<BoxCollider2D>();
            pared.transform.position = Vector3.up;
            pared.transform.localScale = new Vector3(6.25f, 6.25f, 1);
            pared.GetComponent<BoxCollider2D>().size = new Vector2(0.16f, 0.16f);
            pared.tag = "Mapa";
            pared.layer = 9;

            serpiente.DirecciónAnterior = Vector2.up;

            Vector2 valorEsperado = Vector2.down;
            Vector2 valorReal = serpiente.elegirDirecciónMovimiento();

            Assert.AreEqual(valorEsperado, valorReal);
        }
        */
    }
}
