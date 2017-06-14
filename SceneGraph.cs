using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_P3
{
    // TO DO: Add a hierarchy structure for meshes.
    // For example: when they table is moved, the teapot is moved with it automatically. Or the wheels on a car.
    // I'm thinking about a child-parent structure for this.
    class SceneGraph
    {
<<<<<<< HEAD
=======
        List<Mesh> meshes;
>>>>>>> refs/remotes/origin/master

        public SceneGraph()
        {
            meshes = new List<Mesh>();
        }

        public void Add(Mesh mesh, Vector3 position, Mesh parent = null)
        {
<<<<<<< HEAD

            mesh.PositionToParent = Matrix4.CreateTranslation(position);
            mesh.Parent = parent;

=======
            meshes.Add(mesh);
>>>>>>> refs/remotes/origin/master
        }

        public void Render(Matrix4 cameramatrix)
        {

        }

        public List<Mesh> Meshes
        {
            get{ return meshes; }
        }
    }  
}
