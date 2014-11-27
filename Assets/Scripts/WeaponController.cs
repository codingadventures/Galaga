using UnityEngine;

namespace Assets.Scripts
{
    public class WeaponController : MonoBehaviour
    {
        public GameObject Shot;
        public Transform ShotSpawn;
        public float FireRate;
        public float Delay;
        private SwarmFormation _swarmFormation;

        void Start()
        {
            _swarmFormation = FindObjectOfType<SwarmFormation>();
            InvokeRepeating("Fire", Delay, FireRate);

        }

        void Fire()
        {
            if (!_swarmFormation.InFormation) return;

            GameObjectController.Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
            audio.Play();
        }
    }
}
