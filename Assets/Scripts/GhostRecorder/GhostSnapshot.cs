using UnityEngine;


    [System.Serializable]
    public class GhostSnapshot
    {
        public float TimeMark { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public bool IsLastSnapshot { get; private set; }


        //TODO Save Animation state


        public GhostSnapshot(float timeMark, Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
            TimeMark = timeMark;
        }

        public void SetLastSnapshot()
        {
            IsLastSnapshot = true;
        }
    }
