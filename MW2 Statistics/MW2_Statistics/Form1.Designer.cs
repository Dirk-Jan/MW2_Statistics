namespace MW2_Statistics
{
    partial class Form1
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnTimeToLong = new System.Windows.Forms.Button();
            this.btnRegMatch = new System.Windows.Forms.Button();
            this.btnEndMatch = new System.Windows.Forms.Button();
            this.btnGetPlayerId = new System.Windows.Forms.Button();
            this.btnAddHit = new System.Windows.Forms.Button();
            this.btnAddWeapon = new System.Windows.Forms.Button();
            this.btnWepExists = new System.Windows.Forms.Button();
            this.btnAddAlias = new System.Windows.Forms.Button();
            this.btnAliasExists = new System.Windows.Forms.Button();
            this.btnUpdatePlayerLastSeen = new System.Windows.Forms.Button();
            this.btnEmptyAllTables = new System.Windows.Forms.Button();
            this.btnReadLogFile = new System.Windows.Forms.Button();
            this.btnSteamIdConvert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(13, 42);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(822, 563);
            this.listBox1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(292, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(427, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(882, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Add Player";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnTimeToLong
            // 
            this.btnTimeToLong.Location = new System.Drawing.Point(882, 14);
            this.btnTimeToLong.Name = "btnTimeToLong";
            this.btnTimeToLong.Size = new System.Drawing.Size(75, 23);
            this.btnTimeToLong.TabIndex = 4;
            this.btnTimeToLong.Text = "Time to long";
            this.btnTimeToLong.UseVisualStyleBackColor = true;
            this.btnTimeToLong.Click += new System.EventHandler(this.btnTimeToLong_Click);
            // 
            // btnRegMatch
            // 
            this.btnRegMatch.Location = new System.Drawing.Point(882, 72);
            this.btnRegMatch.Name = "btnRegMatch";
            this.btnRegMatch.Size = new System.Drawing.Size(117, 23);
            this.btnRegMatch.TabIndex = 5;
            this.btnRegMatch.Text = "Register Match";
            this.btnRegMatch.UseVisualStyleBackColor = true;
            this.btnRegMatch.Click += new System.EventHandler(this.btnRegMatch_Click);
            // 
            // btnEndMatch
            // 
            this.btnEndMatch.Location = new System.Drawing.Point(882, 102);
            this.btnEndMatch.Name = "btnEndMatch";
            this.btnEndMatch.Size = new System.Drawing.Size(117, 23);
            this.btnEndMatch.TabIndex = 6;
            this.btnEndMatch.Text = "End Match";
            this.btnEndMatch.UseVisualStyleBackColor = true;
            this.btnEndMatch.Click += new System.EventHandler(this.btnEndMatch_Click);
            // 
            // btnGetPlayerId
            // 
            this.btnGetPlayerId.Location = new System.Drawing.Point(882, 132);
            this.btnGetPlayerId.Name = "btnGetPlayerId";
            this.btnGetPlayerId.Size = new System.Drawing.Size(117, 23);
            this.btnGetPlayerId.TabIndex = 7;
            this.btnGetPlayerId.Text = "Get Player ID";
            this.btnGetPlayerId.UseVisualStyleBackColor = true;
            this.btnGetPlayerId.Click += new System.EventHandler(this.btnGetPlayerId_Click);
            // 
            // btnAddHit
            // 
            this.btnAddHit.Location = new System.Drawing.Point(882, 162);
            this.btnAddHit.Name = "btnAddHit";
            this.btnAddHit.Size = new System.Drawing.Size(75, 23);
            this.btnAddHit.TabIndex = 8;
            this.btnAddHit.Text = "Add Hit";
            this.btnAddHit.UseVisualStyleBackColor = true;
            this.btnAddHit.Click += new System.EventHandler(this.btnAddHit_Click);
            // 
            // btnAddWeapon
            // 
            this.btnAddWeapon.Location = new System.Drawing.Point(882, 192);
            this.btnAddWeapon.Name = "btnAddWeapon";
            this.btnAddWeapon.Size = new System.Drawing.Size(117, 23);
            this.btnAddWeapon.TabIndex = 9;
            this.btnAddWeapon.Text = "Add Weapon";
            this.btnAddWeapon.UseVisualStyleBackColor = true;
            this.btnAddWeapon.Click += new System.EventHandler(this.btnAddWeapon_Click);
            // 
            // btnWepExists
            // 
            this.btnWepExists.Location = new System.Drawing.Point(882, 222);
            this.btnWepExists.Name = "btnWepExists";
            this.btnWepExists.Size = new System.Drawing.Size(117, 23);
            this.btnWepExists.TabIndex = 10;
            this.btnWepExists.Text = "Weapon Exists";
            this.btnWepExists.UseVisualStyleBackColor = true;
            this.btnWepExists.Click += new System.EventHandler(this.btnWepExists_Click);
            // 
            // btnAddAlias
            // 
            this.btnAddAlias.Location = new System.Drawing.Point(882, 252);
            this.btnAddAlias.Name = "btnAddAlias";
            this.btnAddAlias.Size = new System.Drawing.Size(75, 23);
            this.btnAddAlias.TabIndex = 11;
            this.btnAddAlias.Text = "Add Alias";
            this.btnAddAlias.UseVisualStyleBackColor = true;
            this.btnAddAlias.Click += new System.EventHandler(this.btnAddAlias_Click);
            // 
            // btnAliasExists
            // 
            this.btnAliasExists.Location = new System.Drawing.Point(882, 282);
            this.btnAliasExists.Name = "btnAliasExists";
            this.btnAliasExists.Size = new System.Drawing.Size(75, 23);
            this.btnAliasExists.TabIndex = 12;
            this.btnAliasExists.Text = "Alias Exists";
            this.btnAliasExists.UseVisualStyleBackColor = true;
            this.btnAliasExists.Click += new System.EventHandler(this.btnAliasExists_Click);
            // 
            // btnUpdatePlayerLastSeen
            // 
            this.btnUpdatePlayerLastSeen.Location = new System.Drawing.Point(841, 312);
            this.btnUpdatePlayerLastSeen.Name = "btnUpdatePlayerLastSeen";
            this.btnUpdatePlayerLastSeen.Size = new System.Drawing.Size(158, 23);
            this.btnUpdatePlayerLastSeen.TabIndex = 13;
            this.btnUpdatePlayerLastSeen.Text = "Update Player LastSeen";
            this.btnUpdatePlayerLastSeen.UseVisualStyleBackColor = true;
            this.btnUpdatePlayerLastSeen.Click += new System.EventHandler(this.btnUpdatePlayerLastSeen_Click);
            // 
            // btnEmptyAllTables
            // 
            this.btnEmptyAllTables.Location = new System.Drawing.Point(882, 341);
            this.btnEmptyAllTables.Name = "btnEmptyAllTables";
            this.btnEmptyAllTables.Size = new System.Drawing.Size(117, 23);
            this.btnEmptyAllTables.TabIndex = 14;
            this.btnEmptyAllTables.Text = "Empty All Tables";
            this.btnEmptyAllTables.UseVisualStyleBackColor = true;
            this.btnEmptyAllTables.Click += new System.EventHandler(this.btnEmptyAllTables_Click);
            // 
            // btnReadLogFile
            // 
            this.btnReadLogFile.Location = new System.Drawing.Point(12, 12);
            this.btnReadLogFile.Name = "btnReadLogFile";
            this.btnReadLogFile.Size = new System.Drawing.Size(247, 23);
            this.btnReadLogFile.TabIndex = 15;
            this.btnReadLogFile.Text = "Read Log File";
            this.btnReadLogFile.UseVisualStyleBackColor = true;
            this.btnReadLogFile.Click += new System.EventHandler(this.btnReadLogFile_Click);
            // 
            // btnSteamIdConvert
            // 
            this.btnSteamIdConvert.Location = new System.Drawing.Point(882, 413);
            this.btnSteamIdConvert.Name = "btnSteamIdConvert";
            this.btnSteamIdConvert.Size = new System.Drawing.Size(117, 23);
            this.btnSteamIdConvert.TabIndex = 16;
            this.btnSteamIdConvert.Text = "Convert SteamID";
            this.btnSteamIdConvert.UseVisualStyleBackColor = true;
            this.btnSteamIdConvert.Click += new System.EventHandler(this.btnSteamIdConvert_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 620);
            this.Controls.Add(this.btnSteamIdConvert);
            this.Controls.Add(this.btnReadLogFile);
            this.Controls.Add(this.btnEmptyAllTables);
            this.Controls.Add(this.btnUpdatePlayerLastSeen);
            this.Controls.Add(this.btnAliasExists);
            this.Controls.Add(this.btnAddAlias);
            this.Controls.Add(this.btnWepExists);
            this.Controls.Add(this.btnAddWeapon);
            this.Controls.Add(this.btnAddHit);
            this.Controls.Add(this.btnGetPlayerId);
            this.Controls.Add(this.btnEndMatch);
            this.Controls.Add(this.btnRegMatch);
            this.Controls.Add(this.btnTimeToLong);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "formpie";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnTimeToLong;
        private System.Windows.Forms.Button btnRegMatch;
        private System.Windows.Forms.Button btnEndMatch;
        private System.Windows.Forms.Button btnGetPlayerId;
        private System.Windows.Forms.Button btnAddHit;
        private System.Windows.Forms.Button btnAddWeapon;
        private System.Windows.Forms.Button btnWepExists;
        private System.Windows.Forms.Button btnAddAlias;
        private System.Windows.Forms.Button btnAliasExists;
        private System.Windows.Forms.Button btnUpdatePlayerLastSeen;
        private System.Windows.Forms.Button btnEmptyAllTables;
        private System.Windows.Forms.Button btnReadLogFile;
        private System.Windows.Forms.Button btnSteamIdConvert;
    }
}

