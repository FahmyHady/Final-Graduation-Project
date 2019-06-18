using System.Collections.Generic;
using UnityEngine;

namespace GamepadInput
{
    internal class KeyboardSetting
    {
        #region Fields
        public KeyCode A;
        public KeyCode B;
        public KeyCode Back;
        public KeyCode Down;
        public KeyCode DpadDown;
        public KeyCode DpadLeft;
        public KeyCode DpadRight;
        public KeyCode DpadUp;
        public KeyCode Left;
        public KeyCode LeftShoulder;
        public KeyCode LeftStick;
        public KeyCode LeftTrigger;
        public KeyCode Right;
        public KeyCode RightShoulder;
        public KeyCode RightStick;
        public KeyCode RightTrigger;
        public KeyCode Start;
        public KeyCode Up;
        public KeyCode X;
        public KeyCode Y;

        //private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
        private static List<KeyboardSetting> settings;

        #endregion Fields

        #region Constructors

        static KeyboardSetting()
        {
            settings = new List<KeyboardSetting>();
            for (int i = 0; i < 3; i++)
            {
                settings.Add(new KeyboardSetting());
            }
        }

        private KeyboardSetting()
        {
        }
        #endregion Constructors

        #region Methods

        public static KeyboardSetting GetKetboard(string keyboard)
        {
            KeyboardSetting setting = settings[0];
            if (keyboard == "Keyboard1")
            {
                setting = settings[1];
                setting.A = KeyCode.K;
                setting.B = KeyCode.L;
                setting.X = KeyCode.J;
                setting.Y = KeyCode.I;

                setting.Start = KeyCode.Return;
                setting.Back = KeyCode.Escape;
                setting.Left = KeyCode.A;
                setting.Right = KeyCode.D;
                setting.Up = KeyCode.W;
                setting.Down = KeyCode.S;
                setting.LeftStick = KeyCode.LeftControl;
                setting.RightStick = KeyCode.LeftAlt;
                setting.RightShoulder = KeyCode.O;
                setting.LeftShoulder = KeyCode.U;

                setting.LeftTrigger = KeyCode.LeftShift;
                setting.RightTrigger = KeyCode.F;

                setting.DpadUp = KeyCode.Alpha1;
                setting.DpadDown = KeyCode.Alpha2;
                setting.DpadLeft = KeyCode.Alpha3;
                setting.DpadRight = KeyCode.Alpha4;
            }
            else if (keyboard == "Keyboard2")
            {
                setting = settings[2];
                setting.A = KeyCode.Keypad5;
                setting.B = KeyCode.Keypad6;
                setting.X = KeyCode.Keypad4;
                setting.Y = KeyCode.Keypad8;

                setting.Start = KeyCode.KeypadEnter;
                setting.Back = KeyCode.Keypad0;
                setting.Left = KeyCode.LeftArrow;
                setting.Right = KeyCode.RightArrow;
                setting.Up = KeyCode.UpArrow;
                setting.Down = KeyCode.DownArrow;
                setting.LeftStick = KeyCode.LeftControl;
                setting.RightStick = KeyCode.LeftAlt;
                setting.RightShoulder = KeyCode.Keypad9;
                setting.LeftShoulder = KeyCode.Keypad7;

                setting.LeftTrigger = KeyCode.RightControl;
                setting.RightTrigger = KeyCode.RightAlt;
                setting.DpadUp = KeyCode.KeypadDivide;
                setting.DpadDown = KeyCode.KeypadMultiply;
                setting.DpadLeft = KeyCode.KeypadMinus;
                setting.DpadRight = KeyCode.KeypadPlus;
            }
            //setting.FillDic();
            return setting;
        }

        public KeyCode GetKey(string key)
        { return (KeyCode)this.GetType().GetField(key).GetValue(this); }

        //private void FillDic()
        //{
        //    keys["A"] = A;
        //    keys["B"] = B;
        //    keys["X"] = X;
        //    keys["Y"] = Y;
        //    keys["Start"] = Start;
        //    keys["Back"] = Back;
        //    keys["Left"] = Left;
        //    keys["Right"] = Right;
        //    keys["Up"] = Up;
        //    keys["Down"] = Down;
        //    keys["LeftStick"] = LeftStick;
        //    keys["RightStick"] = RightStick;
        //    keys["RightShoulder"] = RightShoulder;
        //    keys["LeftShoulder"] = LeftShoulder;
        //    keys["LeftTrigger"] = LeftTrigger;
        //    keys["RightTrigger"] = RightTrigger;
        //    keys["DpadUp"] = DpadUp;
        //    keys["DpadDown"] = DpadDown;
        //    keys["DpadLeft"] = DpadLeft;
        //    keys["DpadRight"] = DpadRight;
        //}
        #endregion Methods
    }
}