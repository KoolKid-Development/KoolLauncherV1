using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Installer;
using CmlLib.Core.Files;
using CmlLib.Core.Downloader;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GalacticCraftLauncher
{
    public partial class MainForm : Form
    {
        public MainForm(MSession session)
        {
            this.session = session;
            InitializeComponent();
        }

        CMLauncher launcher;
        readonly MSession session;
        MinecraftPath gamePath;


        bool useMJava = true;
        string javaPath = "java.exe";
        Form1 usernamechange = new Form1();
        private int uiThreadId = Thread.CurrentThread.ManagedThreadId;

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            this.Refresh();

            var defaultPath = new MinecraftPath(MinecraftPath.GetOSDefaultPath());
            await initializeLauncher(defaultPath);
        }

        private async Task initializeLauncher(MinecraftPath path)
        {
            txtPath.Text = path.BasePath;
            this.gamePath = path;

            if (useMJava)
                lbJavaPath.Text = path.Runtime;

            launcher = new CMLauncher(path);
            launcher.FileChanged += Launcher_FileChanged;
            launcher.ProgressChanged += Launcher_ProgressChanged;
            await refreshVersions(null);
        }

        private void Launcher_FileChanged(DownloadFileChangedEventArgs e)
        {
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                Debug.WriteLine(e);
            }
            Pb_Progress.Maximum = e.TotalFileCount;
            Pb_Progress.Value = e.ProgressedFileCount;
        }

        private void Launcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Thread.CurrentThread.ManagedThreadId != uiThreadId)
            {
                Debug.WriteLine(e);
            }
            Pb_Progress.Maximum = 100;
            Pb_Progress.Value = e.ProgressPercentage;
        }

        private void setUIEnabled(bool value)
        {
            settings.Enabled = value;
            panel1.Enabled = value;
        }

        private void StartProcess(Process process)
        {            
            output(process.StartInfo.Arguments);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.OutputDataReceived += Process_OutputDataReceived;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            output(e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            output(e.Data);
        }

        void output(string msg)
        {
           
        }

        private void btnSetLastVersion_Click(object sender, EventArgs e)
        {
            cbVersion.Text = launcher.Versions.LatestReleaseVersion?.Name;
        }

        private async void btnRefreshVersion_Click(object sender, EventArgs e)
        {
            await refreshVersions(null);
        }

        private async Task refreshVersions(string showVersion)
        {
            cbVersion.Items.Clear();

            var versions = await launcher.GetAllVersionsAsync();

            bool showVersionExist = false;
            foreach (var item in versions)
            {
                if (showVersion != null && item.Name == showVersion)
                    showVersionExist = true;
                cbVersion.Items.Add(item.Name);
            }

            if (showVersion == null || !showVersionExist)
                btnSetLastVersion_Click(null, null);
            else
                cbVersion.Text = showVersion;
        }
      

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbVersion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Btn_Launch_Click_1(object sender, EventArgs e)
        {
            if (session == null)
            {
                MessageBox.Show("Oops, we could not verify your session.");
                return;
            }

            if (cbVersion.Text == "")
            {
                MessageBox.Show("You need to select a version first.");
                return;
            }

            setUIEnabled(false);

            try
            {
                var launchOption = new MLaunchOption()
                {
                    MaximumRamMb = int.Parse(TxtXmx.Text),
                    Session = this.session,
                    ServerIp = Txt_ServerIp.Text,
                };

                

                if (!useMJava)
                    launchOption.JavaPath = javaPath;

                if (!string.IsNullOrEmpty(txtXms.Text))
                    launchOption.MinimumRamMb = int.Parse(txtXms.Text);

                if (!string.IsNullOrEmpty(Txt_ServerPort.Text))
                    launchOption.ServerPort = int.Parse(Txt_ServerPort.Text);

                if (rbParallelDownload.Checked)
                {
                    System.Net.ServicePointManager.DefaultConnectionLimit = 256;
                    launcher.FileDownloader = new AsyncParallelDownloader();
                }
                else
                    launcher.FileDownloader = new SequenceDownloader();

                launcher.GameFileCheckers.AssetFileChecker.CheckHash = cbSkipHashCheck.Checked;
                launcher.GameFileCheckers.ClientFileChecker.CheckHash = cbSkipHashCheck.Checked;
                launcher.GameFileCheckers.LibraryFileChecker.CheckHash = cbSkipHashCheck.Checked;

                if (cbSkipAssets.Checked)
                    launcher.GameFileCheckers.AssetFileChecker = null;

                var process = await launcher.CreateProcessAsync(cbVersion.Text, launchOption);


                StartProcess(process);
            }
            catch (FormatException fex)
            {
                MessageBox.Show("Failed to create MLaunchOption\n\n" + fex);
            }
            catch (MDownloadFileException mex)
            {
                MessageBox.Show(
                    $"FileName : {mex.ExceptionFile.Name}\n" +
                    $"FilePath : {mex.ExceptionFile.Path}\n" +
                    $"FileUrl : {mex.ExceptionFile.Url}\n" +
                    $"FileType : {mex.ExceptionFile.Type}\n\n" +
                    mex.ToString());
            }
            catch (Win32Exception wex)
            {
                MessageBox.Show(wex + "\n\nOops, we found a problem in your Java.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                setUIEnabled(true);
                Application.Exit();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            username.Text = "Logged in as " + Form1.accountname + "!";
            settings.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            settings.Show();
            settings.Location = new Point(182, 176);
            settingsbtn2.Show();
            settingsbtn1.Hide();
        }

        private void settingsbtn2_Click(object sender, EventArgs e)
        {
            settings.Hide();
            settingsbtn2.Hide();
            settingsbtn1.Show();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            settings.Hide();
            settingsbtn1.Hide();
            settingsbtn2.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {   
            usernamechange.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(forumurl.Text);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(storeurl.Text);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(voteurl.Text);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(discordurl.Text);

        }

        private void Txt_ServerIp_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
