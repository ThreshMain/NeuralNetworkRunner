using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace NeuralNetwork
{
    public class GameCotroller : MonoBehaviour
    {
        public GameObject player;
        public GameObject reference;
        public int newOnes=5, copyOfBest=10,cameraHight = 520, countOfDummys = 50;
        public float playerSpeed = 150.0f;
        public double rotationEffect = 100;
        public bool camFolow = true,advanced = false;
        public GameObject lineGeneratorPrefab;

        private List<GameObject> pathsOfTheBest=new List<GameObject>();
        private int numberToKeep = 20;

        float startSpeed;
        string progressFile = "Neural.json";
        string scoreFile = "scores.csv";
        string settingsFile = "settings.conf";
        int numberOfFinished = 0;
        int generationNumber = 0;
        double bestScrore = 0;
        double avgScoreIG = 0;
        float speedSliderValue = 1;
        float cameraSliderValue = 1;
        bool mute=false;

        AiLearning ai;
        Material redMaterial;
        Material normalMaterial;
        CameraFolow cam;
        List<GameObject> players = new List<GameObject>();
        AudioSource loop;

        private void FixedUpdate()
        {
            players.Sort((a,b)=> b.GetComponent<PlayerMovement>().CompareTo(a.GetComponent<PlayerMovement>()));
            bestScrore=players[0].GetComponent<PlayerMovement>().devidedScore;
            //avgScoreIG = 0;
            //for (int i = 0; i < players.Count; i++)
            //{
            //    avgScoreIG += players[i].GetComponent<PlayerMovement>().devidedScore;
            //}
            //avgScoreIG = avgScoreIG / players.Count;
            avgScoreIG = players[0].GetComponent<PlayerMovement>().devidedScore;
            if (camFolow)
            {
                cam.move(players[0].transform.position + new Vector3(0, cameraHight*cameraSliderValue, 0));
            }
            if (bestScrore > 10)
            {
                SkinnedMeshRenderer render = players[0].GetComponentInChildren<SkinnedMeshRenderer>();
                render.sharedMaterial = redMaterial;
            }
        }
        private void SpawnLines(Vector3[] linePoints)
        {
            LineRenderer lRend;
            if (pathsOfTheBest.Count == numberToKeep)
            {
                Destroy(pathsOfTheBest[0]); 
                pathsOfTheBest.RemoveAt(0);
            }
            foreach(GameObject gm in pathsOfTheBest)
            {
                lRend = gm.GetComponent<LineRenderer>();
                lRend.widthMultiplier -= 0.1f;
            }
            GameObject currentPath = Instantiate(lineGeneratorPrefab);
            lRend = currentPath.GetComponent<LineRenderer>();
            lRend.positionCount = linePoints.Length;
            lRend.SetPositions(linePoints);
            pathsOfTheBest.Add(currentPath);
        }
        public static void log(string s)
        {
            Debug.Log(s);
        }
        public void finish()
        {
            numberOfFinished++;
            if (numberOfFinished == countOfDummys-1)
            {
                numberOfFinished = 0;
                SpawnLines(players[0].GetComponent<PlayerMovement>().path.ToArray());
                ai.learn(newOnes, copyOfBest);
                if (generationNumber % 50 == 0)
                {
                    saveNetwork();
                }
                generationNumber++;
                double avgScore = 0;
                double avgRotated = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = normalMaterial;
                    avgScore += ai.networks[i].score;
                    avgRotated += players[i].GetComponent<PlayerMovement>().rotated;
                }
                avgRotated /= (double)players.Count;
                apendToFile(bestScrore + "," + avgScore / players.Count + "," + players[0].GetComponent<PlayerMovement>().rotated + "," + avgRotated, scoreFile);
                sendDataAndStart();
            }
        }
        public void apendToFile(string s, string path)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(s);
            }
        }
        public void sendDataAndStart()
        {
            for (int i = 0; i < countOfDummys; i++)
            {
                players[i].GetComponent<PlayerMovement>().dataIn(ai.networks[i], reference.transform.position, reference.transform.rotation,playerSpeed/startSpeed,rotationEffect*100);
            }
            for (int i = 0; i < countOfDummys; i++)
            {
                players[i].GetComponent<PlayerMovement>().startMoving(playerSpeed);
            }
        }

        void Start()
        {
            File.Delete(progressFile);
            File.Delete(settingsFile);
            File.Delete(scoreFile);
            startSpeed = playerSpeed;
            cam = GameObject.FindObjectOfType<Camera>().GetComponent<CameraFolow>();
            redMaterial = (Material)Resources.Load("RedDummy", typeof(Material));
            normalMaterial = (Material)Resources.Load("Dummy", typeof(Material));
            ai = new AiLearning(countOfDummys, 1, 5);
            for (int i = 0; i < countOfDummys; i++)
            {
                    players.Add(GameObject.Instantiate(player, reference.transform.position, reference.transform.rotation));
                players[i].name = "Clone-" + i;
            }
            sendDataAndStart();
            #region Music
            AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
            AudioSource start=null;
            for (int i = 0; i < audios.Length; i++)
            {
                if (audios[i].name == "Start")
                {
                    start = audios[i];
                }
                if (audios[i].name == "loop")
                {
                    loop = audios[i];
                }
            }
            start.Play(0);
            loop.PlayDelayed(start.clip.length - 1);
            #endregion
            apendToFile(JsonUtility.ToJson(this), settingsFile);
        }
        public void start()
        {
            loadSettings();
            loadAi();
        }
        async void loadSettings()
        {
            if (File.Exists(settingsFile))
            {
                using (StreamReader reader = new StreamReader(settingsFile))
                {
                    //JsonUtility.ToJson(this)
                   this.joinSettings(JsonUtility.FromJson<GameCotroller>(await reader.ReadToEndAsync()));
                }
            }
        }
        void joinSettings(GameCotroller settings)
        {
            newOnes = settings.newOnes;
            copyOfBest = settings.copyOfBest;
            cameraHight = settings.cameraHight;
            countOfDummys = settings.countOfDummys;
            playerSpeed = settings.playerSpeed;
            rotationEffect = settings.rotationEffect;
        }
        async void loadAi()
        {
            if (File.Exists(progressFile))
            {
                using (StreamReader reader = new StreamReader(progressFile))
                {
                    ai = JsonUtility.FromJson<AiLearning>(await reader.ReadToEndAsync());
                }
            }
        }
        void saveNetwork()
        {
            using (StreamWriter outputFile = new StreamWriter(progressFile))
            {
                outputFile.WriteLine(JsonUtility.ToJson(ai));
            }
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 50, 100, 30), "Save procces."))
            {
                saveNetwork();
            }
            if (GUI.Button(new Rect(10, 90, 100, 30),"NextGen."))
            {
                generationNumber++;
                numberOfFinished = 0;
                sendDataAndStart();
            }
            GUI.Label(new Rect(10, 150, 400, 80), generationNumber + ".GEN");
            GUI.Label(new Rect(250, 10, 400, 80), bestScrore + ".BEST LAST");
            GUI.Label(new Rect(250, 50, 400, 80), avgScoreIG + ".Avg");
            speedSliderValue = GUI.HorizontalSlider(new Rect(10, 10, 100, 20), speedSliderValue, 0.1f, 1);
            cameraSliderValue = GUI.HorizontalSlider(new Rect(10, 30, 100, 20), cameraSliderValue, 0.1f, 1);
            playerSpeed = startSpeed * speedSliderValue;
            camFolow = GUI.Toggle(new Rect(10, 125, 100, 30), camFolow, "Cam folow");
            mute = GUI.Toggle(new Rect(10, 180, 100, 30), mute, "Sound mute");
            loop.mute = mute;
        }
    }
}