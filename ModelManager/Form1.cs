using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
using System;
using System.Collections.Generic;

using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace ModelManager
{
    public partial class Form1 : Form
    {



        public Form1()
        {
            InitializeComponent();


            // Initialize ListView sorting
            Utils.InitializeListViewSorting(lvModelle);


            // Guidance Scale Control
            nudGuidanceScale.DecimalPlaces = 1;
            nudGuidanceScale.Minimum = 1.0m;
            nudGuidanceScale.Maximum = 20.0m;
            nudGuidanceScale.Value = 7.0m;
            nudGuidanceScale.Increment = 0.1m;

            // Speed Control (neu)
            nudSpeed.DecimalPlaces = 1;
            nudSpeed.Minimum = 0.5m;
            nudSpeed.Maximum = 2.0m;
            nudSpeed.Value = 1.0m;
            nudSpeed.Increment = 0.1m;

            // Pitch Control (neu)
            nudPitch.DecimalPlaces = 0;
            nudPitch.Minimum = -20;
            nudPitch.Maximum = 20;
            nudPitch.Value = 0;
            nudPitch.Increment = 1;

            // Seed Control
            nudSeed.Minimum = 0;
            nudSeed.Maximum = int.MaxValue;
            nudSeed.Value = 42;
            nudSeed.DecimalPlaces = 0;
            nudSeed.Increment = 1;

            // Steps Control
            nudNumSteps.Minimum = 1;
            nudNumSteps.Maximum = 150;
            nudNumSteps.Value = 20;
            nudNumSteps.DecimalPlaces = 0;
            nudNumSteps.Increment = 1;

            // Formatierung für ALLE NumericUpDowns
            foreach (var nud in new[] { nudGuidanceScale, nudSpeed, nudPitch, nudSeed, nudNumSteps })
            {
                nud.ThousandsSeparator = false;  // Bei Dezimalwerten besser ausschalten
                nud.TextAlign = HorizontalAlignment.Right;
                nud.Width = 120;

                // Für bessere UX bei negativen Werten (Pitch)
                if (nud == nudPitch)
                {
                    nud.ThousandsSeparator = false;
                    nud.DecimalPlaces = 0;
                }
            }
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            ScanHuggingFaceCache();
            //ScanAllGGUFModels();

            // Nach dem Scan sortieren
            lvModelle.ListViewItemSorter = new ListViewItemComparer(0); // Kein Cast notwendig

            lvModelle.Sort();
        }



        private void ScanHuggingFaceCache()
        {
            var cachePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".cache", "huggingface", "hub"
            );

            if (!Directory.Exists(cachePath)) return;

            var modelDirs = Directory.GetDirectories(cachePath, "models--*", SearchOption.TopDirectoryOnly);

            foreach (var modelDir in modelDirs)
            {
                try
                {
                    var modelParts = Path.GetFileName(modelDir)?
                        .Split(new[] { "--" }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(1)
                        .ToArray();

                    if (modelParts == null || modelParts.Length < 2) continue;

                    var modelName = string.Join("/", modelParts);
                    var modelFiles = new List<string>();

                    var snapshotsDir = Path.Combine(modelDir, "snapshots");
                    if (Directory.Exists(snapshotsDir))
                    {
                        modelFiles.AddRange(Directory.GetFiles(snapshotsDir, "*.*", SearchOption.AllDirectories)
                            .Where(f => IsModelFile(f)));
                    }

                    long totalSize = modelFiles.Count > 0
                        ? modelFiles.Sum(f => new FileInfo(f).Length)
                        : 0;

                    AddModelToList(modelName, totalSize, modelDir, "HuggingFace Model");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error processing {modelDir}: {ex.Message}");
                }
            }
        }

        private void AddModelToList(string modelName, long totalSize, string path, string comment)
        {
            lvModelle.Invoke((MethodInvoker)(() =>
            {
                var existingItem = lvModelle.Items.Cast<ListViewItem>()
                    .FirstOrDefault(item => item.Text == modelName);

                if (existingItem != null)
                {
                    // Größe extrahieren und in Bytes umrechnen
                    if (TryParseFileSize(existingItem.SubItems[1].Text, out long existingSize) && existingSize != totalSize)
                    {
                        existingItem.SubItems[1].Text = FormatFileSize(totalSize);
                    }
                }
                else
                {
                    var item = new ListViewItem(modelName);
                    item.SubItems.Add(FormatFileSize(totalSize));
                    item.SubItems.Add(path);
                    item.SubItems.Add(comment);
                    item.SubItems.Add("todo 1");
                    item.SubItems.Add("todo 2");
                    item.SubItems.Add("todo 3");
                    item.SubItems.Add("todo 4");
                    item.SubItems.Add("todo 5");

                    lvModelle.Items.Add(item);
                }
            }));
        }

        // Helfer zum Parsen von Dateigrößen
        private bool TryParseFileSize(string sizeText, out long size)
        {
            size = 0;
            if (string.IsNullOrWhiteSpace(sizeText)) return false;

            sizeText = sizeText.ToUpper().Replace(" ", "").Replace(",", ".");

            if (sizeText.EndsWith("GB") && double.TryParse(sizeText.Replace("GB", ""), out double gb))
            {
                size = (long)(gb * 1024 * 1024 * 1024);
                return true;
            }

            if (sizeText.EndsWith("MB") && double.TryParse(sizeText.Replace("MB", ""), out double mb))
            {
                size = (long)(mb * 1024 * 1024);
                return true;
            }

            return false;
        }

        // Helfer zum Formatieren von Dateigrößen
        private string FormatFileSize(long bytes)
        {
            if (bytes >= 1_073_741_824) // 1 GB
                return $"{bytes / 1_073_741_824.0:F2} GB";
            if (bytes >= 1_048_576) // 1 MB
                return $"{bytes / 1_048_576.0:F2} MB";
            return $"{bytes} Bytes";
        }








        private void ScanAllGGUFModels()
        {
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.IsReady))
            {
                try
                {
                    ScanDirectory(drive.RootDirectory.FullName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error scanning {drive.Name}: {ex.Message}");
                }
            }
        }

        private void ScanDirectory(string path)
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.gguf", SearchOption.AllDirectories))
                {
                    AddFileToList(file, "GGUF Model");
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error scanning {path}: {ex.Message}");
            }
        }


        private void AddFileToList(string filePath, string comment)
        {
            var fileInfo = new FileInfo(filePath);
            var item = new ListViewItem(Path.GetFileName(filePath));
            item.SubItems.Add(FormatFileSize(fileInfo.Length));
            item.SubItems.Add(filePath);
            item.SubItems.Add(comment);
            item.SubItems.Add("Platzhalter1");
            item.SubItems.Add("Platzhalter2");
            item.SubItems.Add("Platzhalter3");
            item.SubItems.Add("Platzhalter4");
            item.SubItems.Add("Platzhalter5");
            lvModelle.Invoke((MethodInvoker)(() => lvModelle.Items.Add(item)));
        }



        private bool IsModelFile(string path)
        {
            var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        ".bin", ".gguf", ".safetensors", ".pt", ".h5"
    };
            return extensions.Contains(Path.GetExtension(path));
        }


        private void lvModelle_DoubleClick(object sender, EventArgs e)
        {
            if (lvModelle.SelectedItems.Count > 0)
            {
                var item = lvModelle.SelectedItems[0];
                using (var editForm = new EditCommentForm(item.SubItems[3].Text, lvModelle))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        item.SubItems[3].Text = editForm.Comment;
                    }
                }
            }
        }



        private readonly string jsonPath = "modelle.json";


        private void Form1_Load(object sender, EventArgs e)
        {
            JsonModelleLaden();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            JsonModelleSpeichern();
        }


        private void JsonModelleSpeichern()
        {
            var modelle = lvModelle.Items.Cast<ListViewItem>()
                .Select(item => new ModelItem
                {
                    Values = item.SubItems.Cast<ListViewItem.ListViewSubItem>()
                        .Select(subItem => subItem.Text)
                        .ToArray()
                })
                .ToList();
            string json = JsonConvert.SerializeObject(modelle, Formatting.Indented);
            File.WriteAllText(jsonPath, json);
        }

        private void JsonModelleLaden()
        {
            if (File.Exists(jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    var modelle = JsonConvert.DeserializeObject<List<ModelItem>>(json);

                    if (modelle != null)
                    {
                        lvModelle.Items.Clear();
                        foreach (var model in modelle)
                        {
                            var item = new ListViewItem(model.Values);
                            lvModelle.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der JSON-Datei: " + ex.Message);
                }
            }
        }




        public class ModelItem
        {
            public string[] Values { get; set; } = Array.Empty<string>();
        }

        private async void btSendCommand_Click(object sender, EventArgs e)
        {
            btSendCommand.Enabled = false;

            if (lvModelle.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte ein Modell aus der Liste auswählen.");
                btSendCommand.Enabled = true;
                return;
            }





            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelName = lvModelle.SelectedItems[0].Text;
            string command = tbCommand.Text;
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string safeModelName = modelName.Replace("/", "_").Replace("\\", "_"); // Sonderzeichen ersetzen
            string outputFileName = $"{safeModelName}.generated.output.{timestamp}.jpg";
            string scriptPath = Path.Combine(appDir, "generate_image.py");
            string fullPath = Path.Combine(appDir, outputFileName);

            // Python-Skript erzeugen
            // Korrektur im Python-Skript-String (beachte die Anführungszeichen)
            string pythonScript = $@"




import sys
import re
import torch
from diffusers import (
    AutoPipelineForText2Image,
    AnimateDiffPipeline,
    MotionAdapter,
    EulerDiscreteScheduler,
    DiffusionPipeline
)
from diffusers.utils import export_to_gif
from llama_cpp import Llama
from huggingface_hub import hf_hub_download
from safetensors.torch import load_file
from transformers import AutoModelForCausalLM, AutoTokenizer

def log(message):
    print(f""[DEBUG] {{message}}\n"")

def generate_media(model_name, prompt, output_file):
    try:
        log(f""Starte Mediengenerierung mit Modell: {{model_name}}"")
        
        device = ""cuda"" if torch.cuda.is_available() else ""cpu""
        dtype = torch.float16 if device == ""cuda"" else torch.float32
        log(f""Verwende Gerät: {{device}} mit Datentyp: {{dtype}}"")
        
        if ""gguf"" in model_name.lower() or ""llama"" in model_name.lower():  # kann man hier die einzelnen if und else cases aus extra dateien laden? um es noch modularer zu gestakten?
            log(""Modus: Textmodell erkannt."")

            if ""gguf"" in model_name.lower():
                log(""Erkenne GGUF-Modell, verwende llama-cpp-python."")
                
                gguf_filename = ""em_german_7b_v01.Q4_K_M.gguf""
                model_path = hf_hub_download(model_name, filename=gguf_filename)
                log(f""Lade GGUF-Modell von: {{model_path}}"")
                
                llm = Llama(model_path=model_path, n_ctx=2048)
                log(f""Generiere Text für: '{{prompt}}'"")
                output = llm(prompt)[""choices""][0][""text""]
            else:
                log(""Erkenne Transformers-Modell, verwende AutoModelForCausalLM."")
                
                tokenizer = AutoTokenizer.from_pretrained(model_name)
                model = AutoModelForCausalLM.from_pretrained(model_name, torch_dtype=dtype).to(device)
                
                inputs = tokenizer(prompt, return_tensors=""pt"").to(device)
                output = model.generate(**inputs, max_length=500)
                output = tokenizer.decode(output[0], skip_special_tokens=True)
            
            
            output_file = re.sub(r'\.(png|jpg|jpeg|gif)$', '', output_file) + "".txt""
            
            log(f""Speichere Text: {{output_file}}"")
            with open(output_file, ""w"", encoding=""utf-8"") as f:
                f.write(output)
        
        elif ""animatediff-lightning"" in model_name.lower():
            log(""Modus: AnimateDiff-Lightning erkannt."")
            
            match = re.search(r'(\d+)step', model_name.lower())
            step = int(match.group(1)) if match else 4
            log(f""Nutze Schritte: {{step}}"")
            
            base_model = ""emilianJR/epiCRealism""
            ckpt = f""animatediff_lightning_{{step}}step_diffusers.safetensors""
            
            log(f""Lade MotionAdapter: {{ckpt}} von {{model_name}}..."")
            adapter = MotionAdapter().to(device, dtype)
            adapter.load_state_dict(load_file(
                hf_hub_download(model_name, ckpt),
                device=device
            ))
            
            log(""Initialisiere AnimateDiffPipeline..."")
            pipe = AnimateDiffPipeline.from_pretrained(
                base_model, motion_adapter=adapter, torch_dtype=dtype
            ).to(device)
            
            log(""Konfiguriere Scheduler..."")
            pipe.scheduler = EulerDiscreteScheduler.from_config(
                pipe.scheduler.config, 
                timestep_spacing=""trailing"", 
                beta_schedule=""linear""
            )
            
            log(f""Generiere Animation: '{{prompt}}'"")
            output = pipe(prompt=prompt, guidance_scale=1.0, num_inference_steps=step)
            
            if not output_file.lower().endswith("".gif""):
                output_file += "".gif""
            
            log(f""Speichere GIF: {{output_file}}"")
            export_to_gif(output.frames[0], output_file)
        
        elif model_name.lower() == ""black-forest-labs/flux.1-schnell"":
            log(""Modus: FLUX.1-schnell erkannt."")
            
            pipe = DiffusionPipeline.from_pretrained(
                ""black-forest-labs/FLUX.1-schnell"",
                torch_dtype=dtype
            ).to(device)
            
            if hasattr(pipe, ""safety_checker"") and pipe.safety_checker is not None:
                pipe.safety_checker = lambda images, **kwargs: (images, [False]*len(images))
                log(""Safety-Checker deaktiviert."")
            
            log(f""Generiere Bild: '{{prompt}}'"")
            image = pipe(prompt=prompt).images[0]
            
            if not output_file.lower().endswith(("".png"", "".jpg"", "".jpeg"")):
                output_file += "".png""
            
            log(f""Speichere Bild: {{output_file}}"")
            image.save(output_file)
        
        else:
            log(""Modus: Standard-Bildgenerierung erkannt."")
            
            pipe = AutoPipelineForText2Image.from_pretrained(
                model_name,
                torch_dtype=dtype,
                use_safetensors=True
            ).to(device)
            
            if hasattr(pipe, ""safety_checker"") and pipe.safety_checker is not None:
                pipe.safety_checker = lambda images, **kwargs: (images, [False]*len(images))
                log(""Safety-Checker deaktiviert."")
            
            log(f""Generiere Bild: '{{prompt}}'"")
            image = pipe(prompt=prompt).images[0]
            
            if not output_file.lower().endswith(("".png"", "".jpg"", "".jpeg"")):
                output_file += "".png""
            
            log(f""Speichere Bild: {{output_file}}"")
            image.save(output_file)
        
        log(""Mediengenerierung erfolgreich!"")
    
    except Exception as e:
        log(f""FEHLER: {{str(e)}}"")
        sys.exit(1)

if __name__ == ""__main__"":
    if len(sys.argv) != 4:
        print(""Verwendung: python main.py <Modellname> <Prompt> <Ausgabedatei>"")
        sys.exit(1)
    generate_media(sys.argv[1], sys.argv[2], sys.argv[3])





    



";

            File.WriteAllText(scriptPath, pythonScript, Encoding.UTF8);

            // Python-Skript ausführen
            //await RunPythonScriptAsync(scriptPath, modelName, command, fullPath);

            btSendCommand.Enabled = true;

            // Skript bereinigen
            try { File.Delete(scriptPath); }
            catch (Exception ex) { AppendDebugText($"Löschfehler: {ex.Message}"); }
        }


        // Zusätzliche Windows-API-Imports
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);

        private Process? _currentProcess; // Klassenvariable zur Prozessverfolgung









        private void AppendDebugText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                this.Invoke(new Action(() =>
                {
                    tbDebug.AppendText(text + Environment.NewLine);
                    tbDebug.ScrollToCaret();
                }));
            }
        }


        private void btKillSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentProcess == null || _currentProcess.HasExited)
                    return;

                // Stufe 1: CTRL+C mit Prozessgruppen-Isolation
                bool attachSuccess = AttachConsole((uint)_currentProcess.Id);
                AppendDebugText($"Debug: AttachConsole result: {attachSuccess} (Error: {Marshal.GetLastWin32Error()})");

                if (attachSuccess)
                {
                    try
                    {
                        // Wichtig: Prozessgruppe = PID durch CREATE_NEW_PROCESS_GROUP
                        bool ctrlSent = GenerateConsoleCtrlEvent(0 /* CTRL_C_EVENT */, (uint)_currentProcess.Id);
                        AppendDebugText($"Debug: CTRL+C gesendet: {ctrlSent} (Error: {Marshal.GetLastWin32Error()})");

                        if (_currentProcess.WaitForExit(5000)) // Kürzere Wartezeit
                        {
                            AppendDebugText("Debug: Prozess ordnungsgemäß beendet (CTRL+C)");
                            return;
                        }
                    }
                    finally
                    {
                        FreeConsole();
                    }
                }
                else
                {
                    AppendDebugText("Debug: Konnte Konsole nicht anbinden - Prozess evtl. ohne Konsole gestartet?");
                }

                // Stufe 2: Direkter Kill (Fallback)
                try
                {
                    if (!_currentProcess.HasExited)
                    {
                        AppendDebugText("Debug: Versuche gewaltsames Kill...");
                        _currentProcess.Kill();

                        if (_currentProcess.WaitForExit(1000))
                        {
                            AppendDebugText("Debug: Prozess gewaltsam beendet");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendDebugText($"Debug: Kill-Fehler: {ex.GetType().Name} - {ex.Message}");
                }

                // Stufe 3: Taskkill-Fallback (für zombie-Prozesse)
                if (!_currentProcess.HasExited)
                {
                    try
                    {
                        AppendDebugText("Debug: Starte Taskkill-Fallback...");
                        using (var killer = new Process())
                        {
                            killer.StartInfo = new ProcessStartInfo("taskkill", $"/PID {_currentProcess.Id} /T /F")
                            {
                                CreateNoWindow = true,
                                UseShellExecute = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true
                            };

                            killer.Start();
                            string output = killer.StandardOutput.ReadToEnd();
                            string error = killer.StandardError.ReadToEnd();

                            if (!killer.WaitForExit(5000))
                            {
                                AppendDebugText("Debug: Taskkill timeout");
                            }

                            AppendDebugText($"Debug: Taskkill exit code: {killer.ExitCode}");
                            AppendDebugText($"Debug: Taskkill output: {output}");
                            AppendDebugText($"Debug: Taskkill error: {error}");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendDebugText($"Debug: Taskkill-Fehler: {ex.GetType().Name} - {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendDebugText($"Debug: Kritischer Fehler: {ex.GetType().Name} - {ex.Message}");
            }
            finally
            {
                // Finaler Zustandscheck mit Null-Check
                bool exited = _currentProcess?.HasExited ?? true;
                AppendDebugText(exited
                    ? "Debug: Prozess erfolgreich beendet"
                    : "Debug: Prozess konnte nicht beendet werden!");
            }
        }






        private void btClearDebug_Click(object sender, EventArgs e)
        {
            tbDebug.Clear();
        }







        private void tbCommand_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;

            // Multiline-Modus erzwingen
            tb.Multiline = true;
            tb.WordWrap = false; // Wichtig für exakte Zeilenberechnung

            // Zeilenumbrüche korrekt erkennen (Windows: \r\n, Unix: \n)
            int lineCount = tb.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Length;

            // Dynamische Höhe mit Puffer
            int lineHeight = TextRenderer.MeasureText("Hg", tb.Font).Height;
            int newHeight = (lineCount * lineHeight) + 10; // 10px Padding

            // Maximalhöhe begrenzen
            tb.Height = Math.Min(newHeight, 200);

            // Scrollbars nur bei Überschreitung
            bool needsScroll = lineCount > (200 / lineHeight);
            tb.ScrollBars = needsScroll ? ScrollBars.Vertical : ScrollBars.None;

            // Parent-Container aktualisieren (wenn in ScrollPanel)
            tb.Parent?.PerformLayout();
        }



        // private bool isUpdating = false;

        private void tbDebug_TextChanged(object sender, EventArgs e)
        {
            //if (isUpdating) return;
            //isUpdating = true;

            //// Zerlege die Debug-Ausgabe anhand von "Debug: " aber behalte es als Prefix
            //string[] lines = tbDebug.Text
            //    .Split(new[] { "Debug: " }, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(line => "Debug: " + line.Trim()) // Wieder "Debug: " hinzufügen und trimmen
            //    .Where(line => line != "Debug:")         // Verhindert leere Debug-Zeilen
            //    .ToArray();

            //// Setze den Text neu mit Zeilenumbrüchen
            //tbDebug.Text = string.Join(Environment.NewLine, lines);

            //// Automatisches Scrollen
            //tbDebug.SelectionStart = tbDebug.Text.Length;
            //tbDebug.ScrollToCaret();

            //isUpdating = false;
        }

        private void alsBasicTestScriptAbspeichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvModelle.SelectedItems.Count > 0)
            {
                // Das erste ausgewählte ListViewItem holen
                ListViewItem selectedItem = lvModelle.SelectedItems[0];

                // Sicherstellen, dass genug SubItems vorhanden sind
                while (selectedItem.SubItems.Count <= 4)
                {
                    selectedItem.SubItems.Add(""); // Leere SubItems auffüllen
                }

                // Text aus tbDebug holen und in Zeilen aufsplitten
                string[] lines = tbDebug.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Falls Zeilen existieren, erste Zeile trimmen
                string debugText = lines.Length > 0 ? lines[0].Trim() : "";

                // Falls die erste Zeile mit "Debug:" beginnt, entfernen
                if (debugText.StartsWith("Debug:"))
                {
                    debugText = debugText.Substring(6).Trim();
                }

                // Bereinigten Text in SubItem[4] speichern
                selectedItem.SubItems[4].Text = debugText;
            }
            else
            {
                MessageBox.Show("Bitte ein Element in der Liste auswählen.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pasteClippboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                int selectionStart = tbDebug.SelectionStart;
                tbDebug.Text = tbDebug.Text.Insert(selectionStart, Clipboard.GetText());
                tbDebug.SelectionStart = selectionStart + Clipboard.GetText().Length; // Cursor nach dem eingefügten Text setzen
            }
            else
            {
                MessageBox.Show("Die Zwischenablage enthält keinen Text.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btStartBasicScript_Click(object sender, EventArgs e)
        {
            //Hier soll er das basic script in subitem [4] des gewählten models ausführen



            //            from diffusers import DiffusionPipeline

            //pipe = DiffusionPipeline.from_pretrained("black-forest-labs/FLUX.1-schnell")

            //prompt = "Astronaut in a jungle, cold color palette, muted colors, detailed, 8k"
            //image = pipe(prompt).images[0]





        }











        private async Task RunPythonScriptAsync(
        string scriptPath,
        Dictionary<string, object> parameters)
        {


            // 2. ProcessStartInfo mit generiertem Argument-String
            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = BuildPythonArguments(scriptPath, parameters),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            };





            using (_currentProcess = new Process { StartInfo = psi })
            {
                try
                {
                    // Eventhandler für Output/Error registrieren
                    _currentProcess.OutputDataReceived += (s, e) => AppendDebugText(e.Data ?? string.Empty);
                    _currentProcess.ErrorDataReceived += (s, e) => AppendDebugText($"Debug: {e.Data}");

                    _currentProcess.Start();

                    // Asynchrone Ausgabe-Überwachung starten
                    _currentProcess.BeginOutputReadLine();
                    _currentProcess.BeginErrorReadLine();

                    await _currentProcess.WaitForExitAsync();
                }
                finally
                {
                    _currentProcess.Close();
                    _currentProcess = null;
                }
            }
        }







        // Service-Instanz als Klassenfeld
        private readonly ModelParameterService _paramService = new ModelParameterService();



        private async void btSendenNeu_Click(object sender, EventArgs e)
        {
            btSendenNeu.Enabled = false;
            lvModelle.Enabled = false;
            tbDebug.Clear();
            DateTime startTime = DateTime.Now;

            try
            {
                // 1. Basisparameter vorbereiten
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string scriptPath = Path.Combine(appDir, "main.py");
                ListViewItem item = lvModelle.SelectedItems[0];
                string modelName = item.Text;

                // 2. Output-Pfad generieren
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string safeModelName = modelName.Replace("/", "_").Replace("\\", "_");
                string outputFileName = $"{safeModelName}.generated.output.{timestamp}";
                string fullPath = Path.Combine(appDir, outputFileName);

                // 3. Parameter-Service verwenden

                var parameters = await _paramService.BuildParametersAsync(
 modelName: modelName,
 command: tbCommand.Text,
 outputPath: fullPath,
 customParameters: new
 {
     // UI-Parameter für Precision
     precision = cbPrecision.SelectedItem?.ToString() ?? "fp16", // Neu hinzugefügt

     // Bestehende Parameter
     voice = cbVoice.SelectedItem?.ToString() ?? "default",
     speed = nudSpeed.Value,
     pitch = nudPitch.Value,
     basemodel = cbBaseModel.Text,
     device = cbDevice.Text,
     seed = GetNudValueAsInt(nudSeed),
     guidance_scale = GetNudValueAsInt(nudGuidanceScale),
     steps = GetNudValueAsInt(nudNumSteps)
 });








                // 4. Status-Update (angepasst an Dictionary)
                tbStatus.Text = $@"Running Python script:
Script: {scriptPath}
Model: {parameters["model"]}
Prompt: {parameters["prompt"]}
Base Model: {parameters.GetValueOrDefault("--basemodel", "")}
Device: {parameters.GetValueOrDefault("--device", "")}
Output: {parameters["output"]}";

                // 5. Python-Skript ausführen
                await RunPythonScriptAsync(scriptPath, parameters);

                // 6. Ergebnisverarbeitung (unverändert)
                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - startTime;
                item.SubItems[4].Text = duration.ToString(@"hh\:mm\:ss");

                bool resultSuccess = CheckScriptSuccess(tbDebug.Text);
                item.SubItems[3].Text = resultSuccess
                    ? "Fertig! Funktioniert."
                    : "Hier muss noch etwas Arbeit reingesteckt werden!";

                item.SubItems[5].Text = tbDebug.Text;
                string outputFilePath = ExtractFilePathFromDebug(tbDebug.Text);

                if (File.Exists(outputFilePath))
                {
                    DynamischeFormen.CreatePreviewForm(outputFilePath, duration);
                }
                else
                {
                    MessageBox.Show("Die Datei konnte nicht gefunden werden.",
                                  "Fehler",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                tbDebug.Text = $"Kritischer Fehler: {ex.Message}";
            }
            finally
            {
                lvModelle.Enabled = true;
                btSendenNeu.Enabled = true;
            }
        }





        private int? GetNudValueAsInt(NumericUpDown nud)
        {
            return nud.Value >= nud.Minimum && nud.Value <= nud.Maximum
                ? (int)nud.Value
                : null;
        }

        private string ExtractFilePathFromDebug(string debugText)
        {
            // Den Teil des Texts extrahieren, der den Dateipfad enthält
            string pattern = @"\[AUSGABE\] Datei: (.*\.png)"; // Suche nach "Datei: <Pfad>"
            var match = Regex.Match(debugText, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value; // Den extrahierten Dateipfad zurückgeben
            }
            return string.Empty; // Falls kein Dateipfad gefunden wurde
        }



        private bool CheckScriptSuccess(string debugText)
        {
            // Suche nach bestimmten Schlüsselwörtern, die auf Erfolg oder Fehler hinweisen
            if (debugText.Contains("Erfolgreich gespeichert") || debugText.Contains("[ERFOLG] Generierung abgeschlossen"))
            {
                return true; // Erfolg
            }

            return false; // Fehler
        }




        private void btFormatListe_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvModelle.Items)
            {
                // Prüfen, ob Subitem 7 oder 8 existieren
                if (item.SubItems.Count <= 7)  // Falls Subitem[7] nicht existiert
                {
                    item.SubItems.Add("todo 4"); // Subitem 7 hinzufügen
                }
                if (item.SubItems.Count <= 8)  // Falls Subitem[8] nicht existiert
                {
                    item.SubItems.Add("todo 5"); // Subitem 8 hinzufügen
                }
            }
        }

        private async void btnGenerateModelInfo_Click(object sender, EventArgs e)
        {
            // Initialisiere den HTTP-Client
            using (HttpClient client = new HttpClient())
            {
                // Base-URL für Hugging Face API
                client.BaseAddress = new Uri("https://huggingface.co/api/models/");

                // Definiere den Pfad zum Ordner für model_infos
                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string modelInfoDir = Path.Combine(appDir, "model_infos");

                // Stelle sicher, dass der Ordner existiert
                if (!Directory.Exists(modelInfoDir))
                {
                    Directory.CreateDirectory(modelInfoDir);
                    tbStatus.AppendText("Ordner 'model_infos' wurde erstellt.\r\n");
                }

                foreach (ListViewItem item in lvModelle.Items)
                {
                    string repoId = item.SubItems[0].Text;

                    try
                    {
                        // API Request, um Modellinformationen zu erhalten
                        tbStatus.AppendText($"Starte API-Anfrage für {repoId}...\r\n");

                        HttpResponseMessage response = await client.GetAsync(repoId);
                        if (response.IsSuccessStatusCode)
                        {
                            tbStatus.AppendText($"Daten für {repoId} erfolgreich abgerufen.\r\n");

                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            dynamic? modelInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);

                            if (modelInfo != null)
                            {
                                // Extrahiere relevante Modelldaten
                                string modelName = modelInfo.modelId ?? "Unbekannt";
                                string description = modelInfo.description ?? "Keine Beschreibung vorhanden.";
                                string license = modelInfo.license ?? "Keine Lizenz angegeben.";
                                string downloads = modelInfo.downloads?.ToString() ?? "Keine Download-Infos.";
                                string likes = modelInfo.likes?.ToString() ?? "Keine Likes";
                                string lastModified = modelInfo.lastModified ?? "Keine Daten zur letzten Änderung.";

                                // Extrahiere die Pipeline- und Task-Informationen
                                string taskType = modelInfo.pipeline_tag ?? "Unbekannt";
                                string taskDescription = "Nicht spezifiziert";
                                if (taskType.Contains("text-to-image"))
                                {
                                    taskDescription = "Text-to-Image Modell";
                                }
                                else if (taskType.Contains("text-to-speech"))
                                {
                                    taskDescription = "Text-to-Speech Modell";
                                }
                                else if (taskType.Contains("text-to-animation"))
                                {
                                    taskDescription = "Text-to-Animation Modell";
                                }

                                // Extrahiere Tags (falls vorhanden)
                                string tags = modelInfo.tags != null ? string.Join(", ", modelInfo.tags) : "Keine Tags vorhanden.";

                                // HTML-Inhalt erstellen
                                string htmlContent = $@"
        <html>
        <head>
            <title>{modelName} Model Information</title>
        </head>
        <body>
            <h1>Model: {modelName}</h1>
            <p><strong>Repo ID:</strong> {repoId}</p>
            <p><strong>Description:</strong> {description}</p>
            <p><strong>License:</strong> {license}</p>
            <p><strong>Downloads:</strong> {downloads}</p>
            <p><strong>Likes:</strong> {likes}</p>
            <p><strong>Last Modified:</strong> {lastModified}</p>
            <p><strong>Tags:</strong> {tags}</p>
            <p><strong>Task Type:</strong> {taskDescription} ({taskType})</p>
        </body>
        </html>";

                                // Setze den HTML-Inhalt in subitem 6 (anstatt ein neues hinzuzufügen)
                                item.SubItems[6].Text = htmlContent;

                                // Speichere als HTML-Datei im model_infos-Ordner
                                string htmlFilePath = Path.Combine(modelInfoDir, $"{repoId}_model_info.html");
                                File.WriteAllText(htmlFilePath, htmlContent);

                                tbStatus.AppendText($"HTML-Datei für {repoId} gespeichert unter {htmlFilePath}.\r\n");
                            }
                            else
                            {
                                tbStatus.AppendText($"Fehler beim Verarbeiten der Daten für {repoId}.\r\n");
                            }
                        }
                        else
                        {
                            tbStatus.AppendText($"Fehler beim Abrufen der Daten für {repoId}.\r\n");
                        }

                        // Füge eine Pause hinzu, um API-Überflutung zu vermeiden
                        await Task.Delay(2000); // 2 Sekunden Pause zwischen den Anfragen
                    }
                    catch (Exception ex)
                    {
                        tbStatus.AppendText($"Fehler bei der Anfrage für {repoId}: {ex.Message}\r\n");
                    }
                }
            }

            tbStatus.AppendText("Model-Infos wurden erfolgreich erstellt!\r\n");
        }



        private void btHtmlReportAnzeigen_Click(object sender, EventArgs e)
        {
            // Überprüfen, ob ein Item in der ListView ausgewählt ist
            if (lvModelle.SelectedItems.Count > 0)
            {
                // Das ausgewählte ListViewItem und das SubItem 6 holen
                string htmlContent = lvModelle.SelectedItems[0].SubItems[6].Text;

                // Überprüfen, ob der HTML-Text nicht leer ist
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    // Erstelle eine temporäre HTML-Datei
                    string tempHtmlFile = Path.Combine(Path.GetTempPath(), "tempModelReport.html");
                    File.WriteAllText(tempHtmlFile, htmlContent);

                    // Öffne die HTML-Datei im Standardbrowser
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = tempHtmlFile,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("Kein HTML-Inhalt im SubItem 6 vorhanden.");
                }
            }
            else
            {
                MessageBox.Show("Kein ListView-Item ausgewählt.");
            }
        }










        // NEUE METHODE: Argumente-Generator
        private string BuildPythonArguments(string scriptPath, Dictionary<string, object> parameters)
        {
            var args = new StringBuilder($"\"{scriptPath}\"");

            // Positionsparameter in fester Reihenfolge
            string[] positionalParams = { "model", "prompt", "output" };
            foreach (var param in positionalParams)
            {
                if (parameters.TryGetValue(param, out var value))
                {
                    args.Append($" \"{value}\"");
                }
            }

            // Benannte Parameter
            foreach (var kvp in parameters)
            {
                if (kvp.Key.StartsWith("--"))
                {
                    var val = kvp.Value switch
                    {
                        bool b => b ? $" {kvp.Key}" : "",
                        _ => $" {kvp.Key} \"{kvp.Value}\""
                    };
                    args.Append(val);
                }
            }

            return args.ToString();
        }


























        // Service-Klasse für Modelldaten
        public class ModelParameterService
        {
            private readonly Dictionary<string, ModelType> _modelTypeCache = new();
            private readonly HttpClient _httpClient = new HttpClient();

            public async Task<Dictionary<string, object>> BuildParametersAsync(
                string modelName,
                string command,
                string outputPath,
                dynamic customParameters)
            {
                var parameters = new Dictionary<string, object>
                {
                    ["model"] = modelName,
                    ["prompt"] = command,
                    ["output"] = outputPath
                };

                // 1. Modelltyp bestimmen (direkter Aufruf der eigenen Methode)
                var modelType = await DetermineModelTypeAsync(modelName);

                // 2. Gerät automatisch bestimmen
                parameters["--device"] = await DetermineOptimalDevice(modelType);

                // 3. Modellspezifische Parameter hinzufügen
                switch (modelType)
                {
                    case ModelType.TextToImage:
                        AddDiffusionParameters(parameters, customParameters);
                        break;
                    case ModelType.TextToSpeech:
                        AddTTSParameters(parameters, customParameters);
                        break;
                    case ModelType.TextToVideo:
                        AddAnimationParameters(parameters, customParameters);
                        break;
                    case ModelType.TextToText:  // Neuer Fall
                        AddTextToTextParameters(parameters, customParameters);
                        break;
                    default:
                        AddDefaultParameters(parameters);
                        break;
                }

                return parameters;
            }




            private async Task<ModelType> DetermineModelTypeAsync(string modelName)
            {
                if (_modelTypeCache.TryGetValue(modelName, out var cachedType))
                    return cachedType;
                try
                {
                    var response = await _httpClient.GetAsync(
                        $"https://huggingface.co/api/models/{modelName}");
                    var content = await response.Content.ReadAsStringAsync();
                    var modelData = JsonConvert.DeserializeObject<HuggingFaceModelData>(content);
                    var type = modelData?.Tags switch
                    {
                        var t when t?.Contains("diffusers") == true => ModelType.TextToImage,
                        var t when t?.Contains("text-to-speech") == true => ModelType.TextToSpeech,
                        var t when t?.Contains("animation") == true => ModelType.TextToVideo,
                        _ => ModelType.Unknown
                    };
                    _modelTypeCache[modelName] = type;
                    return type;
                }
                catch
                {
                    return ModelType.Unknown;
                }
            }






            private async Task<string> DetermineOptimalDevice(ModelType modelType)
            {
                var cudaInfo = await GetCudaInfoAsync();
                return modelType switch
                {
                    ModelType.TextToImage when cudaInfo.IsAvailable => "cuda",
                    ModelType.TextToVideo when cudaInfo.IsAvailable => "cuda",
                    _ => "cpu"
                };
            }

            private void AddDiffusionParameters(
                IDictionary<string, object> parameters,
                dynamic custom)
            {

                // Füge --basemodel hinzu, falls vorhanden
                if (custom.basemodel != null)
                {
                    parameters["--basemodel"] = custom.basemodel;
                }

                parameters["--guidance_scale"] = custom.guidance_scale != null
                    ? Math.Round((decimal)custom.guidance_scale, 1)
                    : 7.0m; // Standardwert
            }


            private void AddTTSParameters(
            IDictionary<string, object> parameters,
            dynamic custom)
            {
                // Voice-Parameter mit Default-Werten
                parameters["--voice"] = custom.voice ?? "default";  // default, male, female etc.
                parameters["--speed"] = custom.speed ?? 1.0;       // 0.5-2.0
                parameters["--pitch"] = custom.pitch ?? 0;          // -20 bis +20

                // Device-Parameter
                parameters["--device"] = custom.device ?? "auto";
                parameters["--precision"] = custom.precision ?? "fp16";
            }

            private void AddAnimationParameters(
                IDictionary<string, object> parameters,
                dynamic custom)
            {
                // Für Video/Animation typische Parameter
                parameters["--fps"] = custom.fps ?? 24;
                parameters["--duration"] = custom.duration ?? 5.0;
                parameters["--resolution"] = $"{custom.width}x{custom.height}";
            }

            private void AddTextToTextParameters(
            IDictionary<string, object> parameters,
            dynamic custom)
            {
                // Typische Parameter für Textgenerierung
                parameters["--max_tokens"] = custom.max_tokens ?? 100;
                parameters["--temperature"] = custom.temperature ?? 0.7;
                parameters["--top_p"] = custom.top_p ?? 0.9;
                parameters["--frequency_penalty"] = custom.frequency_penalty ?? 0;
                parameters["--presence_penalty"] = custom.presence_penalty ?? 0;

                // Stop-Sequenzen als kommagetrennte Liste
                parameters["--stop"] = custom.stop != null
                    ? string.Join(",", custom.stop)
                    : "";

                // Optional: Streaming für Token-Streams
                parameters["--stream"] = custom.stream ?? false;
            }



            private void AddDefaultParameters(
                IDictionary<string, object> parameters)
            {
                // Sinnvolle Defaults statt Platzhaltern
                parameters["--max_length"] = 512;
                parameters["--temperature"] = 0.7;
            }





            private async Task<CudaInfo> GetCudaInfoAsync()
            {
                try
                {
                    var isAvailable = await CheckCudaAvailabilityAsync();
                    var vramSize = isAvailable ? await GetVramSizeAsync() : 0;
                    return new CudaInfo
                    {
                        IsAvailable = isAvailable,
                        VRamGB = vramSize
                    };
                }
                catch
                {
                    return new CudaInfo { IsAvailable = false, VRamGB = 0 };
                }
            }

            private async Task<bool> CheckCudaAvailabilityAsync()
            {
                string? tempScript = null;
                try
                {
                    // Erstelle temporäres Python-Skript
                    tempScript = Path.Combine(Path.GetTempPath(), "check_cuda.py");
                    await File.WriteAllTextAsync(tempScript,
                        "import torch\nprint(torch.cuda.is_available())");
                    // Konfiguriere Prozess-Start
                    var psi = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = tempScript,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    // Starte und verarbeite den Prozess
                    using var process = new Process { StartInfo = psi };
                    process.Start();
                    var output = await process.StandardOutput.ReadToEndAsync();
                    await process.WaitForExitAsync();
                    return output.Trim() == "True";
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (File.Exists(tempScript)) File.Delete(tempScript);
                }
            }

            private async Task<decimal> GetVramSizeAsync()
            {
                string? tempScript = null;
                try
                {
                    // Erstelle temporäres Python-Skript
                    tempScript = Path.Combine(Path.GetTempPath(), "get_vram.py");
                    await File.WriteAllTextAsync(tempScript,
                        "import torch\nprint(torch.cuda.get_device_properties(0).total_memory / 1024**3)");
                    // Konfiguriere Prozess-Start
                    var psi = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = tempScript,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    // Starte und verarbeite den Prozess
                    using var process = new Process { StartInfo = psi };
                    process.Start();
                    var output = await process.StandardOutput.ReadToEndAsync();
                    await process.WaitForExitAsync();
                    return decimal.Parse(output.Trim(), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return 0;
                }
                finally
                {
                    if (File.Exists(tempScript)) File.Delete(tempScript);
                }
            }


        }









        private async Task<string> ExecutePythonScript(string scriptPath)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            await process.WaitForExitAsync();

            return output;
        }










        // Hilfsklassen
        public enum ModelType
        {
            TextToImage,
            TextToSpeech,
            TextToVideo,
            TextToText,
            TextGeneration,
            ImageClassification,
            Unknown
        }

        private class HuggingFaceModelData
        {
            public List<string> Tags { get; set; } = new List<string>();
            public string PipelineTag { get; set; } = string.Empty;
            public Dictionary<string, object> CardData { get; set; } = new Dictionary<string, object>();















        }

        internal class CudaInfo
        {
            public bool IsAvailable { get; set; }
            public decimal VRamGB { get; set; }
        }

        private void lvModelle_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Sortiere die ListView nach der angeklickten Spalte
            lvModelle.ListViewItemSorter = new ListViewItemComparer(e.Column);
            lvModelle.Sort();  //Er muss auf und absteigend sortiern können
        }








    }
}





