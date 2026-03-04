<div align="center">

# 🦜 Parrotnest V10
**Modern Real‑Time Chat & Collaboration Platform**

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-SignalR-5C2D91?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/DB-SQLite-blue?style=for-the-badge&logo=sqlite)
![Status](https://img.shields.io/badge/Version-V10.0_Zajebiste_Edition-brightgreen?style=for-the-badge)

</div>

---

## ✨ Overview

**Parrotnest** to nowoczesna platforma komunikacyjna stworzona z myślą o społecznościach szkolnych, grupach znajomych i lokalnych sieciach LAN. Wersja **V10 "Zajebiste Edition"** to potężny skok jakościowy, wprowadzający zaawansowaną dostępność, nowe motywy oraz... ukryte gniazda pełne niespodzianek. 🦜✨

System składa się z:
- 🖥 **Windows Desktop Host** – powłoka WinForms zarządzająca serwerem.
- 🚀 **ASP.NET Core API** – szybki backend z SignalR do komunikacji w czasie rzeczywistym.
- 🌐 **Web Client** – nowoczesny interfejs PHP + Vanilla JS z pełną responsywnością.
- 📦 **Desktop Installer** – dedykowany instalator dla łatwego wdrożenia.

---

## 🚀 Kluczowe Funkcje

### 💬 Komunikacja
- **Kanał Ogólny** – globalna przestrzeń dla wszystkich użytkowników.
- **Wiadomości Prywatne** – bezpieczny czat 1-na-1.
- **Grupy** – twórz własne pokoje, zapraszaj znajomych i zarządzaj członkami.
- **Bogate wiadomości** – obsługa obrazów, filmów wideo, odpowiedzi (replies) oraz reakcji emoji.

### 🎨 Personalizacja & UI
- **Wiele motywów** – wybierz spośród: *Original, Dark, Classic, Neon, Forest* lub *Kontrast*.
- **Dynamiczna wielkość tekstu** – od małego po X-Duży.
- **Tryb prostego tekstu** – dla tych, którzy cenią minimalizm.
- **Powiadomienia** – pełna kontrola nad głośnością i wybór własnych dźwięków (`1.mp3`, `2.mp3`, `3.mp3`).

### 🛡 Administracja
- Pełny panel zarządzania użytkownikami (banowanie, wyciszanie, usuwanie kont).
- Logi akcji administracyjnych – pełna transparentność działań.
- Narzędzia diagnostyczne API.

---

## 🌈 Dostępność (Accessibility)
Parrotnest V10 stawia na inkluzywność. Nowy panel dostępności oferuje:
- **Filtry dla daltonistów** – Protanopia, Deuteranopia, Tritanopia oraz Achromatopsja.
- **Regulacja kontrastu** – od 50% do 200%.
- **Skalowanie czcionek** – dla lepszej czytelności.
- **Kontrola animacji** – możliwość wyłączenia zbędnych efektów wizualnych.

---

## 🎁 Easter Eggs & Secret Menu
Odkryj tajne funkcje ukryte w "Gnieździe Papugi":
- 🌈 **Rainbow Mode** – poczuj magię kolorów w całym interfejsie.
- 🌀 **Spin Mode** – zakręć swoimi znajomymi (dosłownie!).
- 🔥 **HELL MODE** – uruchamia wbudowaną wersję legendarnego **DOOM** bezpośrednio w przeglądarce.
- 🐻 **FNAF 1 Launcher** – potrzebujesz przerwy? Uruchom Five Nights at Freddy's 1 z poziomu czatu.
- 🦜 **Logo Parrot** – kliknij logo, aby usłyszeć głos naszej maskotki!

---

## 🛠 Stack Technologiczny

- **Backend:** .NET 10.0, ASP.NET Core, SignalR, Entity Framework Core (SQLite).
- **Frontend:** PHP, Vanilla JavaScript, CSS (częściowo generowany przez `CssGen`).
- **Narzędzia:**
  - `CssGen` – autorskie narzędzie w C# do generowania responsywnych arkuszy stylów.
  - `ParrotnestDesktopInstaller` – dedykowana aplikacja do instalacji klienta.
  - `DosBox integration` – dla trybu Hell Mode (Doom).

---

## 📂 Struktura Projektu

```text
Parrotnest/
├─ Client/                  # Frontend (PHP, JS, CSS, Assets)
│  ├─ DOOM/                 # Wbudowany Doom (Hell Mode)
│  ├─ UrUrUr/               # FNAF 1 Launcher
│  ├─ CssGen/               # Generator CSS (C#)
│  ├─ notificationsounds/   # Customowe dźwięki powiadomień
│  └─ uploads/              # Pliki użytkowników (avatary, obrazy, wideo)
├─ Server/                  # Backend C# (ASP.NET Core + WinForms)
│  ├─ Controllers/          # Logika API
│  ├─ Hubs/                 # Komunikacja SignalR
│  └─ Models/               # Modele bazy danych EF Core
└─ ParrotnestDesktopInstaller/ # Projekt instalatora desktopowego
```

---

## ⚙️ Uruchomienie (Development)

1. **Wymagania:** Windows 10/11, .NET SDK (net10.0).
2. **Build:** 
   ```bash
   dotnet build .\Server\ParrotnestServer.csproj -c Debug
   ```
3. **Start:** Uruchom `ParrotnestServer.exe`.
   - Kliknij **"Start Server"**, aby odpalić backend.
   - Kliknij **"Otwórz App"**, aby przejść do panelu logowania.

---

## 👑 JGS Team (Creators)

Projekt rozwijany z pasją (i lekkim brakiem snu) przez zespół **JGS**:

- 👨‍💻 **Adam Hnatko ("Hnato")** – [STATUS: URLOPIK] Główny architekt, który napracował się na święta i teraz zasłużenie odpoczywa.
- 🛠 **Igor Kondraciuk ("Flubi3604")** – [STATUS: RIPDB] Mistrz bazy danych pracujący na 25-godzinne zmiany (nawet jeśli baza czasem stawia opór).
- 🐍 **Jakub Fedorowicz ("John0G1thub")** – [STATUS: FUCKOFF] Człowiek od zadań specjalnych, który nie bierze odpowiedzialności za zakrzywienie czasu przez resztę zespołu.

---
<div align="center">
Built with ❤️ for classrooms and small communities.  
&copy; 2026 Parrotnest - JGS Team.
</div>
