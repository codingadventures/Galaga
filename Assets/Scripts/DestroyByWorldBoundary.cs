using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyByWorldBoundary : MonoBehaviour
    {

        /// <summary>
        /// Called when an object stop touching the collider.
        /// </summary>
        /// <param name="other">The other's game object collider.</param>
        void OnTriggerExit(Collider other)
        {
            Destroy(other.gameObject);
        }


    }
}
