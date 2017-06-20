using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
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
        Vector3 camerapos;
        bool useRenderTarget = true;

        // initialize
        public void Init()
        {

            camera = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
            camerapos = new Vector3(0, 0, 0);
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

            scenegraph = new SceneGraph(shader, postproc, target, quad);
            // TO DO: Make a demo which demonstrates the functionality. THIS IS MOSTLY WHAT OUR GRADE DEPENDS ON.

            // load teapot
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");

            scenegraph.Add(mesh, new Vector3(0, -2, 0), wood, floor);
            scenegraph.Add(floor, new Vector3(0, 0, 0), jacco);

            // set the light
            int lightID = GL.GetUniformLocation(shader.programID,"lightPos");
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, 0.0f, 10.0f, 0.0f);
        }


        // TO DO: Change the camera matrix to user input. MUST support rotation and movement.
        void CameraUpdate()
        {
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            var keyboard = OpenTK.Input.Keyboard.GetState();
            if (keyboard[Key.Left]) camerapos += new Vector3(0.1f, 0, 0);
            if (keyboard[Key.Right]) camerapos += new Vector3(-0.1f, 0, 0);
            if (keyboard[Key.KeypadPlus]) camerapos += new Vector3(0, -0.1f, 0);
            if (keyboard[Key.KeypadMinus]) camerapos += new Vector3(0, 0.1f, 0);
            if (keyboard[Key.Up]) camerapos += new Vector3(0, 0, 0.1f);
            if (keyboard[Key.Down]) camerapos += new Vector3(0, 0, -0.1f);

            camera *= Matrix4.CreateTranslation(camerapos);
            if (keyboard[Key.A]) camera *= Matrix4.CreateRotationY(0.01f);
            if (keyboard[Key.D]) camera *= Matrix4.CreateRotationY(-0.01f);
            if (keyboard[Key.W]) camera *= Matrix4.CreateRotationX(0.01f);
            if (keyboard[Key.S]) camera *= Matrix4.CreateRotationX(-0.01f);

            /*
            camera = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI; */
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            CameraUpdate();
            scenegraph.Render(camera);
        }
    }

} // namespace Template_P3