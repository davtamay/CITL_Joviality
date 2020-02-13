using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace FrameSynthesis.VR
{
    public struct PoseSample
    {
        public float timestamp;
        public Quaternion orientation;
        public Vector3 eulerAngles;

        public PoseSample(float timestamp, Quaternion orientation)
        {
            this.timestamp = timestamp;
            this.orientation = orientation;

            eulerAngles = orientation.eulerAngles;
            eulerAngles.x = MyMath.WrapDegree(eulerAngles.x);
            eulerAngles.y = MyMath.WrapDegree(eulerAngles.y);
        }
    }

    public class VRGestureRecognizer : MonoBehaviour
    {
        public static VRGestureRecognizer Current { get; private set; }

        [SerializeField]
        float recognitionInterval = 0.5f;

        public UnityEvent nod_Event;
        public UnityEvent headShake_Event;

        //public event Action NodHandler;
        //public event Action HeadshakeHandler;

        public Queue<PoseSample> PoseSamples { get; private set; }

        float prevGestureTime;

        void Awake()
        {
            Current = this;
            PoseSamples = new Queue<PoseSample>();
        }

        void LateUpdate()
        {
            var orientation = Camera.main.transform.rotation;// InputTracking.GetLocalRotation(XRNode.Head);

            ///Debug.Log(orientation);
            // Record orientation
            PoseSamples.Enqueue(new PoseSample(Time.time, orientation));
            if (PoseSamples.Count >= 120)
            {
                PoseSamples.Dequeue();
            }

            // Recognize gestures
            RecognizeNod();
            RecognizeHeadshake();

            if (Input.GetKeyDown(KeyCode.N))
                nod_Event.Invoke();

            if (Input.GetKeyDown(KeyCode.S))
                headShake_Event.Invoke();
        }

        IEnumerable<PoseSample> PoseSamplesWithin(float startTime, float endTime)
        {
            return PoseSamples.Where(sample => 
                sample.timestamp < Time.time - startTime && 
                sample.timestamp >= Time.time - endTime);
        }

        void RecognizeNod()
        {
            try
            {
                var averagePitch = PoseSamplesWithin(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                var maxPitch = PoseSamplesWithin(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                var pitch = PoseSamples.First().eulerAngles.x;

                if (maxPitch - averagePitch > 10f &&
                    Mathf.Abs(pitch - averagePitch) < 5f)
                {
                    if (prevGestureTime < Time.time - recognitionInterval)
                    {
                        Debug.Log(prevGestureTime + " past <- current -> " + Time.time);
                        prevGestureTime = Time.time;
                        //if(NodHandler != null)
                        //{
                            //NodHandler.Invoke();
                            nod_Event.Invoke();
                            Debug.Log("I Noded");
                       // }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // PoseSamplesWithin contains no entry
            }
        }

        void RecognizeHeadshake()
        {
            try
            {
                var averageYaw = PoseSamplesWithin(0.2f, 0.4f).Average(sample => sample.eulerAngles.y);
                var maxYaw = PoseSamplesWithin(0.01f, 0.2f).Max(sample => sample.eulerAngles.y);
                var minYaw = PoseSamplesWithin(0.01f, 0.2f).Min(sample => sample.eulerAngles.y);
                var yaw = PoseSamples.First().eulerAngles.y;

                if ((maxYaw - averageYaw > 10f || averageYaw - minYaw > 10f) &&
                    Mathf.Abs(yaw - averageYaw) < 5f)
                {
                    if (prevGestureTime < Time.time - recognitionInterval)
                    {
                        prevGestureTime = Time.time;
                        //if(HeadshakeHandler != null)
                        //{
                          //  HeadshakeHandler.Invoke();
                            headShake_Event.Invoke();
                            Debug.Log("I Shaked Head");
                     //   }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // PoseSamplesWithin contains no entry
            }
        }
    }
}

