using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelManager
{
    public class DynamischeFormen

    {

        public static void CreatePreviewForm(string imagePath, TimeSpan duration)
        {
            var form = new Form { Text = "Bildvorschau", Width = 800, Height = 600 };
            var pb = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };

            // Bild asynchron laden
            Task.Run(() =>
            {
                var img = Image.FromFile(imagePath);
                form.Invoke((MethodInvoker)delegate { pb.Image = img; });
            });

            var btnSave = new Button { Text = "Speichern" };
            var btnRate = new Button { Text = "Bewerten" };
            var btnClose = new Button { Text = "Schließen" };

            btnClose.Click += (s, e) => form.Close();
            btnSave.Click += (s, e) =>
            {
                var sfd = new SaveFileDialog { Filter = "PNG Bild|*.png" };
                if (sfd.ShowDialog() == DialogResult.OK)
                    File.Copy(imagePath, sfd.FileName, true);
            };

            var flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                Controls = { btnSave, btnRate, btnClose }
            };

            form.Controls.Add(pb);
            form.Controls.Add(flowPanel);
            form.Show();
        }



    }

    public class EditCommentForm : Form
    {
        private readonly TextBox _textBox;
        private readonly ListBox _suggestionList;
        private List<string> _availableComments = new();
        public string Comment { get; private set; }

        public EditCommentForm(string initialComment, ListView listView)
        {
            // Sammle alle vorhandenen Kommentare aus der ListView
            _availableComments = listView.Items
                .Cast<ListViewItem>()
                .Select(item => item.SubItems[3].Text)
                .Distinct()
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();

            // TextBox mit Auto-Vervollständigung
            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Text = initialComment,
                ScrollBars = ScrollBars.Vertical
            };

            // Vorschlagsliste
            _suggestionList = new ListBox
            {
                Visible = false,
                Dock = DockStyle.Bottom,
                Height = 100,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Steuerelemente
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                RowStyles =
            {
                new RowStyle(SizeType.Percent, 100),
                new RowStyle(SizeType.AutoSize)
            }
            };

            // Event-Handler
            _textBox.TextChanged += (s, e) => UpdateSuggestions();
            _textBox.KeyDown += HandleKeyNavigation;
            _suggestionList.Click += SelectSuggestion;
            _suggestionList.KeyDown += SelectSuggestion;

            panel.Controls.Add(_textBox, 0, 0);
            panel.Controls.Add(_suggestionList, 0, 1);

            var btnOK = new Button
            {
                Text = "OK",
                Dock = DockStyle.Bottom,
                DialogResult = DialogResult.OK
            };

            btnOK.Click += (s, e) => Comment = _textBox.Text;

            Controls.Add(panel);
            Controls.Add(btnOK);

            AcceptButton = btnOK;
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterParent;
        }








        private void UpdateSuggestions()
        {
            var currentText = _textBox.Text;
            var cursorPos = _textBox.SelectionStart;

            // Finde passende Vorschläge
            var suggestions = _availableComments
                .Where(c => c.StartsWith(currentText, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .ToList();

            // Zeige Vorschläge an
            _suggestionList.Visible = suggestions.Any();
            _suggestionList.Items.Clear();
            _suggestionList.Items.AddRange(suggestions.ToArray());
        }

        private void HandleKeyNavigation(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down when _suggestionList.Visible:
                    _suggestionList.Focus();
                    if (_suggestionList.Items.Count > 0)
                        _suggestionList.SelectedIndex = 0;
                    e.Handled = true;
                    break;

                case Keys.Escape:
                    _suggestionList.Visible = false;
                    e.Handled = true;
                    break;
            }
        }

        private void SelectSuggestion(object sender, EventArgs e)
        {
            if (_suggestionList.SelectedItem == null) return;

            _textBox.Text = _suggestionList.SelectedItem.ToString();
            _textBox.Focus();
            _suggestionList.Visible = false;
        }


    












    
    } 




































   
    }









    