using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    [Serializable]
    public class Settings
    {
        public int newOnes = 10, copyOfBest = 50, cameraHight = 520, countOfDummys = 100;
        public float playerSpeed = 500.0f;
        [NonSerialized()] public float startSpeed = 500.0f;
        public double rotationEffect = 100;
        public bool camFolow = true, advanced = false,mute=false;
    }
}
