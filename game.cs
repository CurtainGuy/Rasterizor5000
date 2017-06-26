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

        Mesh mesh, floor, table, ashtray, bowl, chair, chair2, chair3, lamp;                                   // a mesh to draw using OpenGL
        Texture wood, jacco, white, ceramic, brick, metal, stone, wood2, floor1;                       // texture to use for rendering

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
            jacco = new Texture("../../assets/jacco.png");
            ceramic = new Texture("../../assets/ceramic.jpg");

            white = new Texture("../../assets/white.jpg");
            wood = new Texture("../../assets/wood.jpg");
            wood2 = new Texture("../../assets/wood2.jpg");
            brick = new Texture("../../assets/brick.jpg");
            metal = new Texture("../../assets/metal.jpg");
            stone = new Texture("../../assets/stone.jpg");
            floor1 = new Texture("../../assets/floor.jpg");


            // load the meshes
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");
            table = new Mesh("../../assets/table.obj");
            ashtray = new Mesh("../../assets/ashtray.obj");
            bowl = new Mesh("../../assets/bowl.obj");
            chair2 = new Mesh("../../assets/chair2.obj");
            lamp = new Mesh("../../assets/lamp.obj");
            chair = new Mesh("../../assets/chair.obj");
            chair3 = new Mesh("../../assets/chair3.obj");

            // Add the meshes to the scenegraph. (Mesh, relatieve positie naar parent, relatieve rotatie naar parent, Texture, Parent).

            scenegraph.Add(floor, new Vector3(0, 0, 0), new Vector3(0, 0, 0), floor1, 2);
            //scenegraph.Add(ashtray, new Vector3(0, 26.3f, 0), new Vector3(0, 0, 0), stone, 1);
            scenegraph.Add(lamp, new Vector3(-4, 5.25f, -6), new Vector3(0, 15, 0), metal);
            //scenegraph.Add(bowl, new Vector3(1, 0, 0), new Vector3(0, 0, 0), white);
            scenegraph.Add(chair, new Vector3(0, 0, 0), new Vector3(0, 0, 0), white);
            scenegraph.Add(chair2, new Vector3(-20, 7, 0), new Vector3(0, 90, 0), wood2, 0.5f);
            scenegraph.Add(table, new Vector3(-4, 0, 0), new Vector3(0, 0, 0), wood, 0.1f);


            // set the light
            int lightID = GL.GetUniformLocation(shader.programID,"lightPos");
            //scenegraph.AddLight(lightID, new Vector3(0, 10, 0));
            GL.UseProgram(shader.programID);
            GL.Uniform3(lightID, new Vector3(0, 25, 15));
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
            scenegraph.Meshes[1].ModelViewMatrix *= Matrix4.CreateRotationY(0.0f);
        }
    }

} // namespace Template_P3