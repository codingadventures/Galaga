
using UnityEngine;

namespace Assets.Scripts
{
    public class SwarmFormation : MonoBehaviour
    {
        private Spline Spline;
        public GameObject ITweenPathGameObject;

        iTweenPath iTweenPath
        {
            get { return ITweenPathGameObject.GetComponent<iTweenPath>(); }
        }
        private float currentSpeed;
        int _angle = 0;
        int _angleOffset = 3;
        int _pathCnt = 0;
        private float _speedScalarX, _speedScalarY;
        bool _autoDestroy = true;

        readonly int[] _animationPath = { -5, -5, -5, 5, 5, 5, -5, 5, 3, 3, 5, 5, -5 };
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


            //Initialize Splines Path
            //start position, I have it
            //end z = 13, 14.5, 16.5
            Spline = new Spline();
            Spline.AddKeyframe(-1, iTweenPath.nodes[0]);

            foreach (var vector3 in iTweenPath.nodes)
            {
                Spline.AddKeyframe(t++, vector3);
            }
            Spline.AddKeyframe(t, iTweenPath.nodes[iTweenPath.nodes.Count - 1]);
        }


        private void FixedUpdate()
        {
            //if (_gameControllerObject.EnemiesSpawned % 10 == 0)
            //{
            //    _angleOffset = _animationPath[_pathCnt];
            //    _pathCnt++;
            //    if (_pathCnt > _animationPath.Length - 1)
            //    {
            //        _pathCnt = 0;
            //    }
            //}

            //_angle += _angleOffset;

            //var angleInRad = Mathf.Deg2Rad * _angle;

            //var speed = new Vector3
            //{
            //    x = Mathf.Sin(angleInRad) * 5,
            //    z = -Mathf.Cos(angleInRad) * 5,
            //    y = 0
            //};


            //var velocity = new Vector3(speed.x, 0.0f, speed.z);

            //gameObject.transform.position = new Vector3
            //(
            //    Mathf.Clamp(velocity.x, -16, 16),
            //    0.0f,
            //    Mathf.Clamp(velocity.z, -20, 40)
            //);

        }


        private void Update()
        {
            if (GameObjectController.IsConnected() && !Network.isServer) return;


            gameObject.transform.position = Spline.GetPosition();

            Spline.Update(Time.deltaTime);

        }
    }
}
