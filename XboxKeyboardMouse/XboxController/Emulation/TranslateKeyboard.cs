using SimWinInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using XboxKeyboardMouse.Libs;

namespace XboxKeyboardMouse {
    class TranslateKeyboard {
        public static bool TRIGGER_LEFT_PRESSED = false;
        public static bool TRIGGER_RIGHT_PRESSED = false;

        public enum TriggerType { LeftTrigger, RightTrigger }

        public static void ClearAllDicts()
        {
            mapLeftStickX.Clear();
            mapLeftStickY.Clear();
            mapRightStickX.Clear();
            mapRightStickY.Clear();
            buttons.Clear();
            triggers.Clear();
        }

        public static Dictionary<Key, short> mapLeftStickY = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapLeftStickX = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapRightStickX = new Dictionary<Key, short>();
        public static Dictionary<Key, short> mapRightStickY = new Dictionary<Key, short>();

        public static Dictionary<Key, GamePadControl> buttons = new Dictionary<Key, GamePadControl>();
        public static Dictionary<Key, TriggerType> triggers = new Dictionary<Key, TriggerType>();

        public static Dictionary<RunTimeOptionType, RunTimeOption> mapRunTimeOptions = new Dictionary<RunTimeOptionType, RunTimeOption>();

        //public static List<Tuple<GamePadControl, Key>> defaultKeyMap = new List<Tuple<GamePadControl, Key>>() {
        //    new Tuple<GamePadControl, Key>(GamePadControl.DPadUp,Key.Up),
        //    new Tuple<GamePadControl, Key>(GamePadControl.DPadDown,Key.Down),
        //    new Tuple<GamePadControl, Key>(GamePadControl.DPadLeft,Key.Left),
        //    new Tuple<GamePadControl, Key>(GamePadControl.DPadRight,Key.Right),
        //    new Tuple<GamePadControl, Key>(GamePadControl.Start,Key.M),
        //    new Tuple<GamePadControl, Key>(GamePadControl.Back,Key.V),

        //    new Tuple<GamePadControl, Key>(GamePadControl.LeftStickClick,Key.LeftShift),
        //    new Tuple<GamePadControl, Key>(GamePadControl.RightStickClick,Key.C),
        //    new Tuple<GamePadControl, Key>(GamePadControl.LeftStickClick,Key.G),
        //    new Tuple<GamePadControl, Key>(GamePadControl.RightStickClick,Key.G),

        //    new Tuple<GamePadControl, Key>(GamePadControl.LeftShoulder,Key.Q),
        //    new Tuple<GamePadControl, Key>(GamePadControl.RightShoulder,Key.E),
        //    new Tuple<GamePadControl, Key>(GamePadControl.Guide,Key.Oem3),

        //    new Tuple<GamePadControl, Key>(GamePadControl.A,Key.Space),
        //    new Tuple<GamePadControl, Key>(GamePadControl.A,Key.Return),

        //    new Tuple<GamePadControl, Key>(GamePadControl.B,Key.B),
        //    new Tuple<GamePadControl, Key>(GamePadControl.X,Key.X),
        //    new Tuple<GamePadControl, Key>(GamePadControl.Y,Key.Y),

        //    new Tuple<GamePadControl, Key>(GamePadControl.B,Key.T),
        //    new Tuple<GamePadControl, Key>(GamePadControl.X,Key.R),
        //    new Tuple<GamePadControl, Key>(GamePadControl.Y,Key.F),
        //};

        //public static List<Tuple<GamePadControl, Key>> keyMap = new List<Tuple<GamePadControl, Key>>(defaultKeyMap);
        public static List<Tuple<GamePadControl, Key>> keyMap = new List<Tuple<GamePadControl, Key>>();

        //Parse keymap.txt and load into keyMap.
        public static void LoadKeymap()
        {
            var lines = File.ReadAllLines("keymap.txt");
            if(lines.Length>1)
            {
                keyMap.Clear();
            }
            foreach (var line in lines)
            {
                var body = line.Split(new[] { '#' })[0];
                var parts = body.Split(new[] { '=' });
                if (parts.Length==2)
                {
                    var buttonName = parts[0].Trim();
                    var keyName = parts[1].Trim();
                    GamePadControl button;
                    if (Enum.TryParse(buttonName, true, out button))
                    {
                        Key key;
                        if (Enum.TryParse(keyName, true, out key))
                        {
                            keyMap.Add(new Tuple<GamePadControl, Key>(button, key));
                        }

                    }
                }
            }


        }

        private static void KeyInput(SimulatedGamePadState controller) {
            List<bool> btnStatus = new List<bool>();
            
            try {
                btnStatus.Clear();

                bool mouseDisabled = Program.ActiveConfig.Mouse_Eng_Type == MouseTranslationMode.NONE;

                // -------------------------------------------------------------------------------
                //                                STICKS
                // -------------------------------------------------------------------------------
                if (mouseDisabled || Program.ActiveConfig.Mouse_Is_RightStick)
                {
                    controller.LeftStickX = StickValueFromKeyboardState(mapLeftStickX);
                    controller.LeftStickY = StickValueFromKeyboardState(mapLeftStickY);
                }
                if (mouseDisabled || !Program.ActiveConfig.Mouse_Is_RightStick)
                {
                    controller.RightStickX = StickValueFromKeyboardState(mapRightStickX);
                    controller.RightStickY = StickValueFromKeyboardState(mapRightStickY);
                }

                // -------------------------------------------------------------------------------
                //                                BUTTONS
                // -------------------------------------------------------------------------------

                //Preclear keyboard buttons
                foreach (var item in keyMap)
                {
                    controller.Buttons &= ~item.Item1;
                }
                

                //Now set. 
                foreach (var item in keyMap)
                {
                    if(Keyboard.IsKeyDown(item.Item2))
                        controller.Buttons |= item.Item1;
                }
                
                /*foreach (var control in GamePadControls.BinaryControls)
                {
                    // Explicitly set the state of every binary button we care about: If it's in our map
                    // and the key is currently pressed, set the button to pressed, else set it to unpressed.
                    if (AnyKeyIsPressedForControl(buttons, control))
                        controller.Buttons |= control;
                    else
                        controller.Buttons &= ~control;
                }
                */


                // -------------------------------------------------------------------------------
                //                                TRIGGERS
                // -------------------------------------------------------------------------------
                foreach (KeyValuePair<Key, TriggerType> entry in triggers) {
                    if (entry.Key == Key.None) continue;

                    bool v = Keyboard.IsKeyDown(entry.Key);
                    bool ir = entry.Value == TriggerType.RightTrigger;

                    if (v) {
                        if (ir)
                             controller.RightTrigger = 255;
                        else controller.LeftTrigger = 255;
                    } else {
                        if (!TranslateKeyboard.TRIGGER_RIGHT_PRESSED && ir)
                            controller.RightTrigger = 0;
                        else if (!TranslateKeyboard.TRIGGER_LEFT_PRESSED && ir)
                            controller.LeftTrigger = 0;
                    }
                }

                // -------------------------------------------------------------------------------
                //                                CALIBRATION / RUNTIME OPTIONS
                // -------------------------------------------------------------------------------
                // Calibration / runtime options will detect the first key-down state for their associated button, but 
                // will ignore further iterations where the button is still being held. This can be used to advance
                // through calibration states, or have ways to swap configurations on the fly, etc.
                foreach (var option in mapRunTimeOptions.Values)
                {
                    if (option.Key != Key.None && Keyboard.IsKeyDown(option.Key))
                    {
                        if (!option.Ran)
                        {
                            option.Run();
                            option.Ran = true;
                        }
                    }
                    else
                    {
                        option.Ran = false;
                    }
                }
            }
            catch (Exception) { /* This occures when changing presets */ }
        }

        private static short StickValueFromKeyboardState(Dictionary<Key, short> map)
        {
            return (from entry in map
                    where entry.Key != Key.None && Keyboard.IsKeyDown(entry.Key)
                    select entry.Value).FirstOrDefault();
        }

        private static bool AnyKeyIsPressedForControl(Dictionary<Key, GamePadControl> map, GamePadControl control)
        {
            return map.Any(entry => entry.Value == control && Keyboard.IsKeyDown(entry.Key));
        }

        private static void Debug_TimeTracer(SimulatedGamePadState Controller) {
            // Get the start time
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Call the key input
            KeyInput(Controller);

            // Get the end time
            watch.Stop();
            string time = watch.ElapsedMilliseconds + " MS";
            
            if (time != "0 MS") {
                // Display the time
 //               Logger.appendLogLine("KeyboardInput", $"Timed @ {time}", Logger.Type.Debug);
            }
        }

        public static void KeyboardInput(SimulatedGamePadState controller) =>
            #if (DEBUG)
                    // Only enable if you have timing issues AKA Latency on 
                    // the keyboard inputs
                    Debug_TimeTracer(controller);
                    //KeyInput(controller);
            #else
                    KeyInput(controller);
            #endif
    }
}
