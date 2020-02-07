using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anillo
{
    private int modificadorDaño;
    private int modificadorFuerza;
    private int modificadorDestreza;
    private int modificadorDefenza;
    private int modificadorMagia;
    //private TipoAnillo tipo;
    private bool estáMaldito;
    private int comidaQueConsume;

    public Anillo(int modificadorDaño, int modificadorFuerza, int modificadorDestreza, int modificadorDefenza, int modificadorMagia, bool estáMaldito, int comidaQueConsume)
    {
        this.ModificadorDaño = modificadorDaño;
        this.ModificadorFuerza = modificadorFuerza;
        this.ModificadorDestreza = modificadorDestreza;
        this.ModificadorDefenza = modificadorDefenza;
        this.ModificadorMagia = modificadorMagia;
        this.EstáMaldito = estáMaldito;
        this.ComidaQueConsume = comidaQueConsume;
    }

    public int ModificadorDaño { get => modificadorDaño; set => modificadorDaño = value; }
    public int ModificadorFuerza { get => modificadorFuerza; set => modificadorFuerza = value; }
    public int ModificadorDestreza { get => modificadorDestreza; set => modificadorDestreza = value; }
    public int ModificadorDefenza { get => modificadorDefenza; set => modificadorDefenza = value; }
    public int ModificadorMagia { get => modificadorMagia; set => modificadorMagia = value; }
    public bool EstáMaldito { get => estáMaldito; set => estáMaldito = value; }
    public int ComidaQueConsume { get => comidaQueConsume; set => comidaQueConsume = value; }
}
