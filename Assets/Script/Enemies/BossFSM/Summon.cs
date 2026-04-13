using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Summon: estado que genera una oleada de minions y pasa a Shield
// Mantén los comentarios y Debug.Log para depuración tal y como pediste.
public class Summon : TemplateStateMachine
{
    private BossEvokerFSMManager _fsm;
    private Enemy _life;

    public Summon(BossEvokerFSMManager stateMachineFlow) : base("Summon", stateMachineFlow)
    {
        _fsm = stateMachineFlow;
        _life = _fsm.GetComponent<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("[Summon] Enter: iniciando spawn de minions. CurrentWave = " + _fsm.currentWave);

        int amount = _fsm.currentWave == 1 ? 3 : 5;
        SpawnWave(amount);

        Debug.Log("[Summon] Spawn completado. Minions vivos ahora: " + _fsm.aliveMinions);

        // Pasamos a Shield cuando termine Summon
        stateMachineFlow.ChangeState(_fsm.shieldState);
    }

    private void SpawnWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            // posición alrededor del boss (ajusta si trabajas en 2D)
            Vector3 spawnPos = _fsm.transform.position + Random.insideUnitSphere * 2f;
            spawnPos.y = _fsm.transform.position.y; // opcional: mantener la misma altura

            // Sacamos del pool al minion
            Enemy minion = EnemyPool.Instance.GetFromPool(spawnPos);

            if (minion == null)
            {
                Debug.LogWarning("[Summon] GetFromPool devolvió null para spawnPos: " + spawnPos);
                continue;
            }

            // Asegúrate de que el pool coloque/active correctamente al minion.
            // Si tu pool no posiciona, descomenta estas líneas:
            // minion.transform.position = spawnPos;
            // minion.gameObject.SetActive(true);

            Debug.Log("[Summon] Minion spawnado en " + spawnPos);

            // Registramos el minion para que el boss lo cuente y se desuscriba al morir
            _fsm.RegisterMinion(minion);
        }
    }
}
