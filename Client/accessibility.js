
document.addEventListener('DOMContentLoaded', () => {
    // Elementy DOM
    const accContainer = document.getElementById('accessibility-container');
    const accButton = document.getElementById('accessibility-button');
    const accPopout = document.getElementById('accessibility-popout');
    const closeAccPopout = document.getElementById('close-acc-popout');
    
    const contrastInput = document.getElementById('acc-contrast');
    const fontSizeInput = document.getElementById('acc-font-size');
    const colorblindSelect = document.getElementById('acc-colorblind');
    const animationsToggle = document.getElementById('acc-animations');
    const resetBtn = document.getElementById('acc-reset');
    
    const contrastVal = document.getElementById('contrast-val');
    const fontSizeVal = document.getElementById('font-size-val');
    
    const html = document.documentElement;

    // --- LOGIKA OTWIERANIA/ZAMYKANIA ---
    
    const togglePopout = (forceClose = false) => {
        const isExpanded = accButton.getAttribute('aria-expanded') === 'true';
        const shouldClose = forceClose || isExpanded;
        
        accButton.setAttribute('aria-expanded', !shouldClose);
        accPopout.setAttribute('aria-hidden', shouldClose);
        
        if (!shouldClose) {
            // Focus na pierwszy element panelu po otwarciu
            setTimeout(() => contrastInput.focus(), 100);
        }
    };

    accButton.addEventListener('click', (e) => {
        e.stopPropagation();
        togglePopout();
    });

    closeAccPopout.addEventListener('click', () => togglePopout(true));

    // Zamknij po kliknięciu poza panelem
    document.addEventListener('click', (e) => {
        if (!accContainer.contains(e.target)) {
            togglePopout(true);
        }
    });

    // Zamknij klawiszem ESC
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') togglePopout(true);
    });

    // --- FUNKCJE STOSUJĄCE ZMIANY ---

    const applyContrast = (val) => {
        html.style.setProperty('--acc-contrast', val / 100);
        contrastInput.value = val;
        contrastVal.textContent = val;
        localStorage.setItem('acc-contrast', val);
    };

    const applyFontSize = (val) => {
        html.style.setProperty('--acc-font-multiplier', val / 100);
        fontSizeInput.value = val;
        fontSizeVal.textContent = val;
        localStorage.setItem('acc-font-size', val);
    };

    const applyColorblind = (mode) => {
        html.setAttribute('data-colorblind', mode);
        colorblindSelect.value = mode;
        localStorage.setItem('acc-colorblind', mode);
    };

    const applyAnimations = (enabled) => {
        html.setAttribute('data-animations', enabled);
        animationsToggle.checked = enabled;
        localStorage.setItem('acc-animations', enabled);
    };

    // --- EVENT LISTENERY KONTROLEK ---

    contrastInput.addEventListener('input', (e) => applyContrast(e.target.value));
    fontSizeInput.addEventListener('input', (e) => applyFontSize(e.target.value));
    colorblindSelect.addEventListener('change', (e) => applyColorblind(e.target.value));
    animationsToggle.addEventListener('change', (e) => applyAnimations(e.target.checked));

    resetBtn.addEventListener('click', () => {
        applyContrast(100);
        applyFontSize(100);
        applyColorblind('none');
        applyAnimations(true);
    });

    // --- INICJALIZACJA Z LOCAL STORAGE ---

    const init = () => {
        const savedContrast = localStorage.getItem('acc-contrast') || 100;
        const savedFontSize = localStorage.getItem('acc-font-size') || 100;
        const savedColorblind = localStorage.getItem('acc-colorblind') || 'none';
        const savedAnimations = localStorage.getItem('acc-animations') !== 'false';

        applyContrast(savedContrast);
        applyFontSize(savedFontSize);
        applyColorblind(savedColorblind);
        applyAnimations(savedAnimations);
    };

    init();

    // --- TESTY FUNKCJONALNE (dostępne w konsoli: runAccTests()) ---
    window.runAccTests = () => {
        console.group('Accessibility Functional Tests');
        
        const test = (name, fn) => {
            try {
                fn();
                console.log(`✅ ${name}`);
            } catch (e) {
                console.error(`❌ ${name}: ${e.message}`);
            }
        };

        test('Contrast change updates HTML style', () => {
            applyContrast(150);
            if (html.style.getPropertyValue('--acc-contrast') !== '1.5') throw new Error('Contrast variable not set');
        });

        test('Font size change updates HTML style', () => {
            applyFontSize(120);
            if (html.style.getPropertyValue('--acc-font-multiplier') !== '1.2') throw new Error('Font multiplier variable not set');
        });

        test('Colorblind mode updates data attribute', () => {
            applyColorblind('protanopia');
            if (html.getAttribute('data-colorblind') !== 'protanopia') throw new Error('Data attribute not set');
        });

        test('Animations toggle updates data attribute', () => {
            applyAnimations(false);
            if (html.getAttribute('data-animations') !== 'false') throw new Error('Data attribute not set');
        });

        test('LocalStorage persistence', () => {
            applyContrast(110);
            if (localStorage.getItem('acc-contrast') !== '110') throw new Error('LocalStorage not updated');
        });

        console.groupEnd();
        return 'Tests completed.';
    };
});
