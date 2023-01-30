using Fern.Engine.Clients;
using Fern.Engine.Graphics;
using Fern.Engine.Input;
using Fern.Engine.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen = Fern.Engine.Screens.Screen;

namespace Fern.WF.Demos.RandomShapes
{
    public class MainMenuScreen : Screen
    {
        protected readonly MenuOption[] _menu;
        protected int _selected;

        public MainMenuScreen(Client client) :
            base(client)
        {
            _menu = new MenuOption[]
            {
                new("Random Pixels", new RandomPixelScreen(_client)) { Selected = true },
                new("Random Lines", new RandomLineScreen(_client)),
                new("Random Triangles", new RandomTriangleScreen(_client)),
                new("Random Triangles (Filled)", new RandomFilledTriangleScreen(_client)),
                new("Random Rectangles", new RandomRectangleScreen(_client)),
                new("Random Rectangles (Filled)", new RandomFilledRectangleScreen(_client)),
                new("Random Circles", new RandomCircleScreen(_client)),
                new("Random Circles (Filled)", new RandomFilledCircleScreen(_client)),
            };
            _selected = 0;
        }

        public override void Initialize()  
        {
            _client.Settings.CloseOnEscKey = true;
        }
        public override void LoadContent()
        {
        }
        public override void UnloadContent()
        {

        }
        public override void Update()
        {
            if (_client.Keyboard.Pressed(Key.Down))
                _selected = (_selected + 1) % _menu.Length;
            else if(_client.Keyboard.Pressed(Key.Up))
                _selected = (_selected - 1) >= 0 ? _selected - 1 : _menu.Length - 1;
            for (var i = 0; i < _menu.Length; ++i)
                _menu[i].Selected = false;
            _menu[_selected].Selected = true;

            if(_client.Keyboard.Pressed(Key.Enter))
                _client.Screens.Push(_menu[_selected].Screen);
        }
        public override void Draw()
        {
            _client.Graphics.Clear(Pixel.Black);
            DrawTitle();
        }


        private void DrawTitle()
        {
            var cx = _client.Width >> 1;
            var cy = 24;
            _client.Graphics.DrawTextCentered("Random Shapes", cx, cy, Pixel.White, 24);
            cy <<= 1;
            _client.Graphics.DrawTextCentered("Please choose from one of the options below or press ESC to exit.", cx, cy, Pixel.White, 14);

            cy = _client.Height / 3;
            MenuOption m;
            Pixel c;
            for (var i = 0; i < _menu.Length; ++i)
            {
                m = _menu[i];
                c = m.Selected ? Pixel.Yellow : Pixel.White;
                _client.Graphics.DrawTextCentered(m.Name, cx, cy, c, 18);
                cy += 24;
            }
        }
    }

    public struct MenuOption
    {
        public string Name;
        public Screen Screen;
        public bool Selected;

        public MenuOption(string name, Screen screen)
        {
            Name = name;
            Screen = screen;
            Selected = false;
        }
    }
}
