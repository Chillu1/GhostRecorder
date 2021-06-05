using System;
using UnityEngine;


    public class GhostSystem : MonoBehaviour
    {
        private GhostRecorder[] _recorders;
        private GhostActor[] _ghostActors;

        private CameraFollow _cameraFollow;

        public Transform playerControlled;
        public Transform playerGhost;

        public float recordDuration = 10;

        private void Start()
        {
            _recorders = FindObjectsOfType<GhostRecorder>();
            _ghostActors = FindObjectsOfType<GhostActor>();
            _cameraFollow = FindObjectOfType<CameraFollow>();

            //var timer = Timer.Register(3f, delegate
            //{
            //    _recorders = FindObjectsOfType<GhostRecorder>();
            //    _ghostActors = FindObjectsOfType<GhostActor>();
            //    StartRecording();
            //});
            //timer.Resume();
        }

        public void StartRecording()
        {
            for (int i = 0; i < _recorders.Length; i++)
            {
                _recorders[i].StartRecording(recordDuration);
            }

            OnRecordingStart();
        }

        public void StopRecording()
        {
            for (int i = 0; i < _recorders.Length; i++)
            {
                _recorders[i].StopRecording();
            }

            OnRecordingEnd();
        }

        public void StartReplay()
        {
            for (int i = 0; i < _ghostActors.Length; i++)
            {
                _ghostActors[i].StartReplay();
            }

            for (int i = 0; i < _recorders.Length; i++)
            {
                _recorders[i].GetComponent<Renderer>().enabled = false;
            }

            _cameraFollow.followTarget = playerGhost;

            OnReplayStart();
        }

        public void StopReplay()
        {
            for (int i = 0; i < _ghostActors.Length; i++)
            {
                _ghostActors[i].StopReplay();
            }

            for (int i = 0; i < _recorders.Length; i++)
            {
                _recorders[i].GetComponent<Renderer>().enabled = true;
            }

            _cameraFollow.followTarget = playerControlled;

            OnReplayEnd();
        }

        #region Event Handlers

        public event EventHandler RecordingStarted;
        public event EventHandler RecordingEnded;
        public event EventHandler ReplayStarted;
        public event EventHandler ReplayEnded;

        #endregion

        #region Event Invokers

        protected virtual void OnRecordingStart()
        {
            if (RecordingStarted != null)
            {
                RecordingStarted.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnRecordingEnd()
        {
            if (RecordingEnded != null)
            {
                RecordingEnded.Invoke(this, EventArgs.Empty);
            }
        }


        protected virtual void OnReplayStart()
        {
            if (ReplayStarted != null)
            {
                ReplayStarted.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnReplayEnd()
        {
            if (ReplayEnded != null)
            {
                ReplayEnded.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion
    }
