﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyPixels
{
    class Mapchange
    {
        //int mapindex;
        bool down;
        float turnSpeed;

        public Mapchange()
        {
            //mapindex = 0;
            down = false;
            turnSpeed = 0;
        }

        public void update(ref Spawning sp,ref int mapindex, ref Player player,ref Map map,ref Shots shots,Microsoft.Xna.Framework.Content.ContentManager content)
        {

            if (player.getCurrentPlayerPosition().Y < 10&&!down)
            {
                shots.clearAll();
                turnSpeed +=0.5f;
                player.teleportup(turnSpeed);
            }
            if (player.getCurrentPlayerPosition().Y > 9)
            {
                mapindex += 1;
                map.CreateMap(mapindex);
                down = true;
                PickUps.clearpickup();

                player.teleport();//down move player to start pos (0,0,-4)
                
            }
            if (down==true&& player.getCurrentPlayerPosition().Y >0)
            {
                
                turnSpeed-=0.5f;
                //Adjust what you multiplay turnSpeed for how long you want him to stay spinning.
                //turnSpeed *= 0.90f;
                
                player.teleportdown(turnSpeed);

            }
            if (down == true && player.getCurrentPlayerPosition().Y < 0)
            {
                Spawning.isspawnended = false;
                player.teleportback();
                sp.clearEnemy();
                if (mapindex < 4)
                {
                    sp = new Spawning(map.getmapList());
                    sp.GetEnemy().initialize(content);
                }
                if (mapindex > 3)
                {
                    Enemy.worldMatrix.Clear();
                }
                down = false;
                turnSpeed = 0;
                player.resetbbox();
                Game1.isTeleportPlaying = false;
                Game1.isKeyPickup = false;
                Game1.soundManager.mapChangeEffectStop();

            }

        }
        public void draw(Player player)
        {
            player.Draw();
        }
    }
}
