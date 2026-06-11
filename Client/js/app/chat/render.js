function escapeHtml(text) {
    return String(text)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

export function appendMessageText(messageContainer, content) {
    if (!content || !String(content).trim()) return;

    const messageText = document.createElement('div');
    messageText.className = 'message-text';
    const urlRegex = /(https?:\/\/[^\s]+)/g;
    const escapedContent = escapeHtml(content);
    let html = escapedContent.replace(/\n/g, '<br>');
    
    html = html.replace(/(@[a-zA-Z0-9_]+|@admin|@everyone)/g, '<span class="mention-tag">$1</span>');
    
    const tempDiv = document.createElement('div');
    tempDiv.innerHTML = html;
    let childNodes = Array.from(tempDiv.childNodes);
    
    childNodes.forEach(node => {
        if (node.nodeType === 3) {
            const text = node.textContent;
            const parts = text.split(urlRegex);
            parts.forEach(part => {
                if (part.match(urlRegex)) {
                    let domain = 'Link';
                    try { domain = new URL(part).hostname; } catch {}
                    const card = document.createElement('a');
                    card.href = part;
                    card.target = '_blank';
                    card.rel = 'noopener noreferrer';
                    card.className = 'link-preview-card';
                    card.innerHTML = `
                        <div class="link-icon-container">🔗</div>
                        <div class="link-info">
                            <div class="link-title">${part}</div>
                            <div class="link-domain">${domain}</div>
                        </div>
                    `;
                    messageText.appendChild(card);
                } else if (part) {
                    const span = document.createElement('span');
                    span.textContent = part;
                    messageText.appendChild(span);
                }
            });
        } else {
            messageText.appendChild(node);
        }
    });

    messageContainer.appendChild(messageText);
}

function appendMedia(messageContainer, imageUrl, openLightbox) {
    if (!imageUrl) return;
    const mediaUrl = window.resolveUrl ? window.resolveUrl(imageUrl) : imageUrl;
    const lower = mediaUrl.split('?')[0].toLowerCase();
    const ext = lower.includes('.') ? lower.substring(lower.lastIndexOf('.') + 1) : '';

    if (['mp4', 'webm', 'ogg', 'mov', 'avi', 'mkv', 'm4v', '3gp', 'mpeg', 'mpg'].includes(ext)) {
        const video = document.createElement('video');
        video.src = mediaUrl;
        video.className = 'message-video';
        video.controls = true;
        video.preload = 'metadata';
        messageContainer.appendChild(video);
        return;
    }

    const img = document.createElement('img');
    img.src = mediaUrl;
    img.className = 'message-image';
    img.onclick = () => openLightbox?.(mediaUrl);
    messageContainer.appendChild(img);
}

export function appendReactionBadges({
    container,
    reactions,
    currentUserId,
    messageId,
    onReact,
    onOpenPicker
}) {
    if (!reactions) return;

    try {
        const reactionList = typeof reactions === 'string' ? JSON.parse(reactions) : reactions;
        if (!Array.isArray(reactionList) || reactionList.length === 0) return;

        const reactionsDiv = document.createElement('div');
        reactionsDiv.className = 'message-reactions';
        const grouped = {};

        reactionList.forEach((reaction) => {
            if (!grouped[reaction.e]) grouped[reaction.e] = [];
            grouped[reaction.e].push(reaction.u);
        });

        Object.entries(grouped).forEach(([emoji, userIds]) => {
            const badge = document.createElement('div');
            badge.className = 'reaction-badge';
            if (userIds.includes(parseInt(currentUserId, 10))) {
                badge.classList.add('self-reacted');
            }
            badge.innerHTML = `<span class="emoji">${emoji}</span> <span class="count">${userIds.length}</span>`;
            badge.onclick = (event) => {
                event.stopPropagation();
                onReact?.(messageId, emoji);
            };
            reactionsDiv.appendChild(badge);
        });

        const addBtn = document.createElement('div');
        addBtn.className = 'reaction-badge add-reaction-btn';
        addBtn.innerHTML = `<span class="material-symbols-outlined" style="font-size: 1.1rem;">add_reaction</span>`;
        addBtn.title = 'Dodaj reakcje';
        addBtn.onclick = (event) => {
            event.stopPropagation();
            onOpenPicker?.(messageId, addBtn);
        };
        reactionsDiv.appendChild(addBtn);

        container.appendChild(reactionsDiv);
    } catch (error) {
        console.error('Error parsing reactions', error);
    }
}

export function buildMessageElement({
    senderId,
    senderUsername,
    senderAvatarUrl,
    messageId,
    message,
    imageUrl,
    isOwnMessage,
    isContinuation,
    timestamp,
    replyToId,
    replyToSender,
    replyToContent,
    reactions,
    canEdit,
    canDelete,
    currentUserId,
    onReply,
    onEdit,
    onDelete,
    onReact,
    onOpenPicker,
    onOpenProfile,
    onScrollToReply,
    openLightbox
}) {
    const messageWrapper = document.createElement('div');
    messageWrapper.className = `message-wrapper ${isOwnMessage ? 'own-message' : ''}`;
    if (isContinuation) messageWrapper.classList.add('message-continuation');
    if (senderId != null) messageWrapper.dataset.senderId = senderId;
    if (timestamp) messageWrapper.dataset.timestamp = new Date(timestamp).toISOString();
    if (messageId != null) messageWrapper.dataset.messageId = messageId;
    messageWrapper.dataset.messageContent = message || '';

    const row = document.createElement('div');
    row.className = 'message-row';

    if (messageId != null) {
        const actionsDiv = document.createElement('div');
        actionsDiv.className = 'message-actions';

        const replyBtn = document.createElement('button');
        replyBtn.className = 'btn-msg-action';
        replyBtn.title = 'Odpowiedz';
        replyBtn.innerHTML = `<span class="material-symbols-outlined">reply</span>`;
        replyBtn.onclick = (event) => {
            event.stopPropagation();
            onReply?.(messageId, senderUsername, message);
        };
        actionsDiv.appendChild(replyBtn);

        const reactBtn = document.createElement('button');
        reactBtn.className = 'btn-msg-action';
        reactBtn.title = 'Zareaguj';
        reactBtn.innerHTML = `<span class="material-symbols-outlined">add_reaction</span>`;
        reactBtn.onclick = (event) => {
            event.stopPropagation();
            onOpenPicker?.(messageId, reactBtn);
        };
        actionsDiv.appendChild(reactBtn);

        if (canEdit) {
            const editBtn = document.createElement('button');
            editBtn.className = 'btn-msg-action';
            editBtn.title = 'Edytuj';
            editBtn.innerHTML = `<span class="material-symbols-outlined">edit</span>`;
            editBtn.onclick = (event) => {
                event.stopPropagation();
                onEdit?.(messageId);
            };
            actionsDiv.appendChild(editBtn);
        }

        if (canDelete) {
            const deleteBtn = document.createElement('button');
            deleteBtn.className = 'btn-msg-action btn-delete';
            deleteBtn.title = 'Usun wiadomosc';
            deleteBtn.innerHTML = `<span class="material-symbols-outlined">delete</span>`;
            deleteBtn.onclick = (event) => {
                event.stopPropagation();
                onDelete?.(messageId);
            };
            actionsDiv.appendChild(deleteBtn);
        }

        row.appendChild(actionsDiv);
    }

    const avatarEl = document.createElement('div');
    avatarEl.className = 'message-avatar';
    if (isContinuation) {
        avatarEl.style.visibility = 'hidden';
    } else {
        avatarEl.style.cursor = 'pointer';
        avatarEl.onclick = (event) => {
            event.stopPropagation();
            onOpenProfile?.();
        };
        if (senderAvatarUrl) {
            avatarEl.style.backgroundImage = `url('${window.resolveUrl(senderAvatarUrl)}')`;
            avatarEl.textContent = '';
        } else if (senderUsername) {
            avatarEl.textContent = senderUsername.charAt(0).toUpperCase();
        }
    }

    const messageBox = document.createElement('div');
    messageBox.className = isOwnMessage ? 'message sent' : 'message received';

    if (replyToId && replyToSender) {
        const quote = document.createElement('div');
        quote.className = 'message-reply-quote';
        quote.onclick = (event) => {
            event.stopPropagation();
            onScrollToReply?.(replyToId);
        };
        quote.innerHTML = `<strong>${replyToSender}</strong><span>${replyToContent || 'Obraz'}</span>`;
        messageBox.appendChild(quote);
    }

    if (!isContinuation) {
        const senderName = document.createElement('div');
        senderName.className = 'message-sender';
        senderName.textContent = senderUsername || 'Ty';
        messageBox.appendChild(senderName);
    }

    appendMedia(messageBox, imageUrl, openLightbox);
    appendMessageText(messageBox, message);

    appendReactionBadges({
        container: messageBox,
        reactions,
        currentUserId,
        messageId,
        onReact,
        onOpenPicker
    });

    const timeElement = document.createElement('div');
    timeElement.className = 'message-time';
    if (timestamp) {
        const date = new Date(timestamp);
        timeElement.textContent = Number.isNaN(date.getTime())
            ? ''
            : date.toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' });
    }
    messageBox.appendChild(timeElement);

    row.appendChild(avatarEl);
    row.appendChild(messageBox);
    messageWrapper.appendChild(row);
    return messageWrapper;
}

export function detectMessageContinuation(messagesContainer, senderId, timestamp) {
    if (!messagesContainer || !messagesContainer.lastElementChild) {
        return false;
    }

    const lastWrapper = messagesContainer.lastElementChild;
    const lastSenderId = lastWrapper.dataset.senderId;
    const lastTimestampStr = lastWrapper.dataset.timestamp;
    if (!lastSenderId || senderId == null || `${lastSenderId}` !== `${senderId}`) {
        return false;
    }

    const lastDate = lastTimestampStr ? new Date(lastTimestampStr) : null;
    const currentDate = timestamp ? new Date(timestamp) : new Date();
    if (!lastDate || Number.isNaN(lastDate.getTime())) {
        return false;
    }

    if (currentDate - lastDate >= 60000) {
        return false;
    }

    const lastTime = lastWrapper.querySelector('.message-time');
    if (lastTime) {
        lastTime.style.display = 'none';
    }
    lastWrapper.classList.add('message-continuation-prev');
    return true;
}
