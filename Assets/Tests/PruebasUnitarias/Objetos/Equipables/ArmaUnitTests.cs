using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArmaUnitTests
    {
        Arma arma;

        [SetUp]
        public void SetUp()
        {
            arma = new ArmaPruebas();
            arma.DadoDaño = new Dado(1);
        }

        [Test]
        public void Arma_calcularDañoBase_LlamaMétodoDado()
        {
            Assert.AreEqual(1, arma.calcularDañoBase(1));
        }
    }

    public class ArmaPruebas : Arma
    {
        public override bool esArmaCuerpoACuerpo()
        {
            return true;
        }
    }
}
