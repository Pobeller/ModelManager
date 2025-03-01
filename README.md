Model Manager Alpha

Willkommen zu Model Manager Alpha 🔥

Vielen Dank, dass du hier gelandet bist! Dieses Projekt ist die erste Alpha-Version eines Frontends für die einfache Verwaltung und Ausführung von KI-Modellen in Kombination mit dem Modulrunner v2.1.

Das Tool soll die Interaktion mit Hugging Face Modellen vereinfachen, Downloads automatisieren und die volle Kontrolle über Text-zu-Bild, Bildbearbeitung und andere multimodale Pipelines bieten.

Was ist der Model Manager?

Der Model Manager Alpha ist ein C#-Frontend mit direkter Anbindung an den Modulrunner v2.1. Ziel ist es, eine einfache Benutzeroberfläche zu bieten, die alle Schritte – von der Modellauswahl bis zur Ausgabe – übersichtlich zusammenfasst.

Die Verbindung zwischen Frontend und Backend erfolgt über ein lokales API-System, bei dem der Modulrunner als Python-Backend sämtliche Rechenaufgaben übernimmt.

Features

✅ Automatische Auflistung aller im Hugging Face Cache gespeicherten Modelle
✅ Download-Funktion für Modelle direkt über die Hugging Face API
✅ Starten von Modellen mit wenigen Klicks
✅ Dynamische kwargs-Übergabe aus der GUI an den Modulrunner
✅ Ausgabe von Lognachrichten direkt im Interface
✅ Filterung von Modellen nach Typ (Text-zu-Bild, LoRA, Basismodelle)
✅ Device-Erkennung (CUDA oder CPU)
✅ Minimalistisches WinForms-UI für maximale Funktionalität ohne Schnickschnack


---

So funktioniert's

Voraussetzungen

Windows 10

.NET Framework 4.8 oder höher

CUDA-fähige GPU + aktuelle Treiber (optional, aber dringend empfohlen)

Modulrunner v2.1 (Python)


Installation

1. Modulrunner v2.1 herunterladen und konfigurieren


2. Model Manager Alpha Release herunterladen (kommt noch 😏)


3. appsettings.json im Model Manager anpassen (Pfad zu Python + Modulrunner)


4. Starten und loslegen!




---

Beispiel-Workflow

1. Modell auswählen (z.B. alvdansen/littletinies)


2. Basismodell setzen (z.B. fluently/Fluently-XL-v2)


3. Prompt eingeben


4. Kwargs-Optionen auswählen (z.B. Sampler, Steps, Seed)


5. RUN drücken


6. Fertiges Bild wird automatisch im Output-Ordner gespeichert




---

Beispiel-Logausgabe

========================================
=== Model Manager Alpha ===============
========================================

[INFO] Starte Backend-API...
[INFO] Modulrunner Version: 2.1
[SYSTEM] CUDA gefunden: Ja
[MODEL] Cache: 7 Modelle gefunden
[DOWNLOAD] Start: alvdansen/littletinies
[DOWNLOAD] OK: alvdansen/littletinies (245 MB)
[RUN] Prompt: "Kaffeeautomat auf dem Mars"
[RUN] Basemodel: fluently/Fluently-XL-v2
[OUTPUT] Datei gespeichert: output_kaffee.png


---

Warum?

Weil KI-Tools keine Rocket Science sein sollten – sondern einfach nur Spaß machen!


---

ToDo

GUI-Redesign mit modernem Look (WPF oder Avalonia?)

Multi-Download-Manager

Automatische LoRA-Erkennung

Bild-Viewer direkt in der App

Discord-Bot-Anbindung

Rust-Map-Generierung mit Diffusion-Modellen (ja, das ist ernst gemeint 🤓)



---

Mitmachen?

Du hast eine Idee, möchtest was beitragen oder einfach nur mal "Moin" sagen?
Dann schreib mich an – egal ob hier, auf GitHub oder in irgendeiner Telegram-Welt.


---

Bleibt neugierig!
KI ist nur so schlau, wie wir sie machen.

Alex (aka Brain2k12)


---

"Wenn du dich fragst, ob die KI irgendwann deinen Job macht... dann mach dir lieber Gedanken, warum du nicht gerade ihren machst." 😉
