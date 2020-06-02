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

    /// <summary>
    /// Equipa un objeto equipable en el primer slot que encuentre disponible.
    /// </summary>
    /// <remarks>
    /// Si se intenta equipar un objeto, y no hay un slot disponible, tira una 
    /// <c>InvalidOperationException</c>.
    /// Si se intenta equipar un objeto que no es equipable, genera una 
    /// <c>InvalidOperationException</c>.
    /// </remarks>
    /// <param name="objeto">Objeto equipable.</param>
    public void equiparObjeto(ObjetoAgarrable objeto)
    {
        if (objeto.GetType() == typeof(Arma))
        {
            if (!hayArmaEquipada())
            {
                ArmaEquipada = (Arma) objeto;
            } 
            else
            {
                throw new InvalidOperationException("Se intentó equipar un arma, pero ya había un arma equipada.");
            }
        }
        else if (objeto.GetType() == typeof(Armadura))
        {
            if (!hayArmaduraEquipada())
            {
                ArmaduraEquipada = (Armadura) objeto;
            }
            else
            {
                throw new InvalidOperationException("Se intentó equipar una armadura, pero ya había una armadura equipada.");
            }
        }
        else if (objeto.GetType() == typeof(Anillo))
        {
            if (!hayAnillo1Equipado())
            {
                Anillo1Equipado = (Anillo) objeto;
            }
            else if (!hayAnillo2Equipado())
            {
                Anillo2Equipado = (Anillo) objeto;
            }
            else
            {
                throw new InvalidOperationException("Se intentó equipar un anillo, pero ambos slots estaban ocupados.");
            }
        } 
        else
        {
            throw new InvalidOperationException("Se intentó equipar un objeto no equipable.");
        }
    }

    /// <summary>
    /// Calcula el modificador de fuerza de todos los objetos equipados.
    /// </summary>
    /// <remarks>Sólo tiene en cuenta los anillos. Los modificadores de la 
    /// espada son de daño e impacto, no aplican a la Fuerza en sí misma.</remarks>
    /// <returns>Modificador de Fuerza.</returns>
    public int obtenerModificadorFuerza()
    {
        int modificador = 0;

        modificador += obtenerModificadorFuerzaAnillos();

        return  modificador;
    }

    /// <summary>
    /// Verifica si el arma equipada es un arma cuerpo a cuerpo.
    /// </summary>
    /// <returns>Verdadero si el arma es cuerpo a cuerpo.</returns>
    public bool armaEquipadaEsCuerpoACuerpo()
    {
        return ArmaEquipada.esArmaCuerpoACuerpo();
    }

    /// <summary>
    /// Devuelve el modificador actual del arma equipada.
    /// </summary>
    /// <returns>Modificador actual arma.</returns>
    public int obtenerModificadorArma()
    {
        if (hayArmaEquipada())
        {
            return ArmaEquipada.ModificadorActual;
        }

        return 0;
    }

    /// <summary>
    /// Verifica si hay un arma equipada.
    /// </summary>
    /// <returns>Verdadero si hay un arma equipada.</returns>
    public bool hayArmaEquipada()
    {
        return ArmaEquipada != null;
    }

    /// <summary>
    /// Devuelve el modificador de fuerza de los anillos equipados.
    /// </summary>
    /// <returns>Modificador de Fuerza.</returns>
    public int obtenerModificadorFuerzaAnillos()
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

    /// <summary>
    /// Verifica si el primer slot de anillos está ocupado.
    /// </summary>
    /// <returns>Verdadero si hay un anillo equipado en el primer slot.</returns>
    public bool hayAnillo1Equipado()
    {
        return Anillo1Equipado != null;
    }

    /// <summary>
    /// Verifica si el segundo slot de anillos está ocupado.
    /// </summary>
    /// <returns>Verdadero si hay un anillo equipado en el segundo slot.</returns>
    public bool hayAnillo2Equipado()
    {
        return Anillo2Equipado != null;
    }

    /// <summary>
    /// Verifica si el arma equipada está vorpalizada.
    /// </summary>
    /// <returns>Verdadero si el arma equipada está vorpalizada.</returns>
    public bool armaEstáVorpalizada()
    {
        if (hayArmaEquipada())
        {
            return ArmaEquipada.EsArmaVorpalizada;
        }
        return false;
    }

    /// <summary>
    /// Calcula el daño que realiza el arma equipada. 
    /// Esto es el daño sin tener en cuenta los modificadores de anillos.
    /// </summary>
    /// <returns>Daño realizado con el arma actual.</returns>
    public int calcularDañoBase()
    {
        if (hayArmaEquipada())
        {
            return ArmaEquipada.calcularDañoBase(1);
        }

        return 0;
    }

    /// <summary>
    /// Obtiene los modificadores del equipo actual para la Defensa.
    /// </summary>
    /// <returns>Modificador de Defensa.</returns>
    public int obtenerModificadorDefensa()
    {
        int modificador = 0;

        modificador += obtenerModificadorArmadura();

        modificador += obtenerModificadorDefensaAnillos();

        return modificador;
    }

    /// <summary>
    /// Verifica si hay una armadura equipada.
    /// </summary>
    /// <returns>Verdadero si hay una armadura equipada.</returns>
    public bool hayArmaduraEquipada()
    {
        return armaduraEquipada != null;
    }

    /// <summary>
    /// Calcula el modificador de Defensa de la armadura equipada.
    /// </summary>
    /// <returns>Modificador armadura.</returns>
    public int obtenerModificadorArmadura()
    {
        if (hayArmaduraEquipada())
        {
            return ArmaduraEquipada.ModificadorActual;
        }

        return 0;
    }

    /// <summary>
    /// Calcula el modificador de los anillos equipados para la Defensa.
    /// </summary>
    /// <returns>Modificador de anillos para Defensa.</returns>
    public int obtenerModificadorDefensaAnillos()
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

    /// <summary>
    /// Obtiene los modificadores que aplican a la destreza.
    /// </summary>
    /// <remarks>Sólo considera los modificadores del equipo.</remarks>
    /// <returns>Modificador de destreza.</returns>
    public int obtenerModificadorDestreza()
    {
        int modificador = 0;

        modificador += obtenerModificadorDestrezaAnillos();

        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores que aplican a la destreza de los anillos.
    /// </summary>
    /// <returns>Modificador de destreza.</returns>
    public int obtenerModificadorDestrezaAnillos()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            modificador += Anillo1Equipado.ModificadorDestreza;
        }

        if (hayAnillo2Equipado())
        {
            modificador += Anillo2Equipado.ModificadorDestreza;
        }

        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores que aplican al daño.
    /// </summary>
    /// <remarks>Sólo considera los modificadores del equipo.</remarks>
    /// <returns>Modificador de daño.</returns>
    public int obtenerModificadorDaño()
    {
        int modificador = 0;

        modificador += obtenerModificadorArma();

        modificador += obtenerModificadorDañoAnillos();

        return modificador;
    }

    /// <summary>
    /// Obtiene el modificador de daño de los anillos.
    /// </summary>
    /// <returns>Modificador de daño.</returns>
    public int obtenerModificadorDañoAnillos()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            modificador += Anillo1Equipado.ModificadorDaño;
        }

        if (hayAnillo2Equipado())
        {
            modificador += Anillo2Equipado.ModificadorDaño;
        }

        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores que aplican al impacto.
    /// </summary>
    /// <remarks>No tiene en cuenta los modificadores de destreza. Sólo 
    /// modificadores exclusivos del impacto.</remarks>
    /// <returns>Modificador de impacto.</returns>
    public int obtenerModificadorImpacto()
    {
        int modificador = 0;

        modificador += obtenerModificadorArma();

        modificador += obtenerModificadorImpactoAnillos();

        return modificador;
    }

    /// <summary>
    /// Obtiene el modificador de impacto de los anillos.
    /// </summary>
    /// <returns>Modificador de impacto.</returns>
    public int obtenerModificadorImpactoAnillos()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            //modificador += Anillo1Equipado.ModificadorImpacto;
        }

        if (hayAnillo2Equipado())
        {
            //modificador += Anillo2Equipado.ModificadorImpacto;
        }

        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores que aplican a la magia.
    /// </summary>
    /// <remarks>Sólo considera los modificadores del equipo.</remarks>
    /// <returns>Modificador de magia.</returns>
    public int obtenerModificadorMagia()
    {
        int modificador = 0;

        modificador = obtenerModificadorMagiaAnillos();

        return modificador;
    }

    /// <summary>
    /// Obtiene los modificadores que aplican a la magia de los anillos.
    /// </summary>
    /// <returns>Modificador de magia.</returns>
    public int obtenerModificadorMagiaAnillos()
    {
        int modificador = 0;

        if (hayAnillo1Equipado())
        {
            modificador += Anillo1Equipado.ModificadorMagia;
        }

        if (hayAnillo2Equipado())
        {
            modificador += Anillo2Equipado.ModificadorMagia;
        }

        return modificador;
    }
}
