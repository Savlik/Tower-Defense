using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 * All classes that are connected to the game as a whole
 */
namespace TD
{
    public struct PlayerData
    {
        public int experience;
        public int level_opened;
        public int kills;
        public List<int> best_scores;
    }


    public static class Game
    {
        public enum GameState { gameSelect, mainMenu, inWorld};
        public static GameState state = GameState.gameSelect;
        public static World world;
        private static int slot;
        public static PlayerData playerData;

        public static void StartGame(int slot)
        {
            if (Game.slotExists(slot))
            {
                loadGame(slot);
            }
            else
            {
                initGame(slot);
            }
        }

        public static void initGame(int slot2)
        {
            slot = slot2;
            playerData.experience = 0;
            playerData.level_opened = 1;
            playerData.kills = 0;
            playerData.best_scores = new List<int>();
            for (int i = 0; i < Settings.numberOfLevels; i++)
            {
                playerData.best_scores.Add(0);
            }
        }

        public static void loadGame(int slot2)
        {
            slot = slot2;
            StreamReader sr = new StreamReader("saves/" + "saveGame_" + slot + ".txt");
            playerData.experience = int.Parse(sr.ReadLine());
            playerData.level_opened = int.Parse(sr.ReadLine());
            playerData.kills = int.Parse(sr.ReadLine());
            playerData.best_scores = new List<int>();
            for (int i = 0; i < Settings.numberOfLevels; i++)
            {
                playerData.best_scores.Add(int.Parse(sr.ReadLine()));
            }
            sr.Close();
        }

        public static void saveGame()
        {
            StreamWriter sw = new StreamWriter("saves/" + "saveGame_" + slot + ".txt");
            sw.WriteLine(playerData.experience);
            sw.WriteLine(playerData.level_opened);
            sw.WriteLine(playerData.kills);
            for (int i = 0; i < playerData.best_scores.Count; i++)
            {
                sw.WriteLine(playerData.best_scores[i]);
            }
            sw.Close();
        }

        public static bool slotExists(int slot)
        {
            return File.Exists("saves/" + "saveGame_" + slot + ".txt");
        }

        public static void deleteSlot(int slot)
        {
            if (slotExists(slot))
            {
                File.Delete("saves/" + "saveGame_" + slot + ".txt");
            }
        }

        public static void playLevel(int number){
            state = GameState.inWorld;
            Level level = new Level();
            level.loadFromFile("level_" + number + ".txt");
            level.number = number;
            Game.world = new World(level);         
        }

        public static void endLevel(bool win)
        {
            if (win)
            {
                int level_num = Game.world.level.number;
                if (level_num + 1 > Game.playerData.level_opened)
                {
                    Game.playerData.level_opened = level_num + 1;
                }
                int score = (int)world.score;
                if (score > playerData.best_scores[level_num - 1])
                {
                    playerData.best_scores[level_num - 1] = score;
                }
                Game.saveGame();
            }

            state = GameState.mainMenu;
            Game.world = null;
        }
    }

    public class Level
    {
        public Coord spawn;
        public Coord orb;
        public List<Coord> paths;
        public List<Wave> waves;
        public List<Coord> waypoints;
        public int number;
        
        public void loadFromFile(string file)
        {
            StreamReader sr = new StreamReader("levels/" + file);
            spawn = new Coord(sr.ReadLine());
            orb = new Coord(sr.ReadLine());

            paths = new List<Coord>();
            int number_of_paths = int.Parse(sr.ReadLine());
            for (int i = 0; i < number_of_paths; i++)
            {
                paths.Add(new Coord(sr.ReadLine()));
            }

            waves = new List<Wave>();
            int number_of_waves = int.Parse(sr.ReadLine());
            for (int i = 0; i < number_of_waves; i++)
            {
                Wave w = new Wave(sr.ReadLine());
                w.monster = new Monster(sr.ReadLine());
                waves.Add(w);
            }

            sr.Close();

            makeWaypoints();
        }

        public void makeWaypoints(){
            waypoints = new List<Coord>();

            //init
            int[,] map = new int[16, 16];
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    map[i, j] = -1;
                }
            }
                
            //bfs
            Queue<Coord> q = new Queue<Coord>();
            q.Enqueue(orb);
            map[orb.x, orb.y] = 0;

            while(q.Count!=0){
                Coord akt = q.Dequeue();
                for (int d = 0; d < 4; d++)
                {
                    Coord c = new Coord(akt.x + Direction.dx[d], akt.y + Direction.dy[d]);
                    if (paths.Contains(c) && map[c.x, c.y] == -1)
                    {
                        q.Enqueue(c);
                        map[c.x, c.y] = map[akt.x, akt.y] + 1;
                    }
                }
            }

            //trace back
            Coord way = spawn;
            int dist = 999;
            while (true)
            {
                int best = 999;
                Coord bestc = new Coord(-1,-1);
                for (int d = 0; d < 4; d++)
                {
                    Coord c = new Coord(way.x + Direction.dx[d], way.y + Direction.dy[d]);
                    if (c.x < 0 || c.x >= 16 || c.y < 0 || c.y >= 16) continue;
                    if (map[c.x, c.y]!= -1 && map[c.x, c.y] < best)
                    {
                        best = map[c.x, c.y];
                        bestc = c; 
                    }
                }
                if (best == 999 || best >= dist) { throw new Exception("There is no way from spawn to orb."); }
                waypoints.Add(bestc);
                way = bestc;
                dist = map[bestc.x, bestc.y];
                if (way == orb) break;
            }

            //filter to reduce size of waypoints
            for (int i = 0; i < waypoints.Count - 2; i++)
            {
                int dx1 = waypoints[i].x - waypoints[i+1].x;
                int dx2 = waypoints[i+1].x - waypoints[i+2].x;
                int dy1 = waypoints[i].y - waypoints[i+1].y;
                int dy2 = waypoints[i+1].y - waypoints[i+2].y;
                if (Math.Sign(dx1) == Math.Sign(dx2) && Math.Sign(dy1) == Math.Sign(dy2))
                {
                    waypoints.RemoveAt(i + 1);
                    i--;
                }
            }

            //log
            //for (int i = 0; i < waypoints.Count; i++)
            //{
            //    Logger.log(waypoints[i].x + " " + waypoints[i].y);
            //}
        }
    }

    public class Wave
    {
        public Monster monster; //template for copying
        public int number;  //number of monsters
        public int delay;  //delay between spawning monsters
        public int startTime;  //when the wave starts
        public List<Monster> monsters; //all instances in a wave

        public Wave(string s)
        {
            monsters = new List<Monster>();
            string[] tokens = s.Split();
            if (tokens.Length != 2) throw new Exception("Cannot read wave. Wrong number of arguments.");
            try
            {
                number = int.Parse(tokens[0]);
                delay = int.Parse(tokens[1]);
            }
            catch (Exception)
            {
                throw new Exception("Cannot read wave. Error parsing int.");
            }
            
        }
    }

}