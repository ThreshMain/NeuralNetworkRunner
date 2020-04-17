using System;

namespace NeuralNetwork
{
    [Serializable]
    public class Settings
    {
        public int newOnes = 10, copyOfBest = 75, cameraHight = 520, countOfDummys = 200,numberOfLayers=1;
        public float playerSpeed = 500.0f;
        [NonSerialized()] public float startSpeed = 500.0f;
        public double rotationEffect = 100;
        public bool camFolow = true, advanced = false,mute=false;
        public string server = "84.242.120.206";
        public int port = 5455;
        public int generationNumber = 0;
        public int sesionID=-1;
    }
}
