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
        float rotatefloor = 0.01f;
        float rotatefan = 0.1f;

        float movespeed = 0.25f;
        float turnspeed = 0.03f;

        Mesh mesh, floor, table, chair2, lamp, fan, cup;                                                        // a mesh to draw using OpenGL
        Texture wood, jacco, white, ceramic, brick, metal, stone, wood2, floor1, glass;                         // texture to use for rendering

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
            glass = new Texture("../../assets/glass.jpg");

            // load the meshes
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");
            table = new Mesh("../../assets/table.obj");
            cup = new Mesh("../../assets/cup.obj");
            chair2 = new Mesh("../../assets/chair2.obj");
            lamp = new Mesh("../../assets/lamp.obj");
            fan = new Mesh("../../assets/blades.obj");

            // Add the meshes to the scenegraph. (Mesh, relatieve positie naar parent, relatieve rotatie naar parent, Texture, Parent).
            //parent floor
            scenegraph.Add(floor, new Vector3(0, 0, 0), new Vector3(0, rotatefloor, 0), floor1, 2);
            
            //for a nice game of beerpong
            scenegraph.Add(cup, new Vector3(-9, 30.1f, 33), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-4, 30.1f, 33), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1, 30.1f, 33), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(6, 30.1f, 33), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(11, 30.1f, 33), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-6.5f, 30.1f, 28.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-1.5f, 30.1f, 28.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(3.5f, 30.1f, 28.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(8.5f, 30.1f, 28.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(6, 30.1f, 23.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1, 30.1f, 23.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-4, 30.1f, 23.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-1.5f, 30.1f, 18.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(3.5f, 30.1f, 18.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1, 30.1f, 13.5f), new Vector3(0, 0, 0), glass, 0.2f, floor);
            
            //what's on the other parts of the table
            scenegraph.Add(lamp, new Vector3(-4, 6.3f, -6), new Vector3(0, 25, 0), metal, 1, floor);
            scenegraph.Add(chair2, new Vector3(-20, 2.62f, 6), new Vector3(0, 90, 0), wood2, 0.5f, floor);
            scenegraph.Add(new Mesh("../../assets/chair2.obj"), new Vector3(-20, 2.62f, -12), new Vector3(0, 90, 0), wood2, 0.5f, floor);
            scenegraph.Add(new Mesh("../../assets/chair2.obj"), new Vector3(20, 2.62f, -6), new Vector3(0, 270, 0), wood2, 0.5f, floor);
            scenegraph.Add(new Mesh("../../assets/chair2.obj"), new Vector3(20, 2.62f, 12), new Vector3(0, 270, 0), wood2, 0.5f, floor);
            scenegraph.Add(table, new Vector3(-4, 10.1f, 0), new Vector3(0, 0, 0), wood, 0.1f, floor);
            
            //something floating above it
            scenegraph.Add(fan, new Vector3(-1, 30, 0), new Vector3(0, rotatefan, 0), white, 0.8f, table);

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
            scenegraph.Render(camera);
            Tick();
        }
        void Tick()
        {
            scenegraph.Meshes[22].ModelViewMatrix *= Matrix4.CreateRotationY(rotatefan);
            scenegraph.Meshes[0].ModelViewMatrix *= Matrix4.CreateRotationY(rotatefloor);
        }
    }

} // namespace Template_P3