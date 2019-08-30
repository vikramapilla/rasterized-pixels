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

        public bool isPortalPlay = false;

        public static float Volume;

        public void LoadContent(ContentManager Content)
        {
            menuBGM = Content.Load<Song>("Audio/menu_bgm");
            typing = Content.Load<SoundEffect>("Audio/typing_0");

            Volume = 0.5f;

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

        public void update()
        {
            MediaPlayer.Volume = Volume;
        }

        public void typingEffect()
        {
            typing.Play();
        }

        public void menuBackgroundMusicPlay()
        {
            MediaPlayer.Play(menuBGM);
        }

        public void menuBackgroundMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void shotEffect()
        {
            if (Game1.isGamePlaying)
                shot.Play(Volume, 0, 0);
        }

        public void wallShotEffect()
        {
            if (Game1.isGamePlaying)
                wallShot.Play(Volume, 0, 0);
        }

        public void enemyShotEffect()
        {
            if (Game1.isGamePlaying)
                enemyShot.Play(Volume, 0, 0);
        }

        public void burstEffect()
        {
            if (Game1.isGamePlaying)
                burstMove.Play(Volume, 0, 0);
        }

        public void portalEffectPlay()
        {
            if (Game1.isGamePlaying)
            {
                if (!isPortalPlay)
                {
                    portal.Volume = Volume;
                    portal.Play();
                    isPortalPlay = true;
                }
                else
                {
                    portal.Resume();
                }
            }
        }

        public void portalEffectPause()
        {
            portal.Pause();
        }

        public void portalEffectStop()
        {
            portal.Stop();
            isPortalPlay = false;
        }

        public void mapChangeEffect()
        {
            mapChange.Play(Volume, 0, 0);
        }
    }
}
