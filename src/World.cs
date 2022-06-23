﻿using System;
using System.Collections.Generic;
using SadConsole.Components;
using Microsoft.Xna.Framework;
using clodd.Entities;
using clodd.Tiles;
using clodd.Map;

namespace clodd {

    /// <summary>
    /// Generates and stores all game state data.
    /// </summary>
    public class World {

        private Random RandNumGenerator = new Random();

        // map creation and storage data
        private int _mapWidth = 87;
        private int _mapHeight = 47;
        private int _maxRooms = 1000;
        private int _minRoomSize = 4;
        private int _maxRoomSize = 20;
        private TileBase[] _mapTiles;

        public Map.Map CurrentStage { get; set; }
        public Player Player { get; set; }

        public GoRogue.MultiSpatialMap<Actor> Entities {
            get {
                return CurrentStage.Entities;
            }
        }



        /// <summary>
        /// Creates a new game world and stores it in publicly accessible
        /// </summary>
        public World() {
            CreateMap();
            CreatePlayer();
            CreateMonsters();
            CreateLoot();
        }



        /// <summary>
        /// Create a new map using the Map class and a map generator.
        /// Uses several parameters to determine .
        /// </summary>
        private void CreateMap() {
            _mapTiles = new TileBase[_mapWidth * _mapHeight];
            CurrentStage = new Map.Map(_mapWidth, _mapHeight);
            DungeonGenerator mapGen = new DungeonGenerator();
            CurrentStage = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms);
            //MapGenerator mapGen = new MapGenerator();
            //CurrentMap = mapGen.GenerateMap(_mapWidth, _mapHeight, _maxRooms, _minRoomSize, _maxRoomSize);
        }



        /// <summary>
        /// Create a player using the Player class and set its starting position
        /// </summary>
        private void CreatePlayer() {
            Player = new Player(new Color(20, 255, 255), Color.Transparent);

            // Place the player on the first non-movement-blocking tile on the map
            if (CurrentStage.Rooms.Count > 0) {
                int RoomIndex = RandNumGenerator.Next(0, CurrentStage.Rooms.Count);
                Player.Position = CurrentStage.Rooms[RoomIndex].Center;
            }
            else {
                Player.Position = new Point(10, 10);
            }
            

            // add the player to the Map's collection of Entities
            CurrentStage.Add(Player);
        }


        // Create some random monsters with random attack and defense values
        // and drop them all over the map in
        // random places.
        private void CreateMonsters() {
            // number of monsters to create
            int numMonsters = 10;

            // Create several monsters and 
            // pick a random position on the map to place them.
            // check if the placement spot is blocking (e.g. a wall)
            // and if it is, try a new position
            for (int i = 0; i < numMonsters; i++) {
                int monsterPosition = 0;
                Monster newMonster = new Monster(Color.HotPink, Color.Transparent);

                // pick a random spot on the map
                while (CurrentStage.Tiles[monsterPosition].IsBlockingMove) {
                    monsterPosition = RandNumGenerator.Next(0, CurrentStage.Width * CurrentStage.Height);
                }

                // plug in some magic numbers for attack and defense values
                newMonster.DefenseStrength = RandNumGenerator.Next(0, 10);
                newMonster.DefenseChance = RandNumGenerator.Next(0, 50);
                newMonster.AttackStrength = RandNumGenerator.Next(0, 10);
                newMonster.AttackChance = RandNumGenerator.Next(0, 50);
                newMonster.Name = "a common troll";

                // Set the monster's new position
                newMonster.Position = new Point(monsterPosition % CurrentStage.Width, monsterPosition / CurrentStage.Width);
                CurrentStage.Add(newMonster);
            }
        }



        /// <summary>
        /// Create some sample treasure that can be picked up on the map
        /// </summary>
        private void CreateLoot() {
            // number of treasure drops to create
            int numLoot = 20;

            // Produce lot up to a max of numLoot
            for (int i = 0; i < numLoot; i++) {
                // Create an Item with some standard attributes
                int lootPosition = 0;
                Item newLoot = new Item(Color.Beige, Color.Transparent, "Loot", glyph: 384, 2);

                // Try placing the Item at lootPosition; if this fails, try random positions on the map's tile array
                while (CurrentStage.Tiles[lootPosition].IsBlockingMove) {
                    // pick a random spot on the map
                    lootPosition = RandNumGenerator.Next(0, CurrentStage.Width * CurrentStage.Height);
                }

                // set the loot's new position
                newLoot.Position = new Point(lootPosition % CurrentStage.Width, lootPosition / CurrentStage.Width);

                // add the Item to the MultiSpatialMap
                CurrentStage.Add(newLoot);
            }

        }

    }
}