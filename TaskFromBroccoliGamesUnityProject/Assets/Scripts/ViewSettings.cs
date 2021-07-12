using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViewsComponent
{

    public class ViewSettingsComponent : MonoBehaviour
    {
        public string Id;
        public string Type;
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public void Set(ViewSettingsComponent viewSetting)
        {
            Id = viewSetting.Id;
            Type = viewSetting.Type;
            X = viewSetting.X;
            Y = viewSetting.Y;
            Width = viewSetting.Width;
            Height = viewSetting.Height;
        }
    }
}
