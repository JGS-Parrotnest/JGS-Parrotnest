<?php
session_start();
session_unset();
session_destroy();
?>
<!DOCTYPE html>
<html lang="pl">
<head>
    <script>
        (function() {
            try {
                var t = localStorage.getItem('preferredTheme') || 'original';
                document.documentElement.setAttribute('data-theme', t);
                var s = localStorage.getItem('preferredTextSize') || 'medium';
                document.documentElement.setAttribute('data-text-size', s);

                if (localStorage.getItem('preferredSimpleText') === 'true') {
                    document.documentElement.setAttribute('data-simple-text', 'true');
                }
            } catch (e) {
                document.documentElement.setAttribute('data-theme', 'original');
            }
        })();
    </script>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Parrotnest - Pobierz Aplikację</title>
    <link rel="icon" href="Assets/logo.png" type="image/png">
    <link rel="stylesheet" href="style.css?v=16">
    <link rel="stylesheet" href="mobile.css?v=4" media="(max-width: 768px)">

    <style>
.expanded-card {
            width: 1000px !important;
            max-width: 95vw !important;
            max-height: 450px !important;
            display: flex !important;
            flex-direction: column !important;
            align-items: center !important;
            padding-top: 100px !important;
        }
        .os-grid {
            display: flex !important;
            flex-direction: row !important;
            flex-wrap: nowrap !important;
            justify-content: center !important;
            gap: 20px !important;
            width: 100% !important;
        }
        .os-tile-wrapper {
            flex: 0 0 220px !important;
            display: flex !important;
            flex-direction: column !important;
            align-items: center !important;
            border: 3px solid #ffffff !important;
            border-radius: 20px !important;
            padding: 15px !important;
            background: rgba(255,255,255,0.1) !important;
        }
        .os-icon-img-single {
            filter: brightness(0) invert(1) !important;
            width: 80px !important;
            height: 80px !important;
        }
        .os-tile-label {
            color: #ffffff !important;
            margin-top: 10px !important;
            font-weight: bold !important;
        }
        @media (max-width: 768px) {
            .os-tile-wrapper {
                flex: 0 0 30% !important;
                padding: 10px !important;
            }
            .os-icon-img-single {
                width: 50px !important;
                height: 50px !important;
            }
            .os-tile-label {
                font-size: 0.8rem !important;
            }
        }
    </style>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200" />
</head>
<body>
    <div class="login-container" style="display: flex; flex-direction: column; align-items: center; justify-content: flex-start; min-height: 100vh;">
        <header class="fixed-nav">
            <div class="logo-area">
                <img src="Assets/logo.png" alt="Parrotnest Logo" class="logo">
                <h1>Parrotnest</h1>
            </div>
        </header>
        <div class="login-card expanded-card">
            <h2>Pobierz Aplikację</h2>
            <div class="os-grid">
<div class="os-tile-wrapper">
                    <a href="APP/Windows/ParrotnestDesktopInstaller.exe" class="os-card-icon" download aria-label="Pobierz dla Windows">
                        <img src="Assets/win.png" srcset="Assets/win.png 1x, Assets/win@2x.png 2x" alt="Windows" class="os-icon-img-single">
                    </a>
                    <div class="os-tile-label">Win 10/11</div>
                </div>
<div class="os-tile-wrapper">
                    <div class="os-card-icon disabled" aria-disabled="true">
                        <img src="Assets/mac.png" srcset="Assets/mac.png 1x, Assets/mac@2x.png 2x" alt="macOS" class="os-icon-img-single">
                    </div>
                    <div class="os-tile-label disabled-label">Wkrótce dostępne</div>
                </div>
<div class="os-tile-wrapper">
                    <div class="os-card-icon disabled" aria-disabled="true">
                        <img src="Assets/tux.png" srcset="Assets/tux.png 1x, Assets/tux@2x.png 2x" alt="Linux" class="os-icon-img-single">
                    </div>
                    <div class="os-tile-label disabled-label">Wkrótce dostępne</div>
                </div>
            </div>
            <div class="footer-links">
                 <a href="login.php" class="back-link">← Powrót do logowania</a>
            </div>
        </div>
    </div>
    <script src="particles.js"></script>
</body>
</html>