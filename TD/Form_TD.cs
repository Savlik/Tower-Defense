using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
/*
 * Iterface for the user, handles all input and output of the game using mainly classes Game and World
 */
namespace TD
{
    public partial class Form_TD : Form
    {
        private List<System.Windows.Forms.Control> elements_gameSelect;
        private List<System.Windows.Forms.Control> elements_mainMenu;
        private List<System.Windows.Forms.Control> elements_inGame;
        private List<System.Windows.Forms.Button> button_level;

        public enum CursorState { tower, gem, nothing, newGem };
        private CursorState cursorState = CursorState.nothing;

        public Form_TD()
        {
            InitializeComponent();

            elements_gameSelect = new List<Control>();
            elements_mainMenu = new List<Control>();
            elements_inGame = new List<Control>();
            button_level = new List<Button>();

            elements_gameSelect.Add(button_slot1);
            elements_gameSelect.Add(button_deleteSlot1);
            elements_gameSelect.Add(button_slot2);
            elements_gameSelect.Add(button_deleteSlot2);
            elements_gameSelect.Add(button_slot3);
            elements_gameSelect.Add(button_deleteSlot3);

            elements_mainMenu.Add(button_exit);

            elements_inGame.Add(button_exit);
            elements_inGame.Add(button_mainMenu);
            elements_inGame.Add(button_tower);
            elements_inGame.Add(label_mana);
            elements_inGame.Add(label_speed);
            elements_inGame.Add(label_selected);
            elements_inGame.Add(label_wave);
            elements_inGame.Add(label_score);
            elements_inGame.Add(button_speed0);
            elements_inGame.Add(button_speed1);
            elements_inGame.Add(button_speed2);
            elements_inGame.Add(button_speed3);
            elements_inGame.Add(button_nextWave);
            elements_inGame.Add(label_newGem);
            elements_inGame.Add(button_newGem);
            elements_inGame.Add(label_inventory);
            elements_inGame.Add(button_inventory);
            elements_inGame.Add(button_board);

            createLevelButtons();
            showCurrState();
        }

        private void hide_all()
        {
            foreach (var elem in elements_gameSelect) elem.Visible = false;
            foreach (var elem in elements_mainMenu) elem.Visible = false;
            foreach (var elem in elements_inGame) elem.Visible = false;
        }

        private void show_gameSelect()
        {
            hide_all();
            setSlotProperties();
            foreach (var elem in elements_gameSelect) elem.Visible = true;
        }

        private void show_mainMenu()
        {
            hide_all();
            setLevelProperties();
            foreach (var elem in elements_mainMenu) elem.Visible = true;
        }

        private void show_inGame()
        {
            hide_all();
            redrawAll();
            foreach (var elem in elements_inGame) elem.Visible = true;
        }

        private void showState(Game.GameState gs)
        {
            if (gs == Game.GameState.gameSelect)
            {
                show_gameSelect();
            }
            else if (gs == Game.GameState.mainMenu)
            {
                show_mainMenu();
            }
            else if (gs == Game.GameState.inWorld)
            {
                show_inGame();
            }
        }

        private void showCurrState()
        {
            showState(Game.state);
        }

        public void setSlotProperties()
        {
            if (Game.slotExists(1))
            {
                button_slot1.Text = "Continue";
                button_deleteSlot1.Enabled = true;
            }
            else
            {
                button_slot1.Text = "New Game";
                button_deleteSlot1.Enabled = false;
            }
            if (Game.slotExists(2))
            {
                button_slot2.Text = "Continue";
                button_deleteSlot2.Enabled = true;
            }
            else
            {
                button_slot2.Text = "New Game";
                button_deleteSlot2.Enabled = false;
            }
            if (Game.slotExists(3))
            {
                button_slot3.Text = "Continue";
                button_deleteSlot3.Enabled = true;
            }
            else
            {
                button_slot3.Text = "New Game";
                button_deleteSlot3.Enabled = false;
            }
        }

        private void createLevelButtons()
        {
            int num_levels = Settings.numberOfLevels;
            int horizontal = Settings.levelSelectNumOfCols;
            int vertical = Settings.levelSelectNumOfRows;
            int left = 12;
            int right = 540;
            int top = 12;
            int bottom = 540;

            int sizex = ((right - left) - (horizontal - 1) * 12) / horizontal;
            int sizey = ((bottom - top) - (vertical - 1) * 12) / vertical;
            for (int i = 0; i < num_levels; i++)
            {
                System.Windows.Forms.Button button = new Button();

                button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                button.Location = new System.Drawing.Point(left + (i % horizontal) * (sizex + 12), top + (i / horizontal) * (sizey + 12));
                button.Name = "button_level" + (i + 1);
                button.Size = new System.Drawing.Size(sizex, sizey);
                button.TabIndex = 13;
                button.Tag = i + 1;
                button.Text = "Level " + (i + 1);
                button.UseVisualStyleBackColor = true;
                button.Click += new System.EventHandler(this.button_level_Click);
                this.Controls.Add(button);

                button_level.Add(button);
                elements_mainMenu.Add(button);
            }
        }

        public void setLevelProperties()
        {
            foreach (var button in button_level)
            {
                if (int.Parse(button.Tag.ToString()) > Game.playerData.level_opened)
                {
                    button.Enabled = false;
                    button.Text = "Level " + button.Tag;
                }
                else
                {
                    button.Enabled = true;
                    button.Text = "Level " + button.Tag + "\n" + "Best Score:" + "\n" + Game.playerData.best_scores[int.Parse(button.Tag.ToString())-1];
                }
            }
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            if (!Utils.confirm()) return;
            if (Game.state != Game.GameState.gameSelect)
            {
                Game.saveGame();
            }
            Application.Exit();
        }

        private void button_deleteSlot_Click(object sender, EventArgs e)
        {
            if (Utils.confirm())
            {
                string tag = ((System.Windows.Forms.Button)sender).Tag.ToString();
                Game.deleteSlot(int.Parse(tag));
                setSlotProperties();
                Logger.log("Deleting game slot " + tag);
            }
        }

        private void button_slot_Click(object sender, EventArgs e)
        {
            string tag = ((System.Windows.Forms.Button)sender).Tag.ToString();
            Game.StartGame(int.Parse(tag));
            Game.state = Game.GameState.mainMenu;
            showCurrState();
            Logger.log("Starting game in slot " + tag);
        }

        private void button_level_Click(object sender, EventArgs e)
        {
            string tag = ((System.Windows.Forms.Button)sender).Tag.ToString();
            Game.playLevel(int.Parse(tag));
            timer_tick.Enabled = true;
            cursorState = CursorState.nothing;
            showCurrState();

            redrawAll();
        }

        public void setCursorProperties()
        {
            if (cursorState == CursorState.tower)
            {
                button_tower.Text = "Cancel building tower";
            }
            else
            {
                button_tower.Text = "Build new tower";
                button_tower.Text += " (" + Game.world.getTowerCost() + "g)";
            }

            if (cursorState == CursorState.newGem)
            {
                int gem_quality = int.Parse(button_newGem.Tag.ToString());
                label_newGem.Text = "Create new gem: " + "choose aspect " + "(" + Game.world.getGemCost(gem_quality) + " mana)";
            }
            else
            {
                label_newGem.Text = "Create new gem: " + "choose quality";
            }

            if (cursorState == CursorState.gem)
            {
                Utils.setCursor(this, selected_gem);
            }
            else
            {
                Utils.setCursor(this, null);
            }
        }

        private void renderBoard()
        {
            Board.render_all(button_board, cursorState, cursorState == CursorState.gem ? selected_gem : null);
        }

        private void renderNewGem()
        {
            NewGem.render_all(button_newGem, cursorState, int.Parse(button_newGem.Tag.ToString()));
        }

        private void renderInventory()
        {
            Inventory.render_all(button_inventory, cursorState == CursorState.gem ? selected_gem : null);
        }

        private void redrawAll()
        {
            setCursorProperties();
            renderBoard();
            renderInventory();
            renderNewGem();
            redrawTick();
        }

        private void redrawTick()
        {
            label_mana.Text = "Mana: " + (int)Game.world.mana;
            label_score.Text = "Score: " + (int)Game.world.score;
            label_wave.Text = "Wave: " + (Game.world.getCurrWaveNumber() + 1) + "/" + Game.world.level.waves.Count;
            if (selected_gem != null)
            {
                label_selected.Text = selected_gem.getText();
            }
            else if (selected_monster != null)
            {
                label_selected.Text = selected_monster.getText();
            }
            else
            {
                label_selected.Text = "";
            }
            
            if (Game.world.getCurrWaveNumber() == Game.world.level.waves.Count - 1)
            {
                button_nextWave.Enabled = false;
            }
            else
            {
                button_nextWave.Enabled = true;
            }
            renderBoard();
        }

        private void pictureBox_board_Click(object sender, EventArgs e)
        {
            MouseEventArgs mea = (MouseEventArgs)e;
            if (mea.Button != MouseButtons.Left) return;
            Coord c = new Coord(mea.X / Settings.cellSize, mea.Y / Settings.cellSize);

            Gem clicked_gem = null;
            foreach (Gem g in Game.world.gems)
            {
                if (!g.in_invetory && g.coord == c)
                {
                    clicked_gem = g;
                    break;
                }
            }

            if (cursorState == CursorState.tower)
            {
                Game.world.buildTower(c);
                cursorState = CursorState.nothing;
            }
            else if (cursorState == CursorState.gem)
            {
                if (clicked_gem != null)
                {
                    Game.world.swapGems(clicked_gem, selected_gem);
                }
                else
                {
                    if (Game.world.isTower(c))
                    {
                        Game.world.moveGem(selected_gem, c, false);
                    }
                }
                cursorState = CursorState.nothing;
  
            }
            else
            {
                if (clicked_gem != null)
                {
                    cursorState = CursorState.gem;
                    selected_gem = clicked_gem;
                    selected_monster = null;
                }
                else
                {
                    //select nearby monster
                    int x = mea.X;
                    int y = mea.Y;
                    Monster m = Game.world.getClosestMonster(new Position(c));
                    if (m != null && m.position.distanceTo(new Position(c)) < 1)
                    {
                        selected_monster = m;
                        selected_gem = null;
                    }

                }
            }

            redrawAll();            
        }

        private void button_tower_Click(object sender, EventArgs e)
        {
            if (cursorState == CursorState.tower)
            {
                cursorState = CursorState.nothing;
            }
            else
            {
                cursorState = CursorState.tower;
            }

            redrawAll();
        }

        private void button_mainMenu_Click(object sender, EventArgs e)
        {
            if (Utils.confirm())
            {
                cursorState = CursorState.nothing;
                redrawAll();
                Game.endLevel(false);
                timer_tick.Enabled = false;
                showCurrState();
            }
        }

        private long last_time = 0;
        private int remaining_time = 0;

        private void timer_tick_Tick(object sender, EventArgs e)
        {
            if (Game.world == null) return;
            if (Game.world.state != World.State.play)
            {
                cursorState = CursorState.nothing;
                redrawAll();
                Game.endLevel(Game.world.state == World.State.win);
                timer_tick.Enabled = false;
                last_time = 0;
                remaining_time = 0;
                showCurrState();
                return;
            }

            int tps = Game.world.ticks_per_sec;
            if (tps == 0)
            {
                remaining_time = 0;
                last_time = 0;
                return;
            }

            if (last_time == 0)
            {
                last_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                return;
            }

            long new_time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            int diff = (int)(new_time - last_time);
            remaining_time += diff;
            last_time = new_time;
            while (remaining_time >= 1000 / tps)
            {
                remaining_time -= 1000 / tps;
                Game.world.tick();
            }

            redrawTick();
        }

        private void button_speed_Click(object sender, EventArgs e)
        {
            string tag = ((System.Windows.Forms.Button)sender).Tag.ToString();
            Game.world.ticks_per_sec = int.Parse(tag);

            redrawAll();
        }

        private void button_nextWave_Click(object sender, EventArgs e)
        {
            Game.world.nextWave();

            redrawAll();
        }

        private void button_newGem_Click(object sender, EventArgs e)
        {
            MouseEventArgs mea = (MouseEventArgs)e;
            if (mea.Button != MouseButtons.Left) return;
            Coord c = new Coord(mea.X / Settings.cellSize, mea.Y / Settings.cellSize);

            if (cursorState == CursorState.newGem)
            {
                if (c.x == 7) //cancel button
                {
                    cursorState = CursorState.nothing;
                }
                if (c.x < AspectType.types.Length)
                {
                    Gem g = Game.world.createNewGem(int.Parse(button_newGem.Tag.ToString()), AspectType.types[c.x]);
                    if (g != null)
                    {
                        selected_gem = g;
                        selected_monster = null;
                    }
                    cursorState = CursorState.nothing;
                    
                }
            }
            else
            {
                cursorState = CursorState.newGem;
                button_newGem.Tag = c.x + 1;
            }

            redrawAll();
        }

        Gem selected_gem = null;
        Monster selected_monster = null;

        private void button_inventory_Click(object sender, EventArgs e)
        {
            MouseEventArgs mea = (MouseEventArgs)e;
            if (mea.Button != MouseButtons.Left) return;
            Coord c = new Coord(mea.X / Settings.cellSize, mea.Y / Settings.cellSize);

            Gem clicked_gem = null;
            foreach (Gem g in Game.world.gems)
            {
                if (g.in_invetory && g.coord == c)
                {
                    clicked_gem = g;
                    break;
                }
            }

            if (cursorState == CursorState.gem)
            {
                if (clicked_gem == null)
                {
                    Game.world.moveGem(selected_gem, c, true);
                }
                else
                {
                    Game.world.swapGems(clicked_gem, selected_gem);
                }

                cursorState = CursorState.nothing;
            }
            else if(clicked_gem != null)
            {
                selected_gem = clicked_gem;
                selected_monster = null;
                cursorState = CursorState.gem;
            }

            redrawAll();
        }
    }
}
