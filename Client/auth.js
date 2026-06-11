(() => {
    let serverBase;
    let apiUrl;
    if (window.location.protocol === 'file:') {
        serverBase = 'http://localhost:6069';
    } else {
        serverBase = window.location.origin;
    }
    apiUrl = `${serverBase}/api`;
    window.__SERVER_BASE_DEFAULT__ = serverBase;
    window.__API_URL_DEFAULT__ = apiUrl;
})();

const isLocal = window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1' || window.location.hostname === '0.0.0.0';
let storedBase = localStorage.getItem('serverBase');

if (!isLocal) {
    if (storedBase) {
        localStorage.removeItem('serverBase');
        storedBase = null;
    }
} else if (storedBase) {
    try {
        const u = new URL(storedBase);
        const okHost = u.hostname === 'localhost' || u.hostname === '127.0.0.1' || u.hostname === '0.0.0.0';
        if (!okHost) {
            localStorage.removeItem('serverBase');
            storedBase = null;
        }
    } catch {
        localStorage.removeItem('serverBase');
        storedBase = null;
    }
}
const SERVER_BASE = (storedBase || window.__SERVER_BASE_DEFAULT__).replace(/\/+$/,'');
window.SERVER_BASE = SERVER_BASE;
const API_URL = `${SERVER_BASE}/api`;
window.API_URL = API_URL;

if (isLocal && storedBase && storedBase.includes(':6070')) {
    try {
        const controller = new AbortController();
        const timeoutId = setTimeout(() => controller.abort(), 1000);
        fetch(`${API_URL}/diag/build`, { signal: controller.signal })
            .then(() => clearTimeout(timeoutId))
            .catch((err) => {
                if (err.name === 'AbortError') {
                    console.warn("Diagnostic build check timed out, resetting serverBase.");
                }
                localStorage.removeItem('serverBase');
                window.location.reload();
            });
    } catch (e) {}
}

function showNotification(message, type = 'success') {
    // Skip empty messages
    if (!message || String(message).trim() === '') {
        console.warn('Suppressed empty notification');
        return;
    }
    // Skip notifications about media errors, upload errors or ERR_ABORTED
    const msgLower = String(message).toLowerCase();
    if (msgLower.includes('err_aborted') || 
        msgLower.includes('upload') || 
        msgLower.includes('media') ||
        msgLower.includes('net::')) {
        console.warn('Suppressed notification:', message, type);
        return;
    }

    let container = document.getElementById('notification-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'notification-container';
        container.className = 'notification-container';
        document.body.appendChild(container);
    }
    const toast = document.createElement('div');
    toast.className = `notification-toast ${type}`;
    toast.textContent = message;
    container.appendChild(toast);
    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease forwards';
        setTimeout(() => {
            toast.remove();
        }, 300);
    }, 4000);
}

async function handleApiError(response, defaultMessage = 'Wystąpił błąd') {
    let text = '';
    try {
        text = await response.text();
    } catch {
        text = '';
    }

    let message = defaultMessage;
    if (text && text.trim().length > 0) {
        message = text;
        try {
            const json = JSON.parse(text);
            message = json.message || json.error || json.title || defaultMessage;
            if (json.errors) {
                const details = Object.values(json.errors).flat().join(', ');
                if (details) message += `: ${details}`;
            }
        } catch (e) {
            if (text.trim().startsWith('<')) {
                message = `${defaultMessage} (Status: ${response.status})`;
            }
        }
    } else {
        message = `${defaultMessage} (Status: ${response.status})`;
    }
    showNotification(message, 'error');
}

window.handleLogin = async (e) => {
    if (e) {
        e.preventDefault();
        e.stopPropagation();
    }
    console.log("Rozpoczynam logowanie...");

    const emailInput = document.getElementById('email');
    const passwordInput = document.getElementById('password');

    if (!emailInput || !passwordInput) {
        showNotification('Błąd: Nie znaleziono pól formularza.', 'error');
        return false;
    }

    const email = emailInput.value;
    const password = passwordInput.value;

    try {
        const response = await fetch(`${API_URL}/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        });
        console.log("Otrzymano odpowiedź z serwera", response.status);

        if (response.ok) {
            const data = await response.json();
            console.log('Dane logowania:', data);

            if (!data.token || !data.user) {
                showNotification('Błąd serwera: brak tokena lub danych użytkownika.', 'error');
                return false;
            }

            localStorage.setItem('token', data.token);
            localStorage.setItem('user', JSON.stringify(data.user));
            if (!localStorage.getItem('token')) {
                showNotification('Błąd przeglądarki: localStorage nie działa.', 'error');
                return false;
            }

            showNotification('Zalogowano. Przekierowanie...', 'success');
            setTimeout(() => {
                console.log('Redirecting to /index.php');
                window.location.replace('/index.php');
            }, 500);
            return false;
        } else {
            await handleApiError(response, 'Logowanie nieudane');
            return false;
        }
    } catch (error) {
        console.error('Error:', error);
        showNotification('Błąd połączenia: ' + error.message, 'error');
        return false;
    }
};

window.handleRegister = async (e) => {
    if (e) {
        e.preventDefault();
        e.stopPropagation();
    }
    console.log("Rozpoczynam rejestrację...");

    const username = document.getElementById('username').value;
    if (username.length > 16) {
        showNotification('Nazwa użytkownika nie może być dłuższa niż 16 znaków.', 'error');
        return false;
    }

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;
            if (password !== confirmPassword) {
        showNotification('Hasła nie są identyczne!', 'error');
        return false;
    }
    try {
        const response = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, email, password })
        });
            if (response.ok) {
                showNotification('Rejestracja udana! Przekierowanie...', 'success');
                setTimeout(() => {
                    window.location.replace('/login.php');
                }, 2000);
        } else {
            await handleApiError(response, 'Rejestracja nieudana');
        }
        return false;
    } catch (error) {
        console.error('Error:', error);
        showNotification('Błąd rejestracji: ' + error.message, 'error');
        return false;
    }
};
