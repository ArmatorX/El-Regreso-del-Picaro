using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serpiente : Enemigo
{
    private Vector2 direcciónAnterior;
    public Vector2 DirecciónAnterior { get => direcciónAnterior; set => direcciónAnterior = value; }

    /// <summary>
    /// Elige una dirección para mover a la serpiente.
    /// </summary>
    /// <remarks>La serpiente se mueve de forma aleatoria el 20% de las veces,
    /// y el resto utiliza la dirección anterior.</remarks>
    /// <returns>Dirección movimiento.</returns>
    public override Vector2 elegirDirecciónMovimiento()
    {
        int nro = Controlador.tirarD20(false, false);

        if (DirecciónAnterior == Vector2.zero || nro > 4 || Controlador.hayObstáculoEn((Vector2) this.transform.position + DirecciónAnterior))
        {
            DirecciónAnterior = Controlador.obtenerDirecciónAleatoria(this.transform.position, Tamaño, true);
        }

        return DirecciónAnterior;
    }

    // Start is called before the first frame update
    void Start()
    {
        Estados = new List<EstadoEnemigo>();
        Estados.Add(new EstadoEnemigo(EstadosEnemigo.NORMAL));

        Tamaño = TamañoEntidad.NORMAL;
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

        crearManiquí();
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
