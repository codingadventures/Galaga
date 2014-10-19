using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByContact : MonoBehaviour
    {

        public GameObject Explosion;
        public GameObject PlayerExplosion;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("WorldBoundary"))
                return;

            if (other.tag.Equals("Player"))
                Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);

            Instantiate(Explosion, transform.position, transform.rotation);
            Debug.Log(other.name);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }



    }
}
