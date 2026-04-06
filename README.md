<div align="center">

# 🦜 Parrotnest V12
**Modern Real-Time Chat & Collaboration Platform**

![.NET](https://img.shields.io/badge/.NET-11.0-blueviolet?style=for-the-badge&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-SignalR-5C2D91?style=for-the-badge&logo=dotnet)
![SQLite](https://img.shields.io/badge/DB-SQLite-blue?style=for-the-badge&logo=sqlite)
![Status](https://img.shields.io/badge/Version-V12.0_Parrot_Cove_Edition-brightgreen?style=for-the-badge)

</div>

---

## ✨ Overview

**Parrotnest** is a modern communication platform designed for school communities, groups of friends, and local LAN networks.  
Version **V12 "Parrot Cove Edition"** represents a major technological leap, migrating to **.NET 11** and **C# 15**, and introducing highly requested features like message editing. 🦜🌊

The system consists of:

- 🖥 **Windows Desktop Host** – a WinForms shell responsible for managing the server.
- 🚀 **ASP.NET Core API** – a fast backend powered by SignalR for real-time communication.
- 🌐 **Web Client** – a modern PHP + Vanilla JS interface with full responsiveness.
- 📦 **Desktop Installer** – a dedicated installer for easy deployment.

---

## 🆕 What's New in V12 (Changelog)

### 🚀 Technology Migration
- **.NET 11 Update:** Migrated all projects to the latest .NET 11 framework for improved performance and security.
- **C# 15 Support:** Refactored codebase to utilize modern C# 15 features, including primary constructors and enhanced params collections.
- **SDK Pinning:** Added `global.json` to ensure consistent build environments.

### 💬 Chat Improvements
- **Message Editing:** Users can now edit their own messages within 15 minutes of sending.
- **Edit History:** Full transparency with stored edit history and "edited" labels.
- **Security:** Enhanced XSS protection and SQL injection prevention in message processing.

### 🛠 Fixes & Features
- **Fixed Installer Download:** Resolved a critical issue preventing the download of the desktop client.
- **New App Icon:** Refreshed application icon with proper resource embedding across all platforms.
- **Enhanced Code Analysis:** Full build with enabled .NET analyzers to ensure code quality.

---

## 🚀 Key Features

### 💬 Communication
- **Global Channel** – a shared space for all users.
- **Private Messages** – secure 1-on-1 chat.
- **Groups** – create custom rooms, invite friends, and manage members.
- **Rich Messages** – support for images, videos, replies, and emoji reactions.
- **Message Editing** – (New in v12) Edit your mistakes before anyone notices!

### 🎨 Personalization & UI
- **Multiple Themes** – choose from: *Original, Dark, Classic, Neon, Forest,* or *High Contrast*.
- **Dynamic Text Size** – from small to Extra Large.
- **Plain Text Mode** – for users who prefer minimalism.
- **Notifications** – full volume control and customizable sounds.

---

## 💻 System Requirements

### Server (Host)
- **OS:** Windows 10 (1809+) or Windows 11.
- **Runtime:** .NET 11 Runtime (Desktop).
- **Disk Space:** ~200MB for application + database growth.

### Client (Web)
- **Browser:** Modern browser (Chrome, Firefox, Edge, Safari).
- **Server:** PHP 8.1+ (if hosting separately).

---

## 📥 Installation Instructions

1. **Download:** Get the latest `ParrotnestInstaller.exe` from the download page.
2. **Install:** Run the installer and follow the on-screen instructions.
3. **Launch:** Open the **Parrotnest Server Host** from your desktop.
4. **Configure:** Set your desired port and server settings.
5. **Connect:** Share the server address with your friends and start chatting!

---

## 🛠 Technology Stack

- **Backend:** .NET 11.0, ASP.NET Core, SignalR, Entity Framework Core (SQLite).
- **Frontend:** PHP, Vanilla JavaScript, CSS.
- **Tools:**
  - `CssGen` – custom C# CSS generator.
  - `ParrotnestDesktopInstaller` – dedicated installer project.

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
