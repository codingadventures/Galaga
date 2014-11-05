using System;
using System.Collections;
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
        public GameObject PlayerGameObject; 
        public GameObject Hazard;
        public Transform SpawnValues;
        public bool IsGameStarted;
        public float SpawnTime;

        #endregion

        #region [ Private Fields  ]

        private float _btnX, _btnY, _btnW, _btnH;
        private float _spawnDeltaTime;
        private GameType _gameType;
        #endregion
         

       
      

        #region [ Private Methods ]

        private void SwarmAsteroid()
        {

            _spawnDeltaTime -= Time.deltaTime;

            if (_spawnDeltaTime <= 0)
            {
                for (var i = 0; i < 2; i++)
                {
                    var spawnPosition = new Vector3(Random.Range(-SpawnValues.position.x, SpawnValues.position.x),
                        SpawnValues.position.y, SpawnValues.position.z);
                    GameObjectController.Instantiate(Hazard, spawnPosition, Quaternion.identity, 0);
                }
                _spawnDeltaTime = SpawnTime;
            }

        }

        private void SpawnPlayer()
        {
            GameObjectController.Instantiate(PlayerGameObject, new Vector3(0, 0), Quaternion.identity, 0);
        }

        #endregion

        #region [ Monobehaviors ]

        void OnPlayerConnected(NetworkPlayer player)
        {
            Debug.Log("Player Connected");
            IsGameStarted = true;
        }


        void OnConnectedToServer()
        {
            Debug.Log("Connected to Server");
            Network.Instantiate(PlayerGameObject, new Vector3(3, 0), Quaternion.identity, 0);
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
                IsGameStarted = true;
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
            if (!IsGameStarted) return;

            switch (_gameType)
            {
                case GameType.None:
                    break;
                case GameType.Single:
                    SwarmAsteroid();
                    break;
                case GameType.Multiplayer:
                    if (Network.isServer)
                    {
                        SwarmAsteroid();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
