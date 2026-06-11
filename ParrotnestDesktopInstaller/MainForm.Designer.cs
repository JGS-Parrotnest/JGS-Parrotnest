using System.Drawing;
using System.Windows.Forms;

namespace ParrotnestDesktopInstaller
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pageWelcome;
        private Panel pageOptions;
        private Panel pageConsole;
        private Panel pageFinal;
        private Label lblWelcome;
        private Label lblDescription;
        private PictureBox picLogo;
        private CheckBox chkNode;
        private Button btnBack;
        private Button btnNext;
        private Button btnFinish;
        private Button btnCancel;
        private RichTextBox txtConsole;
        private Panel contentPanel;
        private CheckBox chkRunParrotnest;
        private CheckBox chkCreateShortcut;
        private Label lblFinal;
        private CheckBox chkCustomUrl;
        private Label lblCustomUrl;
        private TextBox txtCustomUrl;
        private Button btnTestUrl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Text = "Parrotnest Desktop Installer";
            Size = new Size(800, 500);
            MinimumSize = new Size(800, 500);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            try
            {
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ParrotnestDesktopInstaller.logo.ico"))
                {
                    if (stream != null)
                    {
                        Icon = new Icon(stream);
                    }
                }
            }
            catch { }

            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            Controls.Add(contentPanel);

            var bottomPanel = new Panel();
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.Height = 50;
            Controls.Add(bottomPanel);

            btnBack = new Button();
            btnBack.Text = "Wstecz";
            btnBack.Enabled = false;
            btnBack.Left = 10;
            btnBack.Top = 10;
            btnBack.Width = 100;
            btnBack.Click += BtnBack_Click;
            bottomPanel.Controls.Add(btnBack);

            btnNext = new Button();
            btnNext.Text = "Dalej";
            btnNext.Left = 120;
            btnNext.Top = 10;
            btnNext.Width = 100;
            btnNext.Click += BtnNext_Click;
            bottomPanel.Controls.Add(btnNext);

            btnFinish = new Button();
            btnFinish.Text = "Zakończ";
            btnFinish.Left = 230;
            btnFinish.Top = 10;
            btnFinish.Width = 100;
            btnFinish.Click += BtnFinish_Click;
            bottomPanel.Controls.Add(btnFinish);

            btnCancel = new Button();
            btnCancel.Text = "Anuluj";
            btnCancel.Left = 340;
            btnCancel.Top = 10;
            btnCancel.Width = 100;
            btnCancel.Click += BtnCancel_Click;
            bottomPanel.Controls.Add(btnCancel);

            pageWelcome = new Panel();
            pageWelcome.Dock = DockStyle.Fill;

            picLogo = new PictureBox();
            picLogo.Size = new Size(128, 128);
            picLogo.Location = new Point(650, 10);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            try
            {
                using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("ParrotnestDesktopInstaller.logo.ico"))
                {
                    if (stream != null)
                    {
                        picLogo.Image = new Icon(stream, 128, 128).ToBitmap();
                    }
                }
            }
            catch { }
            pageWelcome.Controls.Add(picLogo);

            lblWelcome = new Label();
            lblWelcome.Text = "Instalator komunikatora Parrotnest";
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold);
            lblWelcome.Left = 20;
            lblWelcome.Top = 20;
            pageWelcome.Controls.Add(lblWelcome);

            lblDescription = new Label();
            lblDescription.Text = "Parrotnest to aplikacja czatu w czasie rzeczywistym, stworzona dla klas lekcyjnych,\n" +
                                 "małych społeczności i grup znajomych.\n\n" +
                                 "Łączy w sobie:\n" +
                                 "🖥  Host pulpitu Windows (powłoka WinForms), która uruchamia serwer ( ParrotnestServer.exe )\n" +
                                 "🌐  ASP.NET Core API z SignalR do przesyłania wiadomości w czasie rzeczywistym\n" +
                                 "📄  Klient PHP + Vanilla JS serwowany bezpośrednio przez serwer\n" +
                                 "💾  Baza danych SQLite dla użytkowników, znajomości, wiadomości, grup i logów administratora\n\n" +
                                 "Celem jest zapewnienie doświadczenia „podobnego do Discorda”, które jest:\n" +
                                 "łatwe do wdrożenia na jednej maszynie z systemem Windows,\n" +
                                 "samowystarczalne (domyślnie nie wymaga zewnętrznej bazy danych),\n" +
                                 "przyjazne dla środowisk szkolnych i laboratoriów LAN.";
            lblDescription.Size = new Size(600, 350);
            lblDescription.AutoSize = false;
            lblDescription.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            lblDescription.Left = 20;
            lblDescription.Top = 60;
            pageWelcome.Controls.Add(lblDescription);

            pageOptions = new Panel();
            pageOptions.Dock = DockStyle.Fill;
            chkNode = new CheckBox();
            chkNode.Text = "Pobierz NodeJS";
            chkNode.AutoSize = true;
            chkNode.Left = 20;
            chkNode.Top = 20;
            pageOptions.Controls.Add(chkNode);
            chkCustomUrl = new CheckBox();
            chkCustomUrl.Text = "Zmień adres URL klienta";
            chkCustomUrl.AutoSize = true;
            chkCustomUrl.Left = 20;
            chkCustomUrl.Top = 50;
            pageOptions.Controls.Add(chkCustomUrl);
            lblCustomUrl = new Label();
            lblCustomUrl.Text = "Adres URL / IP:";
            lblCustomUrl.AutoSize = true;
            lblCustomUrl.Left = 40;
            lblCustomUrl.Top = 80;
            pageOptions.Controls.Add(lblCustomUrl);
            txtCustomUrl = new TextBox();
            txtCustomUrl.Left = 40;
            txtCustomUrl.Top = 100;
            txtCustomUrl.Width = 500;
            txtCustomUrl.Text = "https://pn.hnato.pl/";
            pageOptions.Controls.Add(txtCustomUrl);
            btnTestUrl = new Button();
            btnTestUrl.Text = "Sprawdź połączenie";
            btnTestUrl.Left = 560;
            btnTestUrl.Top = 98;
            btnTestUrl.Width = 160;
            btnTestUrl.Height = 28;
            btnTestUrl.Enabled = false;
            pageOptions.Controls.Add(btnTestUrl);
            btnTestUrl.Click += async (s, e) => { await TestUrlAsync(); };
            lblCustomUrl.Enabled = false;
            txtCustomUrl.Enabled = false;
            chkCustomUrl.CheckedChanged += (s, e) => {
                lblCustomUrl.Enabled = chkCustomUrl.Checked;
                txtCustomUrl.Enabled = chkCustomUrl.Checked;
                btnTestUrl.Enabled = chkCustomUrl.Checked;
            };

            pageConsole = new Panel();
            pageConsole.Dock = DockStyle.Fill;
            txtConsole = new RichTextBox();
            txtConsole.ReadOnly = true;
            txtConsole.BackColor = Color.Black;
            txtConsole.ForeColor = Color.White;
            txtConsole.Font = new Font("Consolas", 10);
            txtConsole.Dock = DockStyle.Fill;
            pageConsole.Controls.Add(txtConsole);

            pageFinal = new Panel();
            pageFinal.Dock = DockStyle.Fill;
            lblFinal = new Label();
            lblFinal.Text = "Instalacja zakończona pomyślnie.";
            lblFinal.AutoSize = true;
            lblFinal.Left = 20;
            lblFinal.Top = 20;
            pageFinal.Controls.Add(lblFinal);
            chkRunParrotnest = new CheckBox();
            chkRunParrotnest.Text = "Uruchom Parrotnest Desktop Client";
            chkRunParrotnest.AutoSize = true;
            chkRunParrotnest.Left = 20;
            chkRunParrotnest.Top = 60;
            pageFinal.Controls.Add(chkRunParrotnest);

            chkCreateShortcut = new CheckBox();
            chkCreateShortcut.Text = "Utwórz skrót na pulpicie";
            chkCreateShortcut.AutoSize = true;
            chkCreateShortcut.Left = 20;
            chkCreateShortcut.Top = 90;
            chkCreateShortcut.Checked = true;
            pageFinal.Controls.Add(chkCreateShortcut);

            contentPanel.Controls.Add(pageWelcome);
            contentPanel.Controls.Add(pageOptions);
            contentPanel.Controls.Add(pageConsole);
            contentPanel.Controls.Add(pageFinal);
        }
    }
}
