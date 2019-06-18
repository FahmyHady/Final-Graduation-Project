using System.Collections.Generic;
using UnityEngine;

namespace GamepadInput
{
    public static class GamePad
    {
        #region Fields
        private static List<GamepadState> gamepads = new List<GamepadState>();

        //-----------------------------------------------------------
        private static Vector2 axisXY, move, dPad;

        private static GamepadState state;
        private static KeyCode code;
        private static string xName = string.Empty, yName = string.Empty, name;

        //-----------------------------------------------------------
        #endregion Fields

        #region Constructors

        static GamePad()
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(Index)).Length; i++)
            {
                gamepads.Add(new GamepadState());
            }
        }

        #endregion Constructors

        #region Enums

        public enum Axis { LeftStick, RightStick, Dpad }

        public enum Button { A, B, Y, X, RightShoulder, LeftShoulder, RightStick, LeftStick, Back, Start }

        [System.Serializable]
        public enum ButtonPad { A, B, Y, X }

        [System.Serializable]
        public enum Index { Disable = -1, Any, One, Two, Three, Four, Keyboard1, Keyboard2 }

        public enum Trigger { LeftTrigger, RightTrigger }

        #endregion Enums

        #region Methods

        public static Vector2 GetAxis(Axis axis, Index controlIndex, bool raw = false)
        {
            switch (axis)
            {
                case Axis.Dpad:
                    xName = $"DPad_XAxis_{(int)controlIndex}";
                    yName = $"DPad_YAxis_{(int)controlIndex}";
                    break;

                case Axis.LeftStick:
                    xName = $"L_XAxis_{(int)controlIndex}";
                    yName = $"L_YAxis_{(int)controlIndex}";
                    break;

                case Axis.RightStick:
                    xName = $"R_XAxis_{(int)controlIndex}";
                    yName = $"R_YAxis_{(int)controlIndex}";
                    break;
            }

            axisXY.x = 0.0f;
            axisXY.y = 0.0f;
            if (controlIndex == Index.Any || !(controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2))
            {
                try
                {
                    if (raw == false)
                    {
                        axisXY.x += Input.GetAxis(xName);
                        axisXY.y += -Input.GetAxis(yName);
                    }
                    else
                    {
                        axisXY.x += Input.GetAxisRaw(xName);
                        axisXY.y += -Input.GetAxisRaw(yName);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }

            if (controlIndex == Index.Any)
            {
                switch (axis)
                {
                    case Axis.LeftStick:
                        axisXY += GetState(Index.Keyboard1).LeftStickAxis;
                        axisXY += GetState(Index.Keyboard2).LeftStickAxis;
                        break;

                    case Axis.Dpad:
                        axisXY += GetState(Index.Keyboard1).dPadAxis;
                        axisXY += GetState(Index.Keyboard2).dPadAxis;
                        break;
                }
            }
            else if ((controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2))
            {
                switch (axis)
                {
                    case Axis.LeftStick:
                        axisXY += GetState(controlIndex).LeftStickAxis;
                        break;

                    case Axis.Dpad:
                        axisXY += GetState(controlIndex).dPadAxis;
                        break;
                }
            }
            axisXY.x = Mathf.Clamp(axisXY.x, -1.0f, 1.0f);
            axisXY.y = Mathf.Clamp(axisXY.y, -1.0f, 1.0f);
            return axisXY;
        }

        public static bool GetButton(Button button, Index controlIndex)
        {
            code = GetKeycode(button, controlIndex);
            if (controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2)
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(controlIndex.ToString());
                return Input.GetKey(setting1.GetKey(button.ToString()));
            }
            else if (controlIndex != Index.Any)
                return Input.GetKey(code);
            else
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(Index.Keyboard1.ToString());
                KeyboardSetting setting2 = KeyboardSetting.GetKetboard(Index.Keyboard2.ToString());
                return Input.GetKey(code) || Input.GetKey(setting1.GetKey(button.ToString())) || Input.GetKey(setting2.GetKey(button.ToString()));
            }
        }

        public static bool GetButtonDown(Button button, Index controlIndex)
        {
            code = GetKeycode(button, controlIndex);
            if (controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2)
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(controlIndex.ToString());
                return Input.GetKeyDown(setting1.GetKey(button.ToString()));
            }
            else if (controlIndex != Index.Any)
                return Input.GetKeyDown(code);
            else
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(Index.Keyboard1.ToString());
                KeyboardSetting setting2 = KeyboardSetting.GetKetboard(Index.Keyboard2.ToString());
                return Input.GetKeyDown(code) || Input.GetKeyDown(setting1.GetKey(button.ToString())) || Input.GetKeyDown(setting2.GetKey(button.ToString()));
            }
        }

        public static bool GetButtonUp(Button button, Index controlIndex)
        {
            code = GetKeycode(button, controlIndex);
            if (controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2)
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(controlIndex.ToString());
                return Input.GetKeyUp(setting1.GetKey(button.ToString()));
            }
            else if (controlIndex != Index.Any)
                return Input.GetKeyUp(code);
            else
            {
                KeyboardSetting setting1 = KeyboardSetting.GetKetboard(Index.Keyboard1.ToString());
                KeyboardSetting setting2 = KeyboardSetting.GetKetboard(Index.Keyboard2.ToString());
                return Input.GetKeyUp(code) || Input.GetKeyUp(setting1.GetKey(button.ToString())) || Input.GetKeyUp(setting2.GetKey(button.ToString()));
            }
        }

        public static GamepadState ApplyConfused(GamepadState state)
        {
            state.LeftStickAxis *= -1;
            state.dPadAxis *= -1;
            state.Left = (state.dPadAxis.x < 0);
            state.Right = (state.dPadAxis.x > 0);
            state.Up = (state.dPadAxis.y > 0);
            state.Down = (state.dPadAxis.y < 0);
            return state;
        }
        public static GamepadState ApplyMoveDisable(GamepadState state)
        {
            state.LeftStickAxis = Vector2.zero;
            state.dPadAxis = Vector2.zero;
            state.Left = false;
            state.Right = false;
            state.Up = false;
            state.Down = false;
            return state;
        }

        public static GamepadState ApplyOnlyMove(GamepadState state)
        {
            state.A = false;
            state.ADown = false;
            state.AnyKeyPressedDownAXYB = false;
            state.AUp = false;
            state.B = false;
            state.Back = false;
            state.BDown = false;
            state.BUp = false;
            state.LeftShoulder = false;
            state.LeftShoulderDwon = false;
            state.LeftShoulderUp = false;
            state.LeftStick = false;
            state.LeftTrigger = 0;
            state.RightShoulder = false;
            state.RightShoulderDown = false;
            state.RightShoulderUp = false;
            state.RightStick = false;
            state.rightStickAxis = Vector2.zero;
            state.RightTrigger = 0;
            state.X = false;
            state.XDown = false;
            state.XUp = false;
            state.Y = false;
            state.YDown = false;
            state.YUp = false;
            return state;
        }

        public static GamepadState GetState(Index controlIndex, bool raw = false)
        {
            state = gamepads[((int)controlIndex) + 1];
            if (controlIndex == Index.Disable)
                return state;
            else if (controlIndex == Index.Keyboard1 || controlIndex == Index.Keyboard2)
            {
                FillKeyboard(ref state, controlIndex);
                return state;
            }
            state.A = GetButton(Button.A, controlIndex);
            state.B = GetButton(Button.B, controlIndex);
            state.Y = GetButton(Button.Y, controlIndex);
            state.X = GetButton(Button.X, controlIndex);

            state.AUp = GetButtonUp(Button.A, controlIndex);
            state.BUp = GetButtonUp(Button.B, controlIndex);
            state.YUp = GetButtonUp(Button.Y, controlIndex);
            state.XUp = GetButtonUp(Button.X, controlIndex);

            state.ADown = GetButtonDown(Button.A, controlIndex);
            state.BDown = GetButtonDown(Button.B, controlIndex);
            state.YDown = GetButtonDown(Button.Y, controlIndex);
            state.XDown = GetButtonDown(Button.X, controlIndex);

            state.RightShoulder = GetButton(Button.RightShoulder, controlIndex);
            state.LeftShoulder = GetButton(Button.LeftShoulder, controlIndex);
            state.RightStick = GetButton(Button.RightStick, controlIndex);
            state.LeftStick = GetButton(Button.LeftStick, controlIndex);

            state.RightShoulderUp = GetButtonUp(Button.RightShoulder, controlIndex);
            state.LeftShoulderUp = GetButtonUp(Button.LeftShoulder, controlIndex);

            state.RightShoulderDown = GetButtonDown(Button.RightShoulder, controlIndex);
            state.LeftShoulderDwon = GetButtonDown(Button.LeftShoulder, controlIndex);

            state.RightShoulder = GetButton(Button.RightShoulder, controlIndex);
            state.LeftShoulder = GetButton(Button.LeftShoulder, controlIndex);

            state.Start = GetButton(Button.Start, controlIndex);
            state.Back = GetButton(Button.Back, controlIndex);

            state.LeftStickAxis = GetAxis(Axis.LeftStick, controlIndex, raw);
            state.rightStickAxis = GetAxis(Axis.RightStick, controlIndex, raw);
            state.dPadAxis = GetAxis(Axis.Dpad, controlIndex, raw);

            state.Left = (state.dPadAxis.x < 0);
            state.Right = (state.dPadAxis.x > 0);
            state.Up = (state.dPadAxis.y > 0);
            state.Down = (state.dPadAxis.y < 0);

            state.LeftTrigger = GetTrigger(Trigger.LeftTrigger, controlIndex, raw);
            state.RightTrigger = GetTrigger(Trigger.RightTrigger, controlIndex, raw);
            state.AnyKeyPressedDownAXYB = state.XDown || state.ADown || state.YDown || state.BDown;
            if (controlIndex == Index.Any)
            {
                GamepadState k1 = new GamepadState();
                FillKeyboard(ref k1, Index.Keyboard1);
                GamepadState k2 = new GamepadState();
                FillKeyboard(ref k2, Index.Keyboard2);
                Marge(ref state, k1);
                Marge(ref state, k2);
            }
            return state;
        }

        public static float GetTrigger(Trigger trigger, Index controlIndex, bool raw = false)
        {
            //
            name = string.Empty;
            if (trigger == Trigger.LeftTrigger)
                name = $"TriggersL_{(int)controlIndex}";
            else if (trigger == Trigger.RightTrigger)
                name = $"TriggersR_{(int)controlIndex}";

            //
            float axis = 0;
            try
            {
                if (raw == false)
                    axis = Input.GetAxis(name);
                else
                    axis = Input.GetAxisRaw(name);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
            return axis;
        }

        private static void FillKeyboard(ref GamepadState state, Index controlIndex)
        {
            KeyboardSetting setting = KeyboardSetting.GetKetboard(controlIndex.ToString());
            state.A = Input.GetKey(setting.A);
            state.B = Input.GetKey(setting.B);
            state.Y = Input.GetKey(setting.Y);
            state.X = Input.GetKey(setting.X);

            state.AUp = Input.GetKeyUp(setting.A);
            state.BUp = Input.GetKeyUp(setting.B);
            state.YUp = Input.GetKeyUp(setting.Y);
            state.XUp = Input.GetKeyUp(setting.X);

            state.ADown = Input.GetKeyDown(setting.A);
            state.BDown = Input.GetKeyDown(setting.B);
            state.YDown = Input.GetKeyDown(setting.Y);
            state.XDown = Input.GetKeyDown(setting.X);

            state.RightShoulder = Input.GetKey(setting.RightShoulder);
            state.LeftShoulder = Input.GetKey(setting.LeftShoulder);
            state.RightStick = Input.GetKey(setting.RightStick);
            state.LeftStick = Input.GetKey(setting.LeftStick);

            state.RightShoulderUp = Input.GetKeyUp(setting.RightShoulder);
            state.LeftShoulderUp = Input.GetKeyUp(setting.LeftShoulder);

            state.RightShoulderDown = Input.GetKeyDown(setting.RightShoulder);
            state.LeftShoulderDwon = Input.GetKeyDown(setting.LeftShoulder);

            state.RightShoulder = Input.GetKey(setting.RightShoulder);
            state.LeftShoulder = Input.GetKey(setting.LeftShoulder);

            state.Start = Input.GetKey(setting.Start);
            state.Back = Input.GetKey(setting.Back);

            state.rightStickAxis = Vector2.zero;

            state.Left = Input.GetKey(setting.Left);
            state.Right = Input.GetKey(setting.Right);
            state.Up = Input.GetKey(setting.Up);
            state.Down = Input.GetKey(setting.Down);
            move = Vector2.zero;
            if (state.Left)
                move.x += -1.0f;
            if (state.Right)
                move.x += 1.0f;
            if (state.Down)
                move.y += -1.0f;
            if (state.Up)
                move.y += 1.0f;
            state.LeftStickAxis = move;
            dPad = Vector2.zero;
            if (Input.GetKey(setting.DpadUp))
                dPad.y += 1.0f;
            if (Input.GetKey(setting.DpadDown))
                dPad.y += -1.0f;
            if (Input.GetKey(setting.DpadRight))
                dPad.x += 1.0f;
            if (Input.GetKey(setting.DpadLeft))
                dPad.x += -1.0f;
            state.dPadAxis = dPad;

            state.LeftTrigger = Input.GetKey(setting.LeftTrigger) ? 1.0f : 0.0f;
            state.RightTrigger = Input.GetKey(setting.RightTrigger) ? 1.0f : 0.0f;
            state.AnyKeyPressedDownAXYB = state.XDown || state.ADown || state.YDown || state.BDown;
        }

        private static KeyCode GetKeycode(Button button, Index controlIndex)
        {
            switch (controlIndex)
            {
                case Index.One:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick1Button0;
                        case Button.B: return KeyCode.Joystick1Button1;
                        case Button.X: return KeyCode.Joystick1Button2;
                        case Button.Y: return KeyCode.Joystick1Button3;
                        case Button.RightShoulder: return KeyCode.Joystick1Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick1Button4;
                        case Button.Back: return KeyCode.Joystick1Button6;
                        case Button.Start: return KeyCode.Joystick1Button7;
                        case Button.LeftStick: return KeyCode.Joystick1Button8;
                        case Button.RightStick: return KeyCode.Joystick1Button9;
                    }
                    break;

                case Index.Two:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick2Button0;
                        case Button.B: return KeyCode.Joystick2Button1;
                        case Button.X: return KeyCode.Joystick2Button2;
                        case Button.Y: return KeyCode.Joystick2Button3;
                        case Button.RightShoulder: return KeyCode.Joystick2Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick2Button4;
                        case Button.Back: return KeyCode.Joystick2Button6;
                        case Button.Start: return KeyCode.Joystick2Button7;
                        case Button.LeftStick: return KeyCode.Joystick2Button8;
                        case Button.RightStick: return KeyCode.Joystick2Button9;
                    }
                    break;

                case Index.Three:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick3Button0;
                        case Button.B: return KeyCode.Joystick3Button1;
                        case Button.X: return KeyCode.Joystick3Button2;
                        case Button.Y: return KeyCode.Joystick3Button3;
                        case Button.RightShoulder: return KeyCode.Joystick3Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick3Button4;
                        case Button.Back: return KeyCode.Joystick3Button6;
                        case Button.Start: return KeyCode.Joystick3Button7;
                        case Button.LeftStick: return KeyCode.Joystick3Button8;
                        case Button.RightStick: return KeyCode.Joystick3Button9;
                    }
                    break;

                case Index.Four:

                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick4Button0;
                        case Button.B: return KeyCode.Joystick4Button1;
                        case Button.X: return KeyCode.Joystick4Button2;
                        case Button.Y: return KeyCode.Joystick4Button3;
                        case Button.RightShoulder: return KeyCode.Joystick4Button5;
                        case Button.LeftShoulder: return KeyCode.Joystick4Button4;
                        case Button.Back: return KeyCode.Joystick4Button6;
                        case Button.Start: return KeyCode.Joystick4Button7;
                        case Button.LeftStick: return KeyCode.Joystick4Button8;
                        case Button.RightStick: return KeyCode.Joystick4Button9;
                    }

                    break;

                case Index.Any:
                    switch (button)
                    {
                        case Button.A: return KeyCode.JoystickButton0;
                        case Button.B: return KeyCode.JoystickButton1;
                        case Button.X: return KeyCode.JoystickButton2;
                        case Button.Y: return KeyCode.JoystickButton3;
                        case Button.RightShoulder: return KeyCode.JoystickButton5;
                        case Button.LeftShoulder: return KeyCode.JoystickButton4;
                        case Button.Back: return KeyCode.JoystickButton6;
                        case Button.Start: return KeyCode.JoystickButton7;
                        case Button.LeftStick: return KeyCode.JoystickButton8;
                        case Button.RightStick: return KeyCode.JoystickButton9;
                    }
                    break;
            }
            return KeyCode.None;
        }

        private static void Marge(ref GamepadState gamepad, GamepadState state)
        {
            gamepad.A |= state.A;
            gamepad.B |= state.B;
            gamepad.Y |= state.Y;
            gamepad.X |= state.X;

            gamepad.AUp |= state.AUp;
            gamepad.BUp |= state.BUp;
            gamepad.YUp |= state.YUp;
            gamepad.XUp |= state.XUp;

            gamepad.ADown |= state.ADown;
            gamepad.BDown |= state.BDown;
            gamepad.YDown |= state.YDown;
            gamepad.XDown |= state.XDown;

            gamepad.RightShoulder |= state.RightShoulder;
            gamepad.LeftShoulder |= state.LeftShoulder;
            gamepad.RightStick |= state.RightStick;
            gamepad.LeftStick |= state.LeftStick;

            gamepad.RightShoulderUp |= state.RightShoulderUp;
            gamepad.LeftShoulderUp |= state.LeftShoulderUp;

            gamepad.RightShoulderDown |= state.RightShoulderDown;
            gamepad.LeftShoulderDwon |= state.LeftShoulderDwon;

            gamepad.RightShoulder |= state.RightShoulder;
            gamepad.LeftShoulder |= state.LeftShoulder;

            gamepad.Start |= state.Start;
            gamepad.Back |= state.Back;

            gamepad.rightStickAxis = (state.rightStickAxis + gamepad.rightStickAxis) / 2;

            gamepad.Left |= state.Left;
            gamepad.Right |= state.Right;
            gamepad.Up |= state.Up;
            gamepad.Down |= state.Down;

            gamepad.LeftStickAxis = (state.LeftStickAxis + gamepad.LeftStickAxis) / 2;

            gamepad.dPadAxis = (state.dPadAxis + gamepad.dPadAxis) / 2;

            gamepad.LeftTrigger = Mathf.Max(state.LeftTrigger, gamepad.LeftTrigger);
            gamepad.RightTrigger = Mathf.Max(state.RightTrigger, gamepad.RightTrigger); ;
            gamepad.AnyKeyPressedDownAXYB |= state.AnyKeyPressedDownAXYB;
        }

        #endregion Methods
    }

    public class GamepadState
    {
        #region Fields
        public bool A = false;
        public bool ADown = false;
        public bool AnyKeyPressedDownAXYB = false;
        public bool AUp = false;
        public bool B = false;
        public bool Back = false;
        public bool BDown = false;
        public bool BUp = false;
        public bool Down = false;
        public Vector2 dPadAxis = Vector2.zero;
        public bool Left = false;
        public bool LeftShoulder = false;
        public bool LeftShoulderDwon = false;
        public bool LeftShoulderUp = false;
        public bool LeftStick = false;
        public Vector2 LeftStickAxis = Vector2.zero;
        public float LeftTrigger = 0;
        public bool Right = false;
        public bool RightShoulder = false;
        public bool RightShoulderDown = false;
        public bool RightShoulderUp = false;
        public bool RightStick = false;
        public Vector2 rightStickAxis = Vector2.zero;
        public float RightTrigger = 0;
        public bool Start = false;
        public bool Up = false;
        public bool X = false;
        public bool XDown = false;
        public bool XUp = false;
        public bool Y = false;
        public bool YDown = false;
        public bool YUp = false;
        #endregion Fields
    }
}