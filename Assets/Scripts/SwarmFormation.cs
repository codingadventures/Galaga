
using UnityEngine;

namespace Assets.Scripts
{
    public class SwarmFormation : MonoBehaviour
    {
        private Spline Spline;

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
            //Spline = new Spline();

            //Spline.AddKeyframe(-1f, gameObject.transform.position);

            //Spline.AddKeyframe(0f, gameObject.transform.position);

            //Vector3 point1 = gameObject.transform.position;
            //point1.x -= 3 * Mathf.Sign(point1.x);
            //point1.z -= 4;
            //Spline.AddKeyframe(1f, point1);

            //Vector3 point2 = gameObject.transform.position;
            //point2.x -= 8 * Mathf.Sign(point2.x);
            //point2.z -= 8;
            //Spline.AddKeyframe(2f, point2);

            //Vector3 point3 = gameObject.transform.position;
            //point3.x -= 3 * Mathf.Sign(point3.x);
            //point3.z -= 11;
            //Spline.AddKeyframe(3f, point3);

            //Vector3 point4 = gameObject.transform.position;
            //point4.x = 0;
            //point4.z = 17;
            //Spline.AddKeyframe(6f, point4);
            //Spline.AddKeyframe(5f, gameObject.transform.position);
            //Spline.AddKeyframe(7f, gameObject.transform.position);
        }


        private void FixedUpdate()
        {
            if (_gameControllerObject.EnemiesSpawned % 10 == 0)
            {
                _angleOffset = _animationPath[_pathCnt];
                _pathCnt++;
                if (_pathCnt > _animationPath.Length - 1)
                {
                    _pathCnt = 0;
                }
            }

            _angle += _angleOffset;

            var angleInRad = Mathf.Deg2Rad * _angle;

            var speed = new Vector3
            {
                x = Mathf.Sin(angleInRad) * 5,
                z = -Mathf.Cos(angleInRad) * 5,
                y = 0
            };


            var velocity = new Vector3(speed.x, 0.0f, speed.z);

            gameObject.transform.position = new Vector3
            (
                Mathf.Clamp(velocity.x, -16, 16),
                0.0f,
                Mathf.Clamp(velocity.z, -20, 40)
            );

        }


        private void Update()
        {

        }
    }
}
