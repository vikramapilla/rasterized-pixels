using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KeyPixels
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Camera camera;
        SpriteBatch spriteBatch;

        Matrix worldMatrix;
        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;

        Model playerModel;
        Model ground;
        Model wall;
        static Shots shots;

        CreateBoundingBox cbB;

        public static Vector3 playerPosition = Vector3.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera(graphics);
        }


        protected override void Initialize()
        {
            viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.fieldOfView, camera.aspectRatio, camera.nearPlane, camera.farPlane);

            base.Initialize();
        }
        

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerModel = Content.Load<Model>("Models/Body_Tria");
            ground = Content.Load<Model>("Models/Ground_Tria");
            wall = Content.Load<Model>("Models/Wall_Long_Tria");
            shots = new Shots(Content, "Models/Shot_Tria", 0.01f,new Vector3(0,0,1), Color.Red,30);

            cbB = new CreateBoundingBox(wall,Matrix.Identity);
        }
       

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            shots.updateShotsPos(gameTime);
            getPosition();
            worldMatrix = Matrix.CreateTranslation(playerPosition);

            if (shots.IsCollision(cbB.bBox, Matrix.CreateRotationY(0) * Matrix.CreateTranslation(0, 0, 1) * worldMatrix))
            {
                
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            // TODO: Add your drawing code here
            shots.Draw(viewMatrix,projectionMatrix);

            Draw3DModel(playerModel, worldMatrix, viewMatrix, projectionMatrix);
            Draw3DModel(ground, worldMatrix, viewMatrix, projectionMatrix);
            Draw3DModel(wall,Matrix.CreateRotationY(0)*Matrix.CreateTranslation(0,0,1) * worldMatrix, viewMatrix, projectionMatrix);

            base.Draw(gameTime);
        }

        public static void Draw3DModel(Model model, Matrix worldMatrix, Matrix _viewMatrix, Matrix _projectionMatrix)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = worldMatrix;
                    effect.View = _viewMatrix;
                    effect.Projection = _projectionMatrix;

                    effect.DiffuseColor = Color.Blue.ToVector3();
//                    effect.AmbientLightColor = Color.Gray.ToVector3();
                    effect.Alpha = 1.0f;
                    
                }
                mesh.Draw();
            }
        }

        public static Vector3 getPosition()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                playerPosition += new Vector3(0, 0, 0.1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                playerPosition -= new Vector3(0, 0, 0.1f);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                playerPosition -= new Vector3(0.1f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                playerPosition += new Vector3(0.1f, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                shots.createShot(Matrix.CreateTranslation(playerPosition));
                shots.createShot(Matrix.CreateRotationY(1.4f)*Matrix.CreateTranslation(playerPosition));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                shots.clearAll();
            }


                return playerPosition;
        }

    }
}
