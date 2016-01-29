namespace fluidedotnet
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
        this.components = new System.ComponentModel.Container();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.ReceiveWorkerThread = new System.ComponentModel.BackgroundWorker();
        this.SuspendLayout();
        // 
        // timer1
        // 
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        // 
        // ReceiveWorkerThread
        // 
        this.ReceiveWorkerThread.WorkerReportsProgress = true;
        this.ReceiveWorkerThread.WorkerSupportsCancellation = true;
        this.ReceiveWorkerThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ReceiveWorkerThread_DoWork);
        this.ReceiveWorkerThread.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.ReceiveWorkerThread_ProgressChanged);
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(1292, 968);
        this.DoubleBuffered = true;
        this.KeyPreview = true;
        this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "Form1";
        this.TransparencyKey = System.Drawing.Color.DarkGray;
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
        this.Shown += new System.EventHandler(this.Form1_Shown);
        this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
        this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
        this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
        this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer timer1;
    private System.ComponentModel.BackgroundWorker ReceiveWorkerThread;
  }
}

