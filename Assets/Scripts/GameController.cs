using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public enum GameType
    {
        None,
        Single,
        Multiplayer
    }
    public class GameController : MonoBehaviour
    {
        #region [ Public Fields ]

        public GameObject NetworkManager;
        public GameObject Player1;
        public GameObject Player2;
        public GameObject Hazard;
        public GameObject LevelManager;
        public Transform SpawnValues;
        public float SpawnTime;
        public int TotalNumEnemies;
        public Stack<int> Positions;

        #endregion

        #region [ Private Fields  ]

        private float _btnX, _btnY, _btnW, _btnH;
        private float _spawnDeltaTime;
        private GameType _gameType;
        private bool _isGameStarted;
        public int EnemiesSpawned { get; private set; }
        public int EnemiesKilled { get; set; }
        #endregion

        #region [ Private Methods ]

        private void SwarmEnemy()
        {

            _spawnDeltaTime -= Time.deltaTime;

            if (_spawnDeltaTime <= 0 && EnemiesSpawned < TotalNumEnemies)
            {

                var spawnPosition = new Vector3(Random.Range(-SpawnValues.position.x, SpawnValues.position.x),
                    SpawnValues.position.y, SpawnValues.position.z);
                var enemy = GameObjectController.Instantiate(Hazard, spawnPosition, Quaternion.identity);
                EnemiesSpawned++;

                _spawnDeltaTime = SpawnTime;
            }

        }

        private void SpawnPlayer()
        {
            GameObjectController.Instantiate(Player1, new Vector3(0, 0), Quaternion.identity, 0);
        }

        #endregion

        #region [ Monobehaviors ]

        void OnPlayerConnected(NetworkPlayer player)
        {
            Debug.Log("Player Connected");
            _isGameStarted = true;
        }

        /// <summary>
        /// Specifically used when a client connects to a server
        /// </summary>
        void OnConnectedToServer()
        {
            Debug.Log("Connected to Server");
            Network.Instantiate(Player2, new Vector3(3, 0), Quaternion.identity, 0);
        }

        void OnServerInitialized()
        {
            SpawnPlayer();
        }

        private void Start()
        {
            _btnX = Screen.width * 0.01f;
            _btnY = Screen.width * 0.01f;
            _btnW = 100f;
            _btnH = 50f;
            _spawnDeltaTime = SpawnTime;
            LevelManager.SetActive(false);
            Positions = new Stack<int>();
            for (var i = 7; i > 0; i--)
            {
                if (i > 0)
                {
                    Positions.Push(-i);
                }
                Positions.Push(i);
            }
        }


        private void OnGUI()
        {
            if (Network.isClient || Network.isServer) return;

            var startSinglePlayer = GUI.Button(new Rect(_btnX, _btnY, _btnW, _btnH), "Start New Game");
            var startMultiplayer = GUI.Button(new Rect(_btnX, _btnY + 100, _btnW, _btnH), "Start Multiplayer Game");
            var joinGame = GUI.Button(new Rect(_btnX, _btnY + 200f, _btnW, _btnH), "Join Game");

            if (startMultiplayer)
            {
                NetworkManager.GetComponent<NetworkManager>().StartServer();
                _gameType = GameType.Multiplayer; ;
                Debug.Log("Game/Server started");

            }

            if (startSinglePlayer)
            {
                _gameType = GameType.Single;
                _isGameStarted = true;
                SpawnPlayer();
            }

            if (joinGame)
            {
                NetworkManager.GetComponent<NetworkManager>().RefreshHost();
                _gameType = GameType.Multiplayer;
            }

            NetworkManager.GetComponent<NetworkManager>().GetAvailableGames(_btnX, _btnY, _btnW, _btnH);
        }



        private void Update()
        {
            if (!_isGameStarted) return;


            switch (_gameType)
            {
                case GameType.None:
                    break;
                case GameType.Single:
                    SwarmEnemy();
                    break;
                case GameType.Multiplayer:
                    if (Network.isServer)
                    {
                        SwarmEnemy();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (EnemiesKilled == TotalNumEnemies)
            {
                //Application.LoadLevel(0);
                //LevelManager.SetActive(true);

                //LevelManager.GetComponent<TextMesh>().text +=" 2";

                EnemiesSpawned = 0;
                SwarmEnemy();
            }
        }

        #endregion

        #region [ Private Class ]



        #endregion
    }
}
