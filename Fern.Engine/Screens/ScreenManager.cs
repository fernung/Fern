using Fern.Engine.Clients;

namespace Fern.Engine.Screens
{
    public class ScreenManager
    {
        private readonly Client _client;
        protected readonly Stack<Screen> _screens;

        public ScreenManager(Client client)
        {
            _client = client;
            _screens = new Stack<Screen>();
        }

        public void Push(Screen screen)
        {
            _screens.Push(screen);
            _screens.Peek()?.Initialize();
            _screens.Peek()?.LoadContent();
        }
        public void Pop()
        {
            if(_screens.Count == 0)
                return;
            _screens.Peek()?.UnloadContent();
            _screens.Pop();
        }
        public void Clear()
        {
            while (_screens.Count > 0)
                Pop();
        }
        public void Clear(Screen screenToAdd)
        {
            Clear();
            Push(screenToAdd);
        }

        public void Update()
        {
            if(_screens.Count == 0)
                return;
            _screens.Peek()?.Update();
        }
        public void Draw()
        {
            if (_screens.Count == 0)
                return;
            _screens.Peek()?.Draw();
        }
        public void UnloadContent() =>
            Clear();
    }
}
