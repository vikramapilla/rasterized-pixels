using KeyPixels.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace KeyPixels
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Camera camera;
        SpriteBatch spriteBatch;
        Vector2 DesiredResolution;

        static int numberShot;

        public static Matrix viewMatrix;
        public static Matrix projectionMatrix;

        Model playerModel;
        Model ground;
        Model wall;
        Model particle;
        Model projectile;

        Model portal;
        ParticleEngine portalParticle;

        static Shots shots;

        static Map map;
        private bool mapFlag;
        public static bool morebullets;
        public static bool doubleshot;
        public static bool bazookashot;


        public static int mapindex;
        Mapchange change;

        Spawning sp;
        static Player player;
        static Enemy enemy;
        static Boss boss;

        //CreateBoundingBox cbB;
        //List<BoundingBox> collision_return;
        static int colldown;
        static int telecolldown;
        static int bazcolldown;

        public static Vector3 playerPosition = Vector3.Zero;


        StartMenu testMenu;
        EndMenu endMenu;
        bool startMenuFlag;
        public static bool isGameStarted;
        public static bool isGameEnded;
        public static bool isGamePlaying;
        public static bool isTeleportPlaying;
        public static bool isScenePlaying;
        public static bool isKeyFound;
        public static bool isBossFight;

        HUD gameHUD;
        CutScenes cutScenes;
        KeyCutScene keyCutScene;
        int sceneIndex;

        public static SoundManager soundManager;

        public PickUps pickUps;

        ParticleEngine2D particleEngine2D;
        Texture2D textureParticle2D;

        Skybox skybox;
        Matrix world = Matrix.Identity;
        Matrix view = Matrix.CreateLookAt(new Vector3(20, 0, 0), new Vector3(0, 0, 0), Vector3.UnitY);
        Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
        Vector3 cameraPosition;
        float angle = 0;
        float distance = 20;
        

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
            bazcolldown = 1;
            change = new Mapchange();

            mapFlag = false;
            morebullets = false;
            doubleshot = false;
            bazookashot = false;

            startMenuFlag = true;
            isGameStarted = false;
            isGameEnded = false;
            isGamePlaying = false;
            isTeleportPlaying = false;
            isKeyFound = false;
            isScenePlaying = false;

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
            shots.addModel(Content, "Models/Shot_Tria3", 0.03f, new Vector3(0, 0, 1), Color.Green);
            shots.addModel(Content, "Models/Shot_Tria", 0.03f, new Vector3(0, 0, 1), Color.Violet);
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
            keyCutScene = new KeyCutScene();
            keyCutScene.LoadContent(Content);
            soundManager = new SoundManager();
            soundManager.LoadContent(Content);
            portal = Content.Load<Model>("Models/Portal_Tex");
            projectile = Content.Load<Model>("Models/partical");
            portalParticle = new ParticleEngine(projectile, new Vector3(0, 0, -4), 0, "Portal", int.MaxValue);
            soundManager.menuBackgroundMusicPlay();
            pickUps = new PickUps();
            pickUps.LoadContent(Content);
            pickUps.initialize();
            boss = new Boss();
            boss.initialize(Content);
            textureParticle2D = Content.Load<Texture2D>("HUD/hud_point");
            particleEngine2D = new ParticleEngine2D(textureParticle2D, new Vector2(900, 990), 0, int.MaxValue);
            endMenu = new EndMenu();
            endMenu.LoadContent(Content);

            skybox = new Skybox("Skyboxes/Islands", Content);
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || )
            //    Exit();

            //System.Diagnostics.Debug.WriteLine("1");

            if (mapindex == 4) //Set bossFight Flag
            {
                isBossFight = true;
            }
            else
            {
                isBossFight = false;
            }

            if (startMenuFlag)
            {
                testMenu.Update(gameTime);
                particleEngine2D.Update();
            }

            if (startMenuFlag && testMenu.getButtonIndex() == 0 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                startMenuFlag = false;
                isGamePlaying = true;
                if (!isGameStarted)
                    isScenePlaying = true;
                isGameStarted = true;
                soundManager.menuBackgroundMusicStop();
            }
            else if (startMenuFlag && testMenu.getButtonIndex() == 1 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (!isGameStarted)
                    isScenePlaying = true;
                isKeyFound = true;
            }
            else if (startMenuFlag && testMenu.getButtonIndex() == 3 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Exit();
            }

            if (isGamePlaying && isScenePlaying)
            {
                cutScenes.Update(gameTime, sceneIndex);
            }
            if (isGamePlaying && isTeleportPlaying)
            {
                change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                portalParticle = new ParticleEngine(projectile, new Vector3(0, 0, -4), 0, "Portal", int.MaxValue);
            }
            if(isScenePlaying && isKeyFound)
            {
                keyCutScene.Update(gameTime);
            }
            if (isGamePlaying && !isScenePlaying && !isTeleportPlaying)
            {
                
                if (mapindex == 4)
                {
                    boss.update(shots, player, ref map.QTree);
                }
                if (sp.GetEnemy().worldMatrix.Count == 0 && mapindex != 4)
                {
                    //change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                    Vector3 tele_dis = player.getCurrentPlayerPosition() - new Vector3(0, 0, -4);//distans to the Teleporter
                    telecolldown--;

                    if ((tele_dis.X < 0.1f && tele_dis.X > -0.1f) && (tele_dis.Y < 0.1f && tele_dis.Y > -0.1f) && (tele_dis.Z < 0.1f && tele_dis.Z > -0.1f) && telecolldown < 1)
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
                    angle += 0.0001f;
                    cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                    view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.UnitY);


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

                shots.updateShotsPos(gameTime);
                getPosition();

                player.IsCollision(shots);
                player.getPosition(ref map.QTree);
                player.getRotation();

                //spawning and enemy
                if (mapindex != 4)
                {
                    sp.SpawnEnemy(mapindex);
                    if (sp.GetEnemy().IsCollision(shots))

                    {

                    }
                    sp.GetEnemy().enemyChase(player, ref map.QTree);
                }



                if (shots.IsCollision(ref map.QTree))
                {

                }

                //camera.Update(gameTime);
                Vector3 movement = player.getCurrentPlayerPosition() - camera.target;
                camera.position += movement;
                camera.target += movement;
                viewMatrix = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);



                if (colldown > 0)
                    colldown -= 1;

                gameHUD.Update(gameTime);

                base.Update(gameTime);

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    isGamePlaying = false;
                    startMenuFlag = true;
                    soundManager.menuBackgroundMusicPlay();
                }
            }

            if (Player.healthCounter <= 0) //After Player Dies, reset the game
            {
                isGameEnded = true;
                isGamePlaying = false;
                isGamePlaying = false;
                
                cutScenes.makeGameOver();
                endMenu.Update(gameTime);
                particleEngine2D.Update();
                if (endMenu.getButtonIndex() == 0 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Initialize();
                    LoadContent();
                    isGameEnded = false;
                }
                else if (endMenu.getButtonIndex() == 1 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
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

            //Draw3DModel(playerModel, Matrix.CreateTranslation(0, 0, 2.69999f), viewMatrix, projectionMatrix);
            //Draw3DModel(wall,Matrix.CreateRotationY(0)*Matrix.CreateTranslation(0,0,1) * worldMatrix, viewMatrix, projectionMatrix);
            map.Draw();





            //GraphicsDevice.Clear(Color.Black);
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(view, projection, cameraPosition);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;


            base.Draw(gameTime);
            if (mapindex == 4)
            {
                boss.Draw();
            }
            if (mapindex != 4)
            {
                sp.GetEnemy().Draw(ref viewMatrix, ref projectionMatrix);
            }
            if (sp.GetEnemy().worldMatrix.Count == 0 && !isTeleportPlaying && mapindex != 4)
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
            {
                testMenu.Draw(gameTime, spriteBatch);
                particleEngine2D.Draw(spriteBatch);
            }


            if (!startMenuFlag)
            {
                gameHUD.Draw(gameTime, spriteBatch);
                pickUps.Draw(gameTime, spriteBatch);
            }


            if (isGamePlaying && isScenePlaying)
            {
                cutScenes.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
            }

            if (isGameEnded)
            {
                cutScenes.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
                endMenu.Draw(gameTime, spriteBatch);
                particleEngine2D.Draw(spriteBatch);
            }

            if (isKeyFound)
                keyCutScene.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);

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
        public static void Draw3DModelColor(Model model, Matrix worldMatrix, Matrix _viewMatrix, Matrix _projectionMatrix, Color _color)
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
                    effect.DiffuseColor = _color.ToVector3();
                    effect.AmbientLightColor = _color.ToVector3();
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
                    shots.createShot(player.worldMatrix, numberShot);
                    if (Keyboard.GetState().IsKeyDown(Keys.M) || doubleshot == true)
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
                    if (morebullets == true) { colldown = 25; }
                }
            }

            if (bazookashot && Keyboard.GetState().IsKeyDown(Keys.E))
            {
                if (bazcolldown > 0)
                {
                    for (int i = 0; i < 8; i++)
                        shots.createBazookaShot(player.worldMatrix, 0, i);
                    bazcolldown--;
                    bazookashot = false;
                    System.Diagnostics.Debug.WriteLine("{0}", getPosition());
                }
            }
            else if (bazookashot && Keyboard.GetState().IsKeyUp(Keys.E))
            {
                bazcolldown = 1;
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