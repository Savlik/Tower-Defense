using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 * All classes that are connected the world eg. one instance of battle
 */
namespace TD
{
    public class World
    {
        public List<Monster> monsters;
        public List<Gem> gems;
        public List<Shot> shots;
        public List<Coord> towers;
        public Level level;
        public double mana;
        public double score;
        public int ticks = 0;
        public int ticks_per_sec = 33;

        public enum State {play, lost, win};
        public State state;

        public World(Level level2)
        {
            level = level2;
            monsters = new List<Monster>();
            gems = new List<Gem>();
            shots = new List<Shot>();
            towers = new List<Coord>();
            mana = 800;
            score = 0;
            state = State.play;
            enqueueMonsters();
        }

        //create monsters and set their spawn time
        public void enqueueMonsters()
        {
            for (int i = 0; i < level.waves.Count; i++)
            {
                int delay = getStartTickOfWave(i);
                level.waves[i].startTime = delay;
                for(int j = 0; j<level.waves[i].number; j++){
                    delay += level.waves[i].delay;
                    Monster m = (Monster)level.waves[i].monster.Clone();
                    m.spawn_time = delay;
                    monsters.Add(m);
                    level.waves[i].monsters.Add(m);
                }
            }
        }

        public static int getStartTickOfWave(int wave_number)
        {
            return 3 * 30 + 30 * 30 * (wave_number);
        }

        public int getCurrWaveNumber(){
            int num = 0;
            while (num < level.waves.Count)
            {
                if (level.waves[num].startTime > ticks) return num - 1;
                num++;
            }
            return level.waves.Count - 1;
        }
        //send next wave = reduce all enqueued monsters spawn time
        public void nextWave()
        {
            if (getCurrWaveNumber() + 1 >= level.waves.Count) return;
            int nextwaveNum = getCurrWaveNumber() + 1;
            int shift = level.waves[nextwaveNum].startTime - ticks;
            for (int i = nextwaveNum; i < level.waves.Count; i++)
            {
                level.waves[i].startTime -= shift;
                foreach (Monster m in level.waves[i].monsters)
                {
                    m.spawn_time -= shift;
                }
            }
            score += shift;
        }

        public bool buildTower(Coord c)
        {
            if (mana < getTowerCost()) return false;
            if (!isFree(c)) return false;
            mana -= getTowerCost();
            towers.Add(c);
            return true;
        }

        public bool isPath(Coord c)
        {
            return level.paths.Contains(c);
        }

        public bool isOrb(Coord c)
        {
            return c == level.orb;
        }

        public bool isTower(Coord c)
        {
            return towers.Contains(c);
        }

        public bool isFree(Coord c)
        {
            return !isPath(c) && !isTower(c) && !isOrb(c);
        }

        public int getTowerCost()
        {
            return 100 * towers.Count + 30;
        }

        public int getGemCost(int quality)
        {
            return (1 << quality) * 75;
        }

        //one tick of the world
        public void tick()
        {
            foreach (Monster m in monsters)
            {
                m.tick();
            }

            foreach (Gem g in gems)
            {
                g.tick();
            }

            foreach (Shot s in shots.ToList())
            {
                s.tick();
            }

            ticks++;
            mana += (double)1 / 3;

            checkEndLevel();
        }

        //when monster get to the orb
        public void banish(double banish_cost)
        {
            mana -= banish_cost;
            if (mana < 0) lose();
        }

        private void win()
        {
            state = State.win;
        }

        private void lose()
        {
            state = State.lost;
        }

        public void checkEndLevel()
        {
            if (mana < 0)
            {
                lose();
            }

            foreach(Monster m in monsters)
            if (m.state != Monster.State.dead)
            {
                return;
            }
            win();
        }

        public Gem createNewGem(int quality, string aspect_string)
        {
            Coord coord = Game.world.getFirstEmptyCoordInInventory();
            if (coord == new Coord(-1, -1) || getGemCost(quality) > mana)
            {
                return null;
            }
            Gem g = new Gem(quality, aspect_string, coord);
            mana -= getGemCost(quality);
            gems.Add(g);
            return g;
        }

        //returns (-1, -1) if doesnt exist
        private Coord getFirstEmptyCoordInInventory()  
        {
            for (int y = 0; y < Settings.inventoryNumOfRows; y++)
            {
                for (int x = 0; x < Settings.inventoryNumOfCols; x++)
                {
                    Coord c = new Coord(x,y);
                    bool filled = false;
                    foreach (Gem g in gems)
                    {
                        if (g.in_invetory == false) continue;
                        if (g.coord == c)
                        {
                            filled = true;
                            break;
                        }
                    }
                    if (!filled)
                    {
                        return c;
                    }
                }
            }
            return new Coord(-1, -1);
        }

        //adds their aspects and increase quality
        public void combineGems(Gem g, Gem g2)
        {
            gems.Remove(g2);
            
            if (g.quality == g2.quality)
            {
                g.quality++;
            }
            foreach(Aspect a2 in g2.aspects){
                bool added = false;
                foreach (Aspect a in g.aspects)
                {
                    if (a.aspectType == a2.aspectType)
                    {
                        a.amount += a2.amount;
                        added = true;
                        break;
                    }
                }
                if (!added)
                {
                    g.aspects.Add(a2);
                }
            }

            g.reset();
        }

        //swap gems places
        public void swapGems(Gem g, Gem g2)
        {
            if (g == g2) return;
            Coord c = g.coord;
            g.coord = g2.coord;
            g2.coord = c;
            bool b = g.in_invetory;
            g.in_invetory = g2.in_invetory;
            g2.in_invetory = b;
            
            g.reset();
            g2.reset();
        }

        //move gem to a place
        public void moveGem(Gem g, Coord c, bool in_inventory)
        {
            if (g.coord == c && g.in_invetory == in_inventory) return;
            g.coord = c;
            g.in_invetory = in_inventory;

            g.reset();
        }

        public Monster getClosestMonster(Position p, Monster excluding = null)
        {
            Monster closest_monster = null;
            foreach (Monster m in monsters)
            {
                if (m == excluding) continue;
                if (m.state != Monster.State.alive) continue;
                if(closest_monster == null || m.position.distanceTo(p) < closest_monster.position.distanceTo(p)) closest_monster = m;
            }
            return closest_monster;
        }
    }

    public struct Coord
    {
        public int x, y;

        public Coord(string s)
        {
            string[] tokens = s.Split();
            if (tokens.Length != 2) throw new Exception("Cannot read coords. Wrong number of arguments.");
            try
            {
                x = int.Parse(tokens[0]);
                y = int.Parse(tokens[1]);
            }
            catch (Exception)
            {
                throw new Exception("Cannot read coords. Error parsing int.");
            }
        }

        public Coord(int x2, int y2)
        {
            x = x2;
            y = y2;
        }

        public static bool operator ==(Coord c, Coord c2)
        {
            return c.x == c2.x && c.y == c2.y;
        }

        public static bool operator !=(Coord c, Coord c2)
        {
            return c.x != c2.x || c.y != c2.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            return obj is Coord && this == (Coord)obj;
        }
    }

    public class Position : ICloneable
    {
        public double x, y;

        public Position()
        {
            x = 0;
            y = 0;
        }

        public Position(double x2, double y2)
        {
            this.x = x2;
            this.y = y2;
        }

        public Position(Coord c){
            this.x = (double)c.x + 0.5;
            this.y = (double)c.y + 0.5;
        }

        public double distanceTo(Position p)
        {
            if (p == null) return double.PositiveInfinity;
            return Math.Sqrt((p.x - x) * (p.x - x) + (p.y - y) * (p.y - y));
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void set(double x2, double y2)
        {
            this.x = x2;
            this.y = y2;
        }

        
    }

    public class Monster : ICloneable
    {
        public Position position;
        public int direction;
        public int max_health;
        public double health;
        public double armor;
        public double speed;  //distance per tick
        public int image_type;

        public double slow_power = 0;
        public int slow_duration = 0;
        public double poison_power = 0;
        public int poison_duration = 0;

        public State state;
        public int spawn_time; //in ticks from begin
        private int curr_waypoint_index;

        private int banish_count = 0;  //how many times it reached orb

        public enum State {queued, alive, dead}

        public Monster(string s)
        {
            string[] tokens = s.Split();
            if (tokens.Length != 4) throw new Exception("Cannot read monster. Wrong number of arguments.");
            try
            {
                image_type = int.Parse(tokens[0]);
                max_health = int.Parse(tokens[1]);
                armor = int.Parse(tokens[2]);
                speed = double.Parse(tokens[3]);
            }
            catch (Exception)
            {
                throw new Exception("Cannot read monster. Error parsing int.");
            }

            if (max_health <= 0) throw new Exception("Cannot read monster. Health cannot be less or equal to zero.");
            if (speed <= 0) throw new Exception("Cannot read monster. Speed cannot be less or equal to zero.");
            if (armor < 0) throw new Exception("Cannot read monster. Armor cannot be less than zero.");

            health = (double)max_health;
            state = State.queued;
            position = null;
            direction = 0;
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool isPoisoned()
        {
            return (poison_power != 0);
        }

        public bool isSlowed()
        {
            return (slow_power != 0);
        }

        public void dealDamage(double amount, bool penetrate_armor = false)
        {
            if (!penetrate_armor)
            {
                amount -= this.armor;
                if (amount < 1) amount = 1;
            }
            health -= amount;
            if (health < 0) health = 0;
            if (health == 0) this.die();
        }

        public void lowerArmor(double amount)
        {
            armor -= amount;
            if (armor < 0) armor = 0;
        }

        public void inflictPoision(double amount, int duration)
        {
            poison_duration = duration;
            poison_power = amount;
        }

        public void inflictSlow(double amount, int duration)
        {
            slow_duration = duration;
            slow_power = amount;
        }

        public void spawn()
        {
            state = State.alive;
            position = new Position(Game.world.level.spawn);
            curr_waypoint_index = 0;
        }

        public void die()
        {
            state = State.dead;
            Game.world.mana += getManaReward();
            Game.world.score += getScoreReward();
        }

        public void tick()
        {
            if (slow_duration > 0)
            {
                slow_duration--;
                if (slow_duration == 0)
                {
                    slow_power = 0;
                }
            }

            if (poison_duration > 0)
            {
                poison_duration--;
                if (poison_duration == 0)
                {
                    poison_power = 0;
                }
            }

            if (isPoisoned() && state == State.alive)
            {
                dealDamage(poison_power, true);
            }

            if (Game.world.ticks >= spawn_time && state == State.queued)
            {
                spawn();
            }
            else if (state == State.alive)
            {
                move();
            }
        }

        public void banish(){
            Game.world.banish(getBanishCost());
            banish_count++;
        }

        public double getBanishCost()
        {
            double base_cost = 2 * (max_health + 20);
            return (1<<banish_count) * base_cost;
        }

        public double getManaReward()
        {
            return max_health + 20;
        }

        public double getScoreReward()
        {
            return max_health + 20;
        }

        private double getCurrSpeed()
        {
            double mod_speed = speed * (1 - slow_power);
            return mod_speed; //+ mod_speed * (MyRandom.NextDouble() - 0.5);
        }

        public void move()
        {
            Position target = new Position(Game.world.level.waypoints[curr_waypoint_index]);
            double dx = target.x - position.x;
            double dy = target.y - position.y;
            Double distance = Math.Sqrt(dx * dx + dy * dy);
            double currSpeed = getCurrSpeed();
            if (distance <= currSpeed)
            {
                position = target;
                curr_waypoint_index++;
                if (curr_waypoint_index >= Game.world.level.waypoints.Count)
                {
                    banish();
                    //die();
                    spawn();
                }
            }
            else
            {
                //hack for only horizontal or vertical move
                if (dx == 0)
                {
                    if (Math.Sign(dy) > 0) direction = 2; else direction = 0;
                    position.y += Math.Sign(dy) * currSpeed;
                }
                else if (dy == 0)
                {
                    if (Math.Sign(dx) > 0) direction = 1; else direction = 3;
                    position.x += Math.Sign(dx) * currSpeed;
                }

            }
        }

        public string getText()
        {
            if (state != State.alive) return "";
            string s = "Monster\n";
            s += "Health: " + (int)health + "/" + max_health + "\n";
            s += "Armor: " + Math.Round(armor, 1) + "\n";
            s += "Speed: " + Math.Round(getCurrSpeed() * 30 * 100, 1) + "\n";
            s += "Mana on kill: " + Math.Round(getManaReward(), 0) + "\n";
            s += "Banish cost: " + Math.Round(getBanishCost(), 0) + "\n";
            if (isPoisoned()) s += "POISONED" + "\n";
            if (isSlowed()) s += "SLOWED" + "\n";
            return s;
        }
    }

    public class Gem
    {
        public List<Aspect> aspects;
        public int quality;
        public Coord coord;
        public bool in_invetory;
        private double loaded = 0; //adds getAttackSpeed() every tick, when >=1 then it can shoot

        public double getRange()
        {
            return 2.2 + (Math.Log((double)getAspectAmount() / 10, 2.0)) * 0.1 + (quality - 1) * 0.1;
        }

        public int getDamage()
        {
            return (int)(getAspectAmount() * 0.5 + 5);
        }

        //atacks per tick
        public double getAttackSpeed()  
        {
            return (0.45 + 0.10 * quality) / 30;
        }

        private int getAspectAmount()
        {
            int sum = 0;
            foreach (Aspect a in aspects)
            {
                sum += a.amount;
            }
            return sum;
        }

        public Gem(int quality2, string aspect_name, Coord coord2)
        {
            this.quality = quality2;
            this.aspects = new List<Aspect>();
            aspects.Add(new Aspect( (1<<(quality-1)) * 10, aspect_name));
            in_invetory = true;
            coord = coord2;
        }

        public void tick()
        {
            if (in_invetory) return;
            loaded += getAttackSpeed();
            if (loaded >= 1)
            {
                Monster target = getTarget();
                if (target != null)
                {
                    shoot(target);
                    loaded -= 1;
                }
                if (loaded > 1) { loaded = 1; }
            }
        }

        private Monster getTarget()
        {
            foreach (Monster m in Game.world.monsters)
            {
                if (m.state == Monster.State.alive)
                {
                    if (m.position.distanceTo(new Position(coord)) <= getRange())
                    {
                        return m;
                    }
                }
            }
            return null;
        }

        private void shoot(Monster m)
        {
            Shot s = new Shot(m, this);
            Game.world.shots.Add(s);
        }

        public void reset()
        {
            loaded = 0;
        }

        public int getMainAspectNum()
        {
            Type t = null;
            int maxAmount = 0;
            foreach (Aspect a in aspects)
            {
                if (a.amount > maxAmount)
                {
                    maxAmount = a.amount;
                    t = a.aspectType;
                }
            }
            string aspect_string = (String)t.GetProperty("name", typeof(string)).GetValue(null, null);
            return Array.IndexOf(AspectType.types, aspect_string);
        }

        public string getText()
        {
            string s = "";
            s += "Damage: " + getDamage() + "\n";
            s += "Attacks per second: " + Math.Round(getAttackSpeed() * 30, 2) +"\n";
            s += "DPS: " + Math.Round(getDamage()/(getAttackSpeed() * 30), 2) + "\n";
            s += "Range: " + Math.Round(getRange() * 100, 2) + "\n";
            s += "Aspects: \n";
            foreach (Aspect a in aspects)
            {
                s += "   " + a.getText();
            }
            return s;
        }
    }

    public class Shot
    {
        public Monster target;
        public Position position;
        public Position start_position;
        public Position curve_position;
        public int damage;
        public double critical_multiplayer = 1.0;
        public List<Aspect> aspects;
        public int max_ticks = 30; //number of tick to reach the target
        private int ticks = 0;

        public Shot()
        {
        }

        public Shot(Monster monster, Gem gem)
        {
            target = monster;
            start_position = new Position(gem.coord);

            double rotation = MyRandom.NextDouble() * Math.PI * 2;
            curve_position = new Position(start_position.x + Math.Sin(rotation) * 4, start_position.y + Math.Cos(rotation) * 4);

            aspects = new List<Aspect>();
            foreach (Aspect a in gem.aspects)
            {
                aspects.Add((Aspect)a.Clone());
            }

            damage = gem.getDamage();

            max_ticks = (int)((double)1 / gem.getAttackSpeed() - 2);

            position = (Position)start_position.Clone();
            updatePosition();
        }

        public void updatePosition()
        {
            double percent = (double)ticks / (double)max_ticks;
            double start_weight = 1.0 - percent;
            double target_weight = percent;
            double curve_weight = percent * start_weight;
            start_weight -= curve_weight;
            double x = start_weight * start_position.x + target_weight * target.position.x + curve_weight * curve_position.x;
            double y = start_weight * start_position.y + target_weight * target.position.y + curve_weight * curve_position.y;
            position.set(x, y);
        }

        public void tick()
        {
            ticks++;
            updatePosition();
            if (ticks >= max_ticks)
            {
                hitTarget();
                Game.world.shots.Remove(this);
            }
        }

        private void hitTarget()
        {
            foreach (Aspect a in aspects)
            {
                a.applay(this, Game.world, target);
            }
            target.dealDamage(damage * critical_multiplayer);
        }

        public int getMainAspectNum()
        {
            Type t = null;
            int maxAmount = 0;
            foreach (Aspect a in aspects)
            {
                if (a.amount > maxAmount)
                {
                    maxAmount = a.amount;
                    t = a.aspectType;
                }
            }
            string aspect_string = (String)t.GetProperty("name", typeof(string)).GetValue(null, null);
            return Array.IndexOf(AspectType.types, aspect_string);
        }
    }
    
    //aspect of one gem
    public class Aspect : ICloneable
    {
        public Type aspectType;
        public int amount;

        public Aspect(int amount2, string aspect_name)
        {
            this.amount = amount2;
            this.aspectType = Type.GetType("TD.AspectType_" + aspect_name + ", TD");
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }

        public void applay(Shot shot, World world, Monster monster)
        {
            aspectType.GetMethod("applay").Invoke(null, new object[] { amount, shot, world, monster });
        }

        public string getText()
        {
            return (String) aspectType.GetMethod("getText").Invoke(null, new object[] { amount });
        }
    }

    public static class AspectType
    {
       public static String[] types = {"chain", "armor", "critical", "poison", "slow"};
    }

    public static class AspectType_chain
    {
        public static string name
        {
            get { return "chain";}
        }
        public static string description = "There is a chance that shot will bounce to nearby (2 sq) target. Each bounce will reduce the probability of another bounce.";

        private static double radius_constant = 2.0;
        
        private static double getProbability(int amount)
        {
            return (Math.Log((double)amount / 10, 2.0) + 1) * 0.1;
        }

        public static string getText(int amount)
        {
            return "Chain: "+ Math.Round(getProbability(amount)*100, 1) + "% that shot will rebounce to nearby monster";
        }

        public static void applay(int amount, Shot shot, World world, Monster monster)
        {
            Position p = monster.position;
            if (p == null || monster.state != Monster.State.alive) return;

            double probability = getProbability(amount);
            if (MyRandom.NextDouble() > probability) return;

            Monster closest_monster = world.getClosestMonster(monster.position, monster);
            if (closest_monster != null && closest_monster.position.distanceTo(monster.position) < radius_constant)
            {
                //create new shot
                Shot s = new Shot();
                s.start_position = (Position)monster.position.Clone();
                s.position = (Position)s.start_position.Clone();
                s.curve_position = (Position)shot.start_position.Clone();
                s.target = closest_monster;
                s.aspects = new List<Aspect>();
                foreach (Aspect a in shot.aspects)
                {
                    s.aspects.Add((Aspect)a.Clone());
                    if (a.aspectType == Type.GetType("AspectType_chain"))
                    {
                        s.aspects[s.aspects.Count - 1].amount /= 10;
                    }
                }
                s.damage = shot.damage;
                s.max_ticks = 10;
                Game.world.shots.Add(s);
            }
            
        }
    }

    public static class AspectType_armor
    {
        public static string name
        {
            get { return "armor";}
        }
        public static string description = "Permanently reduce armor of a target.";

        private static double getPower(int amount)
        {
            return (double)amount / 10;
        }

        public static string getText(int amount)
        {
            return "Armor reduction: " + "reduce " + Math.Round(getPower(amount), 2) + " armor each hit";
        }

        public static void applay(int amount, Shot shot, World world, Monster monster)
        {
            double power = getPower(amount);
            monster.armor -= power;
            if (monster.armor < 0) monster.armor = 0;
        }
    }

    public static class AspectType_critical
    {
        public static string name
        {
            get { return "critical"; }
        }
        public static string description = "Has chance to cause criticial hit.";

        private static double getPower(int amount)
        {
            return (Math.Log((double)amount / 10, 2.0) + 1) * 0.25 + 1;
        }

        private static double getProbability(int amount)
        {
            return (Math.Log((double)amount / 10, 2.0) + 1) * 0.03 + 0.08;
        }

        public static string getText(int amount)
        {
            return "Critical strike: " + Math.Round(getProbability(amount)*100, 1) + "% chance to cause "+ Math.Round((getPower(amount) - 1) * 100, 1) + "% more damage";
        }

        public static void applay(int amount, Shot shot, World world, Monster monster)
        {
            double power = getPower(amount);
            double probability = getProbability(amount);
            
            if (MyRandom.NextDouble() > probability) return;

            shot.critical_multiplayer = power;
        }
    }

    public static class AspectType_poison
    {
        public static string name
        {
            get { return "poison"; }
        }
        public static string description = "Infect monster by poison that causes damage over time.";

        private static double getPower(int amount)
        {
            return (double)amount / 10 * 7 / getDuration(amount);
        }

        private static int getDuration(int amount)
        {
            return 30 * 9;
        }

        public static string getText(int amount)
        {
            return "Poison: " + "cause " + Math.Round((getPower(amount)*getDuration(amount)), 1) + " damage over " + Math.Round((double)getDuration(amount) / 30, 1) + " seconds";
        }

        public static void applay(int amount, Shot shot, World world, Monster monster)
        {
            double power = getPower(amount);
            int duration = getDuration(amount);

            monster.inflictPoision(power, duration);
        }
    }

    public static class AspectType_slow
    {
        public static string name
        {
            get { return "slow"; }
        }
        public static string description = "Damaged monster is slower for shot period of time.";

        private static double getPower(int amount)
        {
            return (Math.Log((double)amount / 10, 2.0) + 1) * 0.04 + 0.12;
        }

        private static int getDuration(int amount)
        {
            return 30 * 2;
        }

        public static string getText(int amount)
        {
            return "Slow: " + "slows monster by " + Math.Round(((double)getPower(amount)) * 100, 1) + "% over " + Math.Round((double)getDuration(amount) / 30, 1) + " seconds";
        }

        public static void applay(int amount, Shot shot, World world, Monster monster)
        {
            double power = getPower(amount);
            int duration = getDuration(amount);

            monster.inflictSlow(power, duration);
        }
    }

}