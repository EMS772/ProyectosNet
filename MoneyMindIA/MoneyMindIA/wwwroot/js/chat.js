// chat.js

// Toggle del Sidebar - Versión segura
document.addEventListener('DOMContentLoaded', function () {
    const toggleButton = document.querySelector('.toggle-button');
    const sidebar = document.querySelector('.sidebar');
    const chatMain = document.querySelector('.chat-main');

    if (toggleButton && sidebar && chatMain) {
        toggleButton.addEventListener('click', () => {
            sidebar.classList.toggle('collapsed');
            chatMain.classList.toggle('collapsed');

            const icon = toggleButton.querySelector('i');
            if (icon) {
                if (sidebar.classList.contains('collapsed')) {
                    icon.classList.remove('fa-chevron-left');
                    icon.classList.add('fa-chevron-right');
                } else {
                    icon.classList.remove('fa-chevron-right');
                    icon.classList.add('fa-chevron-left');
                }
            }
        });
    } else {
        console.error('One or more required elements not found in DOM');
    }

});

// Variable global para el chat actual
let currentChat = {
    id: null,
    messages: []
};

// Elementos del DOM
const chatMessages = document.getElementById('chatMessages');
const welcomeMessage = document.getElementById('welcomeMessage');
const chatConversation = document.getElementById('chatConversation');

// Función para formatear fecha robusta
function formatDate(dateString) {
    try {
        const date = new Date(dateString);
        if (isNaN(date.getTime())) {
            return dateString; // Si la fecha es inválida, devolver el string original
        }
        return date.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    } catch (e) {
        console.error("Error formateando fecha:", dateString);
        return dateString;
    }
}

// Función para agregar mensajes al chat
function addMessage(text, sender) {

    const isUser = sender === 'user';
    addMessageToCurrentChat(text, isUser);

    const messageElement = document.createElement('div');
    messageElement.classList.add('message', `${sender}-message`);

    const messageContent = document.createElement('div');
    messageContent.classList.add('message-content');
    messageContent.innerHTML = marked.parse(text);

    const messageTime = document.createElement('div');
    messageTime.classList.add('message-time');
    messageTime.textContent = new Date().toLocaleTimeString('es-ES', {
        hour: '2-digit',
        minute: '2-digit'
    });

    messageElement.appendChild(messageContent);
    messageElement.appendChild(messageTime);
    chatMessages.appendChild(messageElement);
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

// Función para agregar mensajes al chat actual
function addMessageToCurrentChat(text, isUser) {
    currentChat.messages.push({
        IsUser: isUser,
        Content: text
    });
    console.log("Mensaje agregado al chat actual:", currentChat);
}

// Función para resetear el chat actual
function resetCurrentChat() {
    currentChat = {
        id: null,
        messages: []
    };
    welcomeMessage.style.display = '';
    chatConversation.style.display = 'none';
    chatMessages.innerHTML = '';
}

// Función para guardar conversación actual
async function saveCurrentConversation() {
    try {
        if (currentChat.messages.length === 0) {
            console.log("No hay mensajes para guardar");
            return true;
        }

        console.log("Intentando guardar conversación:", currentChat);

        const response = await fetch(financialAdviceUrls.saveConversation, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({
                Messages: currentChat.messages
            })
        });

        if (!response.ok) {
            const errorText = await response.text();
            console.error(`HTTP error! status: ${response.status}, message: ${errorText}`);
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data.success) {
            console.log("Conversación guardada con éxito. ID:", data.recomendacionId);
            await loadRecomendationHistory();
            return true;
        } else {
            console.error("Error al guardar:", data.message);
            showError(data.message || 'No se pudo guardar la conversación');
            return false;
        }
    } catch (error) {
        console.error("Error al guardar conversación:", error);
        showError('Error al guardar la conversación');
        return false;
    }
}

// Función para iniciar nueva conversación
async function startNewConversation() {
    try {
        if (currentChat.messages.length > 0) {
            const saved = await saveCurrentConversation();
            if (!saved) {
                showError('No se pudo guardar la conversación actual');
                return false;
            }
        }

        resetCurrentChat();
        console.log("Chat actual reseteado");

        const response = await fetch(financialAdviceUrls.startNew, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        });

        const data = await response.json();
        console.log("Respuesta de nuevo chat:", data);

        if (data.success) {
            currentChat.id = data.recomendacionId;
            console.log("Nuevo chat creado con ID:", currentChat.id);
            await loadRecomendationHistory();
            showSuccess('Nuevo chat creado');
            return true;
        } else {
            showError(data.message || 'Error al crear nuevo chat');
            return false;
        }
    } catch (error) {
        console.error('Error en startNewConversation:', error);
        showError('Error al iniciar nueva conversación');
        return false;
    }
}

// Función para mostrar éxito
function showSuccess(message) {
    const successElement = document.createElement('div');
    successElement.classList.add('success-message');
    successElement.innerHTML = `
        <i class="fas fa-check-circle"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(successElement);

    setTimeout(() => {
        successElement.remove();
    }, 3000);
}

// Función para mostrar error
function showError(message) {
    const errorElement = document.createElement('div');
    errorElement.classList.add('error-message');
    errorElement.innerHTML = `
        <i class="fas fa-exclamation-circle"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(errorElement);

    setTimeout(() => {
        errorElement.remove();
    }, 3000);
}

// Función para cargar el historial de recomendaciones
function loadRecomendationHistory() {
    const $container = $('.history-scroll-container');
    $container.html('<div class="loading">Cargando...</div>');

    $.get(financialAdviceUrls.getHistory)
        .done(function (data) {
            $container.empty();
            const chats = data.recomendations?.$values || data.recomendations || [];

            if (data.success && chats.length > 0) {
                chats.slice(0, 50).forEach(chat => {
                    const previewText = chat.preview?.length > 25
                        ? chat.preview.substring(0, 25) + "..."
                        : chat.preview || "Nuevo chat";

                    const $item = $(`
                        <div class="history-item" data-recomendacionid="${chat.recomendacionId}">
                            <i class="fas fa-comment-alt"></i>
                            <div class="history-content">
                                <div class="history-title">${previewText}</div>
                                <div class="history-date">${formatDate(chat.fechaGeneracion)}</div>
                            </div>
                        </div>
                    `).click(() => loadRecomendation(chat.recomendacionId));

                    if (currentChat.id === chat.recomendacionId) {
                        $item.addClass('active');
                    }

                    $container.append($item);
                });
            } else {
                $container.html(`
                    <div class="history-placeholder">
                        <i class="fas fa-comments"></i>
                        <span>No hay conversaciones recientes</span>
                    </div>
                `);
            }
        })
        .fail(function () {
            $container.html(`
                <div class="error-message">
                    <i class="fas fa-exclamation-triangle"></i>
                    <span>Error al cargar el historial</span>
                </div>
            `);
        });
}

// Función para cargar una recomendación existente
// Versión mejorada de loadRecomendation
function loadRecomendation(recomendacionId) {
    $('#chatMessages').html('<div class="loading-message">Cargando...</div>');

    $.get(financialAdviceUrls.getMessages, { recomendacionId: recomendacionId })
        .done(function (data) {
            console.log('Server response:', data);

            // Validate response structure more thoroughly
            if (!data || typeof data !== 'object') {
                throw new Error('Invalid server response format');
            }

            if (!data.success) {
                throw new Error(data.message || 'Request was not successful');
            }

            // Extract messages from different possible structures
            let messages = [];

            // Case 1: messages.$values exists
            if (data.messages?.$values && Array.isArray(data.messages.$values)) {
                messages = data.messages.$values;
            }
            // Case 2: messages is directly an array
            else if (Array.isArray(data.messages)) {
                messages = data.messages;
            }
            // Case 3: messages exists but is empty/null
            else {
                messages = [];
            }

            // Reset UI and chat state
            welcomeMessage.style.display = 'none';
            chatConversation.style.display = 'flex';
            chatMessages.innerHTML = '';

            // Create NEW chat object (don't mutate existing one)
            currentChat = {
                id: recomendacionId,
                messages: []
            };

            // Process messages with validation
            if (messages && messages.length > 0) {
                messages.forEach(msg => {
                    try {
                        if (msg && typeof msg === 'object') {
                            const content = msg.contenido || msg.Content || '';
                            const isUser = msg.esUsuario || msg.IsUser || false;
                            if (content) {
                                addMessage(content, isUser ? 'user' : 'system');
                            }
                        }
                    } catch (e) {
                        console.error('Error processing message:', msg, e);
                    }
                });
            } else {
                console.log('No messages found for this chat');
                addMessage('No hay mensajes en esta conversación', 'system');
            }

            // Update active state
            $('.history-item').removeClass('active');
            $(`.history-item[data-recomendacionid="${recomendacionId}"]`).addClass('active');
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            console.error('Error loading messages:', textStatus, errorThrown);
            $('#chatMessages').html(`
                <div class="error-message">
                    <i class="fas fa-exclamation-triangle"></i>
                    Error al cargar la conversación: ${errorThrown || 'Error desconocido'}
                </div>
            `);
        });
}
// Función para manejar la búsqueda en el historial
function setupHistorySearch() {
    $('.search-input').on('input', function () {
        const searchTerm = $(this).val().toLowerCase();
        $('.history-item').each(function () {
            const text = $(this).text().toLowerCase();
            $(this).toggle(text.includes(searchTerm));
        });
    });
}

// Event listeners
$(document).ready(function () {
    // Inicializar historial
    loadRecomendationHistory();
    setupHistorySearch();

    // Botón de nuevo chat
    $('.new-chat-button').click(async function () {
        await startNewConversation();
    });

    // Acciones rápidas
    $('.action-button').click(async function () {
        const question = $(this).data('question');

        if (!question || question.trim() === '') {
            showError('La pregunta no puede estar vacía');
            return;
        }

        if (!currentChat.id) {
            await startNewConversation();
        }

        welcomeMessage.style.display = 'none';
        chatConversation.style.display = 'flex';
        addMessage(question, 'user');

        try {
            const response = await fetch(financialAdviceUrls.getAdvice, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({
                    pregunta: question.trim(),
                    recomendacionId: currentChat.id
                })
            });

            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            const data = await response.json();

            if (data.success) {
                addMessage(data.message, 'system');
                loadRecomendationHistory();
            } else {
                showError(data.message || 'Error al procesar la respuesta');
            }
        } catch (error) {
            console.error('Error al obtener la respuesta:', error);
            showError('Hubo un error al procesar tu solicitud');
        }
    });
});