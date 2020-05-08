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
        if (hayArmaEquipada())
        {
            return ArmaEquipada.EsArmaVorpalizada;
        }
        return false;
    }

    public int calcularDañoBase(int cantidadDados)
    {
        if (hayArmaEquipada())
        {
            return ArmaEquipada.calcularDañoBase(cantidadDados);
        }
        return 2;
    }

    public int obtenerModificadoresEquipoParaDefensa()
    {
        int modificador = 0;

        modificador += obtenerModificadorArmaduraEquipada();

        modificador += obtenerModificadorDefensaAnillosEquipados();

        return modificador;
    }

    public bool hayArmaduraEquipada()
    {
        return armaduraEquipada != null;
    }

    public int obtenerModificadorArmaduraEquipada()
    {
        if (hayArmaduraEquipada())
        {
            return ArmaduraEquipada.ModificadorActual;
        }

        return 0;
    }

    public int obtenerModificadorDefensaAnillosEquipados()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            modificador += Anillo1Equipado.ModificadorDefensa;
        }

        if (hayAnillo2Equipado())
        {
            modificador += Anillo2Equipado.ModificadorDefensa;
        }

        return modificador;
    }

    /// <summary>
    /// Verifica si un objeto está equipado.
    /// </summary>
    /// <param name="objetoAgarrable">Objeto para controlar.</param>
    /// <returns>Verdadero si el objeto está equipado</returns>
    public bool esObjetoEquipado(ObjetoAgarrable objetoAgarrable)
    {
        if (objetoAgarrable is Arma)
        {
            return esArmaEquipada((Arma) objetoAgarrable);
        }
        else if (objetoAgarrable is Anillo)
        {
            return esAnilloEquipado((Anillo) objetoAgarrable);
        }
        else if (objetoAgarrable is Armadura)
        {
            return esArmaduraEquipada((Armadura) objetoAgarrable);
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Verifica si el arma es el arma equipada.
    /// </summary>
    /// <param name="arma">Arma a controlar.</param>
    /// <returns>Verdadero si es el arma equipada.</returns>
    public bool esArmaEquipada(Arma arma)
    {
        return arma.Equals(ArmaEquipada);
    }

    /// <summary>
    /// Verifica si la armadura es la equipada.
    /// </summary>
    /// <param name="armadura">Armadura a controlar.</param>
    /// <returns>Verdadero si es la armadura equipada.</returns>
    public bool esArmaduraEquipada(Armadura armadura)
    {
        return armadura.Equals(ArmaduraEquipada);
    }

    /// <summary>
    /// Verifica si el anillo está equipado.
    /// </summary>
    /// <param name="anillo">Anillo a controlar.</param>
    /// <returns>Verdadero si es uno de los anillos equipados.</returns>
    public bool esAnilloEquipado(Anillo anillo)
    {
        return anillo.Equals(Anillo1Equipado) || anillo.Equals(Anillo2Equipado);
    }
}
