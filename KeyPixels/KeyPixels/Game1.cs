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
        Spawning sp;
        static Player player;
        static Enemy enemy;

        CreateBoundingBox cbB;
        List<BoundingBox> collision_return;
        static int colldown;

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
            colldown = 0;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            map.CreateMap();
            sp = new Spawning(map.getmapList());
            enemy =  sp.GetEnemy();
            enemy.initialize(Content);

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
            sp.SpawnEnemy();
            //foreach (Matrix m in enemy.worldMatrix)
            //{
            //    for (int i = 0; i < 2; ++i)
            //    {
            //        cbB = new CreateBoundingBox(enemy.enemyModel._model[i], m);
            //        if (shots.IsCollision(ref cbB.bBox))
            //        {

            //        }
            //    }
            //}

            if (sp.GetEnemy().IsCollision(ref map.QTree, shots, player))
            {
                //Exit();
            }
            //if (sp.GetEnemy().IsCollision(player.worldMatrix))
            //{

            //}
            if (shots.IsCollision(ref map.QTree))
            {

            }

            player.getPosition();
            player.getRotation();
            sp.GetEnemy().enemyChase(player.worldMatrix);

            if (colldown > 0)
                colldown -= 1;
            base.Update(gameTime);


        }

        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SlateGray);

            // TODO: Add your drawing code here
            shots.Draw(ref viewMatrix, ref projectionMatrix);
            player.Draw();
            enemy.Draw(ref viewMatrix, ref projectionMatrix);
            //Draw3DModel(ground, worldMatrix, viewMatrix, projectionMatrix);
            //Draw3DModel(wall,Matrix.CreateRotationY(0)*Matrix.CreateTranslation(0,0,1) * worldMatrix, viewMatrix, projectionMatrix);
            map.Draw();
            base.Draw(gameTime);
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