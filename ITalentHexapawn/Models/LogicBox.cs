using System;
using System.Security.AccessControl;

namespace Hexapawn.Models {
    public class LogicBox {

        public LogicBox() { }
        public LogicBox(int green, int blue, int purple, int orange) {
            Green = green;
            Blue = blue;
            Purple = purple;
            Orange = orange;
        }

        private int _green;
        private int _blue;
        private int _purple;
        private int _orange;

        public int Green {
            get => _green;
            set {
                if (value >= 0) _green = value;
            }
        }

        public int Blue {
            get => _blue;
            set {
                if (value >= 0) _blue = value;
            }
        }

        public int Purple {
            get => _purple;
            set {
                if (value >= 0) _purple = value;
            }
        }

        public int Orange {
            get => _orange;
            set {
                if (value >= 0) _orange = value;
            }
        }
    }
}