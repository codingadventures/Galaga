using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        #region [ Public Fields ]
        public GameObject Hazard;
        public NetworkManager NetworkManager;
        public Transform SpawnValues;
        public bool IsGameStarted;
        #endregion

        #region [ Private Fields  ]

        float _btnX, _btnY, _btnW, _btnH;

        #endregion


        void Start()
        {
            NetworkManager = ScriptableObject.CreateInstance<NetworkManager>();
            _btnX = Screen.width * 0.01f;
            _btnY = Screen.width * 0.01f;
            _btnW = 100f;
            _btnH = 50f;
            StartCoroutine(SpawnWaves());
        }



        IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(1);
            if (!IsGameStarted) yield break;
            
            while (true)
            {
                yield return new WaitForSeconds(1);

                for (var i = 0; i < 3; i++)
                {
                    var spawnPosition = new Vector3(Random.Range(-SpawnValues.position.x, SpawnValues.position.x), SpawnValues.position.y, SpawnValues.position.z);
                    GameObjectController.Instantiate(Hazard, spawnPosition, Quaternion.identity, 0);
                    yield return new WaitForSeconds(1);
                }
            }
        }


        void OnGUI()
        {
            if (Network.isClient || Network.isServer) return;

            var startGame = GUI.Button(new Rect(_btnX, _btnY, _btnW, _btnH), "Start New Game");
            var joinGame = GUI.Button(new Rect(_btnX, _btnY + 100f, _btnW, _btnH), "Joing Game");

            if (startGame)
            {
                NetworkManager.StartServer();

                Debug.Log("Game/Server started");
                IsGameStarted = true;
            }

            if (joinGame)
            {
                NetworkManager.RefreshHost();
            }

            NetworkManager.GetAvailableGames(_btnX, _btnY, _btnW, _btnH);

        }


        void Update()
        {

        }
    }
}
