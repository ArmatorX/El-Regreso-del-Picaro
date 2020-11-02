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
        GameObject prefabHitboxPersonaje;
        Personaje personaje;
        GameObject prefabMurciélago;
        GameObject prefabHitboxEnemigoNormal;
        Murciélago enemigo;

        GameObject prefabEscalera;
        GameObject escaleraGO;

        [SetUp]
        public void SetUp()
        {
            controlador = new GameObject("ControladorJuego").AddComponent<ControladorJuego>();
            controlador.Enemigos = new List<Enemigo>();
            controlador.ModoTest = true;
            
            pantalla = new GameObject("PantallaJuego").AddComponent<PantallaJuego>();
            controlador.Pantalla = pantalla;
            
            // Instancio el prefab del Personaje
            prefabPersonaje = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Entidades/Personaje.prefab");
            prefabHitboxPersonaje = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Hitboxes/Hitbox_Personaje.prefab");
            GameObject personajeGO = GameObject.Instantiate(prefabPersonaje, new Vector3(69, 420), Quaternion.identity);
            personaje = personajeGO.GetComponent<Personaje>();
            personaje.Hitbox = GameObject.Instantiate(prefabHitboxPersonaje, new Vector3(69, 420), Quaternion.identity);
            personaje.Estados = new List<EstadoPersonaje>();
            controlador.Personaje = personaje;
            
            // Instancio el prefab del Murciélago
            prefabMurciélago = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Entidades/Murciélago.prefab");
            prefabHitboxEnemigoNormal = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Hitboxes/Hitbox_EnemigoTamañoNormal.prefab");
            GameObject enemigoGO = GameObject.Instantiate(prefabMurciélago, new Vector3(-69, 420), Quaternion.identity);
            enemigo = enemigoGO.GetComponent<Murciélago>();
            enemigo.Hitbox = GameObject.Instantiate(prefabHitboxEnemigoNormal, new Vector3(-69, 420), Quaternion.identity);
            enemigo.Estados = new List<EstadoEnemigo>();
            controlador.Enemigos.Add(enemigo);
            
            prefabEscalera = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObjects/Prefabs/Mapa/Escalera.prefab");
            escaleraGO = GameObject.Instantiate(prefabEscalera, new Vector3(60, 42), Quaternion.identity);
        }

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
            {
                GameObject.Destroy(o.gameObject);
            }
        }


        [UnityTest]
        public IEnumerator ControladorJuego_obtenerEnemigoEn_DevuelveEnemigoSiHayEnemigo()
        {
            Enemigo enemigoEsperado = enemigo;
            Enemigo enemigoObtenido = controlador.obtenerEnemigoEn(enemigo.transform.position);
            
            yield return null;
            
            Assert.AreEqual(enemigoEsperado, enemigoObtenido);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_obtenerEnemigoEn_DevuelveNullSiNoHayEnemigo()
        {
            Enemigo enemigoObtenido = controlador.obtenerEnemigoEn(new Vector3(12, 20));

            yield return null;

            Assert.IsNull(enemigoObtenido);
        }        

        [UnityTest]
        public IEnumerator ControladorJuego_hayEnemigoEn_DevuelveVerdaderoSiHayUnEnemigoEnEseCasillero()
        {
            bool hayEnemigo = controlador.hayEnemigoEn(enemigo.transform.position);

            yield return null;

            Assert.IsTrue(hayEnemigo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayEnemigoEn_DevuelveFalsoConElPersonaje()
        {
            bool hayEnemigo = controlador.hayEnemigoEn(personaje.transform.position);

            yield return null;

            Assert.IsFalse(hayEnemigo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayPersonajeEn_DevuelveFalseSiNoHayUnEnemigo()
        {
            bool hayEnemigo = controlador.hayEnemigoEn(new Vector3(1000, 1000));

            yield return null;

            Assert.IsFalse(hayEnemigo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayPersonajeEn_DevuelveVerdaderoSiElPersonajeEstáEnEseCasillero()
        {
            bool hayPersonaje = controlador.hayPersonajeEn(personaje.transform.position);

            yield return null;

            Assert.IsTrue(hayPersonaje);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayPersonajeEn_DevuelveFalseSiNoEstáElPersonaje()
        {
            bool hayPersonaje = controlador.hayPersonajeEn(new Vector3(1000, 1000));

            yield return null;

            Assert.IsFalse(hayPersonaje);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayPersonajeEn_DevuelveFalsoConEnemigos()
        {
            bool hayPersonaje = controlador.hayPersonajeEn(enemigo.transform.position);

            yield return null;

            Assert.IsFalse(hayPersonaje);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayEntidadEn_DevuelveVerdaderoConPersonaje()
        {
            bool hayEntidad = controlador.hayEntidadEn(personaje.transform.position);

            yield return null;

            Assert.IsTrue(hayEntidad);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayEntidadEn_DevuelveVerdaderoConEnemigo()
        {
            bool hayEntidad = controlador.hayEntidadEn(enemigo.transform.position);

            yield return null;

            Assert.IsTrue(hayEntidad);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayEntidadEn_DevuelveFalsoSiNoHayNada()
        {
            bool hayEntidad = controlador.hayEntidadEn(new Vector3(1000, 1000));

            yield return null;

            Assert.IsFalse(hayEntidad);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayObstáculoEn_DevuelveFalsoSiNoHayNada()
        {
            bool hayObstáculo = controlador.hayObstáculoEn(new Vector3(1000, 1000));

            yield return null;

            Assert.IsFalse(hayObstáculo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayObstáculoEn_DevuelveFalsoSiHayEscalera()
        {
            bool hayObstáculo = controlador.hayObstáculoEn(escaleraGO.transform.position);

            yield return null;

            Assert.IsFalse(hayObstáculo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayObstáculoEn_DevuelveVerdaderoSiHayEnemigo()
        {
            bool hayObstáculo = controlador.hayObstáculoEn(enemigo.transform.position);

            yield return null;

            Assert.IsTrue(hayObstáculo);
        }

        [UnityTest]
        public IEnumerator ControladorJuego_hayObstáculoEn_DevuelveVerdaderoSiHayPersonaje()
        {
            bool hayObstáculo = controlador.hayObstáculoEn(personaje.transform.position);

            yield return null;

            Assert.IsTrue(hayObstáculo); ;
        }
    }
}
