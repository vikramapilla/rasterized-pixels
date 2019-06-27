using KeyPixels.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace KeyPixels
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Camera camera;
        SpriteBatch spriteBatch;
        Vector2 DesiredResolution;

        static int numberShot;

        Matrix worldMatrix;
        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;

        Model playerModel;
        Model ground;
        Model wall;
        Model particle;
        bool flag = true;
        static Shots shots;

        Map map;
        private bool mapFlag = false;
        int mapindex;

        Spawning sp;
        static Player player;
        static Enemy enemy;

        CreateBoundingBox cbB;
        List<BoundingBox> collision_return;
        static int colldown;

        public static Vector3 playerPosition = Vector3.Zero;



        StartMenu testMenu;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            camera = new Camera(graphics);
        }


        protected override void Initialize()
        {
            DesiredResolution = new Vector2(1366, 768);
            viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.fieldOfView, camera.aspectRatio, camera.nearPlane, camera.farPlane);
            colldown = 0;
            mapindex = 0;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerModel = Content.Load<Model>("Models/Body_Tria");
            ground = Content.Load<Model>("Models/Ground_Tria");
            wall = Content.Load<Model>("Models/Wall_Long_Tria");
            particle = Content.Load<Model>("Models/Shot_Tria");
            shots = new Shots(Content, "Models/Shot_Tria", 0.05f, new Vector3(0, 0, 1),Color.Red);
            shots.initialize(Content);
            shots.addModel(Content, "Models/Shot_Tria", 0.05f, new Vector3(0, 0, 1), Color.Blue);
            numberShot = 0;
            player = new Player();
            player.initialize(Content);
            map = new Map(ground, wall, viewMatrix, projectionMatrix);
            map.CreateMap(0);
            sp = new Spawning(map.getmapList());
            enemy =  sp.GetEnemy();
            enemy.initialize(Content);
            MouseCursor.FromTexture2D(Content.Load<Texture2D>("UI/mouse_cursor"), 0, 0);
            testMenu = new StartMenu();
            testMenu.LoadContent(Content);

        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!mapFlag)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D1)&&mapindex!=0)
                {
                    mapindex -= 1;
                    map.CreateMap(mapindex);
                    sp.clearEnemy();
                    sp = new Spawning(map.getmapList());
                    mapFlag = true;
                    shots.clearAll();
                    sp.GetEnemy().initialize(Content);

                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D2)&&mapindex!=4)
                {
                    mapindex += 1;
                    map.CreateMap(mapindex);
                    sp.clearEnemy();
                    sp = new Spawning(map.getmapList());
                    mapFlag = true;
                    shots.clearAll();
                    sp.GetEnemy().initialize(Content);
                }
            }
            if (Keyboard.GetState().IsKeyUp(Keys.D1)&& Keyboard.GetState().IsKeyUp(Keys.D2))
            {
                mapFlag = false;
            }

            // TODO: Add your update logic here
            sp.SpawnEnemy(mapindex);
            shots.updateShotsPos(gameTime);
            getPosition();

            player.getPosition(ref map.QTree);
            player.getRotation();

            if (sp.GetEnemy().IsCollision(shots))
            {

            }
            if (shots.IsCollision(ref map.QTree))
            {

            }

            
            //sp.GetEnemy().clearList();
            sp.GetEnemy().enemyChase(player, ref map.QTree);

            if (colldown > 0)
                colldown -= 1;
            base.Update(gameTime);

            //testMenu.Update(gameTime);
        }

       

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


            // TODO: Add your drawing code here
            shots.Draw(ref viewMatrix, ref projectionMatrix);
            player.Draw();
            sp.GetEnemy().Draw(ref viewMatrix, ref projectionMatrix);
            //Draw3DModel(playerModel, Matrix.CreateTranslation(0, 0, 2.69999f), viewMatrix, projectionMatrix);
            //Draw3DModel(wall,Matrix.CreateRotationY(0)*Matrix.CreateTranslation(0,0,1) * worldMatrix, viewMatrix, projectionMatrix);
            map.Draw();
            base.Draw(gameTime);


            float scaleX = (float) GraphicsDevice.Viewport.Width / DesiredResolution.X;
            float scaleY = (float) GraphicsDevice.Viewport.Height / DesiredResolution.Y;
            Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 0.0f);
            
            spriteBatch.Begin(transformMatrix: matrix);
            //testMenu.Draw(gameTime, spriteBatch);
            spriteBatch.End();

        }

        protected override void Dispose(bool disposing)
        {
            Content.Dispose();

            base.Dispose(disposing);
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
                    effect.DiffuseColor = Color.MidnightBlue.ToVector3();
                    //                    effect.AmbientLightColor = Color.Gray.ToVector3();
                    effect.Alpha = 1.0f;
                    
                }
                mesh.Draw();
            }
        }

        public static Vector3 getPosition()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (colldown < 1)
                {
                    shots.createShot(player.worldMatrix,numberShot);
                    if (numberShot < 1 && numberShot > -1)
                        numberShot++;
                    else
                        numberShot--;
                    colldown = 50;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                shots.clearAll();
            }

            return player.worldMatrix.Translation;
        }
        

    }
}