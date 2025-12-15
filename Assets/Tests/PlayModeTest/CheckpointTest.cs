using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CheckpointRespawnTest
{
    private GameObject player;
    private PlayerMovement movement;
    private GameObject checkpoint;

    [SetUp]
    public void Setup()
    {
        // Crear jugador
        player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        movement = player.AddComponent<PlayerMovement>();
        player.transform.position = Vector3.zero;

        // Crear checkpoint
        checkpoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        checkpoint.name = "Checkpoint";
        checkpoint.tag = "Checkpoint";
        checkpoint.transform.position = new Vector3(5, 0, 0);

        checkpoint.GetComponent<BoxCollider>().isTrigger = true;
        checkpoint.AddComponent<Checkpoint>(); // tu script que actualiza RespawnSystem
    }

    [UnityTest]
    public IEnumerator CheckpointActivatesAndPlayerRespawns()
    {
        // Simular entrada en el checkpoint
        checkpoint.GetComponent<Checkpoint>().SendMessage("OnTriggerEnter", player.GetComponent<Collider>());
        yield return null;

        // Validar que el checkpoint se activó
        Assert.AreEqual(checkpoint.transform.position, RespawnSystem.LastCheckpointPos,
            "El checkpoint no se activó correctamente");

        // Simular muerte
        movement.TakeDamage(1);
        yield return null;

        // Validar que reaparece en el checkpoint
        Assert.AreEqual(RespawnSystem.LastCheckpointPos, player.transform.position,
            "El jugador no reapareció en el checkpoint tras morir");
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(player);
        Object.Destroy(checkpoint);
    }
}
