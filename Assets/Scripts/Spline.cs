using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Spline
    {
        private readonly bool _cycle;
        //public static event EventHandler SplineEnd;

        //private static void OnSplineEnd()
        //{
        //    EventHandler handler = SplineEnd;
        //    if (handler != null) handler(null, EventArgs.Empty);
        //}
        public bool End;

        private struct KeyFrame
        {
            public float Time { get; private set; }
            public Vector3 Position { get; private set; }


            public KeyFrame(Vector3 position, float time)
                : this()
            {
                Position = position;
                Time = time;
            }
        }

        readonly List<KeyFrame> _keyframes;
        float _timer;

        public Spline(bool cycle = false)
        {
            _cycle = cycle;
            _keyframes = new List<KeyFrame>();
        }


        public void AddKeyframe(float t, Vector3 keyFramePosition)
        {
            _keyframes.Add(new KeyFrame(keyFramePosition, t));
        }

        public Vector3 GetPosition()
        {
            var reset = true;
            var prevKey = 1;
            var nextKey = 1;

            //Find the two keyframes
            for (var keyidx = 1; keyidx < _keyframes.Count - 2; keyidx++)
            {
                prevKey = keyidx;
                nextKey = keyidx + 1;

                if (!(_keyframes[nextKey].Time >= _timer)) continue;
                reset = false;
                break;
            }

            if (!_cycle && reset)
            {
                End = true;
                return _keyframes[nextKey].Position;
            }

            if (reset)
            {
                prevKey = 1;
                nextKey = 2;
                _timer = 0;
            }


            var timeBetweenKeys = _keyframes[nextKey].Time - _keyframes[prevKey].Time;
            var t = (_timer - _keyframes[prevKey].Time) / timeBetweenKeys;

            return CubicLerp(_keyframes[prevKey - 1].Position, _keyframes[prevKey].Position, _keyframes[nextKey].Position, _keyframes[nextKey + 1].Position, t);
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;
        }

        private static Vector3 CubicLerp(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, float t)
        {
            var t2 = t * t;
            var a0 = v3 - v2 - v0 + v1;
            var a1 = v0 - v1 - a0;
            var a2 = v2 - v0;
            var a3 = v1;

            return (a0 * t * t2 + a1 * t2 + a2 * t + a3);
        }
    }

}
