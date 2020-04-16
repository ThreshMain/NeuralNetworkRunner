using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace NeuralNetwork
{
    public class GameCotroller : MonoBehaviour
    {
        public GameObject player;
        public GameObject reference;
        public GameObject lineGeneratorPrefab;
        public Settings setting;

        private List<GameObject> pathsOfTheBest=new List<GameObject>();
        private int numberToKeep = 20;

        string progressFile = "Exports\\Neural.json";
        string settingsFile = "Exports\\settings.conf";
        int numberOfFinished = 0;
        double bestScrore = 0;
        double avgScore = 0;
        float speedSliderValue = 1;
        float cameraSliderValue = 1;
        int indexOfTheBest = 0;


        TcpClient _connection;
        StreamWriter _connectionWriter=null;
        StreamReader _connectionReader = null;
        AiLearning ai;
        Material redMaterial;
        Material normalMaterial;
        CameraFolow cam;
        List<GameObject> players = new List<GameObject>();
        AudioSource loop;

        /// <summary>
        /// Called once at the start of the game
        /// </summary>
        void Start()
        {
            loadSettingsAndAi();
            cam = GameObject.FindObjectOfType<Camera>().GetComponent<CameraFolow>();
            redMaterial = (Material)Resources.Load("RedDummy", typeof(Material));
            normalMaterial = (Material)Resources.Load("Dummy", typeof(Material));
            #region Music
            AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
            AudioSource start = null;
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
        }
        /// <summary>
        /// On end of the program
        /// </summary>
        void OnTriggerExit()
        {
            _connection.Close();
        }
        /// <summary>
        /// Load settings then load Ai
        /// </summary>
        async void loadSettingsAndAi()
        {
            await loadSettings();
            _connection = new TcpClient();
            _connection.Connect(setting.server, setting.port);
            Debug.Log(setting.server + ":" + setting.port);
            _connectionWriter = new StreamWriter(_connection.GetStream());
            _connectionReader = new StreamReader(_connection.GetStream());
            await loadAi();
        }
        /// <summary>
        /// Loads progress from file with all networks
        /// </summary>
        async Task loadAi()
        {
            if (File.Exists(progressFile))
            {
                using (StreamReader reader = new StreamReader(progressFile))
                {
                    ai = JsonUtility.FromJson<AiLearning>(await reader.ReadToEndAsync());
                    if (ai.numberOfNetworks == setting.countOfDummys) {
                        Debug.Log("Loaded process");
                    }
                    else
                    {
                        ai = new AiLearning(setting.countOfDummys, setting.numberOfLayers, 5);
                        Debug.Log("Wrong number of networks");
                    }
                }
            }
            else
            {
                ai = new AiLearning(setting.countOfDummys, setting.numberOfLayers, 5);
            }
            for (int i = 0; i < setting.countOfDummys; i++)
            {
                players.Add(GameObject.Instantiate(player, reference.transform.position, reference.transform.rotation));
                players[i].name = "Clone-" + i;
            }
            sendDataAndStart();
        }
        /// <summary>
        /// Loads settigns from file if file exist,otherwise writes current settings (default) to the file
        /// </summary>
        async Task loadSettings()
        {
            if (File.Exists(settingsFile))
            {
                using (StreamReader reader = new StreamReader(settingsFile))
                {
                    setting = JsonUtility.FromJson<Settings>(await reader.ReadToEndAsync());
                }
                File.WriteAllText(settingsFile, JsonUtility.ToJson(setting));
                setting.startSpeed = setting.playerSpeed;
            }
            else
            {
                setting.sesionID = Node.rand.Next(0, 999999999);
                File.WriteAllText(settingsFile, JsonUtility.ToJson(setting));
            }
        }
        /// <summary>
        /// Called each 1/n of settings based on project settings
        /// </summary>
        private void FixedUpdate()
        {
            if (ai != null)
            {
                indexOfTheBest = 0;
                avgScore = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    avgScore += players[i].GetComponent<PlayerMovement>().devidedScore;
                    if (players[i].GetComponent<PlayerMovement>().devidedScore > players[indexOfTheBest].GetComponent<PlayerMovement>().devidedScore)
                    {
                        indexOfTheBest = i;
                    }
                }
                avgScore /= (double) players.Count;
                bestScrore = players[indexOfTheBest].GetComponent<PlayerMovement>().devidedScore;
                if (setting.camFolow)
                {
                    cam.move(players[indexOfTheBest].transform.position + new Vector3(0, setting.cameraHight * cameraSliderValue, 0));
                }
                if (players[indexOfTheBest].GetComponent<PlayerMovement>().devidedScore > 10)
                {
                    SkinnedMeshRenderer render = players[indexOfTheBest].GetComponentInChildren<SkinnedMeshRenderer>();
                    render.sharedMaterial = redMaterial;
                }
            }
        }
        /// <summary>
        /// Spawn visual continuos line created from list of Vector3
        /// </summary>
        /// <param name="linePoints"></param>
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
        /// <summary>
        /// Called by each player after he finish (Hit wall)
        /// </summary>
        public void finish()
        {
            numberOfFinished++;
            // When all player already finished
            if (numberOfFinished == setting.countOfDummys-1)
            {
                numberOfFinished = 0;

                // Calculate data for analytics
                double avgScore = 0;
                double avgRotated = 0;
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = normalMaterial;
                    avgScore += ai.networks[i].score;
                    avgRotated += players[i].GetComponent<PlayerMovement>().rotated;
                }
                avgRotated /= (double)players.Count;
                avgScore /= (double)players.Count;

                // Draw lines for the best one
                SpawnLines(players[indexOfTheBest].GetComponent<PlayerMovement>().path.ToArray());

                string insert = "INSERT INTO `PlayerData` (`SesionID`,`Clone_Number`,`Score`, `ScoreGrow`, `Dead_X`, `Dead_Y`, `Generation_number`) VALUES ";
                if (ai.old!=null)
                {
                    for (int index = 0; index < ai.networks.Count; index++)
                    {
                        insert +=(string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}),", setting.sesionID, index, ai.networks[index].score, ai.networks[index].score - ai.old[index].score, ai.networks[index].dead.x, ai.networks[index].dead.y, setting.generationNumber));
                    }
                    insert = insert.Remove(insert.Length - 1, 1) + ";";
                    _connectionWriter.WriteLine(insert);
                    _connectionWriter.Flush();
                    if (_connectionReader.ReadLine().Trim() != ai.networks.Count.ToString())
                    {
                        Debug.Log("Some thing went wrong when inserting into DB");
                    }
                }
               
                // Start ai learning
                ai.learn(setting.newOnes, setting.copyOfBest);

                // Each 50th cycle save progress
                if (setting.generationNumber % 50 == 0 && setting.generationNumber > 0)
                {
                    saveNetwork();
                }
                setting.generationNumber++;

                if (avgScore == 0)
                {
                    Debug.LogError("Some thing went wrong");
                    startNewGen();
                }
                else
                {
                    sendDataAndStart();
                }
            }
        }
        /// <summary>
        /// Simple method to append to file
        /// </summary>
        /// <param name="s">
        /// String to append to file
        /// </param>
        /// <param name="path">
        /// Path to the file
        /// </param>
        public void apendToFile(string s, string path)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(s);
            }
        }
        /// <summary>
        /// Initializate all player and start theyir movement
        /// </summary>
        public void sendDataAndStart()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerMovement>().dataIn(ai.networks[i], reference.transform.position, reference.transform.rotation,setting.playerSpeed/setting.startSpeed, setting.rotationEffect);
            }
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerMovement>().startMoving(setting.playerSpeed);
            }
        }
        /// <summary>
        /// Force start of new genration from user or system when error is found
        /// </summary>
        public void startNewGen()
        {
            setting.generationNumber++;
            numberOfFinished = 0;
            sendDataAndStart();
        }
        /// <summary>
        /// Saves progress to file with all networks
        /// </summary>
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
                startNewGen();
            }
            GUI.Label(new Rect(10, 150, 400, 80), setting.generationNumber + ".GEN");
            GUI.Label(new Rect(250, 10, 400, 80), bestScrore + ".BEST LAST");
            GUI.Label(new Rect(250, 50, 400, 80), avgScore + ".Avg");
            speedSliderValue = GUI.HorizontalSlider(new Rect(10, 10, 100, 20), speedSliderValue, 0.1f, 1);
            cameraSliderValue = GUI.HorizontalSlider(new Rect(10, 30, 100, 20), cameraSliderValue, 0.1f, 1);
            setting.playerSpeed = setting.startSpeed * speedSliderValue;
            setting.camFolow = GUI.Toggle(new Rect(10, 125, 100, 30), setting.camFolow, "Cam folow");
            setting.mute = GUI.Toggle(new Rect(10, 180, 100, 30), setting.mute, "Sound mute");
            loop.mute = setting.mute;
        }


    }
}