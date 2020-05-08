using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armadura : ObjetoAgarrable
{
    private int modificadorActual;

    public int ModificadorActual { get => modificadorActual; set => modificadorActual = value; }
}
