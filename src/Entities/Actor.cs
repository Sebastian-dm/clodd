﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using clodd.Tiles;

namespace clodd.Entities {
    public abstract class Actor : Entity {

        //private int _health; //current health
        //private int _maxHealth; //maximum possible health

        public int Health { get; set; } // current health
        public int MaxHealth { get; set; } // maximum health
        public int AttackStrength { get; set; } // attack strength
        public int AttackChance { get; set; } // percent chance of successful hit
        public int DefenseStrength { get; set; } // defensive strength
        public int DefenseChance { get; set; } // percent chance of successfully blocking a hit
        public int Gold { get; set; } // amount of gold carried

        public List<Item> Inventory = new List<Item>(); // the player's collection of items

        protected Actor(Color foreground, Color background, int glyph, int width = 1, int height = 1) : base(foreground, background, glyph, width, height) {
            
        }

        /// <summary>
        /// Moves the Actor BY positionChange tiles in any X/Y direction
        /// </summary>
        /// <param name="positionChange"></param>
        /// <returns>true if actor was able to move, false if failed to move</returns>
        public bool MoveBy(Point positionChange) {
            // Check the current map if we can move to this new position
            if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange)) {
                // if there's a monster here,
                // do a bump attack
                Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
                Item item = GameLoop.World.CurrentMap.GetEntityAt<Item>(Position + positionChange);
                if (monster != null) {
                    GameLoop.CommandManager.Attack(this, monster);
                    return true;
                }
                // if there's an item here,
                // try to pick it up
                else if (item != null) {
                    GameLoop.CommandManager.Pickup(this, item);
                    return true;
                }

                Position += positionChange;
                return true;
            }
            // Handle situations where there are non-walkable tiles that CAN be used
            else {
                // Check for the presence of a door
                TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
                // if there's a door here,
                // try to use it
                if (door != null) {
                    GameLoop.CommandManager.UseDoor(this, door);
                    return true;
                }
                return false;
            }
        }



        /// <summary>
        /// Moves the Actor TO newPosition location
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns>returns true if actor was able to move, false if failed to move</returns>
        public bool MoveTo(Point newPosition) {
            Position = newPosition;
            return true;
        }

    }
}
