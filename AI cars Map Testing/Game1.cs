using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AI_cars_Map_Testing
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        
        Vector3 CamT;
        Vector3 CamPos;
        Vector3 cameraReference;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;
        Matrix rotationsMatrix;


        BasicEffect basicEffect;
        BasicEffect Effect;

        //Geometric Info
        VertexPositionColor[] triangleVertices;
        VertexPositionTexture[] blocks;
        VertexBuffer vertexBuffer;

        MouseState state = Mouse.GetState();

        float xtot=0;
        float ytot=0;

        System.Drawing.Point mousepoint;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,
                PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = false

            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        protected override void Initialize()
        {  
            this.IsMouseVisible = true;
            
            //setup camera
            CamT = new Vector3(0f, 0f, 1f);
            CamPos = new Vector3(0f, 0f, -100f);
            cameraReference = new Vector3(0, 0, 10);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            viewMatrix = Matrix.CreateLookAt(CamPos, CamT, Vector3.Up);

            worldMatrix = Matrix.CreateWorld(CamPos, Vector3.Forward, Vector3.Up);

            rotationsMatrix = Matrix.CreateFromYawPitchRoll(0, 0, 0);

            mousepoint.X = 1000;
            mousepoint.Y = 1000;

            //basic effects
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            Effect = new BasicEffect(GraphicsDevice);
            Effect.Alpha = 1.0f;
            Effect.LightingEnabled = false;

            //generate geometry
            triangleVertices = new VertexPositionColor[18];
            triangleVertices[0] = new VertexPositionColor(new Vector3(10, 0, -10), Color.Blue);
            triangleVertices[1] = new VertexPositionColor(new Vector3(10, 0, 10), Color.Red);
            triangleVertices[2] = new VertexPositionColor(new Vector3(0, 20, 0), Color.DarkGreen);
            triangleVertices[3] = new VertexPositionColor(new Vector3(10, 0, -10), Color.SpringGreen);
            triangleVertices[4] = new VertexPositionColor(new Vector3(-10, 0, -10), Color.Yellow);
            triangleVertices[5] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Orange);
            triangleVertices[6] = new VertexPositionColor(new Vector3(10, 0, 10), Color.BlueViolet);
            triangleVertices[7] = new VertexPositionColor(new Vector3(-10, 0, 10), Color.SaddleBrown);
            triangleVertices[8] = new VertexPositionColor(new Vector3(0, 20, 0), Color.OrangeRed);
            triangleVertices[9] = new VertexPositionColor(new Vector3(-10, 0, -10), Color.MediumTurquoise);
            triangleVertices[10] = new VertexPositionColor(new Vector3(-10, 0, 10), Color.Yellow);
            triangleVertices[11] = new VertexPositionColor(new Vector3(0,20, 0), Color.Yellow);


            triangleVertices[12] = new VertexPositionColor(new Vector3(-10,0, -10), Color.Black);
            triangleVertices[13] = new VertexPositionColor(new Vector3(10, 0, -10), Color.Black);
            triangleVertices[14] = new VertexPositionColor(new Vector3(-10, 0, 10), Color.Black);
            triangleVertices[15] = new VertexPositionColor(new Vector3(10, 0, 10), Color.Black);
            triangleVertices[16] = new VertexPositionColor(new Vector3(10, 0, -10), Color.Black);
            triangleVertices[17] = new VertexPositionColor(new Vector3(-10, 0, 10), Color.Black);

            blocks = new VertexPositionTexture[6];

            blocks[0].Position = new Vector3(20, 0, 20);
            blocks[1].Position = new Vector3(20, 0, 40);
            blocks[2].Position = new Vector3(40, 0, 40);
            blocks[3].Position = new Vector3(40, 0, 40);
            blocks[4].Position = new Vector3(20, 0, 20);
            blocks[5].Position = new Vector3(40, 0, 20);


            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 18, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            Vector3 transformedReference = Vector3.Transform(cameraReference, rotationsMatrix);
            MouseState newstate = Mouse.GetState();

            if (newstate != state && state.RightButton == ButtonState.Pressed)
            {
                float xdif = state.X - newstate.X;
                float ydif = state.Y - newstate.Y;
                xdif *= 0.001f;
                ydif *= 0.001f;
                xtot += xdif;
                ytot -= ydif;
                rotationsMatrix = Matrix.CreateFromYawPitchRoll(xtot, ytot, 0);
                viewMatrix = Matrix.CreateLookAt(CamPos, CamT, Vector3.Up);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {

                CamPos.X += transformedReference.X * 0.1f;
                CamPos.Z += transformedReference.Z * 0.1f;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                CamPos.Z -= transformedReference.X * 0.1f;
                CamPos.X += transformedReference.Z * 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                CamPos.X -= transformedReference.X * 0.1f;
                CamPos.Z -= transformedReference.Z * 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                CamPos.Z += transformedReference.X * 0.1f;
                CamPos.X -= transformedReference.Z * 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                CamPos.Y += 0.5f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            {
                CamPos.Y -= 0.5f;
            }


            CamT = new Vector3(0, 0, 1);
            CamT = Vector3.Transform(CamT, rotationsMatrix);
            CamT += CamPos;
            viewMatrix = Matrix.CreateLookAt(CamPos, CamT, Vector3.Up);
            state = newstate;
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            Effect.Projection = projectionMatrix;
            Effect.View = viewMatrix;
            Effect.World = worldMatrix;



            //Turn off back face culling
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;


            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();             
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, blocks, 0, 2);
            }

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 6);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
