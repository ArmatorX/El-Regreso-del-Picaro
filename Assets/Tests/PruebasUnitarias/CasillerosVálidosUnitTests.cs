using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CasillerosVálidosUnitTests
    {
        
        [Test]
        public void CasillerosVálidos_ObtenerCasillerosVálidos_DevuelveListaNormal()
        {
            Dictionary<Vector2, List<Vector2>> valorEsperado = CasillerosVálidos.CasillerosTamañoNormal;
            Dictionary<Vector2, List<Vector2>> valorObtenido = CasillerosVálidos.ObtenerCasillerosVálidos(TamañoEntidad.NORMAL);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }

        [Test]
        public void CasillerosVálidos_ObtenerCasillerosVálidos_DevuelveListaGrande()
        {
            Dictionary<Vector2, List<Vector2>> valorEsperado = CasillerosVálidos.CasillerosTamañoGrande;
            Dictionary<Vector2, List<Vector2>> valorObtenido = CasillerosVálidos.ObtenerCasillerosVálidos(TamañoEntidad.GRANDE);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }

        [Test]
        public void CasillerosVálidos_ObtenerCasillerosVálidos_DevuelveListaGigante()
        {
            Dictionary<Vector2, List<Vector2>> valorEsperado = CasillerosVálidos.CasillerosTamañoGigante;
            Dictionary<Vector2, List<Vector2>> valorObtenido = CasillerosVálidos.ObtenerCasillerosVálidos(TamañoEntidad.GIGANTE);

            Assert.AreEqual(valorEsperado, valorObtenido);
        }
        
    }
}
