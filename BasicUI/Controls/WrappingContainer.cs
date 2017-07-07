using ImGuiNET;
using System;

namespace BasicUI.Controls
{
    public class WrappingContainer : Container
    {
        public int MaxItemsInRow { get; set; }
        public float EstimatedItemSize { get; set; }

        protected override void InternalRender()
        {
            int maxItems = MaxItemsInRow;

            if (maxItems == 0)
            {
                maxItems = (int)Math.Round(ImGui.GetContentRegionAvailableWidth() / EstimatedItemSize);
            }

            int count = 1;

            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Render();

                if (count < maxItems - 1)
                {
                    ImGui.SameLine();
                    count++;
                }
                else
                {
                    count = 1;
                }
            }
        }

        public WrappingContainer(string id = "") : base(id)
        {
            
        }
    }
}
