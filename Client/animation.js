(function() {
    // Check if animation was already played in this session
    if (sessionStorage.getItem('parrot_animation_played')) {
        return;
    }
    sessionStorage.setItem('parrot_animation_played', 'true');

    // Create overlay immediately
    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.inset = '0';
    overlay.style.backgroundColor = '#020617'; // Original theme color
    overlay.style.zIndex = '999999'; // Even higher z-index
    overlay.style.transition = 'opacity 0.4s ease-out';
    
    // Append to documentElement if body isn't ready yet
    const target = document.body || document.documentElement;
    target.appendChild(overlay);

    // Play sound
    const audio = new Audio('parrot.mp3');
    audio.play().catch(e => console.error("Audio play failed:", e));

    // Setup bird (4x larger: 512px)
    const bird = document.createElement('div');
    bird.style.position = 'fixed';
    bird.style.left = '-600px';
    bird.style.top = '50%';
    bird.style.width = '512px';
    bird.style.height = '512px';
    bird.style.backgroundImage = 'url("logo.png")';
    bird.style.backgroundSize = 'contain';
    bird.style.backgroundRepeat = 'no-repeat';
    bird.style.zIndex = '1000000';
    bird.style.pointerEvents = 'none';
    target.appendChild(bird);

    // Animation settings
    let start = null;
    const duration = 1800; // Faster animation (1.8s)
    
    function animate(timestamp) {
        if (!start) start = timestamp;
        const progress = (timestamp - start) / duration;
        
        // Horizontal movement
        const x = progress * (window.innerWidth + 1200) - 600;
        bird.style.left = x + 'px';
        
        // Vertical movement (sine wave)
        const yOffset = Math.sin(progress * 15) * 80;
        bird.style.top = `calc(50% + ${yOffset}px - 256px)`;
        
        // Rotation
        const rotation = Math.cos(progress * 15) * 15;
        bird.style.transform = `rotate(${rotation}deg)`;

        if (progress < 1) {
            requestAnimationFrame(animate);
        } else {
            bird.remove();
            // Fade out overlay
            overlay.style.opacity = '0';
            setTimeout(() => overlay.remove(), 400);
        }
    }
    
    requestAnimationFrame(animate);
})();
