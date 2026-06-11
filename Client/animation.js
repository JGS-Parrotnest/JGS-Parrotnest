(function() {
    if (sessionStorage.getItem('parrot_animation_played')) {
        return;
    }
    sessionStorage.setItem('parrot_animation_played', 'true');
    const overlay = document.createElement('div');
    overlay.style.position = 'fixed';
    overlay.style.inset = '0';
    overlay.style.backgroundColor = '#020617';
    overlay.style.zIndex = '999999';
    overlay.style.transition = 'opacity 0.4s ease-out';
    const target = document.body || document.documentElement;
    target.appendChild(overlay);
    const audio = new Audio('Assets/parrot.mp3');
    audio.play().catch(e => console.error("Audio play failed:", e));
    const bird = document.createElement('div');
    bird.style.position = 'fixed';
    bird.style.left = '-600px';
    bird.style.top = '50%';
    bird.style.width = '512px';
    bird.style.height = '512px';
    bird.style.backgroundImage = 'url("Assets/logo.png")';
    bird.style.backgroundSize = 'contain';
    bird.style.backgroundRepeat = 'no-repeat';
    bird.style.zIndex = '1000000';
    bird.style.pointerEvents = 'none';
    target.appendChild(bird);
    let start = null;
    const duration = 1800;
    function animate(timestamp) {
        if (!start) start = timestamp;
        const progress = (timestamp - start) / duration;
        const x = progress * (window.innerWidth + 1200) - 600;
        bird.style.left = x + 'px';
        const yOffset = Math.sin(progress * 15) * 80;
        bird.style.top = `calc(50% + ${yOffset}px - 256px)`;
        const rotation = Math.cos(progress * 15) * 15;
        bird.style.transform = `rotate(${rotation}deg)`;

        if (progress < 1) {
            requestAnimationFrame(animate);
        } else {
            bird.remove();
            overlay.style.opacity = '0';
            setTimeout(() => overlay.remove(), 400);
        }
    }
    requestAnimationFrame(animate);
})();
