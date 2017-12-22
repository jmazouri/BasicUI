using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BasicUI.Controls
{
    public class EnumMenuItem<T> : MenuItem where T : struct, IConvertible
    {
        public T Value { get; set; }
        public new Action<T> OnClick { get; set; }

        Dictionary<string, T> enumValues = new Dictionary<string, T>();

        public EnumMenuItem(string label, Action<T> onClick = null) : base(label, label)
        {
            var type = typeof(T);

            if (!type.GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("The type passed to EnumMenuItem must be an Enum");
            }

            Label = label;
            Value = (T)Enum.GetValues(type).GetValue(0);
            OnClick = onClick;

            foreach (T opt in Enum.GetValues(type))
            {
                enumValues.Add(Enum.GetName(type, opt), opt);
            }
        }

        protected override void InternalRender()
        {
            if (ImGui.BeginMenu(Label, true))
            {
                foreach (var element in enumValues)
                {
                    if (ImGui.MenuItem(element.Key, "", element.Value.Equals(Value), true))
                    {
                        Value = element.Value;
                        OnClick?.Invoke(Value);
                    }
                }

                ImGui.EndMenu();
            }
        }
    }
}
