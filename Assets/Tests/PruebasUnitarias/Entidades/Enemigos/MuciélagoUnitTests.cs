using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class MuciélagoUnitTests
    {
        Murciélago murciélago;

        [SetUp]
        public void SetUp()
        {
            murciélago = new GameObject().AddComponent<Murciélago>();

            murciélago.transform.position = Vector3.zero;

            murciélago.Controlador = new GameObject().AddComponent<ControladorJuego>();
        }

        [Test]
        public void Murciélago_elegirDirecciónMovimiento_LlamaAlMétodoControlador()
        {
            Assert.IsInstanceOf<Vector2>(murciélago.elegirDirecciónMovimiento());
        }

    }
}
