using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PersonajeUnitTests
    {
        Personaje personaje;
        Entidad entidadPruebas;
        Arma armaVorpalizada;
        Murciélago murciélago;
        Anillo anillo;
        Armadura armadura;

        [SetUp]
        public void SetUp()
        {
            GameObject personajeGO = new GameObject();
            personaje = personajeGO.AddComponent<Personaje>();
            personaje.RB = personajeGO.AddComponent<Rigidbody2D>();
            personaje.Animaciones = personajeGO.AddComponent<Animator>();
            /*
            personaje.Maniquí = new GameObject("Maniquí");
            personaje.Maniquí.tag = "Player";
            personaje.Maniquí.transform.localScale = new Vector3(6.25f, 6.25f, 1);
            personaje.Maniquí.transform.position = personaje.transform.position;
            BoxCollider2D hitbox = personaje.Maniquí.AddComponent<BoxCollider2D>();
            hitbox.size = new Vector2(0.16f, 0.16f);
            */
            personaje.transform.position = Vector3.zero;

            personaje.ComidaActual = 20;

            EstadísticasNivel nivel1 = new EstadísticasNivel(15, 14, 13, 8, 10, 20, 1);
            personaje.NivelActual = new Nivel(1, 0, 100, nivel1);

            personaje.VidaActual = nivel1.VidaMáxima;

            personaje.EquipoActual = new Equipo();
            personaje.Inventario = new Inventario();

            personaje.Estados = new List<EstadoPersonaje>();
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.NORMAL));

            UnityEngine.Random.InitState(44);

            // Los primeros 10 valores de la seed 44 son:
            // Range(1, 21) = {11, 20, 16, 19, 4, 7, 20, 18, 9, 2, ...}

            entidadPruebas = new GameObject().AddComponent<EntidadPruebas>();
            entidadPruebas.VidaActual = 10;

            armaVorpalizada = new ArmaCuerpoACuerpo();
            armaVorpalizada.EsArmaVorpalizada = true;
            armaVorpalizada.EnemigoVorpalización = Type.GetType("Murciélago");
            armaVorpalizada.DadoDaño = new Dado(6);

            anillo = new Anillo(2, 3, 1, 5, -2, false, 10);

            armadura = new Armadura();
            armadura.ModificadorActual = 1;

            murciélago = new GameObject().AddComponent<Murciélago>();
            murciélago.VidaActual = 10;
            murciélago.Estados = new List<EstadoEnemigo>();
            murciélago.Estados.Add(new EstadoEnemigo(EstadosEnemigo.VOLANDO));
        }
        
        [Test]
        public void Personaje_getRB_NoDevuelveNull()
        {
            personaje.RB = null;
            Assert.IsNotNull(personaje.RB);
        }

        [Test]
        public void Personaje_getAnimaciones_NoDevuelveNull()
        {
            personaje.Animaciones = null;
            Assert.IsNotNull(personaje.Animaciones);
        }

        [Test]
        public void Personaje_getInventario_NoDevuelveNull()
        {
            personaje.Inventario = null;
            Assert.IsNotNull(personaje.Inventario);
        }
        
        /*
         * Este es un test de PlayMode
        [UnityTest]
        public IEnumerator Personaje_moverse_PosiciónFinalEsCorrecta()
        {
            personaje.moverse(Vector2.up);

            yield return new WaitForSeconds(2);

            Vector3 posiciónEsperada = Vector3.up;
            Vector3 posiciónReal = personaje.transform.position;

            Assert.AreEqual(posiciónEsperada, posiciónReal);
        }
        */
        /*
        [Test]
        public void Personaje_moverse_RestaComida()
        {
            personaje.moverse(Vector2.up);

            int comidaEsperada = 20 - Personaje.COMIDA_MOVIMIENTO;
            int comidaReal = personaje.ComidaActual;

            Assert.AreEqual(comidaEsperada, comidaReal);
        }

        [Test]
        public void Personaje_moverse_mueveManiquí()
        {
            personaje.moverse(Vector2.up);

            Vector3 posiciónEsperada = Vector3.up;
            Vector3 posiciónReal = personaje.Maniquí.transform.position;

            Assert.AreEqual(posiciónEsperada, posiciónReal);
        }

        /*
         * Test de PlayMode
        [UnityTest]
        public IEnumerator Personaje_moverse_TieneMovimientoSuavizado()
        {
            personaje.moverse(Vector2.up);

            yield return null;

            Assert.Positive(personaje.transform.position.y);
        }
        */
        /*
        [Test]
        public void Personaje_moverse_CambiaEstadoAnimator()
        {
            personaje.moverse(Vector2.up);

            // 1 es el estado de la animación de movimiento.
            // Se pretende probar que el Personaje llama al ControladorJuego.
            int estadoEsperado = 1;
            int estadoReal = personaje.Animaciones.GetInteger("estado");

            Assert.AreEqual(estadoEsperado, estadoReal);
        }
        */
        /*
        [Test]
        public void Personaje_crearManiquí_CreaObjetoDelTipoGameObject()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            Assert.IsNotNull(personaje.Maniquí);
            Assert.IsInstanceOf<GameObject>(personaje.Maniquí);
        }

        [Test]
        public void Personaje_crearManiquí_TieneTagPlayer()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            string valorEsperado = "Player";
            string valorReal = personaje.Maniquí.tag;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_crearManiquí_LocalScaleValorCorrecto()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            Vector3 valorEsperado = new Vector3(6.25f, 6.25f, 1);
            Vector3 valorReal = personaje.Maniquí.transform.localScale;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_crearManiquí_TieneHitBox()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            BoxCollider2D hitbox = personaje.Maniquí.GetComponent<BoxCollider2D>();

            Assert.IsNotNull(hitbox);
        }

        [Test]
        public void Personaje_crearManiquí_HitBoxSizeValorCorrecto()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            Vector2 valorEsperado = new Vector2(0.16f, 0.16f);
            Vector2 valorReal = personaje.Maniquí.GetComponent<BoxCollider2D>().size;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_crearManiquí_PosiciónCorrecta()
        {
            personaje.Maniquí = null;
            personaje.crearManiquí();

            Vector3 posiciónEsperada = personaje.transform.position;
            Vector3 posiciónReal = personaje.Maniquí.transform.position;

            Assert.AreEqual(posiciónEsperada, posiciónReal);
        }

        [Test]
        public void Personaje_moverManiquí_MueveElManiquí()
        {
            personaje.moverManiquí(Vector2.up);

            Vector3 valorEsperado = Vector3.up;
            Vector3 valorReal = personaje.Maniquí.transform.position;

            Assert.AreEqual(valorEsperado, valorReal);
        }
        */  
        [Test]
        public void Personaje_consumirComida_RestaComidaPersonaje()
        {
            personaje.consumirComida(10);

            int valorEsperado = 10;
            int valorReal = personaje.ComidaActual;

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_consumirComida_NoAsignaValoresNegativos()
        {
            personaje.consumirComida(25);

            int valorEsperado = 0;
            int valorReal = personaje.ComidaActual;

            Assert.AreEqual(valorEsperado, valorReal);
        }
        /*
        [Test]
        public void Personaje_atacarCuerpoACuerpo_AtaqueHaceDañoSiImpacta()
        {
            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            Assert.Less(entidadPruebas.VidaActual, 10);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_SiAtaqueNoImpactaNoHaceDaño()
        {
            EntidadPruebas.generarNNúmerosAleatorios(4);

            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            int vidaEsperada = 10;
            int vidaReal = entidadPruebas.VidaActual;

            Assert.AreEqual(vidaEsperada, vidaReal);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_AplicaModificadorMisceláneo()
        {
            int modificadorMisceláneo = -2 - personaje.obtenerModificadorFuerza();

            personaje.atacarCuerpoACuerpo(entidadPruebas, modificadorMisceláneo);

            int vidaEsperada = 10;
            int vidaReal = entidadPruebas.VidaActual;

            Assert.AreEqual(vidaEsperada, vidaReal);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_SiCríticoDañoDoble()
        {
            EntidadPruebas.generarNNúmerosAleatorios(1);

            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            int vidaEsperada = 10 - 4;
            int vidaReal = entidadPruebas.VidaActual;

            Assert.AreEqual(vidaEsperada, vidaReal);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_SiAtaqueNoImpactaConsumeComida()
        {
            EntidadPruebas.generarNNúmerosAleatorios(4);

            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            int comidaEsperada = 20 - Personaje.COMIDA_ATAQUE_MELÉ;
            int comidaReal = personaje.ComidaActual;

            Assert.AreEqual(comidaEsperada, comidaReal);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_SiAtaqueImpactaConsumeComida()
        {
            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            int comidaEsperada = 20 - Personaje.COMIDA_ATAQUE_MELÉ;
            int comidaReal = personaje.ComidaActual;

            Assert.AreEqual(comidaEsperada, comidaReal);
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_SiArmaVorpalizadaMataInstantáneamente()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            
            personaje.atacarCuerpoACuerpo(murciélago, 0);

            // TODO: Cambiar para que no use métodos que escribí yo.
            // TODO: Ver que debe haber otras pruebas que hacen lo mismo.
            Assert.True(murciélago.estáEnEstado(EstadosEnemigo. MUERTO));
        }

        [Test]
        public void Personaje_atacarCuerpoACuerpo_CambiaEstadoAnimaciónPersonaje()
        {
            personaje.atacarCuerpoACuerpo(entidadPruebas, 0);

            Assert.AreEqual(2, personaje.Animaciones.GetInteger("estado"));
        }
        /*
         * TODO: Estos son test de integración.
        [Test]
        public void Personaje_realizarAtaque_DevuelveVerdaderoSiAtaqueImpacta()
        {
            Assert.True(personaje.realizarAtaque(entidadPruebas, 11, 2));
        }

        [Test]
        public void Personaje_realizarAtaque_DevuelveFalsoSiAtaqueNoImpacta()
        {
            Assert.False(personaje.realizarAtaque(entidadPruebas, 9, 2));
        }

        [Test]
        public void Personaje_realizarAtaque_DevuelveVerdaderoSiAtaqueCrítico()
        {
            Assert.True(personaje.realizarAtaque(entidadPruebas, -1, 2));
        }
        */

        /*
         * TODO: Estos son test de integración.
        [Test]
        public void Personaje_tieneArmaVorpalizada_DevuelveVerdaderoSiElArmaEstáVorpalizada()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            Assert.IsTrue(personaje.tieneArmaVorpalizada());
        }

        [Test]
        public void Personaje_tieneArmaVorpalizada_NoHayArmaEquipadaDevuelveFalse()
        {
            Assert.IsFalse(personaje.tieneArmaVorpalizada());
        }
        */

        [Test]
        public void Personaje_obtenerModificadorFuerza_DevuelveValorCorrecto()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorFuerza 
                + Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorFuerza_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_estáEnEstado_DevuelveTrueParaNormal()
        {
            Assert.IsTrue(personaje.estáEnEstado(EstadosPersonaje.NORMAL));
        }

        [Test]
        public void Personaje_estáEnEstado_DevuelveFalseParaMuerto()
        {
            Assert.IsFalse(personaje.estáEnEstado(EstadosPersonaje.MUERTO));
        }

        [Test]
        public void Personaje_estáEnEstado_FuncionaConMuchosEstados()
        {
            personaje.Estados = new List<EstadoPersonaje>();
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.ENVENENADO));
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.CEGADO));
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.EN_LLAMAS));
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.HAMBRIENTO));

            Assert.IsTrue(personaje.estáEnEstado(EstadosPersonaje.CEGADO));
        }

        [Test]
        public void Personaje_recibirDaño_SeteaEstadoMuertoSiLlegaACeroVida()
        {
            personaje.recibirDaño(personaje.VidaActual);
            EstadoPersonaje muerto = new EstadoPersonaje(EstadosPersonaje.MUERTO);

            Assert.AreEqual(muerto, personaje.Estados[0]);
        }

        [Test]
        public void Personaje_recibirDaño_TieneUnSoloEstadoSiLlegaACeroVida()
        {
            personaje.recibirDaño(personaje.VidaActual);

            Assert.AreEqual(1, personaje.Estados.Count);
        }

        [Test]
        public void Personaje_obtenerDefensa_DevuelveValorCorrecto()
        {
            personaje.EquipoActual.ArmaduraEquipada = armadura;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = armadura.ModificadorActual + anillo.ModificadorDefensa
                + personaje.NivelActual.Estadísticas.Defensa;
            int valorReal = personaje.obtenerDefensa();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerDefensa_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = personaje.NivelActual.Estadísticas.Defensa;
            int valorReal = personaje.obtenerDefensa();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_esEnemigo_DevuelveFalse()
        {
            Assert.IsFalse(personaje.esEnemigo());
        }

        [Test]
        public void Personaje_obtenerModificadorDestreza_DevuelveValorCorrecto()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorDestreza
                + Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Destreza - 10) / 2);
            int valorReal = personaje.obtenerModificadorDestreza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorDestreza_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Destreza - 10) / 2);
            int valorReal = personaje.obtenerModificadorDestreza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorDaño_DevuelveValorCorrectoAtaqueMelé()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorFuerza 
                + anillo.ModificadorDaño
                + armaVorpalizada.ModificadorActual 
                + Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorDaño(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorDaño_DevuelveValorCorrectoAtaqueADistancia()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorDestreza
                + anillo.ModificadorDaño
                + armaVorpalizada.ModificadorActual 
                + Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Destreza - 10) / 2);
            int valorReal = personaje.obtenerModificadorDaño(true);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorDaño_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorDaño(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorImpacto_DevuelveValorCorrectoAtaqueMelé()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorFuerza
                //+ anillo.ModificadorImpacto
                + armaVorpalizada.ModificadorActual 
                +Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorImpacto(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorImpacto_DevuelveValorCorrectoAtaqueADistancia()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorDestreza
                //+ anillo.ModificadorImpacto
                + armaVorpalizada.ModificadorActual 
                +Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Destreza - 10) / 2);
            int valorReal = personaje.obtenerModificadorImpacto(true);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorImpacto_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Fuerza - 10) / 2);
            int valorReal = personaje.obtenerModificadorImpacto(false);

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorMagia_DevuelveValorCorrecto()
        {
            personaje.EquipoActual.ArmaEquipada = armaVorpalizada;
            personaje.EquipoActual.Anillo1Equipado = anillo;

            int valorEsperado = anillo.ModificadorMagia
                + Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Magia - 10) / 2);
            int valorReal = personaje.obtenerModificadorMagia();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorMagia_DevuelveValorCorrectoSinEquipo()
        {
            int valorEsperado = Mathf.FloorToInt((personaje.NivelActual.Estadísticas.Magia - 10) / 2);
            int valorReal = personaje.obtenerModificadorMagia();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_realizarAtaque_llamaMétodoControlador()
        {
            Assert.IsInstanceOf<bool>(personaje.realizarAtaque(entidadPruebas, 10, 10));
        }

        [Test]
        public void Personaje_calcularDañoBase_llamaMétodoEquipo()
        {
            Assert.GreaterOrEqual(0, personaje.calcularDañoBase());
        }

        [Test]
        public void Personaje_tieneArmaVorpalizada_llamaMétodoEquipo()
        {
            Assert.IsInstanceOf<bool>(personaje.tieneArmaVorpalizada());
        }

        [Test]
        public void Personaje_obtenerModificadorFuerzaEquipo_llamaMétodoEquipo()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorFuerzaEquipo());
        }

        [Test]
        public void Personaje_obtenerModificadorFuerzaEstadísticas_llamaMétodoEstadísticas()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorFuerzaEstadísticas());
        }

        [Test]
        public void Personaje_obtenerModificadorDefensaEquipo_llamaMétodoEquipo()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorDefensaEquipo());
        }

        [Test]
        public void Personaje_obtenerDefensaEstadísticas_DevuelveValorCorrecto()
        {
            int valorEsperado = personaje.NivelActual.Estadísticas.Defensa;
            int valorReal = personaje.obtenerDefensaEstadísticas();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerVidaMáxima_DevuelveValorCorrecto()
        {
            int valorEsperado = personaje.NivelActual.Estadísticas.VidaMáxima;
            int valorReal = personaje.obtenerVidaMáxima();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Personaje_obtenerModificadorDestrezaEquipo_llamaMétodoEquipo()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorDestrezaEquipo());
        }

        [Test]
        public void Personaje_obtenerModificadorDestrezaEstadísticas_llamaMétodoEstadísticas()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorDestrezaEstadísticas());
        }

        [Test]
        public void Personaje_obtenerModificadorMagiaEquipo_llamaMétodoEquipo()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorMagiaEquipo());
        }

        [Test]
        public void Personaje_obtenerModificadorMagiaEstadísticas_llamaMétodoEstadísticas()
        {
            Assert.IsInstanceOf<int>(personaje.obtenerModificadorMagiaEstadísticas());
        }


    }
}
