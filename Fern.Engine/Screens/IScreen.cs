using Fern.Engine.Clients;
using Fern.Engine.Graphics;

namespace Fern.Engine.Screens
{
    public interface IScreen
    {
        void Initialize();
        void LoadContent();
        void UnloadContent();
        void Update();
        void Draw();
    }

    public abstract class Screen : IScreen
    {
        protected readonly Client _client;

        public Screen(Client client)
        {
            _client = client;
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update() { }
        public virtual void Draw() { }
    }
}
