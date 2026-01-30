
using System.Collections;
using System.Collections.Generic;
using StateMachineCore;
using UnityEngine;

namespace PoolSystem
{
    public class EnemyPool : Pool<StateMachine>
    {
        [SerializeField] private List<Transform> enemySpawnPosition = new();
        [SerializeField] private float spawnTime;

        private Dictionary<StateMachine, Vector3> enemyDic = new();

        void Start()
        {
            foreach (var pos in enemySpawnPosition)
            {
                StateMachine enemy = Get();
                enemy.transform.position = pos.position;
                enemy.gameObject.SetActive(true);
                enemyDic.Add(enemy, pos.position);
            }
        }

        public override void Return(StateMachine enemy)
        {
            base.Return(enemy);

            StartCoroutine(SpawnTimer(enemy));
        }

        IEnumerator SpawnTimer(StateMachine enemy)
        {
            if (!enemyDic.ContainsKey(enemy))
                enemyDic.Add(enemy, enemy.transform.position);
            enemy.transform.position = enemyDic[enemy];
            yield return new WaitForSeconds(spawnTime);
            StateMachine newEnemy = Get();
            newEnemy.gameObject.SetActive(true);
        }
    }
}