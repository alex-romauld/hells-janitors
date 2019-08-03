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


namespace SurvivalShooter.Survival
{
    class ChallengeSystem
    {
        public String CurChallenge = "";
        public String[] Challenge;
        public String CurReward = "";
        public String[] Reward;


        private int MinTime = 1000;//150000;
        private int MaxTime = 3000;//220000;
        private int CurWaitTime;
        private int CurTime = 0;
        public Boolean playingChallenge = false;
        private SpriteFont font;

        private Texture2D square;

        private int Reward_Money;

        public ChallengeSystem()
        {
        }

        public void Load(ContentManager content)
        {
            square = content.Load<Texture2D>("Sprites/square");
            CurWaitTime = new Random().Next(MinTime, MaxTime);
            SetupChallenges();
            font = content.Load<SpriteFont>("Fonts/SecondaryFont");//SmallFont");
        }

        private void SetupChallenges()
        {
            Challenge = new string[4];
            Challenge[0] = "Don't use your\nability";
            Challenge[1] = "Don't purchase\nanything";
            Challenge[2] = "Don't get hit";
            Challenge[3] = "Get 10 kills in the\nnext 30 Seconds";

            Reward = new string[3];
            Reward[0] = "Max Ammo";
            Reward[1] = "Max Health";
            Reward[2] = "$";
        }

        public void Update(GameTime gameTime)
        {
            if (!playingChallenge)
            {
                if (CurTime < CurWaitTime)
                    CurTime += gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    CurWaitTime = new Random().Next(MinTime, MaxTime);
                    CurTime = 0;
                    playingChallenge = true;
                    CurChallenge = Challenge[new Random().Next(0, Challenge.Length)];
                    CurReward = Reward[new Random().Next(0, Reward.Length)];
                    Reward_Money = new Random().Next(20, 41) * 100;
                }
            }
        }


        int StartY = 50;
        int x;
        public void Draw(SpriteBatch spriteBatch, Viewport viewPort)
        {
            if (playingChallenge)
            {
                if(x > viewPort.Width - 275)
                    x -= 20;
                spriteBatch.Draw(square, new Rectangle(x, StartY, 250, 100), Color.Black);
                spriteBatch.Draw(square, new Rectangle(x + 4, StartY + 4, 242, 92), Color.White);

                spriteBatch.DrawString(font, CurChallenge, new Vector2(x + 6,StartY + 4), Color.Black);

                String _reward = "";
                if (CurReward == "$") _reward = "" + Reward_Money;
                spriteBatch.DrawString(font, "Reward : " + CurReward + _reward, new Vector2(x + 6, StartY + 60), Color.Black);
            }
            else
            {
                x = viewPort.Width;
            }
        }

    }
}
