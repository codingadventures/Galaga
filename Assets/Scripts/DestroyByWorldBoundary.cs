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
            GameObjectController.Destroy(other.gameObject);
             
            Debug.Log(string.Format("Destroyed By World Boundary {3} - Position ({0},{1},{2}", other.transform.position.x, other.transform.position.y,
                other.transform.position.z, other.gameObject.name));

        }


    }
}
