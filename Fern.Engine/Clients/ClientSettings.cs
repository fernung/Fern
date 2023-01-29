using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fern.Engine.Clients
{
    public class ClientSettings
    {
        public static readonly ClientSettings Default = new(800, 600, "Fern Client");

        protected int _width;
        protected int _height;
        protected double _aspect;
        protected string _title;
        protected bool _allowMinimize;
        protected bool _allowMaximize;

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                _height = (int)(value / _aspect);
            }
        }
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                _width = (int)(value * _aspect);
            }
        }
        public double AspectRatio =>
            _aspect;
        public string Title =>
            _title;
        public bool AllowMinimize =>
            _allowMinimize;
        public bool AllowMaximize =>
            _allowMaximize;

        public ClientSettings(int width, int height, string title = "", bool allowMinimize = true, bool allowMaximize = true)
        {
            _width = width;
            _height = height;
            _aspect = (double)width / height;
            _title = title;
            _allowMinimize = allowMinimize;
            _allowMaximize = allowMaximize;
        }
    }
}
