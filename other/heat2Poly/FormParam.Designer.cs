namespace fluidedotnet
{
  partial class FormParam
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
        this.FileList = new System.Windows.Forms.ListBox();
        this.button1 = new System.Windows.Forms.Button();
        this.XTrack = new System.Windows.Forms.TrackBar();
        this.ButDrop = new System.Windows.Forms.Button();
        this.butPipe = new System.Windows.Forms.Button();
        this.butObstacle = new System.Windows.Forms.Button();
        this.butForce = new System.Windows.Forms.Button();
        this.butDelate = new System.Windows.Forms.Button();
        this.butForceDown = new System.Windows.Forms.Button();
        this.butPush = new System.Windows.Forms.Button();
        this.butSave = new System.Windows.Forms.Button();
        this.buttLift = new System.Windows.Forms.Button();
        this.buttMove = new System.Windows.Forms.Button();
        this.butShowNeed = new System.Windows.Forms.Button();
        this.butShowSurf = new System.Windows.Forms.Button();
        this.btnPause = new System.Windows.Forms.Button();
        this.btnStep = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.XTrack)).BeginInit();
        this.SuspendLayout();
        // 
        // FileList
        // 
        this.FileList.FormattingEnabled = true;
        this.FileList.ItemHeight = 16;
        this.FileList.Location = new System.Drawing.Point(-2, 66);
        this.FileList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.FileList.Name = "FileList";
        this.FileList.Size = new System.Drawing.Size(108, 596);
        this.FileList.TabIndex = 0;
        this.FileList.SelectedIndexChanged += new System.EventHandler(this.FileList_SelectedIndexChanged);
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(115, 350);
        this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(78, 28);
        this.button1.TabIndex = 1;
        this.button1.Text = "None";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // XTrack
        // 
        this.XTrack.LargeChange = 1;
        this.XTrack.Location = new System.Drawing.Point(-2, 6);
        this.XTrack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.XTrack.Maximum = 1400;
        this.XTrack.Minimum = 1;
        this.XTrack.Name = "XTrack";
        this.XTrack.Size = new System.Drawing.Size(212, 53);
        this.XTrack.TabIndex = 2;
        this.XTrack.Value = 80;
        this.XTrack.Scroll += new System.EventHandler(this.XTrack_Scroll);
        // 
        // ButDrop
        // 
        this.ButDrop.Location = new System.Drawing.Point(115, 50);
        this.ButDrop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.ButDrop.Name = "ButDrop";
        this.ButDrop.Size = new System.Drawing.Size(78, 28);
        this.ButDrop.TabIndex = 3;
        this.ButDrop.Text = "Drop";
        this.ButDrop.UseVisualStyleBackColor = true;
        this.ButDrop.Click += new System.EventHandler(this.ButDrop_Click);
        // 
        // butPipe
        // 
        this.butPipe.Location = new System.Drawing.Point(115, 113);
        this.butPipe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butPipe.Name = "butPipe";
        this.butPipe.Size = new System.Drawing.Size(78, 28);
        this.butPipe.TabIndex = 4;
        this.butPipe.Text = "Pipe";
        this.butPipe.UseVisualStyleBackColor = true;
        this.butPipe.Click += new System.EventHandler(this.butPipe_Click);
        // 
        // butObstacle
        // 
        this.butObstacle.Location = new System.Drawing.Point(115, 145);
        this.butObstacle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butObstacle.Name = "butObstacle";
        this.butObstacle.Size = new System.Drawing.Size(78, 28);
        this.butObstacle.TabIndex = 5;
        this.butObstacle.Text = "Obstacle";
        this.butObstacle.UseVisualStyleBackColor = true;
        this.butObstacle.Click += new System.EventHandler(this.butObstacle_Click);
        // 
        // butForce
        // 
        this.butForce.Location = new System.Drawing.Point(115, 207);
        this.butForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butForce.Name = "butForce";
        this.butForce.Size = new System.Drawing.Size(78, 28);
        this.butForce.TabIndex = 7;
        this.butForce.Text = "Force Up";
        this.butForce.UseVisualStyleBackColor = true;
        this.butForce.Click += new System.EventHandler(this.butForce_Click);
        // 
        // butDelate
        // 
        this.butDelate.Location = new System.Drawing.Point(115, 176);
        this.butDelate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butDelate.Name = "butDelate";
        this.butDelate.Size = new System.Drawing.Size(78, 28);
        this.butDelate.TabIndex = 6;
        this.butDelate.Text = "Del";
        this.butDelate.UseVisualStyleBackColor = true;
        this.butDelate.Click += new System.EventHandler(this.butDelate_Click);
        // 
        // butForceDown
        // 
        this.butForceDown.Location = new System.Drawing.Point(115, 242);
        this.butForceDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butForceDown.Name = "butForceDown";
        this.butForceDown.Size = new System.Drawing.Size(94, 28);
        this.butForceDown.TabIndex = 8;
        this.butForceDown.Text = "Force Right";
        this.butForceDown.UseVisualStyleBackColor = true;
        this.butForceDown.Click += new System.EventHandler(this.butForceDown_Click);
        // 
        // butPush
        // 
        this.butPush.Location = new System.Drawing.Point(113, 81);
        this.butPush.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butPush.Name = "butPush";
        this.butPush.Size = new System.Drawing.Size(78, 28);
        this.butPush.TabIndex = 9;
        this.butPush.Text = "Fire";
        this.butPush.UseVisualStyleBackColor = true;
        this.butPush.Click += new System.EventHandler(this.butPush_Click);
        // 
        // butSave
        // 
        this.butSave.Location = new System.Drawing.Point(115, 385);
        this.butSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butSave.Name = "butSave";
        this.butSave.Size = new System.Drawing.Size(78, 28);
        this.butSave.TabIndex = 10;
        this.butSave.Text = "Save";
        this.butSave.UseVisualStyleBackColor = true;
        this.butSave.Click += new System.EventHandler(this.butSave_Click);
        // 
        // buttLift
        // 
        this.buttLift.Location = new System.Drawing.Point(115, 278);
        this.buttLift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.buttLift.Name = "buttLift";
        this.buttLift.Size = new System.Drawing.Size(78, 28);
        this.buttLift.TabIndex = 11;
        this.buttLift.Text = "Force Lift";
        this.buttLift.UseVisualStyleBackColor = true;
        this.buttLift.Click += new System.EventHandler(this.buttLift_Click);
        // 
        // buttMove
        // 
        this.buttMove.Location = new System.Drawing.Point(117, 314);
        this.buttMove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.buttMove.Name = "buttMove";
        this.buttMove.Size = new System.Drawing.Size(93, 28);
        this.buttMove.TabIndex = 12;
        this.buttMove.Text = "Force Move";
        this.buttMove.UseVisualStyleBackColor = true;
        this.buttMove.Click += new System.EventHandler(this.buttMove_Click);
        // 
        // butShowNeed
        // 
        this.butShowNeed.Location = new System.Drawing.Point(113, 494);
        this.butShowNeed.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butShowNeed.Name = "butShowNeed";
        this.butShowNeed.Size = new System.Drawing.Size(94, 28);
        this.butShowNeed.TabIndex = 13;
        this.butShowNeed.Text = "ShowNeed";
        this.butShowNeed.UseVisualStyleBackColor = true;
        this.butShowNeed.Click += new System.EventHandler(this.butShowNeed_Click);
        // 
        // butShowSurf
        // 
        this.butShowSurf.Location = new System.Drawing.Point(113, 522);
        this.butShowSurf.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.butShowSurf.Name = "butShowSurf";
        this.butShowSurf.Size = new System.Drawing.Size(94, 28);
        this.butShowSurf.TabIndex = 14;
        this.butShowSurf.Text = "ShowSurf";
        this.butShowSurf.UseVisualStyleBackColor = true;
        this.butShowSurf.Click += new System.EventHandler(this.butShowSurf_Click);
        // 
        // btnPause
        // 
        this.btnPause.Location = new System.Drawing.Point(113, 431);
        this.btnPause.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.btnPause.Name = "btnPause";
        this.btnPause.Size = new System.Drawing.Size(94, 28);
        this.btnPause.TabIndex = 15;
        this.btnPause.Text = "Pause";
        this.btnPause.UseVisualStyleBackColor = true;
        this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
        // 
        // btnStep
        // 
        this.btnStep.Location = new System.Drawing.Point(113, 458);
        this.btnStep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.btnStep.Name = "btnStep";
        this.btnStep.Size = new System.Drawing.Size(94, 28);
        this.btnStep.TabIndex = 16;
        this.btnStep.Text = "Step";
        this.btnStep.UseVisualStyleBackColor = true;
        this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
        // 
        // FormParam
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(209, 666);
        this.Controls.Add(this.btnStep);
        this.Controls.Add(this.btnPause);
        this.Controls.Add(this.butShowSurf);
        this.Controls.Add(this.butShowNeed);
        this.Controls.Add(this.buttMove);
        this.Controls.Add(this.buttLift);
        this.Controls.Add(this.butSave);
        this.Controls.Add(this.butPush);
        this.Controls.Add(this.butForceDown);
        this.Controls.Add(this.butForce);
        this.Controls.Add(this.butDelate);
        this.Controls.Add(this.butObstacle);
        this.Controls.Add(this.butPipe);
        this.Controls.Add(this.ButDrop);
        this.Controls.Add(this.XTrack);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.FileList);
        this.Location = new System.Drawing.Point(1300, 0);
        this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.Name = "FormParam";
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "FormParam";
        this.Load += new System.EventHandler(this.FormParam_Load);
        ((System.ComponentModel.ISupportInitialize)(this.XTrack)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ListBox FileList;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TrackBar XTrack;
    private System.Windows.Forms.Button ButDrop;
    private System.Windows.Forms.Button butPipe;
    private System.Windows.Forms.Button butObstacle;
    private System.Windows.Forms.Button butForce;
    private System.Windows.Forms.Button butDelate;
    private System.Windows.Forms.Button butForceDown;
    private System.Windows.Forms.Button butPush;
    private System.Windows.Forms.Button butSave;
    private System.Windows.Forms.Button buttLift;
    private System.Windows.Forms.Button buttMove;
    private System.Windows.Forms.Button butShowNeed;
    private System.Windows.Forms.Button butShowSurf;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnStep;
  }
}