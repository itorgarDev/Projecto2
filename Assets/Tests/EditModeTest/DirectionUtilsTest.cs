using NUnit.Framework;
using UnityEngine;

public class DirectionUtilsTest
{
    [Test] // comprobaremos que el vector normalizado se calcula correctamente

    public void NormalizeDirection_ReturnsUnitVector()
    {
        Vector2 input = new Vector2(3, 4); // creamos un vector superior a 1
        Vector2 result = input.normalized;

        Assert.AreEqual(1f, result.magnitude, 0.0001f); // comprobamos si devuelvemagnitud de 1
    }
}
