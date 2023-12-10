using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


namespace OctopusController
{
  
    public class MyScorpionController
    {
        //TAIL
        Transform tailTarget;
        Transform tailEndEffector;
        MyTentacleController _tail;
        float animationRange = 3.46f;
        float growthRate = 150.0f;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6];
        bool playingAnimation = false;
        float futureBaseDistance = 1f;
        Vector3[] virtualLegs;
        float[] legDistanceCheck;

        #region public
        public void InitLegs(Transform[] LegRoots,Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for(int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                //Initialize anything needed for the FABRIK implementation
                legFutureBases[i] = LegFutureBases[i];
                legTargets[i] = LegTargets[i];
            }
            virtualLegs = new Vector3[_legs[0].Bones.Length];
            legDistanceCheck = new float[_legs[0].Bones.Length - 1];

        }

        public void InitTail(Transform TailBase)
        {
            
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //Initialize anything needed for the Gradient Descent implementation
            tailEndEffector = _tail.Bones[_tail.Bones.Length - 1];
        }

        //Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
            tailTarget = target;
        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {
            playingAnimation = true;
        }

        //TODO: create the apropiate animations and update the IK from the legs and tail
        public void UpdateIK()
        {
            updateTail();

            Debug.Log("Tail animation distance: " + Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position));

            if(playingAnimation)
            {
                updateLegPos();

                if(Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position) < animationRange)
                {
                    playingAnimation = false;
                }
            }
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            for (int i = 0; i < 6; i++)
            {
                RaycastHit suelo;
                if (Vector3.Distance(_legs[i].Bones[0].position, legFutureBases[i].position) > futureBaseDistance)
                {
                    if (Physics.Raycast(legFutureBases[i].position + Vector3.up * 10.0f, Vector3.down, out suelo, Mathf.Infinity))
                    {
                        _legs[i].Bones[0].position = Vector3.Lerp(_legs[i].Bones[0].position, legFutureBases[i].position, 1.4f);
                    }
                }
                updateLegs(i);
            }
        }

        private void updateTail()
        {
            if (tailEndEffector != null && tailTarget != null)
            {
                if (Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position) < animationRange)
                {
                    Debug.Log($"_tail.Bones.Length: {_tail.Bones.Length}");
                    for (int i = 0; i < _tail.Bones.Length - 1; i++)
                    {
                        Debug.Log($"Rotating bone {i}");
                        float descent = CalculateGradient(_tail.Bones[i]);
                        _tail.Bones[i].transform.Rotate((Vector3.forward * -descent) * growthRate);
                    }
                }
            }
        }

        private float CalculateGradient(Transform joint)
        {
            float distanceEffectorTarget1 = Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position);
            joint.transform.Rotate(Vector3.forward * 0.01f);
            float distanceEffectorTarget2 = Vector3.Distance(tailEndEffector.transform.position, tailTarget.transform.position);
            joint.transform.Rotate(Vector3.forward * -0.01f);
            float result = (distanceEffectorTarget2 - distanceEffectorTarget1) / 0.01f;
            return result;
        }

        //TODO: implement fabrik method to move legs 
        private void updateLegs(int legIndex)
        {
            
        }
        #endregion
    }
}
