using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa un componente del calabozo (calabozo, mapa, habitación, 
/// pasillo).
/// </summary>
public class CompuestoCalabozo : IComponenteCalabozo
{
    private int númeroOrden;
    private Vector2 dimensiones;
    private Vector2 posición;
    private List<IComponenteCalabozo> hijos;
    private Visibilidad visibilidad;
    private TipoComponenteCalabozo tipo;


    public Vector2 Dimensiones { get => dimensiones; set => dimensiones = value; }
    public Vector2 Posición { get => posición; set => posición = value; }
    public int NúmeroOrden { get => númeroOrden; set => númeroOrden = value; }
    public List<IComponenteCalabozo> Hijos { get => hijos; set => hijos = value; }
    public Visibilidad Visibilidad { get => visibilidad; set => visibilidad = value; }
    public TipoComponenteCalabozo Tipo { get => tipo; set => tipo = value; }

    public void añadir(IComponenteCalabozo componente)
    {
        this.hijos.Add(componente);
    }

    public void eliminar(IComponenteCalabozo componente)
    {
        this.hijos.Remove(componente);
    }

    public void iluminar()
    {
        this.hijos.ForEach(componente => componente.iluminar());

        this.visibilidad = new Visibilidad(Visibilidad.VISIBILIDAD.Total, this.GetType().Name);
    }

    public void mostrar()
    {
        throw new System.NotImplementedException();
    }

    public Casillero obtenerCasilleroDestino(Casillero casilleroOrigen, Vector2 dirección)
    {
        if (this.hijos.Contains(casilleroOrigen))
        {
            Casillero casilleroDestino = (Casillero) this.hijos.Find(casillero => 
                ((Casillero) casillero).esCasilleroDestino(casilleroOrigen, dirección));
            
            return casilleroDestino;
        }

        return null;
    }

    public IComponenteCalabozo obtenerHijo(int númeroOrden)
    {
        return this.hijos[númeroOrden];
    }

    public IComponenteCalabozo obtenerHijo(Vector2 posición)
    {
        return this.hijos.Find(componente => componente.Posición == posición);
    }
}
