using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ControladorJuegoUnitTests
    {
        ControladorJuego controlador;
        PantallaJuego pantalla;

        GameObject prefabPersonaje;
        Personaje personaje;
        GameObject prefabMurciélago;
        Murciélago enemigo;

        GameObject prefabEscalera;
        GameObject escaleraGO;

        [SetUp]
        public void SetUp()
        {
            controlador = new GameObject("ControladorJuego").AddComponent<ControladorJuego>();
            pantalla = new GameObject("PantallaJuego").AddComponent<PantallaJuego>();
            controlador.Pantalla = pantalla;

            // Instancio el prefab del Murciélago
            prefabPersonaje = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Entidades/Personaje.prefab");
            GameObject personajeGO = GameObject.Instantiate(prefabPersonaje, new Vector3(69, 420), Quaternion.identity);
            personaje = personajeGO.GetComponent<Personaje>();
            personaje.Estados = new List<EstadoPersonaje>();
            controlador.Personaje = personaje;

            // Instancio el prefab del Murciélago
            prefabMurciélago = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Entidades/Murciélago.prefab");
            GameObject enemigoGO = GameObject.Instantiate(prefabMurciélago, new Vector3(-69, 420), Quaternion.identity);
            enemigo = enemigoGO.GetComponent<Murciélago>();
            enemigo.Estados = new List<EstadoEnemigo>();

            prefabEscalera = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Mapa/Escalera.prefab");
            escaleraGO = GameObject.Instantiate(prefabEscalera, new Vector3(60, 42), Quaternion.identity);
        }

        [Test]
        public void ControladorJuego_getPantalla_NoDevuelveNull()
        {
            controlador.Pantalla = null;
            Assert.IsNotNull(controlador.Pantalla);
        }

        [Test]
        public void ControladorJuego_getPersonaje_NoDevuelveNull()
        {
            controlador.Personaje = null;
            Assert.IsNotNull(controlador.Personaje);
        }

        [Test]
        public void ControladorJuego_getD20_NoDevuelveNull()
        {
            controlador.D20 = null;
            Assert.IsNotNull(controlador.D20);
        }

        [Test]
        public void ControladorJuego_getEscaleraActual_NoDevuelveNull()
        {
            controlador.EscaleraActual = null;
            Assert.IsNotNull(controlador.EscaleraActual);
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_NoTieneDesventaja()
        {
            Assert.IsFalse(controlador.pjAtacaConDesventajaMelé(enemigo));
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_SiPersonajeHambrientoTieneDesventajaContraEnemigo()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.HAMBRIENTO));

            Assert.IsTrue(controlador.pjAtacaConDesventajaMelé(enemigo));
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_SiPersonajeHambrientoTieneDesventajaContraPersonaje()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.HAMBRIENTO));

            Assert.IsTrue(controlador.pjAtacaConDesventajaMelé(personaje));
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_SiPersonajeCegadoTieneDesventajaContraEnemigo()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.CEGADO));

            Assert.IsTrue(controlador.pjAtacaConDesventajaMelé(enemigo));
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_SiPersonajeCegadoNoTieneDesventajaContraPersonaje()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.CEGADO));

            Assert.IsFalse(controlador.pjAtacaConDesventajaMelé(personaje));
        }

        [Test]
        public void ControladorJuego_pjAtacaConDesventajaMelé_SiEnemigoInvisiblePersonajeTieneDesventaja()
        {
            enemigo.Estados.Add(new EstadoEnemigo(EstadosEnemigo.INVISIBLE));

            Assert.IsTrue(controlador.pjAtacaConDesventajaMelé(enemigo));
        }

        [Test]
        public void ControladorJuego_pjAtacaConVentajaMelé_SiPersonajeInvisibleAtacaEnemigoTieneVentaja()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.INVISIBLE));

            Assert.IsTrue(controlador.pjAtacaConVentajaMelé(enemigo));
        }

        [Test]
        public void ControladorJuego_pjAtacaConVentajaMelé_SiPersonajeInvisibleAtacaPersonajeNoTieneVentaja()
        {
            personaje.Estados.Add(new EstadoPersonaje(EstadosPersonaje.INVISIBLE));

            Assert.IsFalse(controlador.pjAtacaConVentajaMelé(personaje));
        }

        [Test]
        public void ControladorJuego_hayEnemigoEn_DevuelveVerdaderoSiHayUnEnemigoEnEseCasillero()
        {
            Assert.IsTrue(controlador.hayEnemigoEn(enemigo.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEnemigoEn_DevuelveFalsoConElPersonaje()
        {
            Assert.IsFalse(controlador.hayEnemigoEn(personaje.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEnemigoEn_DevuelveFalseSiNoHayUnEnemigo()
        {
            Assert.IsFalse(controlador.hayEnemigoEn(new Vector3(1000, 1000)));
        }

        [Test]
        public void ControladorJuego_hayPersonajeEn_DevuelveVerdaderoSiElPersonajeEstáEnEseCasillero()
        {
            Assert.IsTrue(controlador.hayPersonajeEn(personaje.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEnemigoEn_DevuelveFalseSiNoEstáElPersonaje()
        {
            Assert.IsFalse(controlador.hayPersonajeEn(new Vector3(1000, 1000)));
        }

        [Test]
        public void ControladorJuego_hayEnemigoEn_DevuelveFalsoConEnemigos()
        {
            Assert.IsFalse(controlador.hayPersonajeEn(enemigo.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEntidadEn_DevuelveVerdaderoConPersonaje()
        {
            Assert.IsTrue(controlador.hayEntidadEn(personaje.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEntidadEn_DevuelveVerdaderoConEnemigo()
        {
            Assert.IsTrue(controlador.hayEntidadEn(enemigo.transform.position));
        }

        [Test]
        public void ControladorJuego_hayEntidadEn_DevuelveFalsoSiNoHayNada()
        {
            Assert.IsFalse(controlador.hayPersonajeEn(new Vector3(1000, 1000)));
        }

        [Test]
        public void ControladorJuego_hayObstáculoEn_DevuelveFalsoSiNoHayNada()
        {
            Assert.IsFalse(controlador.hayObstáculoEn(new Vector3(1000, 1000)));
        }

        [Test]
        public void ControladorJuego_hayObstáculoEn_DevuelveFalsoSiHayEscalera()
        {
            Assert.IsFalse(controlador.hayObstáculoEn(escaleraGO.transform.position));
        }

        [Test]
        public void ControladorJuego_hayObstáculoEn_DevuelveVerdaderoSiHayEnemigo()
        {
            Assert.IsTrue(controlador.hayObstáculoEn(enemigo.transform.position));
        }

        [Test]
        public void ControladorJuego_hayObstáculoEn_DevuelveVerdaderoSiHayPersonaje()
        {
            Assert.IsTrue(controlador.hayObstáculoEn(personaje.transform.position));
        }

        /*
         * TODO: Revisar si debería hacerlo en Play Mode Tests.
        [Test]
        public void ControladorJuego_hayObstáculoEn_DevuelveVerdaderoSiHayPared()
        {
            Assert.IsTrue(controlador.hayObstáculoEn());
        }
        */

        [Test]
        public void ControladorJuego_obtenerEnemigoEn_DevuelveEnemigoSiHayEnemigo()
        {
            Assert.AreEqual(enemigo, controlador.obtenerEnemigoEn(enemigo.transform.position));
        }

        [Test]
        public void ControladorJuego_obtenerEnemigoEn_DevuelveNullSiNoHayEnemigo()
        {
            Assert.IsNull(controlador.obtenerEnemigoEn(Vector3.zero));
        }

    }
}
