
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class SwarmFormation : MonoBehaviour
    {
        private Spline Spline;
        public List<GameObject> ITweenPathGameObjects;
        private bool _isSwarming;

        
        iTweenPath iTweenPath
        {
            get
            {
                var random = new System.Random();
                var splineNumber = random.Next(0, 2);
                return ITweenPathGameObjects[splineNumber].GetComponent<iTweenPath>();
            }
        }



        private GameController _gameControllerObject;

        void Start()
        {
            float t = 0;


            var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");

            if (gameControllerObject != null)
            {
                _gameControllerObject = gameControllerObject.GetComponent<GameController>();
            }
            if (_gameControllerObject == null)
            {
                Debug.Log("Cannot find 'GameController' script");
            }

            Spline = new Spline();
            var path = iTweenPath;
            Spline.AddKeyframe(-1, path.nodes[0]);

            foreach (var vector3 in path.nodes)
            {
                Spline.AddKeyframe(t++, vector3);
            }
            Spline.AddKeyframe(t, iTweenPath.nodes[path.nodes.Count - 1]);
        }


        private void Update()
        {
            if (GameObjectController.IsConnected() && !Network.isServer) return;


            gameObject.transform.position = Spline.GetPosition();
            Spline.Update(Time.deltaTime);


        }
    }
}
