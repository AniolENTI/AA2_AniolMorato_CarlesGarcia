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
        float animationRange;

        //LEGS
        Transform[] legTargets;
        Transform[] legFutureBases;
        MyTentacleController[] _legs = new MyTentacleController[6];

        
        #region public
        public void InitLegs(Transform[] LegRoots,Transform[] LegFutureBases, Transform[] LegTargets)
        {
            _legs = new MyTentacleController[LegRoots.Length];
            //Legs init
            for(int i = 0; i < LegRoots.Length; i++)
            {
                _legs[i] = new MyTentacleController();
                _legs[i].LoadTentacleJoints(LegRoots[i], TentacleMode.LEG);
                //TODO: initialize anything needed for the FABRIK implementation
            }

        }

        public void InitTail(Transform TailBase)
        {
            _tail = new MyTentacleController();
            _tail.LoadTentacleJoints(TailBase, TentacleMode.TAIL);
            //TODO: Initialize anything needed for the Gradient Descent implementation
            tailEndEffector = _tail.Bones.Last();
        }

        //TODO: Check when to start the animation towards target and implement Gradient Descent method to move the joints.
        public void NotifyTailTarget(Transform target)
        {
            tailTarget = target;
        }

        //TODO: Notifies the start of the walking animation
        public void NotifyStartWalk()
        {

        }

        //TODO: create the apropiate animations and update the IK from the legs and tail

        public void UpdateIK()
        {
            // Update legs (you may need to implement this based on your FABRIK method)
            updateLegs();

            // Update tail using Gradient Descent if necessary
            updateTail();
        }
        #endregion


        #region private
        //TODO: Implement the leg base animations and logic
        private void updateLegPos()
        {
            //check for the distance to the futureBase, then if it's too far away start moving the leg towards the future base position
            //
        }
        //TODO: implement Gradient Descent method to move tail if necessary
        private void updateTail()
        {
            // Check if the tail and target are valid
            if (_tail != null && tailTarget != null && tailEndEffector != null)
            {
                // Check if the tail is within the animation range
                if (Vector3.Distance(tailEndEffector.position, tailTarget.position) <= animationRange)
                {
                    // Calculate error between current position and target position
                    Vector3 error = tailTarget.position - tailEndEffector.position;

                    // Implement Gradient Descent to adjust joint angles
                    for (int i = 0; i < _tail.Bones.Length; i++)
                    {
                        // Adjust joint angles based on the gradient and learning rate
                        // You may need to experiment with these values for your specific case
                        float gradient = calculateGradient(_tail.Bones[i]); // Implement a function to calculate the gradient
                        float learningRate = 0.1f; // Adjust as needed
                        _tail.Bones[i].Rotate(Vector3.up, gradient * learningRate);
                    }
                }
            }
        }

        private float calculateGradient(Transform joint)
        {
            // Calculate gradient based on the difference between current and target rotation
            float gradient = Quaternion.Angle(joint.rotation, Quaternion.LookRotation(tailTarget.position - joint.position, Vector3.up));
            return gradient;
        }

        //TODO: implement fabrik method to move legs 
        private void updateLegs()
        {

        }
        #endregion
    }
}
