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
    <title>Parrotnest - Zaloguj się</title>
    <link rel="icon" href="logo.png" type="image/png">
    <link rel="stylesheet" href="style.css?v=7">
    <link rel="stylesheet" href="mobile.css?v=1" media="(max-width: 768px)">
    <link rel="stylesheet" href="accessibility.css?v=1">
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600&display=swap" rel="stylesheet">
</head>
<body>
    <div class="login-container">
        <div class="logo-area">
            <img src="logo.png" alt="Parrotnest Logo" class="logo">
            <h1>Parrotnest</h1>
        </div>
        <div class="login-card">
            <h2>Zaloguj się</h2>
            <form id="loginForm" onsubmit="event.preventDefault(); handleLogin(event);">
                <div class="input-group">
                    <label for="email">Adres e-mail</label>
                    <input type="email" id="email" name="email" required placeholder="Wpisz swój e-mail">
                </div>
                <div class="input-group">
                    <label for="password">Hasło</label>
                    <input type="password" id="password" name="password" required placeholder="Wpisz hasło">
                </div>
                <div class="options">
                    <label class="checkbox-container">
                        <input type="checkbox" name="remember">
                        <span class="checkmark"></span>
                        Zapamiętaj mnie
                    </label>
                    <a href="forgot-password.php" class="forgot-link">Nie pamiętasz hasła?</a>
                </div>
                <button type="submit" class="btn-primary">Zaloguj się</button>
            </form>
            <div class="footer-links">
                <p>Nie masz konta? <a href="/register.php">Zarejestruj się</a></p>
                <div style="margin-top: 20px; text-align: center; width: 100%; display: flex; flex-direction: column; align-items: center; gap: 15px;">
                    <a href="download.php" style="text-decoration: none;">
                        <button type="button" class="btn-primary" style="background: #28a745; border: 1px solid #28a745; color: #fff; padding: 12px 24px; font-size: 1rem; cursor: pointer; transition: transform 0.2s;">
                            Pobierz aplikację na komputer
                        </button>
                    </a>
                    <a href="/ParrotnestInstaller.dmg" download style="font-size: 0.85rem; color: #555; text-decoration: none;">
                        Pobierz aplikację desktopową
                    </a>
                </div>
            </div>
        </div>
    </div>
    <script src="auth.js?v=9"></script>
    <script src="particles.js"></script>
    <!-- Accessibility Component -->
    <div id="accessibility-container" class="accessibility-container">
        <div id="accessibility-popout" class="accessibility-popout" role="dialog" aria-labelledby="acc-title" aria-hidden="true">
            <div class="acc-header">
                <h2 id="acc-title">Ustawienia dostępności</h2>
                <button id="close-acc-popout" aria-label="Zamknij panel">&times;</button>
            </div>
            <div class="acc-body">
                <div class="acc-option">
                    <label for="acc-contrast">Kontrast: <span id="contrast-val">100</span>%</label>
                    <input type="range" id="acc-contrast" min="50" max="200" value="100" step="10">
                </div>
                <div class="acc-option">
                    <label for="acc-font-size">Wielkość czcionki: <span id="font-size-val">100</span>%</label>
                    <input type="range" id="acc-font-size" min="100" max="200" value="100" step="10">
                </div>
                <div class="acc-option">
                    <label for="acc-colorblind">Tryb daltonizmu:</label>
                    <select id="acc-colorblind">
                        <option value="none">Brak</option>
                        <option value="protanopia">Protanopia</option>
                        <option value="deuteranopia">Deuteranopia</option>
                        <option value="tritanopia">Tritanopia</option>
                        <option value="achromatopsia">Achromatopsja</option>
                    </select>
                </div>
                <div class="acc-option toggle-option">
                    <label>Animacje:</label>
                    <label class="switch">
                        <input type="checkbox" id="acc-animations" checked>
                        <span class="slider round"></span>
                    </label>
                </div>
                <button id="acc-reset" class="acc-reset-btn">Przywróć domyślne</button>
            </div>
        </div>
        <button id="accessibility-button" class="accessibility-button" aria-label="Ustawienia dostępności" aria-expanded="false" aria-controls="accessibility-popout">
            <img src="accbt.png" alt="Dostępność">
        </button>
    </div>
    <script src="accessibility.js?v=2"></script>
</body>
</html>
