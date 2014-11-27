using UnityEngine;

namespace Assets.Scripts
{
    public class WeaponController : MonoBehaviour
    {
        public GameObject Shot;
        public Transform ShotSpawn;
        public float FireRate;
        public float Delay;

        void Start()
        {
            //InvokeRepeating("Fire", Delay, FireRate);
        }

        void Fire()
        {
            GameObjectController.Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
            audio.Play();
        }
    }
}
