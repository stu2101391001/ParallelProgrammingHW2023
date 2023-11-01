namespace HW2
{
    partial class MainForm
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
            imageSourceLabel = new Label();
            downscalingFactorLabel = new Label();
            downscalingFactorTextBox = new TextBox();
            loadImageButton = new Button();
            label1 = new Label();
            downscaleButton = new Button();
            parallelCheck = new CheckBox();
            parallelLabel = new Label();
            twiceTheCores = new CheckBox();
            cancelButton = new Button();
            SuspendLayout();
            // 
            // imageSourceLabel
            // 
            imageSourceLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            imageSourceLabel.AutoSize = true;
            imageSourceLabel.Location = new Point(70, 47);
            imageSourceLabel.Name = "imageSourceLabel";
            imageSourceLabel.Size = new Size(200, 41);
            imageSourceLabel.TabIndex = 0;
            imageSourceLabel.Text = "Image Source";
            // 
            // downscalingFactorLabel
            // 
            downscalingFactorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            downscalingFactorLabel.AutoSize = true;
            downscalingFactorLabel.Location = new Point(70, 121);
            downscalingFactorLabel.Name = "downscalingFactorLabel";
            downscalingFactorLabel.Size = new Size(201, 41);
            downscalingFactorLabel.TabIndex = 2;
            downscalingFactorLabel.Text = "Scaling Factor";
            // 
            // downscalingFactorTextBox
            // 
            downscalingFactorTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            downscalingFactorTextBox.Location = new Point(379, 118);
            downscalingFactorTextBox.MinimumSize = new Size(100, 40);
            downscalingFactorTextBox.Name = "downscalingFactorTextBox";
            downscalingFactorTextBox.Size = new Size(354, 47);
            downscalingFactorTextBox.TabIndex = 3;
            // 
            // loadImageButton
            // 
            loadImageButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            loadImageButton.Location = new Point(70, 257);
            loadImageButton.MaximumSize = new Size(276, 58);
            loadImageButton.MinimumSize = new Size(276, 58);
            loadImageButton.Name = "loadImageButton";
            loadImageButton.Size = new Size(276, 58);
            loadImageButton.TabIndex = 4;
            loadImageButton.Text = "Choose picture";
            loadImageButton.UseVisualStyleBackColor = true;
            loadImageButton.Click += loadImageButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(379, 56);
            label1.Name = "label1";
            label1.Size = new Size(122, 32);
            label1.TabIndex = 6;
            label1.Text = "File Name";
            // 
            // downscaleButton
            // 
            downscaleButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            downscaleButton.Location = new Point(457, 257);
            downscaleButton.MaximumSize = new Size(276, 58);
            downscaleButton.MinimumSize = new Size(276, 58);
            downscaleButton.Name = "downscaleButton";
            downscaleButton.Size = new Size(276, 58);
            downscaleButton.TabIndex = 7;
            downscaleButton.Text = "Scale";
            downscaleButton.UseVisualStyleBackColor = true;
            downscaleButton.Click += downscaleButton_Click;
            // 
            // parallelCheck
            // 
            parallelCheck.AutoSize = true;
            parallelCheck.Location = new Point(379, 189);
            parallelCheck.Name = "parallelCheck";
            parallelCheck.Size = new Size(133, 45);
            parallelCheck.TabIndex = 8;
            parallelCheck.Text = "Parallel";
            parallelCheck.UseVisualStyleBackColor = true;
            parallelCheck.CheckedChanged += parallelCheck_CheckedChanged;
            // 
            // parallelLabel
            // 
            parallelLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            parallelLabel.AutoSize = true;
            parallelLabel.Location = new Point(70, 193);
            parallelLabel.Name = "parallelLabel";
            parallelLabel.Size = new Size(124, 41);
            parallelLabel.TabIndex = 9;
            parallelLabel.Text = "Options";
            // 
            // twiceTheCores
            // 
            twiceTheCores.AutoSize = true;
            twiceTheCores.Location = new Point(578, 192);
            twiceTheCores.Name = "twiceTheCores";
            twiceTheCores.Size = new Size(146, 45);
            twiceTheCores.TabIndex = 10;
            twiceTheCores.Text = "2xCores";
            twiceTheCores.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(457, 257);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(276, 58);
            cancelButton.TabIndex = 11;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += button1_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 353);
            Controls.Add(twiceTheCores);
            Controls.Add(parallelLabel);
            Controls.Add(parallelCheck);
            Controls.Add(label1);
            Controls.Add(loadImageButton);
            Controls.Add(downscalingFactorTextBox);
            Controls.Add(downscalingFactorLabel);
            Controls.Add(imageSourceLabel);
            Controls.Add(cancelButton);
            Controls.Add(downscaleButton);
            Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            Margin = new Padding(6);
            MaximumSize = new Size(1200, 400);
            MinimumSize = new Size(800, 400);
            Name = "MainForm";
            Text = "Image Downscaler";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label imageSourceLabel;
        private Label downscalingFactorLabel;
        private TextBox downscalingFactorTextBox;
        private Button loadImageButton;
        private Label label1;
        private Button downscaleButton;
        private CheckBox parallelCheck;
        private Label parallelLabel;
        private CheckBox twiceTheCores;
        private Button cancelButton;
    }
}