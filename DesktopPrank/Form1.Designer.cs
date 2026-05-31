namespace DesktopPrank;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cleanDesktopButton = new Button();
        cleanProgressBar = new ProgressBar();
        SuspendLayout();
        // 
        // cleanDesktopButton
        // 
        cleanDesktopButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        cleanDesktopButton.Location = new Point(100, 85);
        cleanDesktopButton.Name = "cleanDesktopButton";
        cleanDesktopButton.Size = new Size(200, 60);
        cleanDesktopButton.TabIndex = 0;
        cleanDesktopButton.Text = "Clean Desktop";
        cleanDesktopButton.UseVisualStyleBackColor = true;
        // 
        // cleanProgressBar
        // 
        cleanProgressBar.Location = new Point(75, 170);
        cleanProgressBar.Name = "cleanProgressBar";
        cleanProgressBar.Size = new Size(250, 20);
        cleanProgressBar.TabIndex = 1;
        cleanProgressBar.Visible = false;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(400, 300);
        Controls.Add(cleanProgressBar);
        Controls.Add(cleanDesktopButton);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = true;
        Name = "Form1";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Desktop Cleaner";
        ResumeLayout(false);
    }

    #endregion

    private Button cleanDesktopButton;
    private ProgressBar cleanProgressBar;
}
