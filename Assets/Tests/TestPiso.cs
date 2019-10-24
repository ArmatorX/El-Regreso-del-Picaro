using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    /**
     * <summary>
     * Prueba la clase Piso.
     * </summary> 
     */
    public class TestPiso
    {
        private Piso co1;
        private Piso cd1;
        private Vector3 dir1;
        private Piso co2;
        private Piso cd2;
        private Vector3 dir2;

        /**
         * <summary>
         * Inicializa los objetos para las pruebas.
         * </summary>
         */
        [SetUp]
        protected void SetUp()
        {
            co1 = new Piso(new Vector3(68, 36));
            cd1 = new Piso(new Vector3(69, 36));
            dir1 = Vector3.right;

            co2 = new Piso(new Vector3(68, 36));
            cd2 = new Piso(new Vector3(69, 36));
            dir2 = Vector3.up;
        }

        /**
         * <summary>
         * Verifica que el método esCasilleroDestino devuelve el resultado correcto.
         * </summary>
         */
        [Test]
        public void TestEsCasilleroDestino()
        {
            Assert.IsTrue(cd1.esCasilleroDestino(co1, dir1));
            Assert.IsFalse(cd2.esCasilleroDestino(co2, dir2));
        }
    }
}
