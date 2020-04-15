using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class CameraFolow : MonoBehaviour
    {
        void Start()
        {
            //defaultPos = transform.position;
        }
        public void move(Vector3 defaultPos)
        {
            transform.position = defaultPos;
        }
        // LateUpdate is called after Update each frame
        void LateUpdate()
        {
            //if (offset != null)
            ///{
            //    transform.position = player.transform.position + offset;
            //}
        }
    }
}