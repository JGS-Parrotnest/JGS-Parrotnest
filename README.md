<div align="center">

# 🦜 Parrotnest V11
**Modern Real-Time Chat & Collaboration Platform**

![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-SignalR-5C2D91?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/DB-SQLite-blue?style=for-the-badge&logo=sqlite)
![Status](https://img.shields.io/badge/Version-V11.0_Panther_Lake_Edition-brightgreen?style=for-the-badge)

</div>

---

## ✨ Overview

**Parrotnest** is a modern communication platform designed for school communities, groups of friends, and local LAN networks.  
Version **V11 "Panther Lake Edition"** represents a major quality leap, introducing advanced accessibility features, new themes, and... hidden nests full of surprises. 🦜✨

The system consists of:

- 🖥 **Windows Desktop Host** – a WinForms shell responsible for managing the server.
- 🚀 **ASP.NET Core API** – a fast backend powered by SignalR for real-time communication.
- 🌐 **Web Client** – a modern PHP + Vanilla JS interface with full responsiveness.
- 📦 **Desktop Installer** – a dedicated installer for easy deployment.

---

## 🚀 Key Features

### 💬 Communication
- **Global Channel** – a shared space for all users.
- **Private Messages** – secure 1-on-1 chat.
- **Groups** – create custom rooms, invite friends, and manage members.
- **Rich Messages** – support for images, videos, replies, and emoji reactions.

### 🎨 Personalization & UI
- **Multiple Themes** – choose from: *Original, Dark, Classic, Neon, Forest,* or *High Contrast*.
- **Dynamic Text Size** – from small to Extra Large.
- **Plain Text Mode** – for users who prefer minimalism.
- **Notifications** – full volume control and customizable sounds (`1.mp3`, `2.mp3`, `3.mp3`).

### 🛡 Administration
- Full user management panel (ban, mute, delete accounts).
- Administrative action logs – complete operational transparency.
- API diagnostic tools.

---

## 🦮 Accessibility

Parrotnest V11 focuses on inclusivity. The new accessibility panel offers:

- **Colorblind Filters** – Protanopia, Deuteranopia, Tritanopia, and Achromatopsia.
- **Contrast Adjustment** – from 50% to 200%.
- **Font Scaling** – for improved readability.
- **Animation Control** – option to disable unnecessary visual effects.

---

## 🎁 Easter Eggs & Secret Menu

Discover hidden features inside the "Parrot’s Nest":

- 🌈 **Rainbow Mode** – experience colorful magic across the entire interface.
- 🌀 **Spin Mode** – spin your friends around (literally!).
- 🔥 **HELL MODE** – launches a built-in version of the legendary **DOOM** directly in your browser.
- 🐻 **FNAF 1 Launcher** – need a break? Launch Five Nights at Freddy's 1 directly from the chat.
- 🦜 **Parrot Logo** – click the logo to hear the voice of our mascot!

---

## 🛠 Technology Stack

- **Backend:** .NET 10.0, ASP.NET Core, SignalR, Entity Framework Core (SQLite).
- **Frontend:** PHP, Vanilla JavaScript, CSS (partially generated using `CssGen`).
- **Tools:**
  - `CssGen` – a custom C# tool for generating responsive stylesheets (author: Hnato).
  - `ParrotnestDesktopInstaller` – dedicated desktop client installer.
  - `DosBox integration` – powers Hell Mode (Doom).

---

## 📂 Project Structure

```text
Parrotnest/
├─ Client/                  # Frontend (PHP, JS, CSS, Assets)
│  ├─ APP/                  # Crazy desktop app thingymabom/thingamajig
│  ├─ DOOM/                 # Embedded Doom (Hell Mode)
│  ├─ UrUrUr/               # FNAF 1 Launcher
│  ├─ CssGen/               # CSS Generator (C#)
│  ├─ notificationsounds/   # Custom notification sounds
│  └─ uploads/              # User files (avatars, images, videos)
├─ Server/                  # Backend C# (ASP.NET Core + WinForms)
│  ├─ Controllers/          # API logic
│  ├─ Hubs/                 # SignalR communication
│  └─ Models/               # EF Core database models
└─ ParrotnestDesktopInstaller/ # Desktop installer project
```

---

## ⚙️ Running (Development)

1. **Requirements:** Windows 10/11, .NET SDK (net10.0).
2. **Build:** 
   ```bash
   dotnet build .\Server\ParrotnestServer.csproj -c Debug
   ```
3. **Start:** Run `ParrotnestServer.exe`.
   - Click **"Start Server"** to launch the backend.
   - Click **"Open App"** to open the login panel.

---

## 👑 JGS Team (Creators)

The project is passionately developed by the **JGS Team**:

- 👨‍💻 Adam Hnatko ("Hnato")
- 🛠 Igor Kondraciuk ("Flubi3604")
- 🐍 Jakub Fedorowicz ("John0G1thub")

---

<div align="center"> 
&copy; 2026 Parrotnest - JGS Team.
</div>
