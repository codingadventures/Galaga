using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class DestroyByTime : MonoBehaviour
    {
        public float LifeTime;

        void Start()
        {
            Destroy(gameObject, LifeTime);
            Debug.Log("Object" + gameObject.name + " Destroyed by time");
        }
    }
}
