using System.Diagnostics;

namespace HW2
{
    public partial class MainForm : Form
    {
        string source = string.Empty;
        BicubicInterpolator bicubicInterpolator;
        public MainForm()
        {
            InitializeComponent();
            twiceTheCores.Visible = false;
            cancelButton.Visible = false;
        }
        private void on_downscaleButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string result = ofd.FileName;
            MessageBox.Show(result);

        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG|*.png|JPG|*.jpg";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                source = openFileDialog.FileName;
                int lastIndex = source.LastIndexOf("\\");
                label1.Text = source.Substring(lastIndex + 1);
            }
            else MessageBox.Show("Dialog box closed.");
        }

        private void downscaleButton_Click(object sender, EventArgs e)
        {

            if (source == string.Empty)
            {
                MessageBox.Show("Please select an image.");
                return;
            }
            if (downscalingFactorTextBox.Text is null)
            {
                MessageBox.Show("Please enter a downscaling factor.");
                return;
            }
            string factor = downscalingFactorTextBox.Text.Contains('.') ? downscalingFactorTextBox.Text.Replace('.', ',') : downscalingFactorTextBox.Text;
            double downscalingFactor;
            try
            {
                downscalingFactor = double.Parse(factor);
                bicubicInterpolator = new BicubicInterpolator(source, downscalingFactor);
                Thread worker = new Thread(ThreadWorker);
                worker.Start(downscalingFactor);
                downscaleButton.Visible = false;
                cancelButton.Visible = true;
                cancelButton.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter a valid scaling factor.");
            }



        }
        private void ThreadWorker(object p)
        {
            double factor = (double)p;
            bicubicInterpolator.InitializeNewImage();
            Bitmap newImage;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            if (!parallelCheck.Checked)
            {
                newImage = bicubicInterpolator.beginInterpolationSequential();
            }
            else newImage = bicubicInterpolator.beginInterpolationParallel(twiceTheCores.Checked);
            stopwatch.Stop();
            int lastIndex = source.LastIndexOf("\\");
            string path = source.Substring(0, lastIndex + 1) + "new_" + $"{factor:F4}_" + source.Substring(lastIndex + 1);
            newImage.Save(path);
            MessageBox.Show($"Done. Time taken (ms): {stopwatch.ElapsedMilliseconds}");
            Invoke(() =>
            {
                cancelButton.Visible = false;
                downscaleButton.Visible = true;
            });
        }
        private void parallelCheck_CheckedChanged(object sender, EventArgs e)
        {
            twiceTheCores.Visible = parallelCheck.Checked;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bicubicInterpolator.Cancel();
            cancelButton.Visible = false;
            downscaleButton.Visible = true;
        }
    }
}