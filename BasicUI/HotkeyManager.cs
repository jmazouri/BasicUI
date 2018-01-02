using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicUI
{
    public class HotkeyManager
    {
        private NativeWindow _nativeWindow;

        private List<Key> _pressedKeys = new List<Key>();
        private KeyModifiers _pressedModifiers;

        private List<(Key[] keys, KeyModifiers? modifiers, Action trigger)> _hotkeyActions = new List<(Key[], KeyModifiers?, Action)>();

        public HotkeyManager(NativeWindow window)
        {
            _nativeWindow = window;

            _nativeWindow.KeyDown += (sender, args) =>
            {
                //Skip modifiers
                if ((int)args.Key <= 6) { return; }

                _pressedKeys.Add(args.Key);
                _pressedModifiers = args.Modifiers;

                CheckRegisteredKeys();
            };

            _nativeWindow.KeyUp += (sender, args) =>
            {
                _pressedKeys.RemoveAll(d => d == args.Key);
                _pressedModifiers = args.Modifiers;

                CheckRegisteredKeys();
            };
        }

        public void Register(Action onActivate, KeyModifiers? modifiers, params Key[] keys)
        {
            _hotkeyActions.Add((keys, modifiers, onActivate));
        }

        private void CheckRegisteredKeys()
        {
            var currentPattern = _pressedKeys.OrderBy(s => s).ToList();

            foreach (var (keys, modifiers, trigger) in _hotkeyActions)
            {
                if (keys.OrderBy(d => d).SequenceEqual(currentPattern) && (modifiers == null || _pressedModifiers == modifiers))
                {
                    trigger.Invoke();
                }
            }
        }
    }
}
