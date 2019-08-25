using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPixels
{
    struct BossModel
    {
        public List<Model> _model;
    };
    class Boss
    {
        public BossModel bossModel;
        Model particle;
        List<ParticleEngine> ParticleEngines;
        public CreateBoundingBox cbBarm2;
        public CreateBoundingBox cbBarm3;


        public Matrix worldMatrix;

        static int numberShot;
        static int cooldown = 10;
        static int bazcolldown = 20;
        public static float healthCoolDown = 15;
        public static float HealthCoolDown = 60;

        public static bool morebullets2 = false;
        public static bool doubleshot2 = false;
        public static bool bazookashot2 = false;

        bool chase = false;
        bool round = false;
        bool bazooka = false;
        bool doubles = false;
        bool reset = false;
        int timer;

        int clockorcounter;

        public static int healthCounter { get; set; }
        private Vector3 bossPosition;
        private Vector3 target;

        private float angle = 0f;

        public void initialize(ContentManager contentManager)
        {
            bossModel._model = new List<Model>();
            bossModel._model.Add(contentManager.Load<Model>("Models/Body_Boss"));
            bossModel._model.Add(contentManager.Load<Model>("Models/Arms_Skelett_Tex"));
            bossModel._model.Add(contentManager.Load<Model>("Models/Legs_Skelett_Walk"));
            particle = contentManager.Load<Model>("Models/Shot_Tria");
            bossPosition = new Vector3(0, 0, 2);
            worldMatrix = Matrix.CreateTranslation(bossPosition);
            ParticleEngines = new List<ParticleEngine>();
            cbBarm2 = new CreateBoundingBox(bossModel._model[1], Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(bossPosition));
            cbBarm3 = new CreateBoundingBox(bossModel._model[1], Matrix.CreateTranslation(bossPosition));
            healthCounter = 20;
            numberShot = 2;
        }

        public void update(Shots shots, Player player, ref QuadTree<BoundingBox> map)
        {
            IsCollision(shots);
            if (healthCounter < 1)
            {
                //dead
            }

            if (chase == false && round == false && bazooka == false && doubles == false && reset == false)
            {

                Random r = new Random();
                int i = r.Next(0, 99);
                if (i >= 0 && i < 25)
                {
                    chase = true;
                    timer = 200;
                }
                if (i >= 25 && i < 49)
                {
                    round = true;
                    timer = 250;
                }
                if (i >= 50 && i < 74)
                {
                    doubles = true;
                    timer = 250;
                    Random random = new Random();
                    clockorcounter = random.Next(0, 4);
                }
                if (i >= 75 && i < 99)
                {
                    bazooka = true;
                    timer = 200;
                }
            }

            if (chase == true)
            {
                enemyChase(player, ref map);
                shot(shots);
                timer--;
                if (timer < 1)
                {
                    reset = true;
                }
            }
            if (round == true)
            {
                if (clockorcounter > 1)
                {
                    angle += 0.05f;
                }
                if (clockorcounter < 2)
                {
                    angle -= 0.05f;
                }

                unlimitedshot(shots);

                timer--;
                if (timer < 1)
                {
                    reset = true;
                }
            }
            if (doubles == true)
            {

                doubleshot2 = true;
                morebullets2 = true;
                //look at player
                Vector3 tar = player.getCurrentPlayerPosition() - worldMatrix.Translation;
                if (Keyboard.GetState().IsKeyDown(Keys.W) && timer < 50)
                {
                    tar.Z += 1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A) && timer < 50)
                {
                    tar.X += 1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) && timer < 50)
                {
                    tar.Z -= 1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D) && timer < 50)
                {
                    tar.X -= 1f;
                }
                tar = Vector3.Normalize(tar) / 150;
                target = tar;
                angle = (float)Math.Atan2(target.X, target.Z);
                worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(worldMatrix.Translation);

                shot(shots);
                timer--;
                if (timer < 1)
                {
                    reset = true;
                }
            }
            if (bazooka == true)
            {
                enemyChase(player, ref map);
                bazcolldown--;
                if (bazcolldown < 1)
                {
                    for (int i = 0; i < 8; i++)
                        shots.createBazookaShot(worldMatrix, 2, i);
                    bazcolldown = 40;
                }

                timer--;
                if (timer < 1)
                {
                    reset = true;
                }
            }
            if (reset == true)
            {
                doubleshot2 = false;
                morebullets2 = false;
                bazookashot2 = false;
                bazooka = false;
                doubles = false;
                chase = false;
                round = false;
                reset = false;
            }

            //look at player
            //Vector3 tar = Game1.playerPosition - bossPosition;
            //tar = Vector3.Normalize(tar) / 150;
            //target = tar;
            //angle = (float)Math.Atan2(target.X, target.Z);

            Draw();
        }

        public void shot(Shots shots)
        {
            //shots.createShot(worldMatrix, 2);
            cooldown--;
            if (cooldown < 1)
            {
                shots.createShot(worldMatrix, numberShot);
                if (doubleshot2 == true)
                {
                    if (numberShot == 2) { shots.createShot(worldMatrix, 3); }
                    else shots.createShot(worldMatrix, 2);
                }
                if (numberShot < 3 && numberShot > 1)
                    numberShot++;
                else
                    numberShot--;
                cooldown = 50;
                if (morebullets2 == true) { cooldown = 25; }
            }
        }

        public void unlimitedshot(Shots shots)
        {
            cooldown--;
            if (cooldown < 1)
            {
                shots.createShot(worldMatrix, 2);
                if (Keyboard.GetState().IsKeyDown(Keys.P) || doubleshot2 == true)
                {
                    if (numberShot == 2) { shots.createShot(worldMatrix, 2); }
                    else shots.createShot(worldMatrix, 3);
                }
                cooldown = 5;
            }
        }

        public void enemyChase(Player player, ref QuadTree<BoundingBox> map)
        {

            Matrix m = worldMatrix;
            Vector3 tar = player.worldMatrix.Translation - m.Translation;
            tar = Vector3.Normalize(tar) / 150;
            target = tar * new Vector3(1, 1, 1);
            bossPosition = m.Translation + target;
            angle = (float)Math.Atan2(target.X, target.Z);
            worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(bossPosition);
            if (IsCollision(player, ref map, target) == true)
            {
                //worldMatrix[i] = m;
                target = tar * new Vector3(1, 1, 0);
                angle = (float)Math.Atan2(target.X, target.Z);
                worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                if (IsCollision(player, ref map, target) == true)
                {
                    //worldMatrix[i] = m;
                    target = tar * new Vector3(0, 1, 1);
                    angle = (float)Math.Atan2(target.X, target.Z);
                    worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(m.Translation + target);
                    if (IsCollision(player, ref map, target) == true)
                    {
                        worldMatrix = m;
                    }


                }

            }



        }

        public bool IsCollision(Shots shots)// for shot to enemy
        {
            bool hit = false;

            for (int i = 0; i < 2; i++)
            {
                CreateBoundingBox cbB = new CreateBoundingBox(bossModel._model[i], worldMatrix);
                if (shots.IsCollision(ref cbB.bBox))// test if shot hits enemy
                {
                    if (healthCoolDown < 1)
                    {
                        Vector3 enemyDisappearPosition = worldMatrix.Translation;
                        Game1.soundManager.enemyShotEffect();
                        ParticleEngines.Add(new ParticleEngine(particle, enemyDisappearPosition, 0f, "Enemy"));
                        healthCounter--;
                        //worldMatrix.Remove(worldMatrix[n]);//disapear
                        healthCoolDown = HealthCoolDown;
                    }
                        hit = true;
                        break;
                }


            }

            return hit;
        }

        public bool IsCollision(Player player, ref QuadTree<BoundingBox> _QTree, Vector3 target)
        {
            bool hit = false;
            CreateBoundingBox cbBpbody = player.cbBbody;
            CreateBoundingBox cbBparm = player.cbBarm;
            CreateBoundingBox cbBbody = new CreateBoundingBox(bossModel._model[0], Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(worldMatrix.Translation));
            CreateBoundingBox cbBarm = new CreateBoundingBox(bossModel._model[1], Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(worldMatrix.Translation));

            for (int n = 0; n < 2; n++)
            {
                if (cbBarm.bBox.Intersects(cbBpbody.bBox) || cbBbody.bBox.Intersects(cbBpbody.bBox) || cbBarm.bBox.Intersects(cbBparm.bBox) || cbBbody.bBox.Intersects(cbBparm.bBox))
                {
                    if (Player.healthCoolDown < 1)
                    {
                        Player.healthCounter--;
                        Player.healthCoolDown = Player.HealthCoolDown;
                    }
                    return true;
                }
            }


            cbBarm2.bBox.Max += target;
            cbBarm2.bBox.Min += target;
            cbBarm3.bBox.Max += target;
            cbBarm3.bBox.Min += target;

            List<BoundingBox> temp = _QTree.seekData(new Vector2(cbBarm2.bBox.Min.X, cbBarm2.bBox.Min.Z),
                new Vector2(cbBarm2.bBox.Max.X, cbBarm2.bBox.Max.Z));

            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm2.bBox.Intersects(temp[u]))//test if playerarm hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }

            temp = _QTree.seekData(new Vector2(cbBarm3.bBox.Min.X, cbBarm3.bBox.Min.Z),
            new Vector2(cbBarm3.bBox.Max.X, cbBarm3.bBox.Max.Z));


            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm3.bBox.Intersects(temp[u]))//test if playerarm hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }


            temp = _QTree.seekData(new Vector2(cbBbody.bBox.Min.X, cbBbody.bBox.Min.Z),
            new Vector2(cbBbody.bBox.Max.X, cbBbody.bBox.Max.Z));

            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBbody.bBox.Intersects(temp[u]))//test if bossbody hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }


            temp = _QTree.seekData(new Vector2(cbBarm.bBox.Min.X, cbBarm.bBox.Min.Z),
            new Vector2(cbBarm.bBox.Max.X, cbBarm.bBox.Max.Z));

            for (int u = 0; u < temp.Count; ++u)
            {
                if (cbBarm.bBox.Intersects(temp[u]))//test if bossarm hits map
                {
                    cbBarm2.bBox.Max -= target;
                    cbBarm2.bBox.Min -= target;
                    cbBarm3.bBox.Max -= target;
                    cbBarm3.bBox.Min -= target;
                    return true;
                    //hit = true;
                }
            }
            //if (hit == true)
            //{
            //    cbBarm2.bBox.Max -= target;
            //    cbBarm2.bBox.Min -= target;
            //    cbBarm3.bBox.Max -= target;
            //    cbBarm3.bBox.Min -= target;
            //}
            return hit;
        }


        public void Draw()
        {
            for (int i = 0; i < ParticleEngines.Count; i++)
            {
                ParticleEngines[i].Update();
                ParticleEngines[i].Draw();
            }
            worldMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(worldMatrix.Translation);
            Game1.Draw3DModel(bossModel._model[0], worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(bossModel._model[1], worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
            Game1.Draw3DModel(bossModel._model[2], worldMatrix, Game1.viewMatrix, Game1.projectionMatrix);
        }

        public void Draw2()
        {
            worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(bossPosition);
            for (int i = 0; i < ParticleEngines.Count; i++)
            {
                ParticleEngines[i].Update();
                ParticleEngines[i].Draw();
            }
            for (int z = 0; z < bossModel._model.Count; ++z)
            {
                foreach (ModelMesh mesh in bossModel._model[z].Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;

                        effect.View = Game1.viewMatrix;
                        effect.Projection = Game1.projectionMatrix;

                        //effect.DiffuseColor = Color.MediumBlue.ToVector3();

                        effect.World = worldMatrix;
                        mesh.Draw();

                    }
                }
            }
        }
    }
}
