using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

public class DashTest
{
    private GameObject player;
    private PlayerMovement movement;

    [SetUp]
    public void Setup()
    {
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        movement = player.AddComponent<PlayerMovement>();
        player.transform.position = Vector3.zero;
    }

    [UnityTest]
    public IEnumerator PlayerDoesDash()
    {
        Vector3 startPos = player.transform.position;

        // Con el InputSystem la manera mas facil de hacer el movimiento es esta de reflection
        FieldInfo moveField = typeof(PlayerMovement).GetField("moveInput",
            BindingFlags.NonPublic | BindingFlags.Instance);
        moveField.SetValue(movement, new Vector2(0, 1));
        
        FieldInfo dashField = typeof(PlayerMovement).GetField("isDashing",
            BindingFlags.NonPublic | BindingFlags.Instance);
        dashField.SetValue(movement, true);

        yield return new WaitForSeconds(0.6f); // dash dura 0.2s asi que le doy mas tiempo para que recorra todo

        Vector3 endPos = player.transform.position;

        // Validamos que se haya movido más de lo normal
        Assert.Greater(Vector3.Distance(endPos, startPos), 1f, "El jugador no realizó el dash correctamente");

        // Validamos que la flag IsDashing se haya activado
        Assert.IsTrue(movement.IsDashing, "El jugador no está en estado de dash");

        
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
    }
}
