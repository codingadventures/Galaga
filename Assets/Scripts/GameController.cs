using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        void Start()
        {
            SpawnWaves();
        }

        public GameObject Hazard;
        public Vector3 SpawnValues;



        void SpawnWaves()
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-SpawnValues.x, SpawnValues.x), SpawnValues.y, SpawnValues.z);
            Instantiate(Hazard, spawnPosition, Quaternion.identity);

        }

    }
}
