using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByContact : MonoBehaviour
    {

        public GameObject Explosion;
        public GameObject PlayerExplosion;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("WorldBoundary"))
                return;

            if (other.tag.Equals("Player"))
                GameObjectController.Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);

            try
            {
                GameObjectController.Instantiate(Explosion, transform.position, transform.rotation);
                Debug.Log(other.name);
                GameObjectController.Destroy(gameObject);
                GameObjectController.Destroy(other.gameObject);
            }
            catch (Exception e)
            {
                Debug.Break();
            }
        }



    }
}
