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
        protected bool _closeOnEscKey;

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
        public bool AllowMinimize
        {
            get => _allowMinimize;
            set => _allowMinimize = value;
        }
        public bool AllowMaximize
        {
            get => _allowMaximize;
            set => _allowMaximize = value;
        }
        public bool CloseOnEscKey
        {
            get => _closeOnEscKey;
            set => _closeOnEscKey = value;
        }

        public ClientSettings(int width, int height, string title = "")
        {
            _width = width;
            _height = height;
            _aspect = (double)width / height;
            _title = title;
            _allowMinimize = true;
            _allowMaximize = true;
            _closeOnEscKey = true;
        }
    }
}
