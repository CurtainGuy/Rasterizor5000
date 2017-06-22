using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_P3
{
    struct Light
    {
        public int lightID;
        public Vector3 position;
        public Light(int lightID, Vector3 position)
        {
            this.lightID = lightID;
            this.position = position;
        }
    }
}
