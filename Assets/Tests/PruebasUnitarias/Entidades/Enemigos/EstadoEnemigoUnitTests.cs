using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EstadoEnemigoUnitTests
    {
        [Test]
        public void EstadoEnemigo_Equals_VerificaQueSonIguales()
        {
            EstadoEnemigo estado1 = new EstadoEnemigo(EstadosEnemigo.ENVENENADO);
            EstadoEnemigo estado2 = new EstadoEnemigo(EstadosEnemigo.ENVENENADO);

            Assert.AreEqual(estado1, estado2);
        }

        [Test]
        public void EstadoEnemigo_Equals_VerificaQueSonDistintos()
        {
            EstadoEnemigo estado1 = new EstadoEnemigo(EstadosEnemigo.ENVENENADO);
            EstadoEnemigo estado2 = new EstadoEnemigo(EstadosEnemigo.EN_LLAMAS);

            Assert.AreNotEqual(estado1, estado2);
        }
    }
}
