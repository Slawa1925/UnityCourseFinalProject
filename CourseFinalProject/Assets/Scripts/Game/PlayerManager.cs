using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Action GameOver;
    [SerializeField] private int _numberOfPlayers;
    public PlayerController[] Player;
    [SerializeField] private Transform[] _playerSpawnPoint;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CameraFollow2p _camera;
    [SerializeField] private Transform _canvas;
    [SerializeField] private float _respawnTime;
    private float _respawnTimer;
    [SerializeField] private Vector3 _levelSize;


    private void Start()
    {
        Player = new PlayerController[_numberOfPlayers];

        for (int i = 0; i < _numberOfPlayers; i++)
        {
            Player[i] = Instantiate(_playerPrefab, _playerSpawnPoint[i].position, _playerSpawnPoint[i].rotation).GetComponent<PlayerController>();
            Player[i].gameObject.name = "Player " + i;

            Player[i].DeathEvent += OnPlayerDeath;
            Player[i].GetComponent<Health>().DamageAction += Player[i].TakeDamage;

            Player[i].GetComponent<PlayerInfo>().SetCanvas(_canvas);
            Player[i].SetInput(i);
            Player[i].GetComponent<PlayerWeaponsController>().SetInput(i);
            Player[i].Spawn();
        }

        _camera.Player0 = Player[0].transform;
        _camera.Player1 = Player[1].transform;
    }

    private void Update()
    {
        for (int i = 0; i < _numberOfPlayers; i++)
        {
            if (!Player[i].isAlive)
            {
                if (Time.time - _respawnTimer >= _respawnTime)
                {
                    Player[i].gameObject.SetActive(false);
                    RespawnPlayer(i);
                }
            }
        }
    }

    public GameObject GetPlayer()
    {
        int randomIndex = UnityEngine.Random.Range(0, Player.Length);

        if (Player[randomIndex].isAlive)
            return Player[randomIndex].gameObject;
        else
            return Player[1-randomIndex].gameObject;
    }

    private void RespawnPlayer(int i)
    {
        if (i >= Player.Length)
            return;

        Player[i].transform.position = FindSpawnPoint();
        Player[i].transform.rotation = _playerSpawnPoint[i].rotation;
        Player[i].gameObject.SetActive(true);
        Player[i].Spawn();
    }

    private Vector3 FindSpawnPoint()
    {
        Vector3 playerAlivePos;

        if (Player[0].isAlive)
            playerAlivePos = Player[0].transform.position;
        else
            playerAlivePos = Player[1].transform.position;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var curPos = playerAlivePos + new Vector3(i - 1, 0, j - 1) * 3.0f;

                if ((curPos.x * curPos.x < _levelSize.x * _levelSize.x / 4) && (curPos.z * curPos.z < _levelSize.z * _levelSize.z / 4))
                {
                    if (!Physics.CheckSphere(curPos + Vector3.up * 0.5f, 0.4f))
                    {
                        return curPos;
                    }
                }
            }
        }

        return playerAlivePos;
    }

    private void OnPlayerDeath()
    {
        print("OnPlayerDeath");
        if (!Player[0].isAlive && !Player[1].isAlive)
        {
            GameOver?.Invoke();
            return;
        }

        _respawnTimer = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < _playerSpawnPoint.Length; i++)
        {
            Gizmos.DrawWireCube(_playerSpawnPoint[i].position + Vector3.up * 0.5f, Vector3.one);
        }
        
        /*if (!Application.isPlaying)
            return;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var curPos = _player[0].transform.position + Vector3.up * 0.5f + new Vector3(i - 1, 0, j - 1) * 3.0f;

                if (_spawnPositions[i * 3 + j])
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;
                Gizmos.DrawWireCube(curPos, Vector3.one);
            }
        }*/
    }
}
