using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template_P3
{
    class SceneGraph
    {
        // membervariables
        List<Mesh> meshes;
        List<Light> lights;
        Shader shader;
        Shader postproc;
        RenderTarget target;
        ScreenQuad quad;
        public SceneGraph(Shader shader, Shader postproc, RenderTarget target, ScreenQuad quad)
        {
            meshes = new List<Mesh>();
            lights = new List<Light>();
            // taken directly from game.cs
            this.shader = shader;
            this.postproc = postproc;
            this.target = target;
            this.quad = quad;
        }

        // Adds the meshes to the list.
        public void Add(Mesh mesh, Vector3 position, Vector3 rotate, Texture texture, float size = 1, Mesh parent = null)
        {
            // Each mesh tracks a few variables to calculate their position on rendering
            mesh.Parent = parent;
            rotate *= (float)System.Math.PI / 180;
            mesh.ModelViewMatrix = Matrix4.CreateTranslation(position);
            mesh.Rotation = Matrix4.CreateRotationX(rotate.X) * Matrix4.CreateRotationY(rotate.Y) * Matrix4.CreateRotationZ(rotate.Z);
            mesh.Scale = size;
            mesh.Texture = texture;
            meshes.Add(mesh);
        }

        public void AddLight(int lightID, Vector3 position)
        {
            lights.Add(new Light(lightID, position));
        }

        // Renders each mesh in the list.
        public void Render(Matrix4 cameramatrix)
        {
            // Instead of calculating this for every mesh, we speed up the process by precalculating the universal matrix.
            // This defines the startposition of the camera. 
            Matrix4 uniform = Matrix4.CreateTranslation(0, -4, -15);
            // Then, the cameramatrix is given from the game class.
            uniform *= cameramatrix;
            // Lastly, the prespective is created.
            uniform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            target.Bind();
            foreach (Mesh mesh in meshes)
            {
                // The local position to the parent is taken and the mesh is copied.
                Matrix4 transform = mesh.Rotation;
                transform *= mesh.ModelViewMatrix;
                transform *= Matrix4.CreateScale(mesh.Scale);
                Mesh m = mesh;

                // The transform of the parent is taken into account.
                // Then, if the parent of the current mesh also has a parent, it repeats until we have the worldcoördinates.
                while (m.Parent != null)
                {
                    m = m.Parent;
                    transform*= m.Rotation;
                    transform *= m.ModelViewMatrix;
                }
                Matrix4 toWorld = transform;
                // The universal matrix is added up to the total transform...
                transform *= uniform;
                // and the mesh is finally rendered.
                mesh.Render(shader, transform, toWorld, mesh.Texture);
            }
            // Last but not least, the image is postprocessed. 
            target.Unbind();
            quad.Render(postproc, target.GetTextureID());
        }

        public List<Mesh> Meshes
        {
            get{ return meshes; }
        }

        public List<Light> Lights
        {
            get { return lights; }
        }
    }  
}
