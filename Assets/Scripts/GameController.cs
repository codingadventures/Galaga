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
        private Spline _spline;
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

        /// <summary>
        /// Specifically used when a client connects to a server
        /// </summary>
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

            //Initialize Splines Path
            //start position, I have it
            //end z = 13, 14.5, 16.5
            _spline = new Spline();
            _spline.AddKeyframe(0, SpawnValues.position);
            Vector3 point1 = SpawnValues.position;
            point1.x -= 3*Mathf.Sign(point1.x);
            point1.z -= 4;
            _spline.AddKeyframe(0.3f, point1);

            Vector3 point2 = SpawnValues.position;
            point2.x -= 8 * Mathf.Sign(point2.x);
            point2.z -= 8;

            _spline.AddKeyframe(0.6f, point2);

            Vector3 point3 = SpawnValues.position;
            point3.x -= 3 * Mathf.Sign(point3.x);
            point3.z -= 11;

            _spline.AddKeyframe(1.0f, point3);

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

            _spline.Update(Time.deltaTime);

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

        #region [ Private Class ]



        #endregion
    }
}
