using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemigo : Entidad
{
    private List<EstadoEnemigo> estados;
    private int experienciaQueSuelta;
    private int dificultad;
    private int vidaMáxima;
    private int fuerza;
    private int destreza;
    private int defensa;
    private int magia;
    private Dado dadoDañoAtaqueBase;
    private int cantidadDadosDañoAtaqueBase;
    private Rigidbody2D rb;

    public List<EstadoEnemigo> Estados { get => estados; set => estados = value; }
    public int ExperienciaQueSuelta { get => experienciaQueSuelta; set => experienciaQueSuelta = value; }
    public int Dificultad { get => dificultad; set => dificultad = value; }
    public int VidaMáxima { get => vidaMáxima; set => vidaMáxima = value; }
    public int Fuerza { get => fuerza; set => fuerza = value; }
    public int Destreza { get => destreza; set => destreza = value; }
    public int Defensa { get => defensa; set => defensa = value; }
    public int Magia { get => magia; set => magia = value; }
    public Dado DadoDañoAtaqueBase { get => dadoDañoAtaqueBase; set => dadoDañoAtaqueBase = value; }
    public int CantidadDadosDañoAtaqueBase { get => cantidadDadosDañoAtaqueBase; set => cantidadDadosDañoAtaqueBase = value; }
    public Rigidbody2D RB { get => rb == null ? rb = GetComponent<Rigidbody2D>() : rb; set => rb = value; }

    //private Recompensa recompensa;
    /*
    public Enemigo(Piso ubicación, int vidaActual, List<EstadoEnemigo> estados, int experienciaQueSuelta, int dificultad, int vidaMáxima, int fuerza, int destreza, int defenza, int magia, Dado dadoDañoAtaqueBase, int cantidadDadosDañoAtaqueBase)
        : base(ubicación, vidaActual)
    {
        this.Estados = estados;
        this.ExperienciaQueSuelta = experienciaQueSuelta;
        this.Dificultad = dificultad;
        this.VidaMáxima = vidaMáxima;
        this.Fuerza = fuerza;
        this.Destreza = destreza;
        this.Defensa = defenza;
        this.Magia = magia;
        this.DadoDañoAtaqueBase = dadoDañoAtaqueBase;
        this.CantidadDadosDañoAtaqueBase = cantidadDadosDañoAtaqueBase;
    }
    */

    public bool estáEnEstado(EstadosEnemigo estado)
    {
        foreach (EstadoEnemigo estadoAux in Estados)
        {
            if (estadoAux.Nombre == estado)
            {
                return true;
            }
        }

        return false;
    }

    public override bool verificarSiAtaqueImpacta(int impacto)
    {
        return impacto >= Defensa || impacto == -1;
    }

    public override void recibirDaño(int daño)
    {
        base.recibirDaño(daño);

        if (VidaActual == 0)
        {
            Estados.Clear();
            Estados.Add(new EstadoEnemigo(EstadosEnemigo.MUERTO));
        }
    }

    public virtual void usarTurno()
    {
        if (estoyAdyacenteAlPersonaje())
        {
            realizarAtaqueMelé();
        }
        else
        {
            Vector2 dirección = elegirDirecciónMovimiento();

            if (dirección != null)
            {
                moverse(dirección);
            }
        }
    }
    public virtual void realizarAtaqueMelé()
    {
        int impacto = calcularImpacto(0);
        int daño;
        byte tipo = 1;

        daño = calcularDaño(obtenerModificadorFuerza(0));

        if (impacto == -1)
        {
            daño *= 2;
            tipo = 2;
        }

        if (!realizarAtaque(impacto, daño))
        {
            tipo = 0;
        }

        Controlador.animaciónAtaqueMeléEnemigo(this, daño, tipo);
    }

    public override void moverse(Vector2 dirección)
    {
        moverManiquí(dirección);
        Controlador.animaciónMovimientoEnemigo(this, dirección);
        //crearManiquí(dirección);
    }
    
    /// <summary>
    /// Crea un maniquí. Evita que dos enemigos 
    /// elijan el mismo casillero como destino.
    /// </summary>
    public void crearManiquí()
    {
        Maniquí = new GameObject("Maniquí");

        Maniquí.tag = "Enemigo";

        Maniquí.transform.localScale = new Vector3(6.25f, 6.25f, 1);

        Maniquí.transform.position = this.transform.position;

        BoxCollider2D hitbox = Maniquí.AddComponent<BoxCollider2D>();

        hitbox.size = new Vector2(0.16f, 0.16f);
    }
    
    /// <summary>
    /// Mueve el maniquí en la dirección de movimiento. Evita que dos enemigos 
    /// elijan el mismo casillero como destino.
    /// </summary>
    /// <param name="dirección">Dirección de movimiento.</param>
    public void moverManiquí(Vector2 dirección)
    {
        Maniquí.transform.position += (Vector3)dirección;
    }

    public bool estoyAdyacenteAlPersonaje()
    {
        return Controlador.estáAdyacenteAlPersonaje(RB.position, Tamaño);
    }

    public bool realizarAtaque(int impacto, int daño)
    {
        return Controlador.realizarAtaque(Controlador.Personaje, impacto, daño);
    }

    public int calcularDaño(int modificador)
    {
        int dañoBase = DadoDañoAtaqueBase.tirarDados(CantidadDadosDañoAtaqueBase);
        return dañoBase + modificador;
    }

    public override int obtenerModificadorFuerza(int modificadorMisceláneo)
    {
        return Fuerza + modificadorMisceláneo;
    }

    public abstract Vector2 elegirDirecciónMovimiento();

    public override bool esEnemigo()
    {
        return true;
    }
}
