export function initLightbox() {
    const imageModal = document.getElementById('image-modal');
    const modalImg = document.getElementById('img-preview');
    const modalVideo = document.getElementById('video-preview');
    const closeImageModal = document.getElementsByClassName('close-image-modal')[0];

    function closeModal() {
        if (!imageModal) return;
        if (modalVideo) {
            modalVideo.pause();
            modalVideo.removeAttribute('src');
            modalVideo.load();
            modalVideo.style.display = 'none';
        }
        if (modalImg) {
            modalImg.style.display = '';
            modalImg.classList.remove('zoomed');
        }
        imageModal.style.opacity = '0';
        setTimeout(() => {
            imageModal.style.display = 'none';
        }, 300);
    }

    function openLightbox(src, type = 'image') {
        if (!imageModal || !modalImg) return;
        imageModal.style.display = 'flex';
        modalImg.classList.remove('zoomed');
        if (modalVideo) {
            modalVideo.pause();
            modalVideo.style.display = 'none';
        }
        setTimeout(() => {
            imageModal.style.opacity = '1';
        }, 10);

        if (type === 'video' && modalVideo) {
            modalImg.style.display = 'none';
            modalVideo.style.display = 'block';
            modalVideo.src = src;
            modalVideo.load();
            modalVideo.play().catch(() => {});
            return;
        }

        modalImg.style.display = 'block';
        modalImg.src = src;
    }

    if (modalImg) {
        modalImg.onclick = function handleImageClick(event) {
            event.stopPropagation();
            this.classList.toggle('zoomed');
        };
    }

    if (closeImageModal) {
        closeImageModal.onclick = closeModal;
    }

    if (imageModal) {
        imageModal.onclick = (event) => {
            if (event.target === imageModal) {
                closeModal();
            }
        };
    }

    return { openLightbox };
}
