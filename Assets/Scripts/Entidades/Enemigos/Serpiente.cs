using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serpiente : Enemigo
{
    private Vector3 direcciónAnterior;

    public Vector3 DirecciónAnterior { get => direcciónAnterior; set => direcciónAnterior = value; }

    public override void usarTurno()
    {
        if (estoyAdyacenteAlPersonaje())
        {
            //Debug.Log("Atacar");
            atacar();
        }
        else
        {
            //Debug.Log("Moverse");
            Vector3 dirección = elegirDirecciónMovimiento();

            if (dirección != Vector3.back)
            {
                //Debug.Log("Pude moverme");
                moverse(dirección);
            }
        }
    }

    public override void atacar()
    {
        int impacto = calcularImpacto(0);
        int daño = 0;

        daño = calcularDaño(obtenerModificadorFuerza(0));

        if (EsAtaqueCrítico)
        {
            daño *= 2;
        }

        bool impacta = realizarAtaque(impacto, daño, EsAtaqueCrítico);

        Controlador.mostrarAnimaciónAtaqueMurciélago(impacta, daño, EsAtaqueCrítico);


        EsAtaqueCrítico = false;
        return;
    }

    public bool realizarAtaque(int impacto, int daño, bool esAtaqueCrítico)
    {
        return Controlador.realizarAtaque(impacto, daño, esAtaqueCrítico);
    }

    public override int obtenerModificadorFuerza(int modificadorMisceláneo)
    {
        return Fuerza + modificadorMisceláneo;
    }

    public int calcularDaño(int modificador)
    {
        int dañoBase = DadoDañoAtaqueBase.tirarDados(CantidadDadosDañoAtaqueBase);
        return dañoBase + modificador;
    }

    public bool estoyAdyacenteAlPersonaje()
    {
        return Controlador.verificarSiEnemigoEstáAdyacenteAPersonaje(this);
    }

    public Vector3 elegirDirecciónMovimiento()
    {
        int nro = Controlador.tirarD20(false, false);

        if (DirecciónAnterior == Vector3.zero || nro > 4 || Controlador.verificarSiElCaminoEstáObstruido(DirecciónAnterior, this))
        {
            DirecciónAnterior = Controlador.obtenerDirecciónAleatoria(this);
        }

        return DirecciónAnterior;

    }

    // Start is called before the first frame update
    void Start()
    {
        Estados = new List<EstadoEnemigo>();
        Estados.Add(new EstadoEnemigo(EstadosEnemigo.NORMAL));

        VidaMáxima = 10;
        VidaActual = VidaMáxima;
        Defensa = 5;
        Fuerza = 0;
        Destreza = 0;
        Magia = 0;
        DadoDañoAtaqueBase = new Dado(4);
        CantidadDadosDañoAtaqueBase = 1;

        Ventaja = false;
        Desventaja = false;
        EsAtaqueCrítico = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Estados[0].Nombre == EstadosEnemigo.MUERTO)
        {
            Destroy(gameObject);
            Controlador.Enemigos.Remove(this);
        }
    }
}
