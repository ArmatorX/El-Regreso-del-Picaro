using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Arma
{
    private int modificadorActual;
    private Dado dadoDaño;
    private bool esArmaVorpalizada;
    private Type enemigoVorpalización;
    private Calidad calidad;

    public int ModificadorActual { get => modificadorActual; set => modificadorActual = value; }
    public Dado DadoDaño { get => dadoDaño; set => dadoDaño = value; }
    public bool EsArmaVorpalizada { get => esArmaVorpalizada; set => esArmaVorpalizada = value; }
    public Type EnemigoVorpalización { get => enemigoVorpalización; set => enemigoVorpalización = value; }
    public Calidad Calidad { get => calidad; set => calidad = value; }

    public int calcularDañoBase(int cantidadDados)
    {
        return dadoDaño.tirarDados(cantidadDados);
    }

    public abstract bool esArmaCuerpoACuerpo();
}
