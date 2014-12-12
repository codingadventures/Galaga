using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByContact : MonoBehaviour
    {

        public GameObject Explosion;
        public GameObject PlayerExplosion;
        private GameController _gameControllerObject;
        public int ScoreValue;

        void Start()
        {
            var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

            if (gameControllerObject != null)
            {
                _gameControllerObject = gameControllerObject.GetComponent<GameController>();
            }
            if (_gameControllerObject == null)
            {
                Debug.Log("Cannot find 'GameController' script");
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("WorldBoundary") || other.tag == "Enemy" || other.tag == "EnemyRed" || other.tag == "EnemyBolt")
                return;

            try
            {
                if (other.tag.Equals("Player") && (gameObject.tag.Equals("EnemyBolt") || gameObject.tag.Equals("Enemy") || gameObject.tag.Equals("EnemyRed")))
                {
                    GameObjectController.Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);
                    GameObjectController.Destroy(gameObject);
                    GameObjectController.Destroy(other.gameObject);
                    Debug.Log(string.Format("Destroyed By Contact - {0} Collided with {1}", gameObject.name, other.gameObject.name));

                }

                Debug.Log(other.name);

                if ((gameObject.tag.Equals("Enemy") || gameObject.tag.Equals("EnemyRed")) && other.tag.Equals("Bolt"))
                {
                    _gameControllerObject.EnemiesKilled++;
                    _gameControllerObject.AddScore(ScoreValue);
                }

                if (Explosion != null)
                    GameObjectController.Instantiate(Explosion, transform.position, transform.rotation);


                GameObjectController.Destroy(gameObject);
                GameObjectController.Destroy(other.gameObject);
                Debug.Log(string.Format("Destroyed By Contact - {0} Collided with {1}", gameObject.name, other.gameObject.name));
               
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
