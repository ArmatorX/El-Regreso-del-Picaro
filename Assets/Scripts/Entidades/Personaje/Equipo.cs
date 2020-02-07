using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipo
{
    private Arma armaEquipada;
    private Armadura armaduraEquipada;
    private Anillo anillo1Equipado;
    private Anillo anillo2Equipado;
    /*
    public Equipo(Arma arma, Armadura armadura, Anillo anillo1, Anillo anillo2)
    {
        this.armaEquipada = arma;
        this.armaduraEquipada = armadura;
        this.anillo1Equipado = anillo1;
        this.anillo2Equipado = anillo2;
    }
    */
    public Arma ArmaEquipada { get => armaEquipada; set => armaEquipada = value; }
    public Armadura ArmaduraEquipada { get => armaduraEquipada; set => armaduraEquipada = value; }
    public Anillo Anillo1Equipado { get => anillo1Equipado; set => anillo1Equipado = value; }
    public Anillo Anillo2Equipado { get => anillo2Equipado; set => anillo2Equipado = value; }

    public void equiparObjeto()
    {
        throw new NotImplementedException();
    }

    public int obtenerModificadoresEquipoParaFuerza()
    {
        int modificador = 0;

        if (hayArmaEquipada())
        {
            if (armaEquipadaEsCuerpoACuerpo())
            {
                modificador += obtenerModificadorArmaEquipada();
            }
        }

        modificador += obtenerModificadorFuerzaAnillosEquipados();

        return  modificador;
    }

    public bool armaEquipadaEsCuerpoACuerpo()
    {
        return ArmaEquipada.esArmaCuerpoACuerpo();
    }

    public int obtenerModificadorArmaEquipada()
    {
        if (hayArmaEquipada())
        {
            return ArmaEquipada.ModificadorActual;
        }

        return 0;
    }

    public bool hayArmaEquipada()
    {
        return ArmaEquipada != null;
    }

    public int obtenerModificadorFuerzaAnillosEquipados()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            modificador += Anillo1Equipado.ModificadorFuerza;
        }

        if (hayAnillo2Equipado())
        {
            modificador += Anillo2Equipado.ModificadorFuerza;
        }

        return modificador;
    }
    public bool hayAnillo1Equipado()
    {
        return Anillo1Equipado != null;
    }
    public bool hayAnillo2Equipado()
    {
        return Anillo2Equipado != null;
    }

    public bool armaEquipadaEstáVorpalizada()
    {
        return ArmaEquipada.EsArmaVorpalizada;
    }

    public int calcularDañoBase(int cantidadDados)
    {
        // No controlo si hay un arma equipada porque no se calcula
        // el daño sin calcular el impacto antes.
        return ArmaEquipada.calcularDañoBase(cantidadDados);
    }
}
