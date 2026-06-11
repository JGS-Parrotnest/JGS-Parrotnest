const SERVER_URL =
    window.SERVER_BASE || (window.location.protocol === 'file:' ? 'http://localhost:6069' : window.location.origin);
const HUB_URL = `${SERVER_URL}/chatHub`;
const apiBase = window.API_URL || `${SERVER_URL}/api`;
const currentApiUrl = typeof API_URL !== 'undefined' ? API_URL : apiBase;
const APP_JS_VERSION = '35';
const debugNetwork = (() => {
    try {
        return localStorage.getItem('debugNetwork') === '1';
    } catch {
        return false;
    }
})();

window.API_URL = currentApiUrl;
window.APP_JS_VERSION = APP_JS_VERSION;

export { APP_JS_VERSION, HUB_URL, SERVER_URL, currentApiUrl, debugNetwork };

export function initRuntimeGlobals() {
    if (debugNetwork) {
        console.info(`[diag] app.js loaded v=${window.APP_JS_VERSION} api=${currentApiUrl}`);
        window.addEventListener('load', () => console.info(`[diag] window load v=${window.APP_JS_VERSION}`), { once: true });
    }

    if (typeof window.resolveUrl === 'undefined') {
        window.resolveUrl = function resolveUrl(url) {
            if (!url) return null;
            if (url.startsWith('blob:') || url.startsWith('data:')) return url;

            try {
                if (url.startsWith('http://') || url.startsWith('https://')) {
                    const target = new URL(url);
                    const current = new URL(window.location.origin);
                    if (target.hostname === 'localhost' || target.hostname === '0.0.0.0') {
                        if (window.location.protocol !== 'file:') {
                            target.hostname = current.hostname;
                        } else {
                            target.hostname = 'localhost';
                        }
                        return target.toString();
                    }
                    return url;
                }
            } catch (error) {
                console.warn('resolveUrl fallback', error);
            }

            let normalized = url.replace(/\\/g, '/');
            if (!normalized.startsWith('/')) normalized = `/${normalized}`;

            let base = window.API_BASE_URL || window.SERVER_BASE || SERVER_URL;
            if (!base) {
                base = window.location.protocol === 'file:' ? 'http://localhost:6069' : window.location.origin;
            }
            if (base.endsWith('/')) base = base.slice(0, -1);
            return `${base}${normalized}`;
        };
    }

    if (typeof window.showNotification === 'undefined') {
        window.showNotification = function showNotification(message, type = 'success') {
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
                setTimeout(() => toast.remove(), 300);
            }, 4000);
        };
    }

    if (typeof window.handleApiError === 'undefined') {
        window.handleApiError = async function handleApiError(response, defaultMessage = 'Wystąpił błąd') {
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
                } catch {
                    if (text.trim().startsWith('<')) {
                        message = `${defaultMessage} (Status: ${response.status})`;
                    }
                }
            } else {
                message = `${defaultMessage} (Status: ${response.status})`;
            }

            window.showNotification(message, 'error');
        };
    }
}

export function createProductionBannerController() {
    let productionBanner = null;

    function ensureProductionBanner() {
        if (productionBanner && document.body.contains(productionBanner)) return productionBanner;
        const chatArea = document.querySelector('.chat-area');
        if (!chatArea) return null;
        const header = chatArea.querySelector('.chat-header');
        if (!header) return null;

        let element = document.getElementById('productionBanner');
        if (!element) {
            element = document.createElement('div');
            element.id = 'productionBanner';
            element.className = 'production-banner';
            header.insertAdjacentElement('afterend', element);
        }

        productionBanner = element;
        return element;
    }

    function setContent(content) {
        const element = ensureProductionBanner();
        if (!element) return;

        const text = (content ?? '').toString().trim();
        if (!text) {
            element.textContent = '';
            element.classList.remove('show');
            return;
        }

        element.textContent = text;
        element.classList.add('show');
    }

    return { setContent };
}

export function createGeneralChannelController({ getToken, getCurrentChatType }) {
    let generalChannel = { name: 'Ogolny', avatarUrl: 'Assets/logo.png', updatedAt: null };

    function applyToGlobalItem() {
        const globalChatItem = document.getElementById('globalChatItem');
        if (!globalChatItem) return;

        const title = globalChatItem.querySelector('h4');
        if (title) title.textContent = generalChannel.name || 'Ogolny';

        const avatar = globalChatItem.querySelector('.avatar');
        if (!avatar) return;

        const avatarUrl = generalChannel.avatarUrl;
        if (avatarUrl) {
            avatar.style.backgroundImage = `url('${window.resolveUrl(avatarUrl)}')`;
            avatar.style.backgroundSize = 'cover';
            avatar.style.backgroundPosition = 'center';
            avatar.textContent = '';
            return;
        }

        avatar.style.backgroundImage = '';
        avatar.textContent = (generalChannel.name || 'O').charAt(0).toUpperCase();
    }

    async function load() {
        const token = getToken?.();
        if (!token) return;

        try {
            const response = await fetch(`${currentApiUrl}/general`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            if (!response.ok) return;

            const data = await response.json();
            generalChannel = {
                name: data.name || data.Name || 'Ogolny',
                avatarUrl: data.avatarUrl || data.AvatarUrl || 'Assets/logo.png',
                updatedAt: data.updatedAt || data.UpdatedAt || null
            };

            applyToGlobalItem();

            if (getCurrentChatType?.() === 'global') {
                const chatHeader = document.querySelector('.chat-header h3');
                if (chatHeader) chatHeader.textContent = generalChannel.name || 'Ogolny';

                const headerAvatar = document.querySelector('.chat-header .avatar');
                if (headerAvatar) {
                    headerAvatar.style.backgroundImage = `url('${window.resolveUrl(generalChannel.avatarUrl)}')`;
                    headerAvatar.style.backgroundSize = 'cover';
                    headerAvatar.style.backgroundPosition = 'center';
                    headerAvatar.textContent = '';
                }
            }
        } catch (error) {
            console.warn('loadGeneralChannel failed', error);
        }
    }

    function getState() {
        return generalChannel;
    }

    function updateFromPayload(payload = {}) {
        generalChannel = {
            name: payload.name || generalChannel.name,
            avatarUrl: payload.avatarUrl || generalChannel.avatarUrl,
            updatedAt: payload.updatedAt || generalChannel.updatedAt
        };
        applyToGlobalItem();
    }

    return {
        applyToGlobalItem,
        getState,
        load,
        updateFromPayload
    };
}
