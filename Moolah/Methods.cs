﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using System;
using System.Numerics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Moolah
{
    public partial class ModEntry
    {
        public static int AdjustMoney(Farmer __instance, BigInteger value)
        {
            if (value > maxValue)
            {
                //SMonitor.Log($"Storing excess money: {value - maxValue} with {moocha}");
                __instance.modData[moochaKey] = (value - maxValue) + "";
                value = maxValue;
            }
            return (int)value;
        }

        public static BigInteger GetTotalMoolah(Farmer f)
        {
            BigInteger moocha = f.Money;
            if (f.modData.TryGetValue(moochaKey, out string moochaString))
                moocha += BigInteger.Parse(moochaString);
            return moocha;
        }
        private static void DrawMoneyDial(MoneyDial moneyDial, SpriteBatch b, Vector2 position, BigInteger target)
        {
            int numDigits = currentValue.Value.ToString().Length;
            if (previousTarget.Value != target)
            {
                BigInteger diff = target - currentValue.Value;

                flipSpeed.Value = diff / 100;
                previousTarget.Value = target;
                soundTime.Value = 100 / (flipSpeed.Value * flipSpeed.Value.Sign + 1);
                if (soundTime.Value < 6)
                    soundTime.Value = 6;
            }
            if (moneyShineTimer.Value > 0 && currentValue.Value == target)
            {
                moneyShineTimer.Value -= Game1.currentGameTime.ElapsedGameTime.Milliseconds;
            }
            if (moneyMadeAccumulator.Value > 0)
            {
                moneyMadeAccumulator.Value -= ((flipSpeed.Value * flipSpeed.Value.Sign / 2) + 1) * ((moneyDial.animations.Count <= 0) ? 100 : 1);
                if (moneyMadeAccumulator.Value <= 0)
                {
                    moneyShineTimer.Value = numDigits * 60;
                }
            }
            if (moneyMadeAccumulator.Value > 2000)
            {
                Game1.dayTimeMoneyBox.moneyShakeTimer = 100;
            }
            //currentValue.Value = target;

            if (currentValue.Value != target)
            {
                currentValue.Value += flipSpeed.Value + ((currentValue.Value < target) ? 1 : -1);
                if (currentValue.Value < target)
                {
                    moneyMadeAccumulator.Value += flipSpeed.Value * flipSpeed.Value.Sign;
                }
                soundTime.Value--;

                BigInteger diff = target - currentValue.Value;
                int sign = diff > 0 ? 1 : -1;
                BigInteger abs = diff > 0 ? diff : currentValue.Value - target;

                if (abs <= flipSpeed.Value + 1 || (flipSpeed.Value != 0 && sign != flipSpeed.Value.Sign))
                {
                    currentValue.Value = target;
                }
                if (soundTime.Value <= 0)
                {
                    if (moneyDial.onPlaySound != null)
                    {
                        moneyDial.onPlaySound(sign);
                    }
                    soundTime.Value = 100 / flipSpeed.Value * flipSpeed.Value.Sign + 1;
                    if (soundTime.Value < 6)
                        soundTime.Value = 6;

                    if (Game1.random.NextDouble() < 0.4)
                    {
                        if (target > currentValue.Value)
                        {
                            moneyDial.animations.Add(new TemporaryAnimatedSprite(Game1.random.Next(10, 12), position + new Vector2((float)Game1.random.Next(30, 190), (float)Game1.random.Next(-32, 48)), Color.Gold, 8, false, 100f, 0, -1, -1f, -1, 0));
                        }
                        else if (target < currentValue.Value)
                        {
                            moneyDial.animations.Add(new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(356, 449, 1, 1), 999999f, 1, 44, position + new Vector2((float)Game1.random.Next(160), (float)Game1.random.Next(-32, 32)), false, false, 1f, 0.01f, Color.White, (float)(Game1.random.Next(1, 3) * 4), -0.001f, 0f, 0f, false)
                            {
                                motion = new Vector2((float)Game1.random.Next(-30, 40) / 10f, (float)Game1.random.Next(-30, -5) / 10f),
                                acceleration = new Vector2(0f, 0.25f)
                            });
                        }
                    }
                }
            }
            for (int i = moneyDial.animations.Count - 1; i >= 0; i--)
            {
                if (moneyDial.animations[i].update(Game1.currentGameTime))
                {
                    moneyDial.animations.RemoveAt(i);
                }
                else
                {
                    moneyDial.animations[i].draw(b, true, 0, 0, 1f);
                }
            }
            int xPosition = 0;
            if (numDigits > 8)
            {
                xPosition -= (numDigits - 8) * 24;
            }
            else
            {
                xPosition += (8 - numDigits) * 24;
            }
            bool significant = false;
            numDigits = currentValue.Value.ToString().Length;
            var showSeparator = !string.IsNullOrEmpty(Config.Separator);
            for (int j = 0; j < numDigits; j++)
            {
                int currentDigit = int.Parse(currentValue.Value.ToString()[j].ToString());
                if (currentDigit > 0 || j == numDigits - 1)
                {
                    significant = true;
                }
                if (significant)
                {
                    if (showSeparator && j < numDigits - 1 && (numDigits - j) % Config.SeparatorInterval == 1)
                    {
                        SpriteText.drawString(b, Config.Separator, (int)position.X + xPosition + Config.SeparatorX, (int)position.Y + Config.SeparatorY);
                    }
                    b.Draw(Game1.mouseCursors, position + new Vector2(xPosition, 0f), new Rectangle?(new Rectangle(286, 502 - (currentDigit) * 8, 5, 8)), Color.Maroon, 0f, Vector2.Zero, 4f + ((moneyShineTimer.Value / 60 == numDigits - j) ? 0.3f : 0f), SpriteEffects.None, 1f);
                }
                xPosition += 24;
            }
        }
        private static string CheckIntToString(string input)
        {
            if (!Config.EnableMod || input.Length <= Config.SeparatorInterval)
                return input;
            string output = "";
            for (int i = 0; i < input.Length; i++)
            {
                output += input[i];
                if (i < input.Length - 1 && (input.Length - i) % Config.SeparatorInterval == 1)
                    output += Config.Separator;
            }
            return output;
        }
    }
}