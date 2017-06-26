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
        Shader shader;                          // shader to use for rendering
        Shader postproc;                        // shader to use for post processing
        RenderTarget target;                    // intermediate render target
        ScreenQuad quad;                        // screen filling quad for post processing
        SceneGraph scenegraph;
        Matrix4 camera;

        // TO DO: Make a demo which demonstrates the functionality. THIS IS MOSTLY WHAT OUR GRADE DEPENDS ON.

        float movespeed = 0.25f;
        float turnspeed = 0.03f;
        Mesh mesh, floor;                       // a mesh to draw using OpenGL
        Texture wood, jacco;                    // texture to use for rendering

        // initialize
        public void Init()
        {
            // Begindirection of the camera.
            camera = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);

            // create shaders
            shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            
            // create the render target
            target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();
            // create the scenegraph
            scenegraph = new SceneGraph(shader, postproc, target, quad);

            // load the textures
            wood = new Texture("../../assets/wood.jpg");
            jacco = new Texture("../../assets/jacco.png");

            // load the meshes
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");

            // Add the meshes to the scenegraph. (Mesh, relatieve positie naar parent, relatieve rotatie naar parent, Texture, scale, Parent).
            scenegraph.Add(mesh, new Vector3(10, 0, 0), new Vector3(0, 0, 0), wood, 1, floor);
            scenegraph.Add(floor, new Vector3(0, 0, 0), new Vector3(0, 0, 0), jacco, 4);

            
            // set the light
            int lightID = GL.GetUniformLocation(shader.programID,"lightPos");
            //scenegraph.AddLight(lightID, new Vector3(0, 10, 0));
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, new Vector3(0, 10, 5));
        }

        
        void CameraUpdate()
        {
            // Gets the state of the keyboard
            var keyboard = OpenTK.Input.Keyboard.GetState();
            // Changes the cameraposition with a translationmatrix according to the buttons pressed.
            if (keyboard[Key.Left]) camera *= Matrix4.CreateTranslation(new Vector3(movespeed, 0, 0));
            if (keyboard[Key.Right]) camera *= Matrix4.CreateTranslation(new Vector3(-movespeed, 0, 0));
            if (keyboard[Key.KeypadPlus]) camera *= Matrix4.CreateTranslation(new Vector3(0, -movespeed, 0));
            if (keyboard[Key.KeypadMinus]) camera *= Matrix4.CreateTranslation(new Vector3(0, movespeed, 0));
            if (keyboard[Key.Up]) camera *= Matrix4.CreateTranslation(new Vector3(0, 0, movespeed));
            if (keyboard[Key.Down]) camera *= Matrix4.CreateTranslation(new Vector3(0, 0, -movespeed));
            // Changes the cameradirection with a rotationmatrix according to the buttons pressed.
            if (keyboard[Key.A]) camera *= Matrix4.CreateRotationY(-turnspeed);
            if (keyboard[Key.D]) camera *= Matrix4.CreateRotationY(turnspeed);
            if (keyboard[Key.W]) camera *= Matrix4.CreateRotationX(-turnspeed);
            if (keyboard[Key.S]) camera *= Matrix4.CreateRotationX(turnspeed);
        }

        void LightUpdate()
        {
            foreach (Light l in scenegraph.Lights)
            {
                GL.UseProgram(shader.programID);
                GL.Uniform3(l.lightID, Vector3.Transform(l.position, camera));
            }
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            CameraUpdate();
            //LightUpdate();
            scenegraph.Render(camera);
            Tick();
        }
        void Tick()
        {
            scenegraph.Meshes[1].ModelViewMatrix *= Matrix4.CreateRotationY(0.01f);
        }
    }

} // namespace Template_P3