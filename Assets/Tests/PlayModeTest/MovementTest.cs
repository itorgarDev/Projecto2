using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using System.Reflection;

public class MovementTest
{
    private GameObject player;
    private PlayerMovement movement;

    [SetUp] // creamos al player con todo lo necesario
    public void Setup()
    {
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player"; 
        player.tag = "Player";

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        movement = player.AddComponent<PlayerMovement>();
        player.AddComponent<PlayerAnim>();
        player.AddComponent<Animator>();

        player.transform.position = Vector3.zero;
    }

    [UnityTest]
    public IEnumerator PlayerMovesRight()
    {
        Vector3 startPos = player.transform.position; // posiion inicial

        // FieldInfo es una clase de System.Reflection se usa para añadir un valor a una clase aunque sea privada o protegida
        FieldInfo field = typeof(PlayerMovement).GetField("moveInput",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance); //manera de decir de donde queremos cambiar el valor y como es la clase a la que queremos acceder
        field.SetValue(movement, new Vector2(1, 0)); // añadimos el vector pa que se mueva

        yield return new WaitForSeconds(0.2f);

        Vector3 endPos = player.transform.position; // posicion final
        Assert.Greater(endPos.x, startPos.x, "El jugador no se movió hacia la derecha"); //comparamos posiciones pa comprobar que se mueva
    }

    [TearDown] // limpiamos todo para el siguiente test
    public void Teardown()
    {
        Object.Destroy(player);
    }
}
