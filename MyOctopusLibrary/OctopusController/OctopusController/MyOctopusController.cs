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


            Debug.Log("hello, I am initializing my Octopus Controller in object " + objectName);


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

            for (int i = 0; i < _tentacles.Length; i++)
            {
                NotifyTarget(_randomTargets[i], _randomTargets[i].parent);

                Vector3 goalPosition = Vector3.Lerp(targetRegion, targetPosition, 1f);

                for (int j = 0; j < _tentacles[i].Bones.Count() - 2; j++)
                {
                    for (int k = 1; k < j + 3 && k < _tentacles[i].Bones.Count(); k++)
                    {
                        RotateBone(_tentacles[i].Bones[0], _tentacles[i].Bones[k], goalPosition);

                        sqrDistance = (_tentacles[0].Bones[0].position - goalPosition).sqrMagnitude;
                    }
                }


            }

        }


        public void NotifyTarget(Transform target, Transform region)
        {
            _currentRegion = region;
            _target = target;
            targetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
            targetRegion = new Vector3(region.position.x, region.position.y, region.position.z);


        }

        public void NotifyShoot()
        {
            //TODO. what happens here?
            Debug.Log("Shoot");
        }


        public void UpdateTentacles()
        {
            //TODO: implement logic for the correct tentacle arm to stop the ball and implement CCD method
            update_ccd();
        }




        #endregion


        #region private and internal methods
        //todo: add here anything that you need

        public static void RotateBone(Transform effector, Transform bone, Vector3 targetPosition)
        {
            Vector3 effectorPosition = effector.position;
            Vector3 bonePosition = bone.position;
            Quaternion boneRotation = bone.rotation;

            Vector3 boneToEffector = effectorPosition - bonePosition;
            Vector3 boneToTarget = targetPosition - bonePosition;

            Quaternion fromToRotation = Quaternion.FromToRotation(boneToEffector, boneToTarget);
            Quaternion newRotation = fromToRotation * boneRotation;

            boneRotation = newRotation;
        }

        void update_ccd()
        {


        }




        #endregion






    }
}