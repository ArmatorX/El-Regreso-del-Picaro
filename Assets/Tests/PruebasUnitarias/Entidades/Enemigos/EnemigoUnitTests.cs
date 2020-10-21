using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EnemigoUnitTests
    {
        Enemigo enemigo;
        //Personaje personaje;

        [SetUp]
        public void SetUp()
        {
            GameObject enemigoGO = new GameObject();
            enemigo = enemigoGO.AddComponent<EnemigoPruebas>();
            enemigo.transform.position = Vector3.zero;
            /*
            enemigo.Maniquí = new GameObject("Maniquí");
            enemigo.Maniquí.tag = "Enemigo";
            enemigo.Maniquí.transform.localScale = new Vector3(6.25f, 6.25f, 1);
            enemigo.Maniquí.transform.position = enemigo.transform.position;
            BoxCollider2D hitbox = enemigo.Maniquí.AddComponent<BoxCollider2D>();
            hitbox.size = new Vector2(0.16f, 0.16f);
            */
            enemigo.Fuerza = 11;
            enemigo.Destreza = 14;
            enemigo.Magia = 15;
            enemigo.Defensa = 10;
            enemigo.DadoDañoAtaqueBase = new Dado(6);
            enemigo.CantidadDadosDañoAtaqueBase = 2;

            enemigo.RB = enemigoGO.AddComponent<Rigidbody2D>();
            //enemigo.crearManiquí();

            enemigo.VidaMáxima = 20;
            enemigo.VidaActual = enemigo.VidaMáxima;

            enemigo.Estados = new List<EstadoEnemigo>();
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.NORMAL));
            /*
            personaje = new GameObject().AddComponent<Personaje>();
            personaje.transform.position = Vector3.right;

            EstadísticasNivel nivel1 = new EstadísticasNivel(15, 14, 13, 8, 10, 20, 1);
            personaje.NivelActual = new Nivel(1, 0, 100, nivel1);

            personaje.VidaActual = nivel1.VidaMáxima;
            */

            Random.InitState(44);

            // Los primeros 10 valores de la seed 44 son:
            // Range(1, 21) = {11, 20, 16, 19, 4, 7, 20, 18, 9, 2, ...}
        }

        [Test]
        public void Enemigo_estáEnEstado_DevuelveTrueParaNormal()
        {
            Assert.IsTrue(enemigo.estáEnEstado(EstadosEnemigo.NORMAL));
        }

        [Test]
        public void Enemigo_estáEnEstado_DevuelveFalseParaMuerto()
        {
            Assert.IsFalse(enemigo.estáEnEstado(EstadosEnemigo.MUERTO));
        }

        [Test]
        public void Enemigo_estáEnEstado_FuncionaConMuchosEstados()
        {
            enemigo.Estados = new List<EstadoEnemigo>();
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.ENVENENADO));
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.CEGADO));
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.EN_LLAMAS));
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.HAMBRIENTO));

            Assert.IsTrue(enemigo.estáEnEstado(EstadosEnemigo.CEGADO));
        }

        [Test]
        public void Enemigo_recibirDaño_SeteaEstadoMuertoSiLlegaACeroVida()
        {
            enemigo.recibirDaño(enemigo.VidaActual);
            EstadoEnemigo muerto = new EstadoEnemigo(EstadosEnemigo.MUERTO);

            Assert.AreEqual(muerto, enemigo.Estados[0]);
        }

        [Test]
        public void Enemigo_recibirDaño_TieneUnSoloEstadoSiLlegaACeroVida()
        {
            enemigo.recibirDaño(enemigo.VidaActual);

            Assert.AreEqual(1, enemigo.Estados.Count);
        }

        /*
         * TODO: Estos son tests de integración.
        [Test]
        public void Enemigo_usarTurno_AtacaSiAdyacenteAlPersonaje()
        {

        }

        [Test]
        public void Enemigo_usarTurno_NoSeMueveSiEstáAdyacenteAlPersonaje()
        {
            enemigo.usarTurno();

            Assert.AreEqual(Vector3.zero, enemigo.transform.position);
        }

        [Test]
        public void Enemigo_usarTurno_SeMueveSiNoEstáAdyacenteAlPersonaje()
        {
            personaje.transform.position = new Vector3(200, 0, 0);
            enemigo.usarTurno();

            Assert.AreEqual(Vector3.up, enemigo.transform.position);
        }
        
        [Test]
        public void Enemigo_atacarCuerpoACuerpo_ElAtaqueNoGolpeaAlPersonaje()
        {
            enemigo.atacarCuerpoACuerpo(0);

            Assert.AreEqual(personaje.VidaActual, personaje.NivelActual.Estadísticas.VidaMáxima);
        }

        [Test]
        public void Enemigo_atacarCuerpoACuerpo_ElAtaqueGolpeaAlPersonaje()
        {
            enemigo.atacarCuerpoACuerpo(20);

            Assert.Less(personaje.VidaActual, personaje.NivelActual.Estadísticas.VidaMáxima);
        }
        */

        public void Enemigo_calcularDañoBase_LlamaMétodoDado()
        {
            Assert.GreaterOrEqual(enemigo.calcularDañoBase(), 0);
        }
        /*
        [Test]
        public void Enemigo_crearManiquí_CreaObjetoDelTipoGameObject()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            Assert.IsNotNull(enemigo.Maniquí);
            Assert.IsInstanceOf<GameObject>(enemigo.Maniquí);
        }

        [Test]
        public void Enemigo_crearManiquí_TieneTagEnemigo()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            string valorEsperado = "Enemigo";
            string valorReal = enemigo.Maniquí.tag;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_crearManiquí_LocalScaleValorCorrecto()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            Vector3 valorEsperado = new Vector3(6.25f, 6.25f, 1);
            Vector3 valorReal = enemigo.Maniquí.transform.localScale;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_crearManiquí_TieneHitBox()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            BoxCollider2D hitbox = enemigo.Maniquí.GetComponent<BoxCollider2D>();

            Assert.IsNotNull(hitbox);
        }

        [Test]
        public void Enemigo_crearManiquí_HitBoxSizeValorCorrecto()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            Vector2 valorEsperado = new Vector2(0.16f, 0.16f);
            Vector2 valorReal = enemigo.Maniquí.GetComponent<BoxCollider2D>().size;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_crearManiquí_PosiciónCorrecta()
        {
            enemigo.Maniquí = null;
            enemigo.crearManiquí();

            Vector3 posiciónEsperada = enemigo.transform.position;
            Vector3 posiciónReal = enemigo.Maniquí.transform.position;

            Assert.AreEqual(posiciónEsperada, posiciónReal);
        }

        [Test]
        public void Enemigo_moverManiquí_MueveElManiquí()
        {
            enemigo.moverManiquí(Vector2.up);

            Vector3 valorEsperado = Vector3.up;
            Vector3 valorReal = enemigo.Maniquí.transform.position;

            Assert.AreEqual(valorEsperado, valorReal);
        }
        */
        [Test]
        public void Enemigo_estoyAdyacenteAlPersonaje_LlamaMétodoControlador()
        {
            Assert.IsInstanceOf<bool>(enemigo.estoyAdyacenteAlPersonaje());
        }

        [Test]
        public void Enemigo_realizarAtaque_LlamaMétodoControlador()
        {
            enemigo.Controlador = new GameObject().AddComponent<ControladorJuego>();
            enemigo.Controlador.Personaje = new GameObject().AddComponent<Personaje>();
            enemigo.Controlador.Personaje.NivelActual = new Nivel(1, 0, 0, new EstadísticasNivel(0, 0, 0, 0, 0, 0, 0));
            enemigo.Controlador.Personaje.EquipoActual = new Equipo();
            enemigo.Controlador.Personaje.Estados = new List<EstadoPersonaje>();
            enemigo.Controlador.Personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.NORMAL));

            Assert.IsInstanceOf<bool>(enemigo.realizarAtaque(1, 1));
        }

        [Test]
        public void Enemigo_obtenerModificadorFuerza_DevuelveValorCorrecto()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Fuerza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerModificadorDestreza_DevuelveValorCorrecto()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Destreza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorDestreza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerModificadorMagia_DevuelveValorCorrecto()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Magia - 10) / 2);
            int valorReal = enemigo.obtenerModificadorMagia();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerDefensa_DevuelveValorCorrecto()
        {
            int valorEsperado = enemigo.Defensa;
            int valorReal = enemigo.obtenerDefensa();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerModificadorDaño_DevuelveValorCorrectoAtaqueMelé()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Fuerza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorDaño(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerModificadorDaño_DevuelveValorCorrectoAtaqueADistancia()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Destreza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorDaño(true);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_esEnemigo_DevuelveTrue()
        {
            Assert.IsTrue(enemigo.esEnemigo());
        }

        [Test]
        public void Enemigo_obtenerModificadorImpacto_DevuelveValorCorrectoAtaqueMelé()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Fuerza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorImpacto(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Enemigo_obtenerModificadorImpacto_DevuelveValorCorrectoAtaqueADistancia()
        {
            int valorEsperado = Mathf.FloorToInt((enemigo.Destreza - 10) / 2);
            int valorReal = enemigo.obtenerModificadorImpacto(true);

            Assert.AreEqual(valorEsperado, valorReal);
        }
    }

    public class EnemigoPruebas : Enemigo
    {
        public override Vector2 elegirDirecciónMovimiento()
        {
            return Vector2.up;
        }
    }
}
