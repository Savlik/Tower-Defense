using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
/*
 * Handles graphisc of the game
 * in class Images are saved all needed resources, they should be in res/ directory 
 */
namespace TD
{
    public static class Images
    {
        public static Image image_background = null;
        public static Image image_path = null;
        public static Image image_orb = null;
        public static Image image_tower = null;
        public static Image[,] image_monster = null;
        private static Image[] image_shot = null;

        public static Image image_red = null;
        public static Image image_green = null;

        private static Image[,] image_gem = null;
        public static Image[] image_gem_gray = null;
        public static Image image_cancel = null;
        public static Image image_slot_in = null;
        public static Image image_slot_out = null;

        public static Image getImageGem(int quality, int aspect_num)
        {
            if (quality > 8) quality = 8;
            return image_gem[quality, aspect_num];
        }

        public static Image getImageShot(int aspect_num)
        {
            return image_shot[aspect_num];
        }

        public static void init()
        {
            image_background = Image.FromFile("res/background.png");
            image_path = Image.FromFile("res/path.png");
            image_orb = Image.FromFile("res/orb.png");
            image_tower = Image.FromFile("res/tower.png");
            
            image_red = Image.FromFile("res/red.png");
            image_green = Image.FromFile("res/green.png");

            image_cancel = Image.FromFile("res/cancel.png");

            int number_of_monsters = getNumberOfMonsterImages();
            image_monster = new Image[number_of_monsters, 4];
            for (int i = 0; i < number_of_monsters; i++)
            {
                image_monster[i, 0] = Image.FromFile("res/monster_" + i + ".png");
                for (int d = 1; d < 4; d++)
                {
                    image_monster[i, d] = (Image)image_monster[i, 0].Clone();
                    for (int r = 0; r < d; r++)
                        image_monster[i, d].RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }

            string[] aspects = AspectType.types;
            image_gem = new Image[9, aspects.Length];
            image_gem_gray = new Image[9];
            for (int quality = 1; quality <= 8; quality++)
            {
                for (int aspect_num = 0; aspect_num < aspects.Length; aspect_num++)
                {
                    string aspect_string = aspects[aspect_num];
                    image_gem[quality, aspect_num] = Image.FromFile("res/gem_" + aspect_string + "_" + quality + ".png");
                }
                image_gem_gray[quality] = Image.FromFile("res/gem_" + "undefined" + "_" + quality + ".png");
            }

            image_shot = new Image[aspects.Length];
            for (int aspect_num = 0; aspect_num < aspects.Length; aspect_num++)
            {
                string aspect_string = aspects[aspect_num];
                image_shot[aspect_num] = Image.FromFile("res/shot_" + aspect_string + ".png");
            }

            image_cancel = Image.FromFile("res/cancel.png");
            image_slot_in = Image.FromFile("res/slot_in.png");
            image_slot_out = Image.FromFile("res/slot_out.png");
       }

        public static int getNumberOfMonsterImages()
        {
            int num = 0;
            while (File.Exists("res/" + "monster_" + num + ".png")) { num++; }
            return num;
        }
    }

    public static class Board
    {
        public static void render_all(Button button, Form_TD.CursorState cursorState, Gem selected_gem)
        {
            Bitmap bmp = new Bitmap(button.Height, button.Width);
            Graphics g = Graphics.FromImage(bmp);

            render_background(g);
            render_paths(g);
            render_towers(g);
            render_gems(g, selected_gem);
            render_monsters(g);
            render_orb(g);
            render_shots(g);

            if (cursorState == Form_TD.CursorState.tower)
            {
                render_tower_overlay(g);
            }

            g.DrawString("ticks: " + Game.world.ticks, new Font(FontFamily.GenericSansSerif, 15, FontStyle.Regular), new SolidBrush(Color.Black), 10, 500);

            button.Image = bmp;
            button.Refresh();
        }

        private static void render_background(Graphics g)
        {
            g.DrawImage(Images.image_background, 0, 0);
        }

        private static void render_paths(Graphics g)
        {
            foreach (Coord c in Game.world.level.paths)
            {
                g.DrawImage(Images.image_path, c.x * Settings.cellSize, c.y * Settings.cellSize);
            }
        }

        private static void render_towers(Graphics g)
        {
            foreach (Coord c in Game.world.towers)
            {
                g.DrawImage(Images.image_tower, c.x * Settings.cellSize, c.y * Settings.cellSize);
            }
        }
        
        // render all gems on the board
        private static void render_gems(Graphics g, Gem selected_gem)
        {
            foreach (Gem gem in Game.world.gems)
            {
                if (gem.in_invetory == true) continue;

                Image img = Images.getImageGem(gem.quality, gem.getMainAspectNum());
                if (gem == selected_gem)
                {
                    img = Utils.ChangeOpacity(img, (float)0.5);
                    Position p = new Position(gem.coord);
                    int centerx = (int)(p.x * Settings.cellSize);
                    int centery = (int)(p.y * Settings.cellSize);
                    int radius = (int)(selected_gem.getRange() * Settings.cellSize);
                    g.DrawEllipse(new Pen(Color.Black, 1), centerx - radius, centery - radius, radius * 2, radius * 2);
                }
                
                g.DrawImage(img, gem.coord.x * Settings.cellSize, gem.coord.y * Settings.cellSize);
            }
        }

        // render all alive monsters on the board
        private static void render_monsters(Graphics g)
        {
            foreach (Monster m in Game.world.monsters)
            {
                if (m.position != null && m.state == Monster.State.alive)
                {
                    Image img = Images.image_monster[m.image_type, m.direction];
                    double offestx = img.Height / 2;
                    double offesty = img.Width / 2;
                    g.DrawImage(img, (int)(m.position.x * Settings.cellSize - offestx), (int)(m.position.y * Settings.cellSize - offesty));
                }
            } 
        }

        private static void render_orb(Graphics g)
        {
            g.DrawImage(Images.image_orb, Game.world.level.orb.x * Settings.cellSize, Game.world.level.orb.y * Settings.cellSize);
        }
        
        //render all shots
        private static void render_shots(Graphics g)
        {
            foreach (Shot s in Game.world.shots)
            {
                if (s.position == null) continue;
                Image img = Images.getImageShot(s.getMainAspectNum());
                double offestx = img.Height / 2;
                double offesty = img.Width / 2;
                g.DrawImage(img, (int)(s.position.x * Settings.cellSize - offestx), (int)(s.position.y * Settings.cellSize - offesty));
            } 
        }

        //render red and green overlay layer when building a tower
        private static void render_tower_overlay(Graphics g)
        {
            for (int x = 0; x < Settings.boardSize; x++)
            {
                for (int y = 0; y < Settings.boardSize; y++)
                {
                    if (Game.world.isFree(new Coord(x, y)))
                    {
                        g.DrawImage(Images.image_green, x * Settings.cellSize, y * Settings.cellSize);
                    }
                    else
                    {
                        g.DrawImage(Images.image_red, x * Settings.cellSize, y * Settings.cellSize);
                    }
                }
            }
        }
    }

    public class NewGem
    {
        public static void render_all(Button button, Form_TD.CursorState cursorState, int quality)
        {

            Bitmap bmp = new Bitmap(button.Width, button.Height);
            Graphics g = Graphics.FromImage(bmp);

            if (cursorState == Form_TD.CursorState.newGem)
            {
                render_aspects(g, quality);
            }
            else
            {
                render_quality(g);
            }
            
            button.Image = bmp;
            button.Refresh();
        }

        //render grayscale gems
        private static void render_quality(Graphics g)
        {
            for (int i = 0; i < 8; i++)
            {
                g.DrawImage(Images.image_slot_out, i * Settings.cellSize, 0);
                g.DrawImage(Images.image_gem_gray[i + 1], i * Settings.cellSize, 0);
            }
        }

        //render all colors of one quality and cancel button
        private static void render_aspects(Graphics g, int quality)
        {
            for (int aspect_num = 0; aspect_num < AspectType.types.Length; aspect_num++)
            {
                g.DrawImage(Images.image_slot_out, aspect_num * Settings.cellSize, 0);
                g.DrawImage(Images.getImageGem(quality, aspect_num), aspect_num * Settings.cellSize, 0);
            }
            g.DrawImage(Images.image_cancel, 7 * Settings.cellSize, 0);
        }

    }

    public class Inventory
    {
        public static void render_all(Button button, Gem selected_gem)
        {
            Bitmap bmp = new Bitmap(button.Width, button.Height);
            Graphics g = Graphics.FromImage(bmp);

            render_slots(g);
            render_gems(g, selected_gem);
            
            button.Image = bmp;
            button.Refresh();
        }

        //render matrix of slots in inventory
        private static void render_slots(Graphics g)
        {
            for (int y = 0; y < Settings.inventoryNumOfRows; y++)
            {
                for (int x = 0; x < Settings.inventoryNumOfCols; x++)
                {
                    g.DrawImage(Images.image_slot_in, x * Settings.cellSize, y * Settings.cellSize);
                }
            }
        }

        //render all gems in inventory
        private static void render_gems(Graphics g, Gem selected_gem)
        {
            foreach (Gem gem in Game.world.gems)
            {
                if (gem.in_invetory == false) continue;

                Image img = Images.getImageGem(gem.quality, gem.getMainAspectNum());
                if (gem == selected_gem)
                {
                    img = Utils.ChangeOpacity(img, (float)0.5);
                }

                g.DrawImage(img, gem.coord.x * Settings.cellSize, gem.coord.y * Settings.cellSize);
            }
        }

    }

}