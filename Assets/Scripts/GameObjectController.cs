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

        public static GameObject Instantiate(GameObject objectToInstantiate, Vector3 position, Quaternion rotation, int group = 0)
        {
            GameObject ret = null;

            switch (Network.peerType)
            {
                case NetworkPeerType.Disconnected: 
                    ret = Object.Instantiate(objectToInstantiate, position, rotation) as GameObject;
                    break;
                case NetworkPeerType.Server:
                case NetworkPeerType.Client:
                    ret = Network.Instantiate(objectToInstantiate, position, rotation, group) as GameObject;
                   
                    break;
                case NetworkPeerType.Connecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ret;
        }

        public static void Destroy(GameObject objectToDestroy)
        {
            switch (Network.peerType)
            {
                case NetworkPeerType.Disconnected:
                    Network.Destroy(objectToDestroy);
                    break;
                case NetworkPeerType.Server:
                case NetworkPeerType.Client:
                    Object.Destroy(objectToDestroy);
                    break;
                case NetworkPeerType.Connecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
