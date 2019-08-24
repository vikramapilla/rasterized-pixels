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
        Model projectile;

        Model portal;
        ParticleEngine portalParticle;

        bool flag = true;
        static Shots shots;

        static Map map;
        private bool mapFlag = false;
        public static bool morebullets = false;


        public static int mapindex;
        Mapchange change;

        Spawning sp;
        static Player player;
        static Enemy enemy;

        CreateBoundingBox cbB;
        List<BoundingBox> collision_return;
        static int colldown;
        static int telecolldown;

        public static Vector3 playerPosition = Vector3.Zero;


        StartMenu testMenu;
        bool startMenuFlag = true;
        public static bool isGameStarted = false;
        public static bool isGamePlaying = false;
        public static bool isTeleportPlaying = false;

        HUD gameHUD;
        CutScenes cutScenes;
        int sceneIndex;
        public static bool isScenePlaying = false;

        public static SoundManager soundManager;

        public PickUps pickUps;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            camera = new Camera(graphics);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }


        protected override void Initialize()
        {
            DesiredResolution = new Vector2(1920, 1080);
            viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(camera.fieldOfView, camera.aspectRatio, camera.nearPlane, camera.farPlane);
            colldown = 0;
            mapindex = 0;
            telecolldown = 50;
            change = new Mapchange();

            sceneIndex = 0;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerModel = Content.Load<Model>("Models/Body_Tria");
            ground = Content.Load<Model>("Models/Ground_Tex");
            wall = Content.Load<Model>("Models/Wall_Long_Tex");
            particle = Content.Load<Model>("Models/Shot_Tria");
            shots = new Shots(Content, "Models/Shot_Tria", 0.05f, new Vector3(0, 0, 1), Color.Red);
            shots.initialize(Content);
            shots.addModel(Content, "Models/Shot_Tria3", 0.05f, new Vector3(0, 0, 1), Color.Blue);
            numberShot = 0;
            player = new Player();
            player.initialize(Content);
            map = new Map(ground, wall);
            map.CreateMap(0);
            sp = new Spawning(map.getmapList());
            enemy = sp.GetEnemy();
            enemy.initialize(Content);
            testMenu = new StartMenu();
            testMenu.LoadContent(Content);
            gameHUD = new HUD();
            gameHUD.LoadContent(Content);
            cutScenes = new CutScenes();
            cutScenes.LoadContent(Content);
            soundManager = new SoundManager();
            soundManager.LoadContent(Content);
            portal = Content.Load<Model>("Models/Portal_Tex");
            projectile = Content.Load<Model>("Models/partical");
            portalParticle = new ParticleEngine(projectile, new Vector3(0, 0, -4), 0, "Portal", 500);
            soundManager.menuBackgroundMusicPlay();
            pickUps = new PickUps();
            pickUps.LoadContent(Content);
            pickUps.initialize();

        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || )
            //    Exit();

            if (testMenu.getButtonIndex() == 3 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Exit();
            }
            else if (testMenu.getButtonIndex() == 0 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                startMenuFlag = false;
                isGamePlaying = true;
                isGameStarted = true;
                isScenePlaying = true;
                soundManager.menuBackgroundMusicStop();
            }

            if (startMenuFlag)
            {
                testMenu.Update(gameTime);
            }

            if (isGamePlaying && isScenePlaying)
            {
                cutScenes.Update(gameTime, sceneIndex);
            }
            if (isGamePlaying && isTeleportPlaying)
            {
                change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                portalParticle = new ParticleEngine(projectile, new Vector3(0, 0, -4), 0, "Portal", 500);
            }

            if (isGamePlaying && !isScenePlaying && !isTeleportPlaying)
            {
                if (sp.GetEnemy().worldMatrix.Count == 0 && mapindex != 4)
                {
                    //change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                    Vector3 tele_dis = player.getCurrentPlayerPosition() - new Vector3(0, 0, -4);//distans to the Teleporter
                    telecolldown--;

                    if ((tele_dis.X<0.1f && tele_dis.X > -0.1f) && (tele_dis.Y < 0.1f && tele_dis.Y > -0.1f) && (tele_dis.Z < 0.1f && tele_dis.Z > -0.1f) && telecolldown < 1)
                    {
                        isTeleportPlaying = true;
                        soundManager.mapChangeEffect();
                        soundManager.portalEffectStop();
                        telecolldown = 50;
                    }
                }
                if (!mapFlag)
                {
                    //Changes when the game is playing goes here

                    if (Keyboard.GetState().IsKeyDown(Keys.D1) && mapindex != 0)
                    {
                        mapindex -= 1;
                        map.CreateMap(mapindex);
                        sp.clearEnemy();
                        sp = new Spawning(map.getmapList());
                        mapFlag = true;
                        shots.clearAll();
                        sp.GetEnemy().initialize(Content);

                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2) && mapindex != 4)
                    {
                        mapindex += 1;
                        map.CreateMap(mapindex);
                        sp.clearEnemy();
                        sp = new Spawning(map.getmapList());
                        mapFlag = true;
                        shots.clearAll();
                        sp.GetEnemy().initialize(Content);
                    }
                    pickUps.Update(gameTime);
                    
                }
                if (Keyboard.GetState().IsKeyUp(Keys.D1) && Keyboard.GetState().IsKeyUp(Keys.D2))
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

                //camera.Update(gameTime);
                Vector3 movement = player.getCurrentPlayerPosition() - camera.target;
                camera.position += movement;
                camera.target += movement;
                viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);

                //sp.GetEnemy().clearList();
                sp.GetEnemy().enemyChase(player, ref map.QTree);

                if (colldown > 0)
                    colldown -= 1;
                base.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    isGamePlaying = false;
                    startMenuFlag = true;
                    soundManager.menuBackgroundMusicPlay();
                }

                gameHUD.Update(gameTime);

            }

        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Teal);
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
            pickUps.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);

            if (sp.GetEnemy().worldMatrix.Count == 0 && !isTeleportPlaying)
            {
                Matrix m = Matrix.CreateTranslation(new Vector3(0, 0, -4));
                portalParticle.Update();
                portalParticle.Draw();
                soundManager.portalEffectPlay();
                testModel(portal, m, viewMatrix, projectionMatrix);
            }

            float scaleX = (float)GraphicsDevice.Viewport.Width / DesiredResolution.X;
            float scaleY = (float)GraphicsDevice.Viewport.Height / DesiredResolution.Y;
            Matrix matrix = Matrix.CreateScale(scaleX, scaleY, 0.0f);

            spriteBatch.Begin(transformMatrix: matrix);
            if (startMenuFlag)
                testMenu.Draw(gameTime, spriteBatch);

            if (!startMenuFlag)
                gameHUD.Draw(gameTime, spriteBatch);


            if (isGamePlaying && isScenePlaying)
            {
                cutScenes.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
            }

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
                    //effect.DiffuseColor = Color.MidnightBlue.ToVector3();
                    //                    effect.AmbientLightColor = Color.Gray.ToVector3();
                    effect.Alpha = 1.0f;

                }
                mesh.Draw();
            }
        }
        public static void testModel(Model model, Matrix worldMatrix, Matrix _viewMatrix, Matrix _projectionMatrix)
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
                    effect.DiffuseColor = Color.DarkMagenta.ToVector3();
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
                    if (Keyboard.GetState().IsKeyDown(Keys.M))
                    {
                        if (numberShot == 0) { shots.createShot(player.worldMatrix, 1); }
                        else shots.createShot(player.worldMatrix, 0);
                    }
                    if (player.shotsCounter > 0)
                        player.shotsCounter--;
                    if (numberShot < 1 && numberShot > -1)
                        numberShot++;
                    else
                        numberShot--;
                    colldown = 50;
                    if (morebullets==true) { colldown = 25; }
                }
            }
            return player.worldMatrix.Translation;
        }

        public static int numberOfShots()
        {
            return player.shotsCounter;
        }

        public static List<int[,]> getMapList()
        {
            return map.getmapList();
        }

        public static int getMapindex()
        {
            return mapindex;
        }

    }
}