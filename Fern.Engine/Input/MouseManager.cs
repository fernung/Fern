using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Fern.Engine.Clients;

namespace Fern.Engine.Input
{
    public class MouseManager
    {
        private readonly Client _client;
        private Point _location;
        public int X => _location.X;
        public int Y => _location.Y;

        public MouseManager(Client client)
        {
            _client = client;
            _location = new Point();
            _client.MouseMove += OnMouseMove;
        }

        public bool Pressed(MouseButtons button)
        {
            return (Control.MouseButtons & button) == button;
        }

        public bool Released(MouseButtons button)
        {
            return (Control.MouseButtons & button) != button;
        }

        public void Update()
        {
            _location = _client?.PointToClient(Control.MousePosition) ?? Point.Empty;
        }

        private void OnMouseMove(object? sender, MouseEventArgs e)
        {
            _location = e.Location;
        }
    }
}
