using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace OctopusController
{
    public enum TentacleMode { LEG, TAIL, TENTACLE };

    public class MyOctopusController
    {

        MyTentacleController[] _tentacles = new MyTentacleController[4];

        Transform _currentRegion;
        Transform _target;


        Transform[] _randomTargets;// = new Transform[4];



        Vector3 targetPosition;
        Vector3 targetRegion;
        float sqrDistance;
        bool random = false;
        float time = 3f;

        float _twistMin, _twistMax;
        float _swingMin, _swingMax;

        #region public methods
        //DO NOT CHANGE THE PUBLIC METHODS!!

        public float TwistMin { set => _twistMin = value; }
        public float TwistMax { set => _twistMax = value; }
        public float SwingMin { set => _swingMin = value; }
        public float SwingMax { set => _swingMax = value; }


        public void TestLogging(string objectName)
        {


            UnityEngine.Debug.Log("hello, I am initializing my Octopus Controller in object " + objectName);


        }

        public void Init(Transform[] tentacleRoots, Transform[] randomTargets)
        {
            _tentacles = new MyTentacleController[tentacleRoots.Length];


            // foreach (Transform t in tentacleRoots)
            for (int i = 0; i < tentacleRoots.Length; i++)
            {

                _tentacles[i] = new MyTentacleController();
                _tentacles[i].LoadTentacleJoints(tentacleRoots[i], TentacleMode.TENTACLE);
                //TODO: initialize any variables needed in ccd
            }

            _randomTargets = randomTargets;
            //TODO: use the regions however you need to make sure each tentacle stays in its region
            for (int i = 0; i < tentacleRoots.Length; i++)
            {
                // _tentacles[i].Bones[1].position
            }


        }


        public void NotifyTarget(Transform target, Transform region)
        {
            _currentRegion = region;
            _target = target;
            targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
            targetRegion = new Vector3(region.position.x, region.position.y, region.position.z);
            UnityEngine.Debug.Log("notify target");

        }

        public void NotifyShoot()
        {
            //TODO. what happens here?
            random = true;

            UnityEngine.Debug.Log("Shoot");
        }


        public void UpdateTentacles()
        {
            //TODO: implement logic for the correct tentacle arm to stop the ball and implement CCD method

            if (random)
            {
                for (int i = 0; i < _tentacles.Length; i++)
                {
                    NotifyTarget(_target, _randomTargets[i].parent);
                }

            }


            if (random && (time - Time.deltaTime <= 0))
            {
                random = false;
                time = 3;

            }

            update_ccd();
        }




        #endregion


        #region private and internal methods
        //todo: add here anything that you need

        public static void RotateJoint(Transform bone, float angle, Vector3 targetDirection)
        {
            if (targetDirection == Vector3.up)
            {
                bone.transform.Rotate(Vector3.up, angle);
            }
            else if (targetDirection == Vector3.down)
            {
                bone.transform.Rotate(Vector3.down, angle);
            }
            else if (targetDirection == Vector3.left)
            {
                bone.transform.Rotate(Vector3.left, angle);
            }
            else if (targetDirection == Vector3.right)
            {
                bone.transform.Rotate(Vector3.right, angle);
            }
            else
            {
                bone.transform.Rotate(targetDirection, angle);
            }

        }

        void update_ccd()
        {

            for (int i = 0; i < _tentacles.Length; i++)
            {
                MyTentacleController tentacle = _tentacles[i];

                NotifyTarget(_randomTargets[i], _randomTargets[i].parent);
                Vector3 currentPosition = tentacle.GetEffector.position;
                Vector3 targetDirection = _target.position - currentPosition;


                for (int j = tentacle.Bones.Length - 1; j >= 0; j--)
                {

                    Vector3 jointPosition = tentacle.Bones[j].position;
                    Vector3 jointToTarget = _target.position - jointPosition;


                    float angle = Vector3.Angle(jointToTarget, targetDirection);


                    RotateJoint(tentacle.Bones[j], angle, targetDirection);
                }
            }
        }




        #endregion






    }
}

