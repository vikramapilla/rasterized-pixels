using KeyPixels.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;

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

        Model key1;
        Model key2;

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
        ControlsMenu controlsMenu;
        OptionsMenu optionsMenu;

        bool startMenuFlag;
        bool controlsMenuFlag;
        bool optionsMenuFlag;
        public static bool isGameStarted;
        public static bool isGameEnded;
        public static bool isGameCompletelyEnded;
        public static bool isGamePlaying;
        public static bool isCreditsPlaying;
        public static bool isTeleportPlaying;
        public static bool isScenePlaying;
        public static bool isKeyFound;
        public static bool isKeyPickup;
        public static bool isBossFight;
        public static bool isEndCutScene1;
        public static bool isEndCutScene2;

        public static bool ismultitread;
        public static bool multitreadflag;
        public static bool damage;

        public static int isKeyFoundIndexHUD;

        HUD gameHUD;
        CutScenes cutScenes;
        KeyCutScene keyCutScene;
        EndCutScene endCutScene;
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
        float matrixangle;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            camera = new Camera(graphics);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }


        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

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
            controlsMenuFlag = false;
            optionsMenuFlag = false;
            isGameStarted = false;
            isGameEnded = false;
            isGameCompletelyEnded = false;
            isGamePlaying = false;
            isTeleportPlaying = false;
            isKeyFound = false;
            isKeyPickup = false;
            isScenePlaying = false;
            isEndCutScene1 = false;
            isEndCutScene2 = false;

            ismultitread = false;

            float matrixangle = 0f;
            sceneIndex = 0;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerModel = Content.Load<Model>("Models/Body_Tria");
            ground = Content.Load<Model>("Models/Ground_Tex");
            wall = Content.Load<Model>("Models/Wall_Long_Small");
            particle = Content.Load<Model>("Models/Shot_Tria");
            shots = new Shots(Content, "Models/Shot_Big3", 0.05f, new Vector3(0, 0, 1), Color.Red);
            shots.initialize(Content);
            shots.addModel(Content, "Models/Shot_Big4", 0.05f, new Vector3(0, 0, 1), Color.Blue);
            shots.addModel(Content, "Models/Shot_Big3", 0.05f, new Vector3(0, 0, 1), Color.Green);
            shots.addModel(Content, "Models/Shot_Big4", 0.05f, new Vector3(0, 0, 1), Color.Violet);
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
            endCutScene = new EndCutScene();
            endCutScene.LoadContent(Content);
            soundManager = new SoundManager();
            soundManager.LoadContent(Content);
            SoundManager.Volume = 0.5f;
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
            controlsMenu = new ControlsMenu();
            controlsMenu.LoadContent(Content);
            optionsMenu = new OptionsMenu();
            optionsMenu.LoadContent(Content);

            key1 = Content.Load<Model>("Models/keypart2");
            key2 = Content.Load<Model>("Models/keypart1");
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
            if (isBossFight && !isTeleportPlaying && !isEndCutScene1)
            {
                isGamePlaying = false;
                isScenePlaying = true;
            }
            if (isBossFight && !isGamePlaying && isScenePlaying && !isTeleportPlaying && !isEndCutScene1)
            {
                if (!soundManager.isCutscenePlay)
                {
                    soundManager.CutsceneMusicPlay();
                    soundManager.isCutscenePlay = true;
                }
                endCutScene.Update(gameTime);
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

                soundManager.menuclickEffect();
                //soundManager.menuBackgroundMusicStop();

                if (mapindex == 4)
                {
                    soundManager.FightMusicPlay();
                    soundManager.fightPlay = true;

                }
                else soundManager.BackgroundMusicPlay();
            }
            else if (startMenuFlag && testMenu.getButtonIndex() == 1 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                startMenuFlag = false;
                optionsMenuFlag = true;
                optionsMenu.tempChangesFlag = true;
                soundManager.menuclickEffect();


                /*if (!isGameStarted)
                    isScenePlaying = true;
                isKeyFound = true;*/
            }
            else if (startMenuFlag && testMenu.getButtonIndex() == 2 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                startMenuFlag = false;
                controlsMenuFlag = true;
                soundManager.menuclickEffect();
            }
            else if (startMenuFlag && testMenu.getButtonIndex() == 3 && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                soundManager.menuclickEffect();
                Exit();
            }

            //test for multitread
            if (Keyboard.GetState().IsKeyUp(Keys.NumPad1))
            {
                multitreadflag = false;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
            {
                if (!multitreadflag)
                {
                    if (ismultitread) { ismultitread = false; }
                    else ismultitread = true;
                    multitreadflag = true;
                }
            }
            //

            if (!startMenuFlag && controlsMenuFlag)
            {
                controlsMenu.Update(gameTime);
                particleEngine2D.Update();


                if (controlsMenu.goBackFlag())
                {
                    startMenuFlag = true;
                    controlsMenuFlag = false;
                }

            }
            else if (!startMenuFlag && optionsMenuFlag)
            {
                optionsMenu.Update(gameTime);
                particleEngine2D.Update();

                if (optionsMenu.saveChanges())
                {
                    for (int i = 0; i < 4; i++)
                    {
                        optionsMenu.optionActivatedValues[i] += optionsMenu.tempOptionActivatedValues[i];
                        optionsMenu.tempOptionActivatedValues[i] = 0;
                    }
                    int[] values = optionsMenu.getOptionValues();
                    if (values[0] == 0)
                    {
                        graphics.PreferredBackBufferWidth = 1024;
                        graphics.PreferredBackBufferHeight = 576;
                    }
                    else if (values[0] == 1)
                    {
                        graphics.PreferredBackBufferWidth = 1280;
                        graphics.PreferredBackBufferHeight = 720;
                    }
                    else if (values[0] == 2)
                    {
                        graphics.PreferredBackBufferWidth = 1920;
                        graphics.PreferredBackBufferHeight = 1080;
                    }

                    if (values[1] == 0)
                        graphics.IsFullScreen = false;
                    else
                        graphics.IsFullScreen = true;

                    graphics.ApplyChanges();

                    SoundManager.Volume = (values[2] / 10f);
                }

                if (optionsMenu.goBackFlag())
                {
                    startMenuFlag = true;
                    optionsMenu.initializeFlag = true;
                    optionsMenu.tempChangesFlag = false;
                    optionsMenuFlag = false;
                }
            }

            soundManager.update();

            if (isGamePlaying && isScenePlaying)
            {
                if (!soundManager.isCutscenePlay)
                {
                    soundManager.CutsceneMusicPlay();
                    soundManager.isCutscenePlay = true;
                }
                cutScenes.Update(gameTime, sceneIndex);
            }
            if (isGamePlaying && isTeleportPlaying)
            {
                change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                portalParticle = new ParticleEngine(projectile, new Vector3(0, 0, -4), 0, "Portal", int.MaxValue);
            }
            if (isScenePlaying && isKeyFound)
            {
                if (!soundManager.isCutscenePlay)
                {
                    soundManager.CutsceneMusicPlay();
                    soundManager.isCutscenePlay = true;
                }
                keyCutScene.Update(gameTime);
            }
            if (isGamePlaying && !isScenePlaying && !isTeleportPlaying && !isGameEnded)
            {

                if (keyCutScene.keyFoundIndex == 2)
                {
                    isKeyFound = true;
                    isKeyFoundIndexHUD = 2;
                    isKeyPickup = true;
                    telecolldown = 50;
                    if (isGameStarted)
                        isScenePlaying = true;
                    isGamePlaying = false;

                }
                else soundManager.isCutscenePlay = false;

                if (mapindex == 4)
                {
                    boss.update(shots, player, ref map.QTree);
                    if (!soundManager.fightPlay)
                    {
                        soundManager.FightMusicPlay();
                        soundManager.fightPlay = true;
                    }
                }
                if (Enemy.worldMatrix.Count == 0 && ((mapindex == 0 || mapindex == 2) || ((mapindex == 1 || mapindex == 3) && isKeyPickup)) && Spawning.isspawnended)
                {
                    //change.update(ref sp, ref mapindex, ref player, ref map, ref shots, Content);
                    Vector3 tele_dis = player.getCurrentPlayerPosition() - new Vector3(0, 0, -4);//distance to the Teleporter
                    telecolldown--;

                    if ((tele_dis.X < 0.1f && tele_dis.X > -0.1f) && (tele_dis.Y < 0.1f && tele_dis.Y > -0.1f) && (tele_dis.Z < 0.1f && tele_dis.Z > -0.1f) && telecolldown < 1)
                    {
                        isTeleportPlaying = true;
                        soundManager.mapChangeEffect();
                        soundManager.portalEffectStop();
                        telecolldown = 50;
                    }
                }
                if ((mapindex == 1 || mapindex == 3) && Enemy.worldMatrix.Count == 0 && !isKeyPickup)
                {
                    Vector3 key_dis = player.getCurrentPlayerPosition() - new Vector3(0, 0, 0);//distance to the key
                    telecolldown--;
                    matrixangle += 0.05f;
                    if ((key_dis.X < 0.1f && key_dis.X > -0.1f) && (key_dis.Y < 0.1f && key_dis.Y > -0.1f) && (key_dis.Z < 0.1f && key_dis.Z > -0.1f) && telecolldown < 1)
                    {
                        isKeyFound = true;
                        isKeyFoundIndexHUD = 1;
                        isKeyPickup = true;
                        telecolldown = 50;
                        if (isGameStarted)
                            isScenePlaying = true;
                        isGamePlaying = false;
                        matrixangle = 0f;
                    }
                }
                //Changes when the game is playing goes here
                angle += 0.0001f;
                cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                view = Matrix.CreateLookAt(cameraPosition, new Vector3(0, 0, 0), Vector3.UnitY);

                pickUps.Update(gameTime);

                if (!mapFlag)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.D1) && mapindex != 0)
                    {
                        mapindex -= 1;
                        map.CreateMap(mapindex);
                        sp.clearEnemy();
                        Spawning.isspawnended = false;
                        sp = new Spawning(map.getmapList());
                        mapFlag = true;
                        shots.clearAll();
                        soundManager.portalEffectStop();
                        sp.GetEnemy().initialize(Content);

                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.D2) && mapindex != 4)
                    {
                        mapindex += 1;
                        map.CreateMap(mapindex);
                        sp.clearEnemy();
                        Spawning.isspawnended = false;
                        sp = new Spawning(map.getmapList());
                        mapFlag = true;
                        shots.clearAll();
                        soundManager.portalEffectStop();
                        sp.GetEnemy().initialize(Content);
                    }

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
                //player.getRotation();

                //spawning and enemy
                if (mapindex != 4)
                {
                    sp.SpawnEnemy(mapindex, player);
                    if (sp.GetEnemy().IsCollision(shots))

                    {

                    }
                    //System.Console.WriteLine(Enemy.worldMatrix.Count);
                    //System.Console.WriteLine(ismultitread);
                    if (!ismultitread) { sp.GetEnemy().enemyChase(player, ref map.QTree); }
                    else if (Enemy.worldMatrix.Count >= 1)
                    {
                        sp.GetEnemy().Thread(player, map.QTree);

                    }
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
                    if (soundManager.isPortalPlay)
                        soundManager.portalEffectPause();
                    soundManager.menuBackgroundMusicPlay();
                }
            }

            if (Player.healthCounter <= 0) //After Player Dies, reset the game
            {
                isGameEnded = true;
                isGamePlaying = false;
                soundManager.gameOverEffect();
                cutScenes.makeGameOver();
                endMenu.Update(gameTime);
                particleEngine2D.Update();
                if (endMenu.getButtonIndex() == 0 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {


                    int rewith = graphics.PreferredBackBufferWidth;
                    int reheight = graphics.PreferredBackBufferHeight;
                    bool refull = graphics.IsFullScreen;
                    float resound = SoundManager.Volume;
                    int[] values = optionsMenu.getOptionValues();

                    Initialize();
                    LoadContent();
                    SoundManager.Volume = resound;
                    isGameEnded = false;
                    isGameCompletelyEnded = false;

                    graphics.PreferredBackBufferWidth = rewith;
                    graphics.PreferredBackBufferHeight = reheight;
                    graphics.IsFullScreen = refull;
                    graphics.ApplyChanges();


                    optionsMenu.setOptionValues(values);



                }
                else if (endMenu.getButtonIndex() == 1 && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }
            else if (Boss.healthCounter <= 0) //After boss Dies, end the game
            {
                isEndCutScene2 = true;
                isGameEnded = true;
                isGamePlaying = false;
                endCutScene.Update(gameTime);
                if (!soundManager.isCutscenePlay)
                {
                    soundManager.CutsceneMusicPlay();
                    soundManager.isCutscenePlay = true;
                }
            }
            if (isCreditsPlaying)
            {
                if (!soundManager.isCreditPlay)
                {
                    soundManager.CreditMusicPlay();
                    soundManager.isCreditPlay = true;
                }
            }
            if (isGameCompletelyEnded)
            {
                int[] values = optionsMenu.getOptionValues();

                int rewith = graphics.PreferredBackBufferWidth;
                int reheight = graphics.PreferredBackBufferHeight;
                bool refull = graphics.IsFullScreen;
                float resound = SoundManager.Volume;

                Initialize();
                LoadContent();
                SoundManager.Volume = resound;
                isGameEnded = false;
                isGameCompletelyEnded = false;

                graphics.PreferredBackBufferWidth = rewith;
                graphics.PreferredBackBufferHeight = reheight;
                graphics.IsFullScreen = refull;
                graphics.ApplyChanges();


                optionsMenu.setOptionValues(values);

            }

        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Teal);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;


            if (damage)
            {
                System.Console.WriteLine(damage);
                damagedraw();
                damage = false;
            }

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
            if (Enemy.worldMatrix.Count == 0 && !isTeleportPlaying && (mapindex == 0 || mapindex == 2) && isGamePlaying && !isScenePlaying && Spawning.isspawnended)
            {
                Matrix m = Matrix.CreateTranslation(new Vector3(0, 0, -4));
                portalParticle.Update();
                portalParticle.Draw();
                soundManager.portalEffectPlay();
                testModel(portal, m, viewMatrix, projectionMatrix);
            }
            if (Enemy.worldMatrix.Count == 0 && !isTeleportPlaying && (mapindex == 1 || mapindex == 3) && isGamePlaying && !isScenePlaying && Spawning.isspawnended && !isKeyPickup)
            {
                Matrix m = Matrix.CreateRotationY(matrixangle) * Matrix.CreateTranslation(new Vector3(0, 0, 0));
                if (mapindex == 1)
                {
                    testModel(key1, m, viewMatrix, projectionMatrix);
                }
                else testModel(key2, m, viewMatrix, projectionMatrix);
            }
            if (Enemy.worldMatrix.Count == 0 && !isTeleportPlaying && ((mapindex == 1 || mapindex == 3) && isKeyPickup) && isGamePlaying && !isScenePlaying && Spawning.isspawnended)
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
            if (!startMenuFlag && controlsMenuFlag)
            {
                controlsMenu.Draw(gameTime, spriteBatch);
                particleEngine2D.Draw(spriteBatch);
            }
            else if (!startMenuFlag && optionsMenuFlag)
            {
                optionsMenu.Draw(gameTime, spriteBatch);
                particleEngine2D.Draw(spriteBatch);
            }

            if (!startMenuFlag && !controlsMenuFlag && !optionsMenuFlag)
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

            if (isBossFight && !isGamePlaying && isScenePlaying && !isTeleportPlaying && !isEndCutScene1)
            {
                endCutScene.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
            }
            if (isEndCutScene2 && !isGamePlaying)
            {
                endCutScene.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
            }
            if (isEndCutScene2 & isCreditsPlaying && isGameEnded)
            {
                particleEngine2D.Update();
                particleEngine2D.Draw(spriteBatch);
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
                    //effect.DiffuseColor = Color.DarkMagenta.ToVector3();
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
                    if (doubleshot == true)
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
        public void damagedraw()
        {
            Draw3DModelColor(playerModel, player.worldMatrix * Matrix.CreateScale(1.001f, 1.001f, 1.001f), viewMatrix, projectionMatrix, Color.IndianRed);
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