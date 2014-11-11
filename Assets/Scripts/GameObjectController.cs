using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class GameObjectController
    {
        private static int _internalCounter;

        public static GameObject Instantiate(GameObject objectToInstantiate, Vector3 position, Quaternion rotation, int group = 0, bool ignoreServerCheck = false)
        {
            GameObject ret = null;

            if (IsConnected())
            {
                if (Network.isServer || ignoreServerCheck)
                    ret = Network.Instantiate(objectToInstantiate, position, rotation, group) as GameObject;

            }
            else
                ret = Object.Instantiate(objectToInstantiate, position, rotation) as GameObject;

            if (ret == null) return null;

            var name = ret.name.Replace("clone", string.Empty);
            ret.name = string.Format("{0} - #{1}  NetworkView - {2}", name, _internalCounter++, ret.networkView != null ? ret.networkView.viewID.ToString() : "None");

            return ret;
        }

        public static void Destroy(GameObject objectToDestroy, bool ignoreServerCheck = false)
        {
            Debug.Log(string.Format("GameObject {0} is about to be destroyed", objectToDestroy.name));

            if (IsConnected())
            {
                if ((Network.isServer || ignoreServerCheck) && objectToDestroy.networkView != null)
                {
                    Network.Destroy(objectToDestroy.networkView.viewID);
                    Debug.Log(string.Format("GameObject {0} IsServer True - Network Destroyed", objectToDestroy.name));
                }
                //else
                //{
                //    Object.Destroy(objectToDestroy);
                //    Debug.Log(string.Format("GameObject {0} IsServer False IsConnected True - Destroyed", objectToDestroy.name));
                //}
            }
            else
                Object.Destroy(objectToDestroy);
        }

        public static bool IsConnected()
        {

            switch (Network.peerType)
            {
                case NetworkPeerType.Disconnected:
                    return false;
                case NetworkPeerType.Server:
                case NetworkPeerType.Client:
                case NetworkPeerType.Connecting:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }


    }
}
