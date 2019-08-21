using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KeyPixels
{
    public class SoundManager
    {
        Song menuBGM;
        SoundEffect typing;
        SoundEffect shot, wallShot, enemyShot;
        SoundEffect burstMove, portalShrine, mapChange;
        SoundEffectInstance portal;

        bool isPortalPlay = false;

        public void LoadContent(ContentManager Content)
        {
            menuBGM = Content.Load<Song>("Audio/menu_bgm");
            typing = Content.Load<SoundEffect>("Audio/typing_0");

            //Shooting
            shot = Content.Load<SoundEffect>("Audio/Shooting/shot");
            wallShot = Content.Load<SoundEffect>("Audio/Shooting/wall_shot");
            enemyShot = Content.Load<SoundEffect>("Audio/Shooting/enemy_shot");

            //Player
            burstMove = Content.Load<SoundEffect>("Audio/Player/burst_move");
            mapChange = Content.Load<SoundEffect>("Audio/Player/map_change");
            portalShrine = Content.Load<SoundEffect>("Audio/Player/portal");
            portal = portalShrine.CreateInstance();
            portal.IsLooped = true;

        }


        public void typingEffect()
        {
            typing.Play();  
        }

        public void menuBackgroundMusicPlay()
        {
            MediaPlayer.Play(menuBGM);
            MediaPlayer.Volume = 0.5f;
        }

        public void menuBackgroundMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void shotEffect()
        {
            shot.Play();
        }

        public void wallShotEffect()
        {
            wallShot.Play();
        }

        public void enemyShotEffect()
        {
            enemyShot.Play();
        }

        public void burstEffect()
        {
            burstMove.Play();
        }

        public void portalEffectPlay()
        {
            if (!isPortalPlay)
            {
                portal.Play();
                isPortalPlay = true;
            }
        }

        public void portalEffectStop()
        {
            portal.Stop();
            isPortalPlay = false;
        }

        public void mapChangeEffect()
        {
            mapChange.Play();
        }
    }
}
