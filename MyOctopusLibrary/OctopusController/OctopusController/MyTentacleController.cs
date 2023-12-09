using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




namespace OctopusController
{


    internal class MyTentacleController

    //MAINTAIN THIS CLASS AS INTERNAL
    {

        TentacleMode tentacleMode;
        Transform[] _bones;
        Transform _endEffectorSphere;

        public Transform[] Bones { get => _bones; }

        //Exercise 1.
        public Transform[] LoadTentacleJoints(Transform root, TentacleMode mode)
        {
            //TODO: add here whatever is needed to find the bones forming the tentacle for all modes
            //you may want to use a list, and then convert it to an array and save it into _bones
            List<Transform> bonesListLeg = new List<Transform>();
            List<Transform> bonesListTail = new List<Transform>();
            List<Transform> bonesListTentacle = new List<Transform>();

            tentacleMode = mode;

            switch (tentacleMode)
            {
                case TentacleMode.LEG:
                    //TODO: in _endEffectorsphere you keep a reference to the base of the leg                   
                    AddToList(root, bonesListLeg);
                    _endEffectorSphere = bonesListLeg.First();
                    _bones = bonesListLeg.ToArray();
                    break;
                case TentacleMode.TAIL:
                    //TODO: in _endEffectorsphere you keep a reference to the red sphere 
                    AddToList(root, bonesListTail);
                    _endEffectorSphere = bonesListTail.Last();
                    _bones = bonesListTail.ToArray();
                    break;
                case TentacleMode.TENTACLE:
                    //TODO: in _endEffectorphere you  keep a reference to the sphere with a collider attached to the endEffector
                    AddToList(root, bonesListTentacle);
                    _endEffectorSphere = bonesListTentacle.Last();
                    _bones = bonesListTentacle.ToArray();
                    break;


            }
            return Bones;
        }

        void AddToList(Transform articulation, List<Transform> lista)
        {
            // Agregar la articulación actual a la lista
            lista.Add(articulation);

            // Verificar si la articulación actual tiene hijos
            if (articulation.childCount > 0)
            {
                // Recorrer de manera recursiva los hijos de la articulación actual
                foreach (Transform hijo in articulation)
                {
                    AddToList(hijo, lista);
                }
            }
        }

    }
}