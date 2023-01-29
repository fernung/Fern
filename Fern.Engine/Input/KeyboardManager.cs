using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Fern.Engine.Clients;

namespace Fern.Engine.Input
{
    public class KeyboardManager
    {
        public const int MaxKeys = 256;
        private readonly Client _client;
        protected readonly short[] _newStates;
        protected readonly short[] _oldStates;
        protected readonly KeyState[] _keys;

        public KeyState this[Key key] => 
            GetState(key);

        public KeyboardManager(Client client)
        {
            _client = client; 
            _newStates = new short[MaxKeys];
            _oldStates = new short[MaxKeys];
            _keys = new KeyState[MaxKeys];
        }

        public void Update()
        {
            for (var i = 0; i < MaxKeys; ++i)
            {
                _newStates[i] = GetAsyncKeyState(i);
                _keys[i].Pressed = false;
                _keys[i].Released = false;

                if (_newStates[i] != _oldStates[i])
                {
                    if (0 != (_newStates[i] & 0x8000))
                    {
                        _keys[i].Pressed = !_keys[i].Held;
                        _keys[i].Held = true;
                    }
                    else
                    {
                        _keys[i].Released = true;
                        _keys[i].Held = false;
                    }
                }
                _oldStates[i] = _newStates[i];
            }
        }

        public KeyState GetState(Key key) =>
            _keys[(int)key];
        public bool Pressed(Key key) =>
            GetState(key).Pressed;
        public bool Released(Key key) =>
            GetState(key).Released;
        public bool Held(Key key) =>
            GetState(key).Held;

        #region P/Invoke Methods
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        #endregion
    }
}
