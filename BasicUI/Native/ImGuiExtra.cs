using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicUI
{
    public static class ImGuiExtra
    {
        public static unsafe bool InputFloat(string label, ref float value, float step, float step_fast, int decimal_precision, InputTextFlags extra_flags)
        {
            fixed (float* ptr_value = &value)
            {
                return ImGuiNative.igInputFloat(label, ptr_value, step, step_fast, decimal_precision, extra_flags);
            }
        }
    }
}
