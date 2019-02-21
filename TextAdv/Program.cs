using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Text_Adventure
{
    public class Game
    {
        private static Parser parser = new Parser();
        private static Room CurRoom;
        private static Room NewRoom;
        private static Room LastRoom;
        private static Stack<Room> StackBack;
        private static List<Items> Inventory;
        private static bool quit = false;

        static void Main(string[] args)
        {
            Initialize();
            Console.WriteLine("You wake up. " + CurRoom.Gdesc());
            Console.WriteLine("" + CurRoom.showDoors() + "\n");
            PLay();
            Console.ReadLine();
        }

        public static void Initialize()
        {
            Room medical;
            Room operating;
            Room lounge;
            Room lobby;
            Room bathroom;
            medical = new Room("\n You are in an old medical closet. \n\n");
            CurRoom = medical;
            operating = new Room("\n You enter an operating room.\n\n");
            lounge = new Room("You enter a waiting room.\n\n");
            lobby = new Room("You enter the lobby. \n\n");
            bathroom = new Room("You walk into the bathroom. \n\n");

            medical.Sdoor("north", operating);
            operating.Sdoor("west", lounge);
            lounge.Sdoor("west", bathroom);
            lounge.Sdoor("north", lobby);

            operating.Sdoor("south", medical);
            lounge.Sdoor("east", operating);
            bathroom.Sdoor("east", lounge);
            lobby.Sdoor("south", lounge);

            medical.AddItems("Epic Sword of POWER!:", " IT IS EPIC! \n", 1000, 999999999, 1);  
            operating.AddItems("knife:", " It's Rusty and was probably used in surgeries \n", 1, 10, 1);
            operating.AddItems("Sign:", " It explains stuff like... \n", 100, 500000, 10);
            lounge.AddItems("Chair:", " It's broken \n", 1, 1, 5);
            bathroom.AddItems("Tissues: ", "Normal tissues \n", 1, 0, 0);
        }

        public static void PLay() 
        {
            while (quit == false) 
            {
                Commands com = parser.words2();
                CommandWords commandWords = parser.words2();
                parscom(, com);
            }
        }
        public static void parscom(CommandWords commandWords, Commands command) 
        {
            if (commandWords.isvalid(command) == false) 
            {
                Console.WriteLine("That is not a valid command.");
            }
            String CommandWord = command.getCommandWord();
            if (CommandWord == "walk") 
            {
                walk(command);
            }
            if (CommandWord == "look")
            {
                look();
            }
            if (CommandWord == "take")
            {
                take();
            }
            if (CommandWord == "help")
            {
            }
        }
        public static void walk(Commands command) 
        {
            if (!command.hasSecondWord())
            {
                Console.WriteLine("Where do you whant to go? \n");
                Console.WriteLine(CurRoom.showDoors() + "\n");
            }
            else
            {
                String direction = command.getCommmandWord();
                NewRoom = CurRoom.Gdoor(direction);
                if (NewRoom == null)
                {
                    Console.WriteLine("You are blind there are no doors there.");
                }
                else
                {
                    Console.Clear();
                    LastRoom = CurRoom;
                    CurRoom = NewRoom;
                    Console.WriteLine("" + CurRoom.Gdesc());
                    Console.WriteLine("" + CurRoom.showDoors());
                    look();
                }
            }
        }
        public static void look() 
        {
            Console.WriteLine("\n HELEODKD! \n");
            Console.WriteLine("" + CurRoom.StuffInRoom());
        }
        public static void take() 
        {
        
        }
    }
    public class CommandWords
    {
        private static readonly string[] validcom = { "look", "walk", "take", "help"};

        public bool isvalid(string comm)
        {
            for (int i = 0; i < validcom.Length; i++)
            {
                if (validcom[i] == comm)
                {
                    return true;
                }
            }
            return false;
        }

        public string help()
        {
            string commandlist = "";
            for (int j = 0; j < validcom.Length; j++)
            {
                commandlist = commandlist + "-_-_-" + validcom[j];

            }
            return commandlist;
        }
    }
    public class Commands
    {
        private string onec;
        private string twoc;

        public Commands(string command1, string command2)
        {
            this.onec = command1;
            this.twoc = command2;

        }
        public bool hasSecondWord()
        {
            if (twoc == null)
            {
                return false;
            }
            return true;
        }
        public string getCommandWord()
        {
            return onec;
        }
        public string getCommmandWord()
        {
            return twoc;
        }
    }
    public class Items
    {
        
        private String name;
        private String Descrip;
        private int level;
        private int damage;
        private int weight;

        public Items(String name, String Descrip, int level, int damage,int weight)
        {
            this.name = name;
            this.Descrip = Descrip;
            this.level = level;
            this.damage = damage;
            this.weight = weight;
        }
        public String ItemName()
        {
            return name;
        }
        public String ItemID() 
        {
            return name + Descrip + " Lvl=" + level + " Damage="+ damage + " Weight=" + weight + "\n\n";
        }
    }
    public class Room
    {
        public String desc;
        public List<Items> list = new List<Items>();
        public Dictionary<String, Room> Doors = new Dictionary<String,Room>(); 

        public Room(String descrip)
        {
            this.desc = descrip;
        }

        public String Gdesc()
        {
            return desc;
        }
        public Room Gdoor(String direction)
        {
            if (Doors.ContainsKey(direction)) 
            {
                return Doors[direction];
            }
            return null;
        }
        public void Sdoor(String keys, Room roomname)
        {
            Doors.Add(keys, roomname);

        }
        public String showDoors() 
        {
            String exits = ("The possible exits are ");
            foreach(String Key in Doors.Keys)
            {
                exits += " " + Key + ",";
            }
            if (exits != " ")
            {
                return exits;
            }
            else 
            {
                Console.WriteLine("There are no possible exits.");
                return null;
            }
        }
        public void AddItems(String name, String descrip, int level, int damage,int weight)
        {
            list.Add(new Items(name, descrip, level, damage, weight));
        }
        public List<Items> RoomItems() 
        {
            return list;
        }
        public String StuffInRoom()
        {
            if (list.Count == 0)
            {
                return "There are no items in the room";
            }
            else
            {
                String box = "";
                foreach (Items I in RoomItems())
                {
                    box += I.ItemID() + " ";
                }
                return "The items in the room are \n\n" + box;
            }
        }
    }
    public class Parser 
    {
        private CommandWords CW;
        public Parser() 
        {
            CW = new CommandWords();
        }            
        public Commands words2()
        {
            String firstword, allwords, secondword;
            allwords = Console.ReadLine();
            String[] commArray;
            commArray = allwords.Split(' ');
            firstword = commArray[0];
            try
            {
                secondword = commArray[1];
            }
            catch (Exception e) 
            {
                secondword = null;
            }
            if(CW.isvalid(firstword))
            {
               Commands newcom = new Commands(firstword, secondword); 
               return newcom;   
            }
            else
            {
                Commands newcom = new Commands(firstword, secondword);
                return newcom; 
            }
        }
    }
}
