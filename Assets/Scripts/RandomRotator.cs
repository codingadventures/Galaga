using UnityEngine;

namespace Assets.Scripts
{
    public class RandomRotator : MonoBehaviour
    {

        public float Tumble; 

        void Start()
        {
            rigidbody.angularVelocity = Random.insideUnitSphere*Tumble; 
        }

    }
}
