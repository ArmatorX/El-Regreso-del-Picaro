using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EstadoPersonajeUnitTests
    {
        [Test]
        public void EstadoPersonaje_esEstadoCongelado_DevuelveVerdaderoSiEsCongelado()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.CONGELADO);

            Assert.IsTrue(estado.esEstadoCongelado());
        }

        [Test]
        public void EstadoPersonaje_esEstadoCongelado_DevuelveFalsoSiNoEsCongelado()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.PARALIZADO);

            Assert.IsFalse(estado.esEstadoCongelado());
        }

        [Test]
        public void EstadoPersonaje_esEstadoConfundido_DevuelveVerdaderoSiEsConfundido()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);

            Assert.IsTrue(estado.esEstadoConfundido());
        }

        [Test]
        public void EstadoPersonaje_esEstadoConfundido_DevuelveFalsoSiNoEsConfundido()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.PARALIZADO);

            Assert.IsFalse(estado.esEstadoConfundido());
        }

        [Test]
        public void EstadoPersonaje_esEstadoParalizado_DevuelveVerdaderoSiEsParalizado()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.PARALIZADO);

            Assert.IsTrue(estado.esEstadoParalizado());
        }

        [Test]
        public void EstadoPersonaje_esEstadoParalizado_DevuelveFalsoSiNoEsParalizado()
        {
            EstadoPersonaje estado = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);

            Assert.IsFalse(estado.esEstadoParalizado());
        }

        [Test]
        public void EstadoPersonaje_Equals_VerificaQueSonIguales()
        {
            EstadoPersonaje estado1 = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);
            EstadoPersonaje estado2 = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);

            Assert.AreEqual(estado1, estado2);
        }

        [Test]
        public void EstadoPersonaje_Equals_VerificaQueSonDistintos()
        {
            EstadoPersonaje estado1 = new EstadoPersonaje(EstadosPersonaje.CONFUNDIDO);
            EstadoPersonaje estado2 = new EstadoPersonaje(EstadosPersonaje.CEGADO);

            Assert.AreNotEqual(estado1, estado2);
        }
    }
}
