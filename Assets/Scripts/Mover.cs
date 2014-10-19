using UnityEngine;
using System.Collections;



namespace Assets.Scripts
{
    public class Mover : MonoBehaviour
    {
        public float Speed;

        /// <summary>
        /// Starts this instance. This method will be executed at the very first frame
        /// </summary>
        void Start()
        {
            rigidbody.velocity = transform.forward * Speed;
        }

    }
}