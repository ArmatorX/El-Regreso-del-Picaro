using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspadaLarga : ArmaCuerpoACuerpo
{
    public EspadaLarga() 
    { 
        ModificadorActual = 0;
        DadoDaño = new Dado(8);
        EsArmaVorpalizada = false;
        //Calidad = new Calidad();

        Nombre = "Espada Larga";
        DescripciónParcial = "Espada de hierro.";
        DescripciónCompleta = "Espada de hierro +" + ModificadorActual + ". Daño: 1d" + DadoDaño.CantidadCaras;
        RutaSprite = "Gráficos/items/espada";
    }
}