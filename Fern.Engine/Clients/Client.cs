using Fern.Engine.Graphics;
using Fern.Engine.Input;
using Fern.Engine.Screens;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Fern.Engine.Clients
{
    public class Client : Form
    {
        protected readonly ClientSettings _settings;
        protected readonly GraphicsManager _graphics;
        protected readonly KeyboardManager _keyboard;
        protected readonly MouseManager _mouse;
        protected readonly ScreenManager _screens;

        protected readonly Random _random;
        protected readonly Stopwatch _stopwatch;
        protected double _elapsed;

        public ClientSettings Settings =>
            _settings;
        public GraphicsManager Graphics =>
            _graphics;
        public KeyboardManager Keyboard =>
            _keyboard;
        public MouseManager Mouse =>
            _mouse;
        public ScreenManager Screens =>
            _screens;

        public Random Random =>
            _random;
        public double Elapsed =>
            _elapsed;
        public double Fps =>
            1.0 / _elapsed;

        public Client() :
            this(ClientSettings.Default)
        { }
        public Client(ClientSettings settings)
        {
            _settings = settings;
            _graphics = new GraphicsManager(this);
            _keyboard = new KeyboardManager(this);
            _mouse = new MouseManager(this);
            _screens = new ScreenManager(this);

            _random = new Random();
            _stopwatch = Stopwatch.StartNew();
            _elapsed = 1.0;
            DoubleBuffered = true;
        }

        public void Run(Screens.Screen startScreen)
        {
            _screens.Push(startScreen);
            Initialize();
            LoadContent();
            Application.Idle += IdleTick;
            Application.Run(this);
        }

        protected virtual void Initialize() { }
        protected virtual void LoadContent() { }

        protected virtual void BeginUpdate()
        {
            _mouse.Update();
            _keyboard.Update();
            if (_settings.CloseOnEscKey && _keyboard.Released(Key.Escape))
                Close();
        }
        protected virtual void Update(double elapsed) { _screens.Update(); }
        protected virtual void EndUpdate() { }

        protected virtual void BeginDraw() { _graphics.ShowFps(); }
        protected virtual void Draw() { _screens.Draw(); }
        protected virtual void EndDraw() { _graphics.Present(); }

        protected virtual void UnloadContent()
        {
            _stopwatch.Stop();
            _screens.UnloadContent();
            _graphics.Dispose();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnloadContent();
            base.OnFormClosing(e);
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            _graphics.Resize(Width, Height);
        }

        private void IdleTick(object? sender, EventArgs e)
        {
            Message msg;
            while (!PeekMessage(out msg, IntPtr.Zero, 0, 0, 0))
            {
                _stopwatch.Stop();
                _elapsed = _stopwatch.Elapsed.TotalSeconds;
                BeginUpdate();
                Update(_elapsed);
                EndUpdate();

                BeginDraw();
                Draw();
                EndDraw();
                _stopwatch.Restart();
            }
        }

        #region P/Invoke Methods
        [StructLayout(LayoutKind.Sequential)]
        private struct Message
        {
            public IntPtr hWnd;
            public uint Msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint Time;
            public System.Drawing.Point Point;
        }

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PeekMessage(out Message message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);
        #endregion
    }
}