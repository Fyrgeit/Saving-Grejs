using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace Saving_Grejs
{
    class Program
    {
        static void Main(string[] args)
        {
            //Log a few games examples into the class
            Game[] gameList = {
                new Game(
                    "Minecraft",
                    2010,
                    new List<string>{
                        "Survival",
                        "SinglePlayer",
                        "Sandbox",
                        "Adventure",
                        "Multiplayer",
                        "3D"
                    },
                    "Mojang",
                    false
                ),

                new Game(
                    "Celeste",
                    2018,
                    new List<string>{
                        "SinglePlayer",
                        "Platformer",
                        "2D"
                    },
                    "Matt Makes Games",
                    false
                ),

                new Game(
                    "Untitled goose game",
                    2019,
                    new List<string>{
                        "Puzzle",
                        "Stealth",
                        "Single player",
                        "Topdown 3D"
                    },
                    "House House",
                    false
                )
            };
            
            //Variable for seeing which alternative is selected in the menu
            int selected = 0;
            //Variable for tracking key presses
            string key = "";
            //Variable for breaking the while loop
            bool exit = false;

            
            int level = 0;
            int selectedGame = 0;
            int selectedParam = 0;

            int listLength = 0;
            
            //Program loop
            while (!exit)
            {   
                //Clear the console and send instructions
                Console.Clear();
                Console.WriteLine(
                    "Welcome to IGDB, number of games logged: " + gameList.GetLength(0) +
                    "\nNavigate using W and S or arrow keys" +
                    "\nPress Enter to select a game and Escape to go back or exit" +
                    "\nPress J to save all games to JSON\n"
                );

                //
                switch (level)
                {
                    case 0:
                        PrintGames(gameList, selected);
                        break;
                    case 1:
                        PrintGameParams(gameList, selected, selectedGame, listLength);
                        break;
                    case 2:
                        EditParam(gameList, selectedGame, selectedParam);
                        break;
                    default:
                        break;
                }                

                //Assign key to the pressed key
                if(level != 2)
                    key = Console.ReadKey().Key.ToString();

                switch(level)
                {
                    case 0:
                        listLength = gameList.GetLength(0);
                        break;
                    case 1:
                        listLength = 5;
                        break;
                    case 2:
                        listLength = gameList[selectedGame].genres.Count-1;
                        break;
                }

                //Decide outcome based on key press
                switch (key)
                {
                    case "DownArrow" or "S":
                        //Step down (go to top at the bottom)
                        selected ++;
                        if (selected == listLength)
                            selected = 0;
                        break;
                    case "UpArrow" or "W":
                        //Step up (go to bottom at the top)
                        selected --;
                        if (selected == -1)
                            selected = listLength-1;
                        break;
                    case "J":
                        SaveToJSON(gameList);
                        break;
                    case "L":
                        LoadFromJSON(gameList);
                        break;
                    case "Enter":
                        //Go a different menu based on the selected game
                        switch(level)
                        {
                            case 0:
                                level = 1;
                                selectedGame = selected;
                                selected = 0;
                                break;
                            case 1:
                                level = 2;
                                selectedParam = selected;
                                selected = 0;
                                break;
                        }
                        break;
                    case "Escape":
                        //Go back to default menu
                        level --;
                        selected = 0;
                        //Close the program if alread in the default menu
                        if (level == -1)
                        {
                            exit = true;
                        }
                        break;
                }

                /*Souvenir
                if(exit)
                {
                    break;
                }
                */
            }
        }

        /// <summary>
        /// Prints the current list of games including and arrow pointed at the selected one
        /// </summary>
        /// <param name="gameList"></param>
        /// <param name="selected"></param>
        static void PrintGames(Game[] gameList, int selected)
        {
            string prefix;
            
            for (var i = 0; i < gameList.GetLength(0); i++)
            {
                if (i == selected)
                {
                    prefix = "-> ";
                }
                else
                {   
                    prefix = "   ";
                }
                
                Console.WriteLine($"{prefix}{i+1}: {gameList[i].name}");
            }
        }

        /// <summary>
        /// Prints out all the paramaters of the selected game. (except for the genres)
        /// </summary>
        /// <param name="gameList"></param>
        /// <param name="selected"></param>
        /// <param name="selectedGame"></param>
        static void PrintGameParams(Game[] gameList, int selected, int selectedGame, int listLength)
        {
            //Initialise the prefix variable
            string prefix;
            
            //Cycle through parameters
            for (var i = 0; i < listLength; i++)
            {
                //Show the arrow before the selected alternative
                if (i == selected)
                {
                    prefix = "-> ";
                }
                else
                {   
                    prefix = "   ";
                }
                
                //Write all parameters of the game prefixed with the selection prefix and a parameter lable
                switch (i) {
                    case 0:
                        Console.WriteLine($"{prefix}Name: {gameList[selectedGame].name}");
                        break;
                    case 1:
                        Console.WriteLine($"{prefix}Launch year: {gameList[selectedGame].launchYear}");
                        break;
                    case 2:
                        //Write out all the games genres and put commas between them if there are several
                        Console.Write($"{prefix}Genres: ");
                        foreach (var genre in gameList[selectedGame].genres)
                        {
                            if(gameList[selectedGame].genres.IndexOf(genre) == 0)
                                Console.Write(genre);
                            else
                                Console.Write($", {genre}");
                        }
                        Console.Write("\n");
                        break;
                    case 3:
                        Console.WriteLine($"{prefix}Publisher: {gameList[selectedGame].publisher}");
                        break;
                    case 4:
                        Console.WriteLine($"{prefix}Early access?: {gameList[selectedGame].earlyAccess}");
                        break;
                    default:
                        break;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameList"></param>
        /// <param name="selected"></param>
        /// <param name="listLength"></param>
        static void EditParam(Game[] gameList, int selectedGame, int selectedParam)
        {
            string editCommand;

            if(selectedParam == 2)
            {

            }
            else
            {
                Console.WriteLine( "These are the current parameter:\n" +
                (selectedParam == 0 ? gameList[selectedGame].name: 
                (
                    selectedParam == 1 ? gameList[selectedGame].launchYear:
                    (
                        selectedParam == 3 ? gameList[selectedGame].publisher:
                        gameList[selectedGame].earlyAccess
                )))
                + "\nWrite something in the console to replace it. Write \"back\" to not change it"
                );
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameList"></param>
        static void SaveToJSON(Game[] gameList)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize<Game[]>(gameList, options);

            File.WriteAllText("GameList.json", jsonString);
        }
        
        static void LoadFromJSON(Game[] gameList)
        {
            string jsonString = File.ReadAllText(@"GameList.json");

            gameList = JsonSerializer.Deserialize<Game[]>(jsonString);
        }
    }


    class Game
    {
        public string name {get; set;}
        public int launchYear {get; set;}
        public List<string> genres {get; set;}
        public string publisher {get; set;}
        public bool earlyAccess {get; set;}
        
        public Game(string name, int launchYear, List<string> genres, string publisher, bool earlyAccess)
        {
            this.name = name;
            this.launchYear = launchYear;
            this.genres = genres;
            this.publisher = publisher;
            this.earlyAccess = earlyAccess;
        } 
    }
}
