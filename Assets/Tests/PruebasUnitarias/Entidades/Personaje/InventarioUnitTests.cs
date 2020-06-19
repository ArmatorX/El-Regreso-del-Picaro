using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class InventarioUnitTests
    {
        Inventario inventario;

        [SetUp]
        public void SetUp()
        {
             inventario = new Inventario();
        }

        [Test]
        public void Inventario_getDetalle_NoDevuelveNulo()
        {
            Assert.IsNotNull(inventario.Detalle);
        }
    }
}
