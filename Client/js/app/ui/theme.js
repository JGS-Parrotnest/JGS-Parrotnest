const ALLOWED_THEMES = ['dark', 'classic', 'original', 'neon', 'forest', 'kontrast'];
const ALLOWED_TEXT_SIZES = ['small', 'medium', 'large', 'xlarge'];

export function applyTheme(themeName) {
    const root = document.documentElement;
    if (!root) return;
    const finalTheme = ALLOWED_THEMES.includes(themeName) ? themeName : 'original';
    root.setAttribute('data-theme', finalTheme);
}

export function applyTextSize(size) {
    const root = document.documentElement;
    if (!root) return;
    const finalSize = ALLOWED_TEXT_SIZES.includes(size) ? size : 'medium';
    root.setAttribute('data-text-size', finalSize);
}

export function applySimpleText(isSimple) {
    const root = document.documentElement;
    if (!root) return;
    if (isSimple) {
        root.setAttribute('data-simple-text', 'true');
    } else {
        root.removeAttribute('data-simple-text');
    }
}

export async function saveThemeSettings(currentApiUrl) {
    const theme = localStorage.getItem('preferredTheme') || 'original';
    const textSize = localStorage.getItem('preferredTextSize') || 'medium';
    const isSimpleText = localStorage.getItem('preferredSimpleText') === 'true';

    try {
        const token = localStorage.getItem('token');
        if (!token) return;

        const response = await fetch(`${currentApiUrl}/users/profile`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`
            },
            body: JSON.stringify({
                theme,
                textSize,
                isSimpleText
            })
        });

        if (!response.ok) return;

        const userStr = localStorage.getItem('user');
        if (!userStr) return;

        try {
            const user = JSON.parse(userStr);
            user.Theme = theme;
            user.TextSize = textSize;
            user.IsSimpleText = isSimpleText;
            localStorage.setItem('user', JSON.stringify(user));
        } catch (error) {
            console.warn('Failed to persist theme settings locally', error);
        }
    } catch (error) {
        console.error('Failed to save theme settings to server', error);
    }
}

export function bindThemeControls({
    currentApiUrl,
    themeDarkRadio,
    themeClassicRadio,
    themeOriginalRadio,
    themeNeonRadio,
    themeForestRadio,
    themeKontrastRadio,
    textSizeSlider,
    simpleTextToggle
}) {
    const preferredTheme = localStorage.getItem('preferredTheme') || 'original';
    const preferredTextSize = localStorage.getItem('preferredTextSize') || 'medium';
    const preferredSimpleText = localStorage.getItem('preferredSimpleText') === 'true';

    applyTheme(preferredTheme);
    applyTextSize(preferredTextSize);
    applySimpleText(preferredSimpleText);

    const sizeMap = { small: 0, medium: 1, large: 2, xlarge: 3 };
    const sizeRevMap = ['small', 'medium', 'large', 'xlarge'];

    if (textSizeSlider) {
        textSizeSlider.value = sizeMap[preferredTextSize] !== undefined ? sizeMap[preferredTextSize] : 1;
        textSizeSlider.addEventListener('input', () => {
            const value = parseInt(textSizeSlider.value, 10);
            const size = sizeRevMap[value] || 'medium';
            applyTextSize(size);
        });
    }

    if (simpleTextToggle) {
        simpleTextToggle.checked = preferredSimpleText;
        simpleTextToggle.addEventListener('change', () => {
            applySimpleText(simpleTextToggle.checked);
            localStorage.setItem('preferredSimpleText', simpleTextToggle.checked);
            saveThemeSettings(currentApiUrl);
        });
    }

    const radioMappings = [
        ['dark', themeDarkRadio],
        ['classic', themeClassicRadio],
        ['original', themeOriginalRadio],
        ['neon', themeNeonRadio],
        ['forest', themeForestRadio],
        ['kontrast', themeKontrastRadio]
    ];

    radioMappings.forEach(([themeName, radio]) => {
        if (!radio) return;
        radio.checked = preferredTheme === themeName;
        radio.addEventListener('change', () => {
            if (!radio.checked) return;
            applyTheme(themeName);
            localStorage.setItem('preferredTheme', themeName);
            saveThemeSettings(currentApiUrl);
        });
    });
}
