using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArmaCuerpoACuerpoUnitTests
    {
        ArmaCuerpoACuerpo armaCuerpoACuerpo;

        [SetUp]
        public void SetUp()
        {
            armaCuerpoACuerpo = new ArmaCuerpoACuerpo();
        }

        [Test]
        public void ArmaCuerpoACuerpo_esArmaCuerpoACuerpo_DevuelveTrue()
        {
            Assert.IsTrue(armaCuerpoACuerpo.esArmaCuerpoACuerpo());
        }
    }
}
