using System;
using UnityEngine;


    public class GhostRecorder : MonoBehaviour
    {
        public GhostSnapshot[] Snapshots { get; private set; }

        private bool _isRecording;

        private int _recordIndex;
        private float _recordTime; // in milliseconds

        private Transform _playerTransform;
        //private Transform _playerAimTransform;

        public event EventHandler RecordingStarted;
        public event EventHandler RecordingEnded;

        protected virtual void OnRecordingStart()
        {
            RecordingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnRecordingEnd()
        {
            RecordingEnded?.Invoke(this, EventArgs.Empty);
        }

        public void StartRecording(float duration)
        {
            //RecordingEnded += delegate(object sender, EventArgs args) { FindObjectOfType<GhostSystem>().StartReplay(); };
            _playerTransform = transform;
            //_playerAimTransform = GetComponentInChildren<PlayerAiming>().transform;
            if (!_isRecording)
            {
                Snapshots = new GhostSnapshot[(int) (60 * duration)];
                _recordIndex = 0;
                _recordTime = Time.time * 1000;

                _isRecording = true;
                OnRecordingStart();

                Debug.LogFormat("Recording of {0} started", gameObject.name);
            }
        }

        public void StopRecording()
        {
            if (_isRecording)
            {
                Snapshots[_recordIndex - 1].SetLastSnapshot();

                _isRecording = false;
                OnRecordingEnd();

                Debug.LogFormat("Recording of {0} ended at frame {1}", gameObject.name, _recordIndex);
            }
        }

        private void FixedUpdate()
        {
            if (_isRecording)
            {
                RecordFrame(Time.deltaTime);
            }
        }

        private void RecordFrame(float deltaTime)
        {
            if (_recordIndex < Snapshots.Length)
            {
                _recordTime += deltaTime * 1000;
                GhostSnapshot snapshot = new GhostSnapshot(_recordTime, _playerTransform.position, _playerTransform.rotation);

                Snapshots[_recordIndex] = snapshot;

                _recordIndex++;
            }
            else
            {
                StopRecording();
            }
        }
    }
