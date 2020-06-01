using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;
        public static double maxRotate = 6;
        public float movementSpeed = 0;
        public float multiplier = 1f;
        public double score;
        public double devidedScore;
        public double rotationEffect = 360.0;
        public double rotated = 1;
        public bool done = true;
        public NeuralNetwork current;
        public List<Vector3> path = new List<Vector3>();
        RaycastHit forwardHit, rightHit, leftHit, forwardLeftHit, forwardRightHit;

        public void dataIn(NeuralNetwork nn, Vector3 pos, Quaternion rotation, float multiplier, double rotationEff)
        {
            this.rotationEffect = rotationEff;
            this.multiplier = multiplier;
            transform.position = pos;
            transform.rotation = rotation;
            current = nn;
            score = 1;
            devidedScore = 1;
            rotated = 1;
            path = new List<Vector3>();
        }
        public void startMoving(float speed)
        {
            movementSpeed = speed;
            done = false;
            maxRotate = movementSpeed / 400;
        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            if (movementSpeed > 0)
            {
                #region Rays
                Ray forwardRay = new Ray(transform.position, transform.TransformDirection(Vector3.forward));
                Ray rightRay = new Ray(transform.position, transform.TransformDirection(new Vector3(1, 0, 0)));
                Ray leftRay = new Ray(transform.position, transform.TransformDirection(new Vector3(-1, 0, 0)));
                Ray forwardLeftRay = new Ray(transform.position, transform.TransformDirection(new Vector3(-1, 0, 1)));
                Ray forwardRightRay = new Ray(transform.position, transform.TransformDirection(new Vector3(1, 0, 1)));
                #endregion
                #region RayCasts
                Physics.Raycast(forwardRay, out forwardHit);
                Physics.Raycast(rightRay, out rightHit);
                Physics.Raycast(leftRay, out leftHit);
                Physics.Raycast(forwardLeftRay, out forwardLeftHit);
                Physics.Raycast(forwardRightRay, out forwardRightHit);
                #endregion
                #region Lines
                Debug.DrawLine(transform.position, forwardHit.point, Color.red);
                Debug.DrawLine(transform.position, rightHit.point, Color.red);
                Debug.DrawLine(transform.position, leftHit.point, Color.red);
                Debug.DrawLine(transform.position, forwardLeftHit.point, Color.red);
                Debug.DrawLine(transform.position, forwardRightHit.point, Color.red);
                #endregion
                #region Data for Neural Network
                List<double> distances = new List<double>();
                distances.Add(forwardHit.distance);
                distances.Add(rightHit.distance);
                distances.Add(leftHit.distance);
                distances.Add(forwardLeftHit.distance);
                distances.Add(forwardLeftHit.distance);
                #endregion
                //Debug.Log("scor"+((forwardHit.distance * 2 - Math.Abs(rightHit.distance - leftHit.distance)*8 + forwardLeftHit.distance * 1.2 + forwardRightHit.distance * 1.2) / 250.0) * multiplier);
                score += Math.Max(0, (forwardHit.distance * 2 - Math.Abs(rightHit.distance - leftHit.distance)*8 + forwardLeftHit.distance + forwardRightHit.distance) * multiplier)/1000 + 20;
                //score += (forwardHit.distance + rightHit.distance + leftHit.distance + forwardLeftHit.distance + forwardRightHit.distance)/250;
                double moveHorizontal = Math.Min(maxRotate, Math.Max(-maxRotate, current.resoult(distances))) * multiplier;
                //double moveHorizontal = -maxRotate * multiplier;
                //Debug.Log("ROT:"+current.resoult(distances));
                rotated += Math.Abs(Convert.ToSingle(moveHorizontal)) / rotationEffect;
                Vector3 movement = transform.TransformDirection(Vector3.forward);
                transform.Rotate(0, Convert.ToSingle(moveHorizontal), 0);
                rb.velocity = (movement * movementSpeed);
                Vector3 linePoint = this.transform.position;
                linePoint.y += 2;
                path.Add(linePoint);
                devidedScore = score;// /rotated
                current.score = devidedScore;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.tag != "Ground" && collision.collider.gameObject.tag != "Player")
            {
                if (!done)
                {
                    current.dead = new Vector2(this.transform.position.x, this.transform.position.z);
                    GameObject.FindGameObjectWithTag("Ground").GetComponent<GameCotroller>().finish();
                    movementSpeed = 0;
                    current.score = devidedScore;
                    done = true;
                }
            }
        }
        public int CompareTo(PlayerMovement playerMovement)
        {
            return devidedScore.CompareTo(playerMovement.devidedScore);
        }
    }
}
