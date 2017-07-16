using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace BasicUI.Controls
{
    public class InfiniteSpinner : ProgressBar
    {
        public bool Enabled { get; set; }
        public float Speed { get; set; } = 0.05f;

        private float LoopingIndex
        {
            get
            {
                return Progress;
            }
            set
            {
                Progress = value;

                if (Progress >= 1)
                {
                    Progress = 0;
                }
            }
        }

        protected override void InternalRender()
        {
            if (Enabled)
            {
                LoopingIndex += Speed;
                base.InternalRender();
            }
        }

        public InfiniteSpinner(string id = "") : base(id) { }
    }
}
