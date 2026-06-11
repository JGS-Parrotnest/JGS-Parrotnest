using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Runtime.InteropServices;

namespace ParrotnestDesktopInstaller
{
    public partial class MainForm : Form
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder? strReturn, int iReturnLength, IntPtr hwndCallback);

        int pageIndex = 0;
        bool isRunning = false;

        public MainForm()
        {
            InitializeComponent();
            UpdatePages();
            PlaySound();
        }

        void PlaySound()
        {
            try
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var soundPath = Path.Combine(appData, "parrot_installer.mp3");

                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ParrotnestDesktopInstaller.parrot.mp3"))
                {
                    if (stream != null)
                    {
                        using (var fs = new FileStream(soundPath, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fs);
                        }
                    }
                }

                mciSendString($"open \"{soundPath}\" type mpegvideo alias parrot", null, 0, IntPtr.Zero);
                mciSendString("play parrot", null, 0, IntPtr.Zero);
            }
            catch { }
        }

        void BtnBack_Click(object? sender, EventArgs e)
        {
            if (isRunning) return;
            if (pageIndex > 0) pageIndex--;
            UpdatePages();
        }

        async void BtnNext_Click(object? sender, EventArgs e)
        {
            if (isRunning) return;
            if (pageIndex == 0)
            {
                pageIndex = 1;
                UpdatePages();
                return;
            }
            if (pageIndex == 1)
            {
                isRunning = true;
                btnBack.Enabled = false;
                btnNext.Enabled = false;
                btnFinish.Enabled = false;
                btnCancel.Enabled = false;
                try
                {
                    if (chkNode.Checked)
                    {
                        var installed = await IsNodeInstalledAsync();
                        if (!installed)
                        {
                            await DownloadAndInstallNodeAsync();
                        }
                    }
                    pageIndex = 2;
                    UpdatePages();
                    await RunPowershellSequenceAsync();
                    pageIndex = 3;
                    isRunning = false;
                    UpdatePages();
                }
                catch (Exception ex)
                {
                    AppendConsole("Błąd: " + ex.Message + Environment.NewLine);
                    isRunning = false;
                    btnCancel.Text = "Zamknij";
                    btnCancel.Enabled = true;
                }
                return;
            }
            if (pageIndex == 2)
            {
                pageIndex = 3;
                UpdatePages();
                return;
            }
        }

        void BtnFinish_Click(object? sender, EventArgs e)
        {
            if (isRunning) return;
            if (pageIndex == 3)
            {
                if (chkCreateShortcut.Checked)
                {
                    CreateDesktopShortcut();
                }
                if (chkRunParrotnest.Checked)
                {
                    TryLaunchParrotnest();
                }
            }
            Close();
        }

        void CreateDesktopShortcut()
        {
            try
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var targetFolder = Path.Combine(appData, "Parrotnest Desktop Client");
                var targetExe = Path.Combine(targetFolder, "Parrotnest Desktop Client.exe");

                if (File.Exists(targetExe))
                {
                    var shortcutPath = Path.Combine(desktop, "Parrotnest Desktop Client.lnk");
                    var script = $"$s=(New-Object -ComObject WScript.Shell).CreateShortcut('{shortcutPath}');$s.TargetPath='{targetExe}';$s.WorkingDirectory='{targetFolder}';$s.Save()";
                    var psi = new ProcessStartInfo("powershell.exe")
                    {
                        ArgumentList = { "-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", script },
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process.Start(psi)?.WaitForExit();
                }
            }
            catch { }
        }

        void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (isRunning) return;
            Close();
        }

        void UpdatePages()
        {
            pageWelcome.Visible = pageIndex == 0;
            pageOptions.Visible = pageIndex == 1;
            pageConsole.Visible = pageIndex == 2;
            pageFinal.Visible = pageIndex == 3;

            btnBack.Enabled = pageIndex > 0 && !isRunning;
            btnNext.Enabled = (pageIndex == 0 || pageIndex == 1 || pageIndex == 2) && !isRunning && pageIndex < 3;
            btnFinish.Enabled = pageIndex == 3 && !isRunning;
        }

        async Task DownloadAndInstallNodeAsync()
        {
            var url = "https://nodejs.org/dist/v24.14.0/node-v24.14.0-x64.msi";
            var tmp = Path.Combine(Path.GetTempPath(), "node-installer.msi");
            using (var http = new HttpClient())
            {
                using var resp = await http.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                await using var fs = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None);
                await resp.Content.CopyToAsync(fs);
            }
            var psi = new ProcessStartInfo("msiexec.exe");
            psi.ArgumentList.Add("/i");
            psi.ArgumentList.Add(tmp);
            psi.ArgumentList.Add("/passive");
            psi.ArgumentList.Add("/norestart");
            psi.UseShellExecute = true;
            psi.Verb = "runas";
            var p = Process.Start(psi);
            if (p != null)
            {
                await Task.Run(() => p.WaitForExit());
            }
            await Task.Delay(2000);
        }

        async Task RunPowershellSequenceAsync()
        {
            txtConsole.Clear();
            RefreshPath();

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var system32 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32");
            try
            {
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ParrotnestDesktopInstaller.logo.ico"))
                {
                    if (stream != null)
                    {
                        var iconPath = Path.Combine(appData, "logo.ico");
                        using (var fs = new FileStream(iconPath, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fs);
                        }
                    }
                }
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ParrotnestDesktopInstaller.animation.js"))
                {
                    if (stream != null)
                    {
                        var animPath = Path.Combine(appData, "animation.js");
                        using (var fs = new FileStream(animPath, FileMode.Create, FileAccess.Write))
                        {
                            await stream.CopyToAsync(fs);
                        }
                    }
                }
            }
            catch { }

            var sysCmds = new[]
            {
                "if ((Get-ExecutionPolicy) -ne 'Bypass') { Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force -ErrorAction SilentlyContinue }",
                "npm install -g npm",
                "npm -v",
                "npm install -g nativefier",
                "nativefier --version"
            };
            foreach (var c in sysCmds)
            {
                AppendConsole($"> {c}{Environment.NewLine}");
                var code = await ExecPowerShellAsync(c, system32);
                if (code != 0 && !c.Contains("Set-ExecutionPolicy"))
                {
                    AppendConsole($"Zakończono z kodem {code}{Environment.NewLine}");
                    RefreshPath();
                }
            }
            string targetUrl = "https://pn.hnato.pl/";
            try
            {
                if (chkCustomUrl != null && chkCustomUrl.Checked && txtCustomUrl != null)
                {
                    var input = (txtCustomUrl.Text ?? "").Trim();
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (!(input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                              input.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                        {
                            input = "http://" + input;
                        }
                        targetUrl = input;
                    }
                }
            }
            catch { }
            var finalCmd = $"nativefier \"{targetUrl}\" --name \"Parrotnest Desktop Client\" --width 1920 --height 1080 --icon \"logo.ico\" --inject \"animation.js\"";
            AppendConsole($"> {finalCmd}{Environment.NewLine}");
            var finalCode = await ExecPowerShellAsync(finalCmd, appData);
            if (finalCode == 0)
            {
                try
                {
                    var sourceDir = Path.Combine(appData, "Parrotnest Desktop Client-win32-x64");
                    var targetDir = Path.Combine(appData, "Parrotnest Desktop Client");
                    if (Directory.Exists(targetDir)) Directory.Delete(targetDir, true);
                    if (Directory.Exists(sourceDir))
                    {
                        Directory.Move(sourceDir, targetDir);
                        var oldExe = Path.Combine(targetDir, "Parrotnest Desktop Client.exe");
                        if (!File.Exists(oldExe))
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendConsole($"Błąd podczas porządkowania plików: {ex.Message}{Environment.NewLine}");
                }
            }
            else
            {
                AppendConsole($"Zakończono z kodem {finalCode}{Environment.NewLine}");
            }
        }

        async Task<bool> IsNodeInstalledAsync()
        {
            try
            {
                var psi = new ProcessStartInfo("node");
                psi.ArgumentList.Add("-v");
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                var p = new Process();
                p.StartInfo = psi;
                p.Start();
                await Task.Run(() => p.WaitForExit());
                return p.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        void RefreshPath()
        {
            try
            {
                var machinePath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine) ?? "";
                var userPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                var combinedPath = machinePath;
                if (!string.IsNullOrEmpty(userPath))
                {
                    combinedPath = combinedPath.TrimEnd(';') + ";" + userPath;
                }
                Environment.SetEnvironmentVariable("Path", combinedPath, EnvironmentVariableTarget.Process);
                AppendConsole("Zsynchronizowano zmienne środowiskowe PATH." + Environment.NewLine);
            }
            catch (Exception ex)
            {
                AppendConsole("Błąd podczas odświeżania PATH: " + ex.Message + Environment.NewLine);
            }
        }

        async Task<int> ExecPowerShellAsync(string command, string workingDir)
        {
            var psi = new ProcessStartInfo("powershell.exe");
            psi.ArgumentList.Add("-NoProfile");
            psi.ArgumentList.Add("-ExecutionPolicy");
            psi.ArgumentList.Add("Bypass");
            psi.ArgumentList.Add("-Command");
            psi.ArgumentList.Add(command);
            psi.WorkingDirectory = workingDir;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            var currentPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Process);
            if (currentPath != null)
            {
                psi.EnvironmentVariables["Path"] = currentPath;
            }

            var p = new Process();
            p.StartInfo = psi;
            p.OutputDataReceived += (s, e) => { if (e.Data != null) AppendConsole(e.Data + Environment.NewLine); };
            p.ErrorDataReceived += (s, e) => { if (e.Data != null) AppendConsole(e.Data + Environment.NewLine); };
            p.Start();
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();
            await Task.Run(() => p.WaitForExit());
            return p.ExitCode;
        }

        async Task TestUrlAsync()
        {
            string url = "https://pn.hnato.pl/";
            try
            {
                if (chkCustomUrl != null && txtCustomUrl != null)
                {
                    var input = (txtCustomUrl.Text ?? "").Trim();
                    if (chkCustomUrl.Checked && !string.IsNullOrEmpty(input))
                    {
                        if (!(input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                              input.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
                        {
                            input = "http://" + input;
                        }
                        url = input;
                    }
                }
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(6) };
                using var req = new HttpRequestMessage(HttpMethod.Get, url);
                var resp = await http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead);
                if ((int)resp.StatusCode < 400)
                {
                    MessageBox.Show(this, $"Połączenie OK ({(int)resp.StatusCode})", "Sprawdź połączenie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(this, $"Serwer zwrócił kod {(int)resp.StatusCode}", "Sprawdź połączenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Błąd połączenia: {ex.Message}", "Sprawdź połączenie", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void AppendConsole(string text)
        {
            if (txtConsole.InvokeRequired)
            {
                txtConsole.Invoke(new Action<string>(AppendConsole), text);
                return;
            }
            txtConsole.SelectionStart = txtConsole.TextLength;
            txtConsole.SelectionLength = 0;
            txtConsole.AppendText(text);
            txtConsole.SelectionStart = txtConsole.TextLength;
            txtConsole.ScrollToCaret();
        }

        void TryLaunchParrotnest()
        {
            try
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string exePath = Path.Combine(appData, "Parrotnest Desktop Client", "Parrotnest Desktop Client.exe");

                if (File.Exists(exePath))
                {
                    var psi = new ProcessStartInfo(exePath) { UseShellExecute = true };
                    Process.Start(psi);
                }
                else
                {
                    foreach (var file in Directory.EnumerateFiles(appData, "Parrotnest Desktop Client.exe", SearchOption.AllDirectories))
                    {
                        var psi = new ProcessStartInfo(file) { UseShellExecute = true };
                        Process.Start(psi);
                        break;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
