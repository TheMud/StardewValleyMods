﻿using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using System.Collections.Generic;

namespace HedgeMaze
{
    public class MazeData
    {
        public Point corner;
        public Point mapSize;
        public List<Vector2> fairyTiles = new();
        public List<Point> openTiles = new();
        public List<Point> endTiles = new();
        public List<Point> vertTiles = new();
        public string mapPath;
        public EntranceSide entranceSide;
        public int entranceOffset;

        public bool[,] tiles;

        public bool HideMaze = true;
        public bool HideBorders = false;
        public bool AddTorches = true;
        public bool AddDwarf = true;
        public Dictionary<string, int> ItemListChances = new Dictionary<string, int>
        {
            {"MeleeWeapon", 100},
            {"Shirt", 100},
            {"Pants", 100},
            {"Hat", 100},
            {"Boots", 100},
            {"BigCraftable", 100},
            {"Ring", 100},
            {"Seed", 100},
            {"Mineral", 100},
            {"Relic", 100},
            {"Cooking", 100},
            {"Fish", 0},
            {"BasicObject", 100}
        };
        public int MinItemValue = 20;
        public int MaxItemValue = -1;
        public int ItemsBaseMaxValue = 100;
        public int Mult = 100;
        public float RarityChance = 0.2f;
        public float IncreaseRate = 0.3f;
        public int CoinBaseMin = 20;
        public int CoinBaseMax = 100;
        public int MineLevelMin = 10;
        public int MineLevelMax = 100;
        public int MaxItems;
        public int FairiesMin;
        public int FairiesMax;
        public int TreasureMin;
        public int TreasureMax;
        public int ForageMin;
        public int ForageMax;
        public int SlimeMin;
        public int SlimeMax;
        public int SerpentMin;
        public int SerpentMax;
        public int BatMin;
        public int BatMax;
        public int ShadowBruteMin;
        public int ShadowBruteMax;
        public int ShadowShamanMin;
        public int ShadowShamanMax;
        public int SquidMin;
        public int SquidMax;
        public int SkeletonMin;
        public int SkeletonMax;
        public int DustSpriteMin;
        public int DustSpriteMax;
    }

    public enum EntranceSide
    {
        Top,
        Bottom, 
        Left, 
        Right
    }
}