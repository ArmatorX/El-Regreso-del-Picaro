using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EquipoUnitTests
    {
        Equipo equipo;
        Armadura armadura;
        Anillo anillo1;
        Anillo anillo2;
        Anillo anillo3;
        ArmaCuerpoACuerpo arma;

        [SetUp]
        public void SetUp()
        {
            armadura = new Armadura();
            armadura.ModificadorActual = 1;

            anillo1 = new Anillo(2, 3, 1, 5, -2, false, 10);
            anillo2 = new Anillo(-2, -1, 3, 1, 0, true, 10);
            anillo3 = new Anillo(-1, 0, 0, 0, 0, true, 20);

            arma = new ArmaCuerpoACuerpo();
            arma.ModificadorActual = 3;
            arma.DadoDaño = new Dado(6);

            UnityEngine.Random.InitState(42);
            // Range(1, 7) = {2, 1, 6, 2, 3, 1, 6, 3, 3, 5, ...}

            equipo = new Equipo();
        }

        [Test]
        public void Equipo_equiparObjeto_EquipaAnillo()
        {
            equipo.equiparObjeto(anillo1);

            Assert.AreEqual(anillo1, equipo.Anillo1Equipado);
        }

        [Test]
        public void Equipo_equiparObjeto_EquipaDosAnillos()
        {
            equipo.equiparObjeto(anillo1);
            equipo.equiparObjeto(anillo2);

            Assert.AreEqual(anillo2, equipo.Anillo2Equipado);
        }

        [Test]
        public void Equipo_equiparObjeto_EquipaArma()
        {
            equipo.equiparObjeto(arma);

            Assert.AreEqual(arma, equipo.ArmaEquipada);
        }

        [Test]
        public void Equipo_equiparObjeto_EquipaArmadura()
        {
            equipo.equiparObjeto(armadura);

            Assert.AreEqual(armadura, equipo.ArmaduraEquipada);
        }

        [Test]
        public void Equipo_obtenerModificadorFuerza_DevuelveValorCorrecto()
        {
            equipo.ArmaduraEquipada = armadura;
            equipo.Anillo1Equipado = anillo1;
            equipo.ArmaEquipada = arma;

            int valorEsperado = anillo1.ModificadorFuerza;
            int valorReal = equipo.obtenerModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorFuerza_DevuelveCeroSinEquipo()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorFuerza();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        
        [Test]
        public void Equipo_armaEquipadaEsCuerpoACuerpo_LlamaMétodoArma()
        {
            equipo.ArmaEquipada = arma;

            Assert.IsInstanceOf<bool>(equipo.armaEquipadaEsCuerpoACuerpo());
        }

        [Test]
        public void Equipo_obtenerModificadorArma_DevuelveValorCorrecto()
        {
            equipo.ArmaEquipada = arma;

            int valorEsperado = 3;
            int valorReal = equipo.obtenerModificadorArma();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorArma_DevuelveCeroSinArma()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorArma();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_hayArmaEquipada_DevuelveVerdaderoSiEquipada()
        {
            equipo.ArmaEquipada = arma;

            Assert.IsTrue(equipo.hayArmaEquipada());
        }

        [Test]
        public void Equipo_hayArmaEquipada_DevuelveFalsoSiNoEstáEquipada()
        {
            Assert.IsFalse(equipo.hayArmaEquipada());
        }

        [Test]
        public void Equipo_obtenerModificadorFuerzaAnillos_DevuelveValorCorrectoDosAnillos()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int valorEsperado = anillo1.ModificadorFuerza + anillo2.ModificadorFuerza;
            int valorReal = equipo.obtenerModificadorFuerzaAnillos();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorFuerzaAnillos_DevuelveValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;

            int valorEsperado = anillo1.ModificadorFuerza;
            int valorReal = equipo.obtenerModificadorFuerzaAnillos();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorFuerzaAnillos_DevuelveCeroSinAnillos()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorFuerzaAnillos();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_hayAnillo1Equipado_DevuelveVerdaderoSiEquipado()
        {
            equipo.Anillo1Equipado = anillo1;

            Assert.IsTrue(equipo.hayAnillo1Equipado());
        }

        [Test]
        public void Equipo_hayAnillo1Equipado_DevuelveFalsoSiNoEstáEquipado()
        {
            Assert.IsFalse(equipo.hayAnillo1Equipado());
        }

        [Test]
        public void Equipo_hayAnillo2Equipado_DevuelveVerdaderoSiEquipado()
        {
            equipo.Anillo2Equipado = anillo2;

            Assert.IsTrue(equipo.hayAnillo2Equipado());
        }

        [Test]
        public void Equipo_hayAnillo2Equipado_DevuelveFalsoSiNoEstáEquipado()
        {
            Assert.IsFalse(equipo.hayAnillo2Equipado());
        }

        [Test]
        public void Equipo_armaEstáVorpalizada_DevuelveVerdaderoSiArmaVorpalizada()
        {
            equipo.ArmaEquipada = new ArmaCuerpoACuerpo();
            equipo.ArmaEquipada.EsArmaVorpalizada = true;

            Assert.IsTrue(equipo.armaEstáVorpalizada());
        }

        [Test]
        public void Equipo_armaEstáVorpalizada_DevuelveFalsoSiArmaNoVorpalizada()
        {
            equipo.ArmaEquipada = arma;

            Assert.IsFalse(equipo.armaEstáVorpalizada());
        }

        [Test]
        public void Equipo_armaEstáVorpalizada_DevuelveFalsoSiNoHayArmaEquipada()
        {
            Assert.IsFalse(equipo.armaEstáVorpalizada());
        }

        [Test]
        public void Equipo_calcularDañoBase_LlamaMétodoSiArmaEquipada()
        {
            equipo.ArmaEquipada = arma;

            Assert.Greater(equipo.calcularDañoBase(), 0);
        }

        [Test]
        public void Equipo_calcularDañoBase_DevuelveCeroSiNoHayArmaEquipada()
        {
            int valorEsperado = 0;
            int valorReal = equipo.calcularDañoBase();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDefensa_DevuelveValorCorrecto()
        {
            equipo.ArmaduraEquipada = armadura;
            equipo.Anillo1Equipado = anillo1;

            int valorEsperado = 6;
            int valorReal = equipo.obtenerModificadorDefensa();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDefensa_DevuelveCeroSiNoHayEquipo()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorDefensa();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_hayArmaduraEquipada_DevuelveVerdaderoSiEquipada()
        {
            equipo.ArmaduraEquipada = armadura;

            Assert.IsTrue(equipo.hayArmaduraEquipada());
        }

        [Test]
        public void Equipo_hayArmaduraEquipada_DevuelveFalsoSiNoEstáEquipada()
        {
            Assert.IsFalse(equipo.hayArmaduraEquipada());
        }

        [Test]
        public void Equipo_obtenerModificadorArmadura_DevuelveValorCorrecto()
        {
            equipo.ArmaduraEquipada = armadura;

            int valorEsperado = 1;
            int valorReal = equipo.obtenerModificadorArmadura();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorArmadura_DevuelveCeroSiNoHayArmadura()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorArmadura();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test] 
        public void Equipo_obtenerModificadorDefensaAnillos_DevuelveValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int valorEsperado = 6;
            int valorReal = equipo.obtenerModificadorDefensaAnillos();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDefensaAnillos_DevuelveCeroSiNoHayAnillos()
        {
            int valorEsperado = 0;
            int valorReal = equipo.obtenerModificadorDefensaAnillos();

            Assert.AreEqual(valorEsperado, valorReal);
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveVerdaderoArmaduraEquipada()
        {
            equipo.ArmaduraEquipada = armadura;

            Assert.IsTrue(equipo.esObjetoEquipado(armadura));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveVerdaderoAnillo1Equipada()
        {
            equipo.Anillo1Equipado = anillo1;

            Assert.IsTrue(equipo.esObjetoEquipado(anillo1));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveVerdaderoAnillo2Equipada()
        {
            equipo.Anillo2Equipado = anillo2;

            Assert.IsTrue(equipo.esObjetoEquipado(anillo2));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveVerdaderoArmaEquipada()
        {
            equipo.ArmaEquipada = arma;

            Assert.IsTrue(equipo.esObjetoEquipado(arma));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveVerdaderoArmaEquipadaEquipoLleno()
        {
            equipo.ArmaduraEquipada = armadura;
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;
            equipo.ArmaEquipada = arma;

            Assert.IsTrue(equipo.esObjetoEquipado(arma));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveFalsoObjetoNoEquipableEquipoLleno()
        {
            equipo.ArmaduraEquipada = armadura;
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;
            equipo.ArmaEquipada = arma;

            ObjetoAgarrable objeto = new ObjetoAgarrable();

            Assert.IsFalse(equipo.esObjetoEquipado(objeto));
        }

        [Test]
        public void Equipo_esObjetoEquipado_DevuelveFalsoAnilloNoEquipadoEquipoLleno()
        {
            equipo.ArmaduraEquipada = armadura;
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;
            equipo.ArmaEquipada = arma;

            Assert.IsFalse(equipo.esObjetoEquipado(anillo3));
        }

        [Test]
        public void Equipo_esArmaEquipada_DevuelveVerdaderoSiEsArmaEquipada()
        {
            equipo.ArmaEquipada = arma;

            Assert.IsTrue(equipo.esArmaEquipada(arma));
        }

        [Test]
        public void Equipo_esArmaEquipada_DevuelveFalsoSiNoEsArmaEquipada()
        {
            ArmaCuerpoACuerpo armaNueva = new ArmaCuerpoACuerpo();
            equipo.ArmaEquipada = arma;

            Assert.IsFalse(equipo.esArmaEquipada(armaNueva));
        }

        [Test]
        public void Equipo_esArmaEquipada_DevuelveFalsoSiNoHayArmaEquipada()
        {
            Assert.IsFalse(equipo.esArmaEquipada(arma));
        }

        [Test]
        public void Equipo_esArmaduraEquipada_DevuelveVerdaderoSiEsArmaduraEquipada()
        {
            equipo.ArmaduraEquipada = armadura;

            Assert.IsTrue(equipo.esArmaduraEquipada(armadura));
        }

        [Test]
        public void Equipo_esArmaduraEquipada_DevuelveFalsoSiNoEsArmaduraEquipada()
        {
            Armadura armaduraNueva = new Armadura();
            equipo.ArmaduraEquipada = armadura;

            Assert.IsFalse(equipo.esArmaduraEquipada(armaduraNueva));
        }

        [Test]
        public void Equipo_esArmaduraEquipada_DevuelveFalsoSiNoHayArmaduraEquipada()
        {
            Assert.IsFalse(equipo.esArmaduraEquipada(armadura));
        }

        [Test]
        public void Equipo_esAnilloEquipado_DevuelveVerdaderoSiEsAnilloEquipadoEnSlot1Equipada()
        {
            equipo.Anillo1Equipado = anillo1;

            Assert.IsTrue(equipo.esAnilloEquipado(anillo1));
        }

        [Test]
        public void Equipo_esAnilloEquipado_DevuelveVerdaderoSiEsAnilloEquipadoEnSlot2Equipada()
        {
            equipo.Anillo2Equipado = anillo1;

            Assert.IsTrue(equipo.esAnilloEquipado(anillo1));
        }

        [Test]
        public void Equipo_esAnilloEquipado_DevuelveFalsoSiNoEsAnilloEquipado()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            Assert.IsFalse(equipo.esAnilloEquipado(anillo3));
        }

        [Test]
        public void Equipo_esAnilloEquipado_DevuelveFalsoSiNoHayAnillosEquipados()
        {
            Assert.IsFalse(equipo.esAnilloEquipado(anillo1));
        }

        [Test]
        public void Equipo_obtenerModificadorDestreza_DevuelveElValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int modificadorEsperado = anillo1.ModificadorDestreza + anillo2.ModificadorDestreza;
            int modificadorReal = equipo.obtenerModificadorDestreza();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDestreza_UnSoloAnillo()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorDestreza;
            int modificadorReal = equipo.obtenerModificadorDestreza();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDestreza_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorDestreza();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDestrezaAnillos_DevuelveElValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int modificadorEsperado = anillo1.ModificadorDestreza + anillo2.ModificadorDestreza;
            int modificadorReal = equipo.obtenerModificadorDestrezaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDestrezaAnillos_UnSoloAnillo()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorDestreza;
            int modificadorReal = equipo.obtenerModificadorDestrezaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDestrezaAnillos_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorDestrezaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDaño_DevuelveValorCorrecto()
        {
            equipo.ArmaEquipada = arma;
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorDaño + arma.ModificadorActual;
            int modificadorReal = equipo.obtenerModificadorDaño();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDaño_SoloArma()
        {
            equipo.ArmaEquipada = arma;

            int modificadorEsperado = arma.ModificadorActual;
            int modificadorReal = equipo.obtenerModificadorDaño();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDaño_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorDaño();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDañoAnillos_DevuelveValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorDaño;
            int modificadorReal = equipo.obtenerModificadorDañoAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorDañoAnillos_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorDañoAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorImpacto_DevuelveValorCorrecto()
        {
            equipo.ArmaEquipada = arma;
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = arma.ModificadorActual;
            int modificadorReal = equipo.obtenerModificadorImpacto();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorImpacto_SoloArma()
        {
            equipo.ArmaEquipada = arma;

            int modificadorEsperado = arma.ModificadorActual;
            int modificadorReal = equipo.obtenerModificadorImpacto();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorImpacto_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorImpacto();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorImpactoAnillos_DevuelveValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorImpactoAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorImpactoAnillos_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorImpactoAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagia_DevuelveElValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int modificadorEsperado = anillo1.ModificadorMagia + anillo2.ModificadorMagia;
            int modificadorReal = equipo.obtenerModificadorMagia();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagia_UnSoloAnillo()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorMagia;
            int modificadorReal = equipo.obtenerModificadorMagia();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagia_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorMagia();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagiaAnillos_DevuelveElValorCorrecto()
        {
            equipo.Anillo1Equipado = anillo1;
            equipo.Anillo2Equipado = anillo2;

            int modificadorEsperado = anillo1.ModificadorMagia + anillo2.ModificadorMagia;
            int modificadorReal = equipo.obtenerModificadorMagiaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagiaAnillos_UnSoloAnillo()
        {
            equipo.Anillo1Equipado = anillo1;

            int modificadorEsperado = anillo1.ModificadorMagia;
            int modificadorReal = equipo.obtenerModificadorMagiaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }

        [Test]
        public void Equipo_obtenerModificadorMagiaAnillos_DevuelveCeroSiNoHayEquipo()
        {
            int modificadorEsperado = 0;
            int modificadorReal = equipo.obtenerModificadorMagiaAnillos();

            Assert.AreEqual(modificadorEsperado, modificadorReal);
        }
    }
}
