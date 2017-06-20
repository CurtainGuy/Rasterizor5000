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
        List<Mesh> meshes;
        Shader shader;
        Shader postproc;
        RenderTarget target;
        ScreenQuad quad;
        public SceneGraph(Shader shader, Shader postproc, RenderTarget target, ScreenQuad quad)
        {
            meshes = new List<Mesh>();
            this.shader = shader;
            this.postproc = postproc;
            this.target = target;
            this.quad = quad;
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
            Matrix4 uniform = Matrix4.CreateTranslation(0, -4, -15);
            uniform *= cameramatrix;
            uniform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            target.Bind();
            foreach (Mesh mesh in meshes)
            {
                Matrix4 transform = mesh.PositionToParent;
                Mesh m = mesh;
                
                // Calculates worldcoördinates.
                while (m.Parent != null)
                {
                    m = mesh.Parent;
                    transform *= m.PositionToParent;
                }
                transform *= uniform;
                mesh.Render(shader, transform, mesh.Texture);
            }

            target.Unbind();
            quad.Render(postproc, target.GetTextureID());
        }

        public List<Mesh> Meshes
        {
            get{ return meshes; }
        }
    }  
}
