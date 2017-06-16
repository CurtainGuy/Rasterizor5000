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
        List<Mesh> meshes;
        Shader shader;
        public SceneGraph(Shader shader)
        {
            meshes = new List<Mesh>();
            this.shader = shader;
        }

        public void Add(Mesh mesh, Vector3 position, Texture texture, Mesh parent = null)
        {
            mesh.Parent = parent;
            mesh.PositionToParent = Matrix4.CreateTranslation(position);
            mesh.Texture = texture;
            meshes.Add(mesh);
        }

        public void Render(Matrix4 cameramatrix)
        {
            foreach(Mesh mesh in meshes)
            {
                Matrix4 transform = Matrix4.Identity;
                Mesh m = mesh;
                transform *= mesh.PositionToParent;

                // Calculates worldcoördinates.
                while (m.Parent != null)
                {
                    m = mesh.Parent;
                    transform *= m.PositionToParent;
                }
                transform *= cameramatrix;
                transform *= Matrix4.CreateTranslation(0, -4, -15);
                transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

                m.Render(shader, transform, m.Texture);
            }
        }

        public List<Mesh> Meshes
        {
            get{ return meshes; }
        }
    }  
}
