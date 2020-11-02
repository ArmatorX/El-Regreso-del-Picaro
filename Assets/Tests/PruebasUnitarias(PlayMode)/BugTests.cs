using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class BugTests
    {
        [UnityTest]
        public IEnumerator OverlappedEnemies()
        {
            // Setup
            SceneManager.LoadScene("Bug_OverlappedEnemies");

            // Espero que cargue la escena
            yield return null;

            ControladorJuego controlador = GameObject.Find("ControladorJuego").GetComponent<ControladorJuego>();

            // Espero que se instancien todos los elementos
            yield return null;

            // Hago que sea el turno de los enemigos
            controlador.FaseActual = ControladorJuego.FasesTurno.TURNO_ENEMIGOS;

            // Espero a que terminen las animaciones
            yield return new WaitForSeconds(1);

            // Verifico cuántos enemigos hay en el casillero libre
            Vector3 posicionAControlar = new Vector3(-4, 2);
            Vector3 puntoA = posicionAControlar + new Vector3(0.25f, 0.25f);
            Vector3 puntoB = posicionAControlar + new Vector3(-0.25f, -0.25f);

            List<Collider2D> resultados = new List<Collider2D>();
            ContactFilter2D filtro = new ContactFilter2D();
            filtro.layerMask = 8;

            int cantidadDeEnemigos = Physics2D.OverlapArea(puntoA, puntoB, filtro, resultados);

            // Espero a que pase un ciclo de Unity (para detectar la colisión)
            yield return null;

            // Verifico si el bug sigue ocurriendo
            Assert.AreEqual(1, cantidadDeEnemigos);
        }
    }
}
