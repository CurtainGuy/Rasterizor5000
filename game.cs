﻿using System.Diagnostics;
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
        
        float rotatefloor = 0.01f;
        float rotatefan = 0.1f;
        float rotatepot = 0.2f;

        float movespeed = 0.25f;
        float turnspeed = 0.05f;

        // Color of the (ambient) light.
        Vector3 lightcolor = new Vector3(850, 850, 850);
        Vector3 ambient = new Vector3(2.5f, 2.5f, 2.5f);

        Mesh mesh, floor, table, lamp, fan, cup, chair;                                  // a mesh to draw using OpenGL
        Texture wood, jacco, white, metal, wood2, floor1, glass, ceramic;                // texture to use for rendering

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
            white = new Texture("../../assets/white.jpg");
            wood = new Texture("../../assets/wood.jpg");
            wood2 = new Texture("../../assets/wood2.jpg");
            metal = new Texture("../../assets/metal.jpg");
            floor1 = new Texture("../../assets/floor.jpg");
            glass = new Texture("../../assets/glass.jpg");
            ceramic = new Texture("../../assets/ceramic.jpg");

            // load the meshes
            mesh = new Mesh("../../assets/teapot.obj");
            floor = new Mesh("../../assets/floor.obj");
            table = new Mesh("../../assets/table.obj");
            cup = new Mesh("../../assets/cup.obj");
            lamp = new Mesh("../../assets/lamp.obj");
            fan = new Mesh("../../assets/blades.obj");
            chair = new Mesh("../../assets/chair.obj");

            // Add the meshes to the scenegraph. (Mesh, relatieve positie naar parent, relatieve rotatie naar parent, Texture, size, Parent).
            //parent floor
            scenegraph.Add(floor, new Vector3(0, 0, 0), new Vector3(0, rotatefloor, 0), floor1, 2);

            scenegraph.Add(table, new Vector3(0, 0, 0), new Vector3(0, 0, 0), wood, 0.1f, floor);
            //for a nice game of beerpong
            scenegraph.Add(cup, new Vector3(-2, 5, 9), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-1, 5, 9), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(0, 5, 9), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1, 5, 9), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(2, 5, 9), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-1.5f, 5, 8), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-0.5f, 5, 8), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(0.5f, 5,8), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1.5f, 5, 8), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(1, 5, 7), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(0, 5, 7), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-1, 5, 7), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(-0.5f, 5, 6), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(0.5f, 5, 6), Vector3.Zero, glass, 0.2f, table);
            scenegraph.Add(new Mesh("../../assets/cup.obj"), new Vector3(0, 5, 5), Vector3.Zero, glass, 0.2f, table);
            
            //what's on the other parts of the table
            scenegraph.Add(lamp, new Vector3(2, 5, -5), new Vector3(0, 25, 0), metal, 1, table);
            scenegraph.Add(chair, new Vector3(-5, -5, -5), new Vector3(0, 90, 0), wood2, 10, floor);
            scenegraph.Add(new Mesh("../../assets/chair.obj"), new Vector3(-5, -5, 5), new Vector3(0, 90, 0), wood2, 10, floor);
            scenegraph.Add(new Mesh("../../assets/chair.obj"), new Vector3(5, -5, -5), new Vector3(0, 270, 0), wood2, 10, floor);
            scenegraph.Add(new Mesh("../../assets/chair.obj"), new Vector3(5, -5, 5), new Vector3(0, 270, 0), wood2, 10, floor);
            
            // Some other objects
            scenegraph.Add(fan, new Vector3(0, 30, 0), new Vector3(0, rotatefan, 0), white, 0.8f);
            scenegraph.Add(mesh, new Vector3(-3, 5, -7), new Vector3(0, rotatepot, 0), ceramic, 0.25f, table);
            scenegraph.Add(new Mesh("../../assets/floor.obj"), new Vector3(0, 5.75f, 0), Vector3.Zero, jacco, 0.5f, table);

            // set the light
            int lightID = GL.GetUniformLocation(shader.programID,"lightPos");
            int colorID = GL.GetUniformLocation(shader.programID, "lightColor");
            int ambientID = GL.GetUniformLocation(shader.programID, "ambientColor");
            Light light = new Light(lightID, new Vector3(0, 25, 15));
            GL.UseProgram(shader.programID);
            GL.Uniform3(light.lightID, light.position);
            GL.Uniform3(colorID, lightcolor);
            GL.Uniform3(ambientID, ambient);
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

            // If space is pressed, the table goes up.
            if (keyboard[Key.Space]) scenegraph.Meshes[1].ModelViewMatrix *= Matrix4.CreateTranslation(new Vector3(0, 0.1f, 0));
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            CameraUpdate();
            scenegraph.Render(camera);
            Tick();
        }

        // Moves the objects for the next Render.
        void Tick()
        {
            scenegraph.Meshes[0].Rotation *= Matrix4.CreateRotationY(rotatefloor);
            scenegraph.Meshes[22].Rotation *= Matrix4.CreateRotationY(rotatefan);
            scenegraph.Meshes[23].Rotation *= Matrix4.CreateRotationY(rotatepot);
        }
    }

} // namespace Template_P3