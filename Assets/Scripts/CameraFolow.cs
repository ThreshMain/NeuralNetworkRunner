using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class CameraFolow : MonoBehaviour
    {
        public void move(Vector3 defaultPos)
        {
            transform.position = defaultPos;
        }
    }
}