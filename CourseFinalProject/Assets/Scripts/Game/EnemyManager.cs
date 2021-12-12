using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    class Wave
    {
        public int EnemyCount;
        public float SpawnTime;
        public float MovementSpeed;
    }

    public Action Victory;
    public Action<int> AddScore;
    [SerializeField] private Wave[] _wave;
    [SerializeField] private int _currentWave;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _enemiesToSpawn;
    [SerializeField] private int _enemiesAlive;
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] List<GameObject> _enemieList;
    private float _spawnTimer;
    [SerializeField] private int _scoreForEnemy;
    
    public PlayerManager PlayerManager;
    

    private void Start()
    {
        _enemiesAlive = 0;
        _currentWave = 0;
        StartWave();
    }

    private void Update()
    {
        if (_enemiesToSpawn > 0)
        {
            if (Time.time - _spawnTimer > _wave[_currentWave].SpawnTime)
            {
                Spawn();
                _spawnTimer = Time.time;
                _enemiesToSpawn--;
                _enemiesAlive++;
            }
        }
        else
        {
            if (_enemiesAlive <= 0)
            {
                _currentWave++;
                if (_currentWave >= _wave.Length)
                {
                    Victory?.Invoke();
                    return;
                }
                StartWave();
                return;
            }
        }
    }

    private void StartWave()
    {
        _enemiesToSpawn = _wave[_currentWave].EnemyCount;
        _spawnTimer = Time.time;
    }

    private void OnZombieDeath(ZombieController enemy)
    {
        enemy.GetComponent<ZombieController>().OnDeath -= OnZombieDeath;
        _enemiesAlive--;
        enemy.enabled = false;
        AddScore?.Invoke(_scoreForEnemy);
    }

    private void Spawn()
    {
        var newEnemy = Instantiate(_zombiePrefab, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length)].position, transform.rotation);
        //int randomPoint = UnityEngine.Random.Range(0, _spawnPoints.Length);
        //newEnemy.transform.position = _spawnPoints[randomPoint].position;
        newEnemy.GetComponent<Health>().DamageAction += newEnemy.GetComponent<ZombieController>().TakeDamage;
        //newEnemy.GetComponent<ZombieController>().GetTarget += UpdateEnemyTarget;
        newEnemy.GetComponent<ZombieController>().OnDeath += OnZombieDeath;
        newEnemy.GetComponent<ZombieController>().SetSpeed(_wave[_currentWave].MovementSpeed);
        UpdateEnemyTarget(newEnemy.GetComponent<ZombieController>());
        _enemieList.Add(newEnemy);
    }

    public void UpdateEnemyTarget(ZombieController enemy)
    {
        var player = PlayerManager.GetPlayer();
        enemy.SetTarget(player);
        //player.GetComponent<PlayerController>().ResetZombieTarget -= enemy.OnTargetDeath;
        //player.GetComponent<PlayerController>().ResetZombieTarget += enemy.OnTargetDeath;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            Gizmos.DrawWireCube(_spawnPoints[i].position + Vector3.up * 0.5f, Vector3.one);
        }
    }
}
