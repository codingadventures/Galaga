using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByContact : MonoBehaviour
    {

        public GameObject Explosion;
        public GameObject PlayerExplosion;
        private GameController _gameControllerObject;

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
            if (other.tag.Equals("WorldBoundary") || other.tag == "Enemy")
                return;

            if (other.tag.Equals("Player"))
                GameObjectController.Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);

            try
            {
                if (Explosion != null)
                    GameObjectController.Instantiate(Explosion, transform.position, transform.rotation);
                
                Debug.Log(other.name);

                if (gameObject.tag.Equals("Enemy"))
                    _gameControllerObject.EnemiesKilled++;

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
