using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ImGuiNET;
using System.Numerics;

namespace BasicUI
{
    public static class ThemeLoader
    {
        public static void LoadTheme(string jsonTheme)
        {
            var style = ImGui.GetStyle();
            var theme = JObject.Parse(jsonTheme);

            var variables = theme["Variables"];

            T GetVariable<T>(string key) where T : JToken
            {
                if (variables == null || variables[key] == null)
                {
                    throw new KeyNotFoundException($"The variable \"{key}\" was not defined.");
                }

                return variables[key] as T;
            }

            var colors = theme["Colors"];

            if (colors != null)
            {
                foreach (JProperty prop in colors)
                {
                    ColorTarget target = (ColorTarget)Enum.Parse(typeof(ColorTarget), prop.Name);

                    if (prop.Value is JArray color)
                    {
                        style.SetColor(target, ParseColor(color));
                    }
                    else
                    {
                        style.SetColor(target, ParseColor(GetVariable<JArray>(prop.Value.ToString())));
                    }
                }
            }

            var styles = theme["Styles"];

            if (styles != null)
            {
                foreach (JProperty prop in styles)
                {
                    StyleVar styleVar = (StyleVar)Enum.Parse(typeof(StyleVar), prop.Name);

                    if (prop.Value.Type == JTokenType.String)
                    {
                        SetStyleVar(styleVar, GetVariable<JValue>(prop.Value.ToString()));
                    }
                    else
                    {
                        SetStyleVar(styleVar, prop.Value);
                    }
                }
            }

        }

        static void SetStyleVar(StyleVar styleVar, JToken val)
        {
            if (val is JArray arr)
            {
                ImGui.PushStyleVar(styleVar, new Vector2(arr[0].Value<float>(), arr[1].Value<float>()));
            }
            else
            {
                ImGui.PushStyleVar(styleVar, val.Value<float>());
            }
        }

        static Color ParseColor(JArray colorArray)
        {
            if (colorArray.Count == 3)
            {
                return new Color(colorArray[0].Value<int>(), colorArray[1].Value<int>(), colorArray[2].Value<int>());
            }

            if (colorArray.Count == 4)
            {
                return new Color(colorArray[0].Value<int>(), colorArray[1].Value<int>(), colorArray[2].Value<int>(), colorArray[3].Value<int>());
            }

            return Color.Red;
        }
    }
}
