using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTWM.Server.DataTypes
{
    public class Vec3f
    {

        public Vec3f() { _x = 0; _y = 0; _z = 0; }
        public Vec3f(float x = 0, float y = 0, float z = 0) { _x = x; _y = y; _z = z; }
        public Vec3f(float[] val) { _x = val[0]; _y = val[1]; _z = val[2]; }
        public Vec3f(List<float> val) { _x = val[0]; _y = val[1]; _z = val[2]; }

        private float _x;

        public float x
        {
            get { return _x; }
            set { _x = value; }
        }

        private float _y;

        public float y
        {
            get { return _y; }
            set { _y = value; }
        }

        private float _z;

        public float z
        {
            get { return _z; }
            set { _z = value; }
        }

        public float GetDistance(Vec3f Coords) { return (float)Math.Sqrt(Math.Pow(this.x - Coords.x, 2) + Math.Pow(this.z - Coords.z, 2)); }

        public List<float> ToList()
        {
            return new List<float>() { _x, _y, _z };
        }

        public override string ToString()
        {

            return $"[{_x}, {_y}, {_z}]";

        }

        public float this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public float GetValue(int key)
        {
            switch(key)
            {
                case 0: 
                    return _x; 
                    break;
                case 1: 
                    return _y; 
                    break;
                case 2: 
                    return _z; 
                    break;
                default: throw new ArgumentException();
            }
        }

        public void SetValue(int key, float value)
        {

            switch(key)
            {
                case 0: 
                    _x = value; 
                    break;
                case 1: 
                    _y = value; 
                    break;
                case 2: 
                    _z = value; 
                    break;
                default: 
                    throw new ArgumentException();
            }

        }

    }
}
