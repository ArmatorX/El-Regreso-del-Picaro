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

    /// <summary>
    /// Verifica si el enemigo está en un estado.
    /// </summary>
    /// <param name="estado">Estado a verificar.</param>
    /// <returns>Verdadero, si el enemigo está en el estado.</returns>
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

    /// <summary>
    /// Aplica el daño al enemigo y si su <c>VidaActual</c> es 0, lo coloca en 
    /// el estado "Muerto".
    /// </summary>
    /// <param name="daño">Puntos de daño recibidos.</param>
    public override void recibirDaño(int daño)
    {
        base.recibirDaño(daño);

        if (VidaActual == 0)
        {
            Estados.Clear();
            Estados.Add(new EstadoEnemigo(EstadosEnemigo.MUERTO));
        }
    }
    
    /// <summary>
    /// Utiliza el turno del enemigo. Si el personaje se encuentra en un 
    /// casillero adyacente, lo ataca. De otro modo, se mueve.
    /// </summary>
    /// <param name="modificadorMisceláneo">Modificador que aplica en ocasiones
    /// especiales, como estados alterados.</param>
    public virtual void usarTurno(int modificadorMisceláneo)
    {
        if (estoyAdyacenteAlPersonaje())
        {
            atacarCuerpoACuerpo(modificadorMisceláneo);
        }
        else
        {
            Vector2 dirección = elegirDirecciónMovimiento();

            if (dirección != Vector2.zero)
            {
                moverse(dirección);
            }
        }
    }

    /// <summary>
    /// Ataca al personaje con un ataque básico.
    /// </summary>
    /// <param name="modificadorMisceláneo">Modificador que aplica en ocasiones
    /// especiales, como estados alterados.</param>
    public virtual void atacarCuerpoACuerpo(int modificadorMisceláneo)
    {
        int impacto = calcularImpacto(modificadorMisceláneo);
        byte tipo = 1;
        bool esCrítico = false;

        if (impacto == -1)
        {
            tipo = 2;
            esCrítico = true;
        }

        int daño = calcularDaño(esCrítico, false);

        if (!realizarAtaque(impacto, daño))
        {
            tipo = 0;
        }

        Controlador.animaciónAtaqueMeléEnemigo(this, daño, tipo);
    }

    public override void moverse(Vector2 dirección)
    {
        moverHitbox(dirección);

        Controlador.animaciónMovimientoEnemigo(this, dirección);
        //crearManiquí(dirección);
    }

    /// <summary>
    /// Calcula el daño del ataque base (sin modificadores) del enemigo.
    /// </summary>
    /// <returns>Daño ataque base.</returns>
    public override int calcularDañoBase()
    {
        return DadoDañoAtaqueBase.tirarDados(CantidadDadosDañoAtaqueBase);
    }

    /*
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
    */

    /// <summary>
    /// Verifica si el enemigo se encuentra en un casillero adyacente al 
    /// personaje.
    /// </summary>
    /// <returns>Verdadero, si está adyacente al personaje.</returns>
    public bool estoyAdyacenteAlPersonaje()
    {
        return Controlador.estáAdyacenteAlPersonaje(RB.position, Tamaño);
    }

    /// <summary>
    /// Ejecuta el ataque. Es decir, delega al <c>ControladorJuego</c> la tarea 
    /// de verificar si el ataque impacta y aplicar el daño correspondiente.
    /// </summary>
    /// <param name="impacto">Valor de impacto. -1 si es crítico.</param>
    /// <param name="daño">Puntos de daño.</param>
    /// <returns>Verdadero si el ataque impacta.</returns>
    public bool realizarAtaque(int impacto, int daño)
    {
        return Controlador.realizarAtaque(Controlador.Personaje, impacto, daño);
    }

    /// <summary>
    /// Calcula el modificador de la <c>Fuerza</c> del enemigo.
    /// </summary>
    /// <returns>Modificador de Fuerza.</returns>
    public override int obtenerModificadorFuerza()
    {
        return Mathf.FloorToInt((Fuerza - 10) / 2);
    }

    /// <summary>
    /// Calcula el modificador de la <c>Destreza</c> del enemigo.
    /// </summary>
    /// <returns>Modificador de Destreza.</returns>
    public override int obtenerModificadorDestreza()
    {
        return Mathf.FloorToInt((Destreza - 10) / 2);
    }

    /// <summary>
    /// Calcula el modificador de la <c>Magia</c> del enemigo.
    /// </summary>
    /// <returns>Modificador de Magia.</returns>
    public override int obtenerModificadorMagia()
    {
        return Mathf.FloorToInt((Magia - 10) / 2);
    }

    /// <summary>
    /// Devuelve la Defensa del enemigo. Es equivalente a un get a la propiedad.
    /// </summary>
    /// <returns>Defensa.</returns>
    public override int obtenerDefensa()
    {
        return Defensa;
    }

    /// <summary>
    /// Devuelve el modificador de daño del enemigo.
    /// </summary>
    /// <param name="ataqueADistancia">Indica si se trata de un ataque a 
    /// distancia.</param>
    /// <returns>Modifcador de daño.</returns>
    public override int obtenerModificadorDaño(bool ataqueADistancia = false)
    {
        if (ataqueADistancia)
        {
            return obtenerModificadorDestreza();
        } 
        else
        {
            return obtenerModificadorFuerza();
        }
    }

    /// <summary>
    /// Permite controlar si una entidad es un enemigo.
    /// </summary>
    /// <returns>Siempre devuelve verdadero.</returns>
    public override bool esEnemigo()
    {
        return true;
    }

    /// <summary>
    /// Obtiene el modificador de impacto del enemigo.
    /// </summary>
    /// <param name="ataqueADistancia">Indica si se trata de un ataque a 
    /// distancia.</param>
    /// <returns>Modificador de impacto.</returns>
    public override int obtenerModificadorImpacto(bool ataqueADistancia = false)
    {
        if (ataqueADistancia)
        {
            return obtenerModificadorDestreza();
        }
        else
        {
            return obtenerModificadorFuerza();
        }
    }

    public abstract Vector2 elegirDirecciónMovimiento();
}
