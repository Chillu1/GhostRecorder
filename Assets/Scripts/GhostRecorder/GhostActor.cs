using System;
using UnityEngine;



    public class GhostActor : MonoBehaviour
    {
        public GhostRecorder recorder;

        public float replayTimescale = 1;

        private GhostSnapshot[] _frames;

        private bool _isReplaying;

        private int _replayIndex;
        private float _replayTime; // in milliseconds

        private Renderer _render;

        #region Event Handlers

        public event EventHandler ReplayStarted;
        public event EventHandler ReplayEnded;

        #endregion

        #region Event Invokers

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

        private void Awake()
        {
            _render = GetComponent<Renderer>();
        }

        public void StartReplay()
        {
            SetFrames(recorder.Snapshots);

            if (!_isReplaying)
            {
                _replayIndex = 0;
                _replayTime = 0;

                transform.position = _frames[0].Position;
                transform.rotation = _frames[0].Rotation;

                _render.enabled = true;

                _isReplaying = true;

                OnReplayStart();
            }
        }

        public void StopReplay()
        {
            if (_isReplaying)
            {
                _replayIndex = 0;
                _replayTime = 0.0f;

                _render.enabled = false;
                _isReplaying = false;

                OnReplayEnd();
            }
        }

        private void Update()
        {
            if (!_isReplaying)
                return;

            if (_replayIndex >= _frames.Length)
            {
                StopReplay();
                return;
            }

            GhostSnapshot frame = _frames[_replayIndex];

            if (frame.IsLastSnapshot)
            {
                StopReplay();
                return;
            }

            if (_replayTime < frame.TimeMark)
            {
                if (_replayIndex == 0)
                {
                    _replayTime = frame.TimeMark;
                }
                else
                {
                    DoLerp(_frames[_replayIndex - 1], frame);
                    _replayTime += Time.deltaTime * 1000 * replayTimescale;
                }
            }
            else
            {
                _replayIndex++;
            }
        }

        private void DoLerp(GhostSnapshot lastSnapshot, GhostSnapshot nextSnapshot)
        {
            float interpolationRatio = (_replayTime - lastSnapshot.TimeMark) / (nextSnapshot.TimeMark - lastSnapshot.TimeMark);
            //Log.Info("_replayTime: "+_replayTime+"a.TimeMark: "+a.TimeMark+"_"+"b.TimeMark: "+b.TimeMark+"_Clamp: "+Mathf.Clamp(_replayTime, a.TimeMark, b.TimeMark));
            //Log.Info("b.Position: "+b.Position+"_Slerp: "+Vector3.Slerp(a.Position, b.Position, Mathf.Clamp(_replayTime, a.TimeMark, b.TimeMark)));
            transform.position = Vector3.Lerp(lastSnapshot.Position, nextSnapshot.Position, interpolationRatio);
            transform.rotation = Quaternion.Lerp(lastSnapshot.Rotation, nextSnapshot.Rotation, interpolationRatio);
        }

        public void SetFrames(GhostSnapshot[] frames)
        {
            _frames = frames;
        }
    }
