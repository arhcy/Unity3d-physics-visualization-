// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license

using UnityEngine;
using System.Collections.Generic;
using Artics.Physics.UnityPhisicsVisualizers.Base;

namespace Artics.Physics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Stores global position of object, and collider visualization data
    /// </summary>
    public struct MovementData
    {
        public Vector3 Position;
        public IDrawData DrawData;
    }

    /// <summary>
    /// 
    /// </summary>
    public class MovementLogger : MonoBehaviour
    {
        /// <summary>
        /// limit number of records, set 0 if unlimited
        /// </summary>
        public uint BufferSize = 60 * 5;

        /// <summary>
        /// limit time of data storing, set 0 if you wat to do it every Update
        /// </summary>
        public float RecordInterval = 0;

        /// <summary>
        /// 
        /// </summary>
        public Color Color = Color.white;

        /// <summary>
        /// enable recording
        /// </summary>
        public bool Record = true;

        /// <summary>
        /// record state even when the object wasn't moved
        /// </summary>
        public bool RecordUnmoved = true;

        /// <summary>
        /// enable visualizing of position points
        /// </summary>
        public bool DrawPoints = true;

        /// <summary>
        /// enable visualizing of path 
        /// </summary>
        public bool DrawLines = true;

        /// <summary>
        /// Records collider visualizer state, requires class inherited from <see cref="BaseVisualizer"/>
        /// </summary>
        public bool RecordObjectState = false;

        /// <summary>
        /// Draws collider visualizer state
        /// </summary>
        public bool DrawState = false;

        protected Queue<MovementData> Buffer;
        protected BaseVisualizer Visualizer;
        protected float TimeLeft;

        void Start()
        {
            Visualizer = GetComponent<BaseVisualizer>();

            Buffer = new Queue<MovementData>();
            Buffer.Enqueue(GetData());
        }

        void Update()
        {
            DoUpdate();
        }

        private void OnDrawGizmos()
        {
            OnGizmos();
        }

        protected void DoUpdate()
        {
            if (!Record)
                return;

            if (RecordInterval > 0)
            {
                TimeLeft += Time.deltaTime;

                if (TimeLeft < RecordInterval)
                    return;

                TimeLeft = 0;
            }

            if (RecordUnmoved || Buffer.Peek().Position != transform.position)
            {
                Buffer.Enqueue(GetData());
            }

            if (BufferSize > 0 && Buffer.Count > BufferSize)
                Buffer.Dequeue();
        }

        /// <summary>
        /// creates object's snapshot
        /// </summary>
        /// <returns></returns>
        protected MovementData GetData()
        {
            var data = new MovementData();
            data.Position = transform.position;

            if (RecordObjectState)
                data.DrawData = Visualizer.CreateDrawData();

            return data;
        }

        protected void OnGizmos()
        {
            if (!Application.isPlaying)
                return;

            var count = 0;
            var lastPosition = new Vector3();

            foreach (var data in Buffer)
            {
                if (DrawPoints)
                    DrawPoint(data.Position, 0.2f, Color);

                if (DrawLines)
                {
                    if (count > 0)
                    {
                        Gizmos.color = Color;
                        Gizmos.DrawLine(data.Position, lastPosition);
                    }

                    lastPosition = data.Position;
                }

                if (DrawState)
                    data.DrawData?.Draw();

                count++;
            }
        }

        protected void DrawPoint(Vector3 position, float size, Color color)
        {
            Gizmos.color = color;

            Gizmos.DrawLine(new Vector3(position.x - size, position.y, position.z), new Vector3(position.x + size, position.y, position.z));
            Gizmos.DrawLine(new Vector3(position.x, position.y - size, position.z), new Vector3(position.x, position.y + size, position.z));
        }


        [ContextMenu("Clear buffer")]
        public void ClearBuffer()
        {
            Buffer.Clear();
            Buffer.Enqueue(GetData());
        }
    }
}