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
        Song background;
        Song fight;
        Song cutscenes;
        Song credit;
        SoundEffect typing;
        SoundEffect shot, wallShot, enemyShot, hurt;
        SoundEffect burstMove, portalShrine, mapChange, portalopen;
        SoundEffect menuclick, pickup;
        SoundEffectInstance portal;

        public bool isPortalPlay = false;
        public bool fightPlay = false;
        public bool isCutscenePlay = false;

        public static float Volume;
        public static float Music;
        public static float Effects;

        public void LoadContent(ContentManager Content)
        {
            menuBGM = Content.Load<Song>("Audio/Songs/416632__sirkoto51__castle-music-loop-1");
            background = Content.Load<Song>("Audio/Songs/337789__astronautchild__one-year-of-error-in-human-calendar");
            fight = Content.Load<Song>("Audio/Songs/443128__sirkoto51__boss-battle-loop-3");
            cutscenes = Content.Load<Song>("Audio/Songs/326553__shadydave__the-sonata-piano-loop");
            //credit = Content.Load<Song>("Audio/Songs/???");
            typing = Content.Load<SoundEffect>("Audio/typing_0");

            Volume = 0.5f;
            Music = 0.5f;
            Effects = 1f;

            //Shooting
            shot = Content.Load<SoundEffect>("Audio/Shooting/shot");
            wallShot = Content.Load<SoundEffect>("Audio/Shooting/257929__kane53126__bat-hit-against-wall");
            enemyShot = Content.Load<SoundEffect>("Audio/Shooting/enemy_shot");
            hurt = Content.Load<SoundEffect>("Audio/Shooting/350920__cabled-mess__hurt-c-06");

            //Player
            burstMove = Content.Load<SoundEffect>("Audio/Player/burst_move");
            mapChange = Content.Load<SoundEffect>("Audio/Player/map_change");
            portalopen = Content.Load<SoundEffect>("Audio/Player/401324__alanmcki__magical-portal-open");
            portalShrine = Content.Load<SoundEffect>("Audio/Player/170523__alexkandrell__royal-sparkle-whoosh-centre");
            portal = portalShrine.CreateInstance();
            portal.IsLooped = true;
            pickup = Content.Load<SoundEffect>("Audio/Player/332629__treasuresounds__item-pickup");
            MediaPlayer.Volume = 0.25f;
            menuclick = Content.Load<SoundEffect>("Audio/145443__soughtaftersounds__menu-click-dry");
        }

        public void update()
        {
            MediaPlayer.Volume = Volume*Music;
            MediaPlayer.IsRepeating = true;
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

        public void BackgroundMusicPlay()
        {
            MediaPlayer.Play(background);
        }

        public void BackgroundMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void CreditMusicPlay()
        {
            MediaPlayer.Play(credit);
        }

        public void CreditMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void CutsceneMusicPlay()
        {
            MediaPlayer.Play(cutscenes);
        }

        public void CutsceneMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void FightMusicPlay()
        {
            MediaPlayer.Play(fight);
        }

        public void FightMusicStop()
        {
            MediaPlayer.Stop();
        }

        public void shotEffect()
        {
            if (Game1.isGamePlaying)
                shot.Play(Volume* Effects, 0, 0);
        }

        public void wallShotEffect()
        {
            if (Game1.isGamePlaying)
                wallShot.Play(Volume * Effects, 0, 0);
        }

        public void enemyShotEffect()
        {
            if (Game1.isGamePlaying)
                enemyShot.Play(Volume * Effects, 0, 0);
        }

        public void burstEffect()
        {
            if (Game1.isGamePlaying)
                burstMove.Play(Volume * Effects, 0, 0);
        }

        public void hurtEffect()
        {
            if (Game1.isGamePlaying)
                hurt.Play(Volume * Effects, 0, 0);
        }

        public void menuclickEffect()
        {
                menuclick.Play(Volume * Effects, 0, 0);
        }

        public void pickupEffect()
        {
            pickup.Play(Volume * Effects, 0, 0);
        }

        public void portalEffectPlay()
        {
            if (Game1.isGamePlaying)
            {
                if (!isPortalPlay)
                {
                    portalopen.Play(Volume * Effects, 0, 0);
                    
                    portal.Volume = Volume * Effects;
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
            mapChange.Play(Volume * Effects, 0, 0);
        }
    }
}
