namespace TD
{
    partial class Form_TD
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_exit = new System.Windows.Forms.Button();
            this.button_mainMenu = new System.Windows.Forms.Button();
            this.button_speed0 = new System.Windows.Forms.Button();
            this.button_speed1 = new System.Windows.Forms.Button();
            this.button_speed2 = new System.Windows.Forms.Button();
            this.button_speed3 = new System.Windows.Forms.Button();
            this.label_newGem = new System.Windows.Forms.Label();
            this.label_speed = new System.Windows.Forms.Label();
            this.label_inventory = new System.Windows.Forms.Label();
            this.button_slot2 = new System.Windows.Forms.Button();
            this.button_slot1 = new System.Windows.Forms.Button();
            this.button_slot3 = new System.Windows.Forms.Button();
            this.button_deleteSlot1 = new System.Windows.Forms.Button();
            this.button_deleteSlot2 = new System.Windows.Forms.Button();
            this.button_deleteSlot3 = new System.Windows.Forms.Button();
            this.button_tower = new System.Windows.Forms.Button();
            this.timer_tick = new System.Windows.Forms.Timer(this.components);
            this.label_mana = new System.Windows.Forms.Label();
            this.label_wave = new System.Windows.Forms.Label();
            this.label_selected = new System.Windows.Forms.Label();
            this.button_nextWave = new System.Windows.Forms.Button();
            this.label_score = new System.Windows.Forms.Label();
            this.button_newGem = new System.Windows.Forms.Button();
            this.button_inventory = new System.Windows.Forms.Button();
            this.button_board = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_exit
            // 
            this.button_exit.Location = new System.Drawing.Point(681, 517);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(129, 23);
            this.button_exit.TabIndex = 1;
            this.button_exit.Text = "Exit";
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // button_mainMenu
            // 
            this.button_mainMenu.Location = new System.Drawing.Point(546, 517);
            this.button_mainMenu.Name = "button_mainMenu";
            this.button_mainMenu.Size = new System.Drawing.Size(129, 23);
            this.button_mainMenu.TabIndex = 2;
            this.button_mainMenu.Text = "Main Menu";
            this.button_mainMenu.UseVisualStyleBackColor = true;
            this.button_mainMenu.Click += new System.EventHandler(this.button_mainMenu_Click);
            // 
            // button_speed0
            // 
            this.button_speed0.Location = new System.Drawing.Point(648, 12);
            this.button_speed0.Name = "button_speed0";
            this.button_speed0.Size = new System.Drawing.Size(36, 23);
            this.button_speed0.TabIndex = 3;
            this.button_speed0.Tag = "0";
            this.button_speed0.Text = "||";
            this.button_speed0.UseVisualStyleBackColor = true;
            this.button_speed0.Click += new System.EventHandler(this.button_speed_Click);
            // 
            // button_speed1
            // 
            this.button_speed1.Location = new System.Drawing.Point(690, 12);
            this.button_speed1.Name = "button_speed1";
            this.button_speed1.Size = new System.Drawing.Size(36, 23);
            this.button_speed1.TabIndex = 4;
            this.button_speed1.Tag = "30";
            this.button_speed1.Text = ">";
            this.button_speed1.UseVisualStyleBackColor = true;
            this.button_speed1.Click += new System.EventHandler(this.button_speed_Click);
            // 
            // button_speed2
            // 
            this.button_speed2.Location = new System.Drawing.Point(732, 12);
            this.button_speed2.Name = "button_speed2";
            this.button_speed2.Size = new System.Drawing.Size(36, 23);
            this.button_speed2.TabIndex = 5;
            this.button_speed2.Tag = "90";
            this.button_speed2.Text = ">>";
            this.button_speed2.UseVisualStyleBackColor = true;
            this.button_speed2.Click += new System.EventHandler(this.button_speed_Click);
            // 
            // button_speed3
            // 
            this.button_speed3.Location = new System.Drawing.Point(774, 12);
            this.button_speed3.Name = "button_speed3";
            this.button_speed3.Size = new System.Drawing.Size(36, 23);
            this.button_speed3.TabIndex = 6;
            this.button_speed3.Tag = "270";
            this.button_speed3.Text = ">>>";
            this.button_speed3.UseVisualStyleBackColor = true;
            this.button_speed3.Click += new System.EventHandler(this.button_speed_Click);
            // 
            // label_newGem
            // 
            this.label_newGem.AutoSize = true;
            this.label_newGem.Location = new System.Drawing.Point(546, 104);
            this.label_newGem.Name = "label_newGem";
            this.label_newGem.Size = new System.Drawing.Size(87, 13);
            this.label_newGem.TabIndex = 8;
            this.label_newGem.Text = "Create new gem:";
            // 
            // label_speed
            // 
            this.label_speed.AutoSize = true;
            this.label_speed.Location = new System.Drawing.Point(593, 17);
            this.label_speed.Name = "label_speed";
            this.label_speed.Size = new System.Drawing.Size(0, 13);
            this.label_speed.TabIndex = 9;
            this.label_speed.Visible = false;
            // 
            // label_inventory
            // 
            this.label_inventory.AutoSize = true;
            this.label_inventory.Location = new System.Drawing.Point(546, 164);
            this.label_inventory.Name = "label_inventory";
            this.label_inventory.Size = new System.Drawing.Size(54, 13);
            this.label_inventory.TabIndex = 11;
            this.label_inventory.Text = "Inventory:";
            // 
            // button_slot2
            // 
            this.button_slot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_slot2.Location = new System.Drawing.Point(200, 206);
            this.button_slot2.Name = "button_slot2";
            this.button_slot2.Size = new System.Drawing.Size(438, 80);
            this.button_slot2.TabIndex = 12;
            this.button_slot2.Tag = "2";
            this.button_slot2.Text = "New Game";
            this.button_slot2.UseVisualStyleBackColor = true;
            this.button_slot2.Click += new System.EventHandler(this.button_slot_Click);
            // 
            // button_slot1
            // 
            this.button_slot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_slot1.Location = new System.Drawing.Point(200, 120);
            this.button_slot1.Name = "button_slot1";
            this.button_slot1.Size = new System.Drawing.Size(438, 80);
            this.button_slot1.TabIndex = 13;
            this.button_slot1.Tag = "1";
            this.button_slot1.Text = "New Game";
            this.button_slot1.UseVisualStyleBackColor = true;
            this.button_slot1.Click += new System.EventHandler(this.button_slot_Click);
            // 
            // button_slot3
            // 
            this.button_slot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_slot3.Location = new System.Drawing.Point(200, 292);
            this.button_slot3.Name = "button_slot3";
            this.button_slot3.Size = new System.Drawing.Size(438, 80);
            this.button_slot3.TabIndex = 14;
            this.button_slot3.Tag = "3";
            this.button_slot3.Text = "New Game";
            this.button_slot3.UseVisualStyleBackColor = true;
            this.button_slot3.Click += new System.EventHandler(this.button_slot_Click);
            // 
            // button_deleteSlot1
            // 
            this.button_deleteSlot1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteSlot1.Location = new System.Drawing.Point(648, 138);
            this.button_deleteSlot1.Name = "button_deleteSlot1";
            this.button_deleteSlot1.Size = new System.Drawing.Size(45, 45);
            this.button_deleteSlot1.TabIndex = 15;
            this.button_deleteSlot1.Tag = "1";
            this.button_deleteSlot1.Text = "X";
            this.button_deleteSlot1.UseVisualStyleBackColor = true;
            this.button_deleteSlot1.Click += new System.EventHandler(this.button_deleteSlot_Click);
            // 
            // button_deleteSlot2
            // 
            this.button_deleteSlot2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteSlot2.Location = new System.Drawing.Point(648, 218);
            this.button_deleteSlot2.Name = "button_deleteSlot2";
            this.button_deleteSlot2.Size = new System.Drawing.Size(45, 45);
            this.button_deleteSlot2.TabIndex = 16;
            this.button_deleteSlot2.Tag = "2";
            this.button_deleteSlot2.Text = "X";
            this.button_deleteSlot2.UseVisualStyleBackColor = true;
            this.button_deleteSlot2.Click += new System.EventHandler(this.button_deleteSlot_Click);
            // 
            // button_deleteSlot3
            // 
            this.button_deleteSlot3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteSlot3.Location = new System.Drawing.Point(648, 304);
            this.button_deleteSlot3.Name = "button_deleteSlot3";
            this.button_deleteSlot3.Size = new System.Drawing.Size(45, 45);
            this.button_deleteSlot3.TabIndex = 17;
            this.button_deleteSlot3.Tag = "3";
            this.button_deleteSlot3.Text = "X";
            this.button_deleteSlot3.UseVisualStyleBackColor = true;
            this.button_deleteSlot3.Click += new System.EventHandler(this.button_deleteSlot_Click);
            // 
            // button_tower
            // 
            this.button_tower.Location = new System.Drawing.Point(546, 70);
            this.button_tower.Name = "button_tower";
            this.button_tower.Size = new System.Drawing.Size(264, 23);
            this.button_tower.TabIndex = 18;
            this.button_tower.Tag = "";
            this.button_tower.Text = "Build new tower";
            this.button_tower.UseVisualStyleBackColor = true;
            this.button_tower.Click += new System.EventHandler(this.button_tower_Click);
            // 
            // timer_tick
            // 
            this.timer_tick.Interval = 30;
            this.timer_tick.Tick += new System.EventHandler(this.timer_tick_Tick);
            // 
            // label_mana
            // 
            this.label_mana.AutoSize = true;
            this.label_mana.Location = new System.Drawing.Point(546, 14);
            this.label_mana.Name = "label_mana";
            this.label_mana.Size = new System.Drawing.Size(46, 13);
            this.label_mana.TabIndex = 19;
            this.label_mana.Text = "Mana: 0";
            // 
            // label_wave
            // 
            this.label_wave.AutoSize = true;
            this.label_wave.Location = new System.Drawing.Point(546, 50);
            this.label_wave.Name = "label_wave";
            this.label_wave.Size = new System.Drawing.Size(59, 13);
            this.label_wave.TabIndex = 20;
            this.label_wave.Text = "Wave: 0/0";
            // 
            // label_selected
            // 
            this.label_selected.AutoSize = true;
            this.label_selected.Location = new System.Drawing.Point(546, 315);
            this.label_selected.Name = "label_selected";
            this.label_selected.Size = new System.Drawing.Size(52, 13);
            this.label_selected.TabIndex = 21;
            this.label_selected.Text = "Selected:";
            // 
            // button_nextWave
            // 
            this.button_nextWave.Location = new System.Drawing.Point(729, 41);
            this.button_nextWave.Name = "button_nextWave";
            this.button_nextWave.Size = new System.Drawing.Size(81, 23);
            this.button_nextWave.TabIndex = 22;
            this.button_nextWave.Text = "Next Wave";
            this.button_nextWave.UseVisualStyleBackColor = true;
            this.button_nextWave.Click += new System.EventHandler(this.button_nextWave_Click);
            // 
            // label_score
            // 
            this.label_score.AutoSize = true;
            this.label_score.Location = new System.Drawing.Point(546, 32);
            this.label_score.Name = "label_score";
            this.label_score.Size = new System.Drawing.Size(47, 13);
            this.label_score.TabIndex = 23;
            this.label_score.Text = "Score: 0";
            // 
            // button_newGem
            // 
            this.button_newGem.BackColor = System.Drawing.SystemColors.Control;
            this.button_newGem.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_newGem.FlatAppearance.BorderSize = 0;
            this.button_newGem.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_newGem.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_newGem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_newGem.Location = new System.Drawing.Point(546, 128);
            this.button_newGem.Name = "button_newGem";
            this.button_newGem.Padding = new System.Windows.Forms.Padding(0, 0, 1, 1);
            this.button_newGem.Size = new System.Drawing.Size(264, 33);
            this.button_newGem.TabIndex = 24;
            this.button_newGem.Tag = "0";
            this.button_newGem.UseVisualStyleBackColor = false;
            this.button_newGem.Click += new System.EventHandler(this.button_newGem_Click);
            // 
            // button_inventory
            // 
            this.button_inventory.BackColor = System.Drawing.SystemColors.Control;
            this.button_inventory.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_inventory.FlatAppearance.BorderSize = 0;
            this.button_inventory.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_inventory.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_inventory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_inventory.Location = new System.Drawing.Point(546, 180);
            this.button_inventory.Name = "button_inventory";
            this.button_inventory.Padding = new System.Windows.Forms.Padding(0, 0, 1, 1);
            this.button_inventory.Size = new System.Drawing.Size(264, 132);
            this.button_inventory.TabIndex = 25;
            this.button_inventory.UseVisualStyleBackColor = false;
            this.button_inventory.Click += new System.EventHandler(this.button_inventory_Click);
            // 
            // button_board
            // 
            this.button_board.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.button_board.FlatAppearance.BorderSize = 0;
            this.button_board.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.button_board.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.button_board.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_board.Location = new System.Drawing.Point(12, 12);
            this.button_board.Name = "button_board";
            this.button_board.Padding = new System.Windows.Forms.Padding(0, 0, 1, 1);
            this.button_board.Size = new System.Drawing.Size(528, 528);
            this.button_board.TabIndex = 26;
            this.button_board.UseVisualStyleBackColor = true;
            this.button_board.Click += new System.EventHandler(this.pictureBox_board_Click);
            // 
            // Form_TD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 552);
            this.Controls.Add(this.button_board);
            this.Controls.Add(this.button_inventory);
            this.Controls.Add(this.button_newGem);
            this.Controls.Add(this.label_score);
            this.Controls.Add(this.button_nextWave);
            this.Controls.Add(this.label_selected);
            this.Controls.Add(this.label_wave);
            this.Controls.Add(this.label_mana);
            this.Controls.Add(this.button_tower);
            this.Controls.Add(this.label_inventory);
            this.Controls.Add(this.label_speed);
            this.Controls.Add(this.label_newGem);
            this.Controls.Add(this.button_speed3);
            this.Controls.Add(this.button_speed2);
            this.Controls.Add(this.button_speed1);
            this.Controls.Add(this.button_speed0);
            this.Controls.Add(this.button_mainMenu);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.button_slot1);
            this.Controls.Add(this.button_slot2);
            this.Controls.Add(this.button_slot3);
            this.Controls.Add(this.button_deleteSlot3);
            this.Controls.Add(this.button_deleteSlot2);
            this.Controls.Add(this.button_deleteSlot1);
            this.Name = "Form_TD";
            this.Text = "TD";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Button button_mainMenu;
        private System.Windows.Forms.Button button_speed0;
        private System.Windows.Forms.Button button_speed1;
        private System.Windows.Forms.Button button_speed2;
        private System.Windows.Forms.Button button_speed3;
        private System.Windows.Forms.Label label_newGem;
        private System.Windows.Forms.Label label_speed;
        private System.Windows.Forms.Label label_inventory;
        private System.Windows.Forms.Button button_slot2;
        private System.Windows.Forms.Button button_slot1;
        private System.Windows.Forms.Button button_slot3;
        private System.Windows.Forms.Button button_deleteSlot1;
        private System.Windows.Forms.Button button_deleteSlot2;
        private System.Windows.Forms.Button button_deleteSlot3;
        private System.Windows.Forms.Button button_tower;
        private System.Windows.Forms.Timer timer_tick;
        private System.Windows.Forms.Label label_mana;
        private System.Windows.Forms.Label label_wave;
        private System.Windows.Forms.Label label_selected;
        private System.Windows.Forms.Button button_nextWave;
        private System.Windows.Forms.Label label_score;
        private System.Windows.Forms.Button button_newGem;
        private System.Windows.Forms.Button button_inventory;
        private System.Windows.Forms.Button button_board;
    }
}

