using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaCuerpoACuerpo : Arma
{
    /// <summary>
    /// Verifica si el arma es un arma cuerpo a cuerpo.
    /// </summary>
    /// <returns>Siempre devuelve verdadero.</returns>
    public override bool esArmaCuerpoACuerpo()
    {
        return true;
    }
}
