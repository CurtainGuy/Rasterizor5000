using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

// minimal OpenTK rendering framework for UU/INFOGR
// Jacco Bikker, 2016

namespace Template_P3
{
    class Game
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh mesh, floor;                       // a mesh to draw using OpenGL
        const float PI = 3.1415926535f;         // PI
        float a = 0;                            // teapot rotation angle
        Stopwatch timer;                        // timer for measuring frame duration
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        Texture wood, jacco;                    // texture to use for rendering
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        SceneGraph scenegraph;
        Matrix4 camera;
        bool useRenderTarget = true;

        // initialize
        public void Init()
        {
            scenegraph = new SceneGraph();
            // TO DO: Make a demo which demonstrates the functionality. THIS IS MOSTLY WHAT OUR GRADE DEPENDS ON.

            // load teapot
            scenegraph.Add(new Mesh("../../assets/teapot.obj"));
            scenegraph.Add(new Mesh("../../assets/floor.obj"));

            // initialize stopwatch
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // load a texture
            wood = new Texture("../../assets/wood.jpg");
            jacco = new Texture("../../assets/jacco.png");
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();
            
            // set the light
            int lightID = GL.GetUniformLocation(shader.programID,"lightPos");
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, 0.0f, 10.0f, 0.0f);
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            screen.Print("hello world", 2, 2, 0xffff00);

            CameraUpdate();
        }

        // TO DO: Change the camera matrix to user input. MUST support rotation and movement.
        void CameraUpdate()
        {

        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();
            
            scenegraph.Render(camera);
            // TO DO: Move this to Scenegraph.Render()

            
            // prepare matrix for vertex shader
            Matrix4 transform = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            transform *= Matrix4.CreateTranslation(0, -4, -15);
            transform *= Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

            // update rotation
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            
            if (useRenderTarget)
            {
                // enable render target
                target.Bind();

                // render scene to render target
                scenegraph.Meshes[0].Render( shader, transform, wood );
                scenegraph.Meshes[1].Render( shader, transform, wood );

                // render quad
                target.Unbind();
                quad.Render( postproc, target.GetTextureID() );
            }
            else
            {
                // render scene directly to the screen
                scenegraph.Meshes[0].Render( shader, transform, wood );
                scenegraph.Meshes[1].Render( shader, transform, wood );
            }
            
        }
    }

} // namespace Template_P3