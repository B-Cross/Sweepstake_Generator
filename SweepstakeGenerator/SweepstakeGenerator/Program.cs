namespace SweepstakeGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Hello and welcome to the fifa world cup 2022 sweepstake generator!\r\n_______________________________________________");
            ///Just displays all teams.
            Console.WriteLine("All teams in ranking order:");

            ///Change colour for UX
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < 32; i++)
            {
                Console.WriteLine((Teams)i);
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\r\nPress any key to progress to player input...");

            Console.ReadKey();
            Console.Clear();


            ///Collect list of all players involved from user input
            Dictionary<string,List<Teams>> players = GetPlayers();


            Console.WriteLine("\r\nPress any key to progress toward bracket generation...");
            Console.ReadKey();
            Console.Clear();

            ///Assign teams to players
            Dictionary<string, List<Teams>> bracket = GenerateBracket(players);



            ///Display players with their assigned teams 
            foreach(string key in bracket.Keys)
            {
                Console.WriteLine(key);
                foreach(Teams team in bracket[key])
                {
                    Console.WriteLine("    ==>" + team);
                }
                Console.WriteLine("---");
            }

            ///Generate a csv of the players and their assigned teams. Save to a local file and print location


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bracket generation complete! Best of luck!\r\nPress any key to finish...");
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.White;

        }
        public static Dictionary<string, List<Teams>> GetPlayers()
        {
            Dictionary<string, List<Teams>> players = new Dictionary<string, List<Teams>>();

            Console.WriteLine("Please enter the names of the players that you would like to join.\r\n\r\n****ONE AT A TIME****");

            bool allPlayers = false;
            while (!allPlayers)
            {
                Console.WriteLine("Enter Name: ");
                string playerName = Console.ReadLine().ToUpper();
                if (!players.ContainsKey(playerName))
                {
                    players.Add(playerName, new List<Teams>());



                    bool addAnother = false;

                    while (!addAnother)
                    {
                        Console.WriteLine("Would you like to add another player? y/n");
                        string result = Console.ReadLine();

                        switch (result.ToLower())
                        {
                            case "y":
                            case "yes":
                                addAnother = true;
                                break;
                            case "n":
                            case "no":
                                addAnother = true;
                                allPlayers = true;
                                break;
                            default:
                                Console.WriteLine("Try again. Enter one of the specified values");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please try again with a unique name.\r\nPress any key to continue the operation...");
                    Console.ReadKey();
                }
                Console.Clear();
            }
            Console.Clear();
            Console.WriteLine("The list of players is as follows:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (string key in players.Keys)
            {
                Console.WriteLine(key);
            }
            Console.ForegroundColor = ConsoleColor.White;

            return players;
        }
        public static Dictionary<string, List<Teams>> GenerateBracket(Dictionary<string, List<Teams>> players)
        {
            Console.WriteLine("Generating bracket");

            Dictionary<string, List<Teams>> bracket = new Dictionary<string, List<Teams>>();


            ///Generate random number generators with random seeds
            Random randomSeedForAllRandoms = new Random();

            Random randomTeamSeed = new Random(randomSeedForAllRandoms.Next());
            Random randomPlayerSeed = new Random(randomSeedForAllRandoms.Next());

            Random randomTeamGenerator = new Random(randomTeamSeed.Next());
            Random randomPlayerGenerator = new Random(randomPlayerSeed.Next());


            List<int> remainingTeams = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

            int leftovers = remainingTeams.Count % players.Count;
            bracket.Add("UNUSED TEAMS", new List<Teams>());
            ///Disqualify teams that would lead to players having unequal amounts
            for (int i = 0; i < leftovers; i++)
            {
                int random = randomTeamGenerator.Next(0, remainingTeams.Count-1);
                bracket["UNUSED TEAMS"].Add((Teams)remainingTeams[random]);
                remainingTeams.RemoveAt(random);
            }


            ///Start assigning teams to players
            int amount_of_teams_for_1 = remainingTeams.Count / players.Count;
            int teamsToAssign = remainingTeams.Count;


            for(int i = 0; i < teamsToAssign; i++)
            {
                ///Retrieve random player assignment and dictionary entry for that player
                int playerNum = randomPlayerGenerator.Next(0, players.Count-1);
                string playerKey = players.ElementAt(playerNum).Key;


                ///Retrieve team to assign to player and remove from list of teams
                int teamNum = randomTeamGenerator.Next(0, remainingTeams.Count-1);
                players[playerKey].Add((Teams)remainingTeams[teamNum]);
                remainingTeams.RemoveAt(teamNum);

                ///If the player has the max assigned teams then add to bracket and remove from players
                if (players[playerKey].Count==amount_of_teams_for_1)
                {
                    bracket.Add(playerKey, players[playerKey]);
                    players.Remove(playerKey);
                }
            }

            return bracket;
        }
    }
}