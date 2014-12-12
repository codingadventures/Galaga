using System;
using System.Collections;
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
        public GameObject PurpleEnemy;
        public GameObject RedEnemy;
        public GameObject LevelManager;
        public Transform SpawnValues;
        public float SpawnTime;
        public float TimeBetweenSpawns;
        public int TotalNumEnemies;
        public List<Stack<int>> Positions;
        public int EnemiesSpawnedPerSwarm { get; private set; }

        #endregion

        #region [ Private Fields  ]
        private System.Random _random = new System.Random();

        private const String _levelText = "Level {0}";
        private int _level = 1;
        private float _btnX, _btnY, _btnW, _btnH;
        private float _spawnDeltaTime;
        private float _timeBetweenSpawns;
        private GameType _gameType;
        private bool _isGameStarted;
        private int _totalEnemiesSwarmed;
        public int EnemiesKilled { get; set; }

        #endregion

        #region [ Private Methods ]

        private void SwarmEnemy()
        {

            _spawnDeltaTime -= Time.deltaTime;
            _timeBetweenSpawns -= Time.deltaTime;

            var number = _random.Next(-5, 5);


            if (_spawnDeltaTime <= 0 && EnemiesSpawnedPerSwarm < TotalNumEnemies / 4 && _totalEnemiesSwarmed < TotalNumEnemies)
            {
                var spawnPosition = new Vector3(Random.Range(-SpawnValues.position.x, SpawnValues.position.x),
                    SpawnValues.position.y, SpawnValues.position.z);
            
                if (number > 0)
                    GameObjectController.Instantiate(PurpleEnemy, spawnPosition, Quaternion.identity);
                else
                    GameObjectController.Instantiate(RedEnemy, spawnPosition, Quaternion.identity);
                
                EnemiesSpawnedPerSwarm++;
                _totalEnemiesSwarmed++;
                _spawnDeltaTime = SpawnTime;
            }

            if (_timeBetweenSpawns <= 0)
                EnemiesSpawnedPerSwarm = 0;




        }

        private void SpawnPlayer()
        {
            GameObjectController.Instantiate(Player1, new Vector3(0, 0), Quaternion.identity, 0);
        }

        #endregion

        #region [ Monobehaviors ]

        private void OnPlayerConnected(NetworkPlayer player)
        {
            Debug.Log("Player Connected");
            _isGameStarted = true;
        }

        /// <summary>
        /// Specifically used when a client connects to a server
        /// </summary>
        private void OnConnectedToServer()
        {
            Debug.Log("Connected to Server");
            Network.Instantiate(Player2, new Vector3(3, 0), Quaternion.identity, 0);
        }

        private void OnServerInitialized()
        {
            SpawnPlayer();
        }

        void Start()
        {
            _btnX = Screen.width * 0.01f;
            _btnY = Screen.width * 0.01f;
            _btnW = 100f;
            _btnH = 50f;
            _spawnDeltaTime = SpawnTime;
            _timeBetweenSpawns = TimeBetweenSpawns;
            LevelManager.SetActive(false);
            FillFormationPositions();
        }


        void OnGUI()
        {
            if (Network.isClient || Network.isServer) return;

            var startSinglePlayer = GUI.Button(new Rect(_btnX, _btnY, _btnW, _btnH), "Start New Game");
            var startMultiplayer = GUI.Button(new Rect(_btnX, _btnY + 100, _btnW, _btnH), "Start Multiplayer Game");
            var joinGame = GUI.Button(new Rect(_btnX, _btnY + 200f, _btnW, _btnH), "Join Game");

            if (startMultiplayer)
            {
                NetworkManager.GetComponent<NetworkManager>().StartServer();
                _gameType = GameType.Multiplayer;
                ;
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



        void Update()
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
                StartCoroutine(NewLevel());
            }
        }

        #endregion

        #region [ Private Class ]

        private IEnumerator NewLevel()
        {
            //Application.LoadLevel(0);
            LevelManager.SetActive(true);
            EnemiesKilled = 0;
            EnemiesSpawnedPerSwarm = 0;
            _totalEnemiesSwarmed = 0;
            LevelManager.GetComponent<TextMesh>().text = string.Format(_levelText, ++_level);
            yield return new WaitForSeconds(1);
            LevelManager.SetActive(false);

            SwarmEnemy();

        }

        public void FillFormationPositions()
        {
            Positions = new List<Stack<int>>();
            for (int i = 0; i < 3; i++)
            {
                var temp = new Stack<int>();
                for (var j = 10; j > 0; j--)
                {
                    if (i > 0)
                    {
                        temp.Push(-j);
                    }
                    temp.Push(j);
                }
                Positions.Add(temp);
            }
        }
        #endregion
    }


}

