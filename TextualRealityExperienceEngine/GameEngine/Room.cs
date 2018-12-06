﻿/*
MIT License

Copyright (c) 2018 

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;

using TextualRealityExperienceEngine.GameEngine.Interfaces;

namespace TextualRealityExperienceEngine.GameEngine 
{
    public class Room : IRoom
    {
        readonly IRoomExits _roomExits = new RoomExits();

        public string Name { get; set; }
        public string Description { get; set; }
        IGame _game;

        public Room()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public Room(IGame game)
        {
            Name = string.Empty;
            Description = string.Empty;
            _game = game;
        }

        public Room(IRoomExits roomExits, IGame game)
        {
            Name = string.Empty;
            Description = string.Empty;
            _roomExits = roomExits;
            _game = game;
        }

        public IGame Game
        {
            get
            {
                return _game;
            }
            set
            {
                _game = value;
            }
        }

        public Room(string name, string description, IGame game)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(Name), "The room name can not be empty.");
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentNullException(nameof(Description), "The room description can not be empty.");
            }

            Name = name;
            Description = description;
            _game = game;
        }

        public void AddExit(Direction direction, IRoom room, bool withExit = true)
        {
            _roomExits.AddExit(direction, room);

            if (!withExit)
            {
                return;
            }

            switch (direction)
            {
                case Direction.North:
                    room.AddExit(Direction.South, this, false);
                    break;

                case Direction.South:
                    room.AddExit(Direction.North, this, false);
                    break;

                case Direction.East:
                    room.AddExit(Direction.West, this, false);
                    break;

                case Direction.West:
                    room.AddExit(Direction.East, this, false);
                    break;

                case Direction.NorthEast:
                    room.AddExit(Direction.SouthWest, this, false);
                    break;

                case Direction.SouthEast:
                    room.AddExit(Direction.NorthWest, this, false);
                    break;

                case Direction.NorthWest:
                    room.AddExit(Direction.SouthEast, this, false);
                    break;

                case Direction.SouthWest:
                    room.AddExit(Direction.NorthEast, this, false);
                    break;
            }
        }

        public virtual string ProcessCommand(ICommand command)
        {
            switch (command.Verb)
            {
                case Synonyms.VerbCodes.Go:
                {
                    try
                    {
                        var room = _roomExits.GetRoomForExit((Direction)Enum.Parse(typeof(Direction), command.Noun, true));

                            if (room == null)
                                return "There is no exit to the " + command.Noun.ToLower();

                            _game.CurrentRoom = room;
                            _game.NumberOfMoves++;

                        return room.Description;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine();
                        return "Oops";
                    }
                }
                case Synonyms.VerbCodes.Look:
                {
                    if (string.IsNullOrEmpty(command.Noun))
                    {
                        return Description;
                    }
                        break;
                }


            }

            return string.Empty;

        }
    }
}
