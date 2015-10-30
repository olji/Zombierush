using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Game1 {
    interface ICommand {
        void execute(GameTime gametime, Player player, bool sound);
    }
    class jumpCommand : ICommand{
        public void execute(GameTime gametime, Player player, bool sound) {
            if (!player.iAir && !player.cDead) {
                player.JumpFunc();
                if (sound)
                    player.jSound.Play();
            }
        }
    }
    class moveLeftCommand : ICommand {
        public void execute(GameTime gametime, Player player, bool sound) {
            if (!player.cDead) {
                player.rCheck = true;
                player.rCheck = true;
                player.flip = false;
                if (player.runningFrame > 11)
                    player.runningFrame = 6;
                player.runningTime += gametime.ElapsedGameTime.Milliseconds;
                if (player.runningTime >= player.animSpeed) {
                    player.runningTime = 0;
                    player.runningFrame++;
                    if (player.runningFrame > 11)
                        player.runningFrame = 6;
                }

                if (player.stepsInst.State == SoundState.Stopped && !player.iAir && sound) {
                    player.stepsInst.Volume = 0.1f;
                    player.stepsInst.Play();
                }

                player.momentumX -= player.rAcc;
                if (player.momentumX < -player.pMaxSpeed)
                    player.momentumX = -player.pMaxSpeed;
            }
        }
    }
    class moveRightCommand : ICommand {
        public void execute(GameTime gametime, Player player, bool sound) {
            if (!player.cDead) {
                player.rCheck = true;
                player.flip = true;
                if (player.runningFrame < 12)
                    player.runningFrame = 12;
                player.runningTime += gametime.ElapsedGameTime.Milliseconds;
                if (player.runningTime >= player.animSpeed) {
                    player.runningTime = 0;
                    player.runningFrame++;
                    if (player.runningFrame > 17)
                        player.runningFrame = 12;
                }

                if (player.stepsInst.State == SoundState.Stopped && !player.iAir && sound) {
                    player.stepsInst.Volume = 0.1f;
                    player.stepsInst.Play();
                }

                player.momentumX += player.rAcc;
                if (player.momentumX > player.pMaxSpeed)
                    player.momentumX = player.pMaxSpeed;
            }
        }
    }
    class idleCommand : ICommand {
        public void execute(GameTime gametime, Player player, bool sound) {
            player.stepsInst.Stop();
            player.moveState = 0;
            player.rCheck = false;
        }
    }
}
