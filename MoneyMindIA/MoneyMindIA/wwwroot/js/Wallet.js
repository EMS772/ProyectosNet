// Función para formatear el número de tarjeta
function formatCardNumber(value) {
    let v = value.replace(/\D/g, '');
    v = v.replace(/(\d{4})(?=\d)/g, '$1 ');
    return v;
}

// Función para formatear la fecha de expiración
function formatCardExpiry(value) {
    let v = value.replace(/\D/g, '');
    if (v.length > 2) {
        v = v.slice(0, 2) + '/' + v.slice(2, 4);
    }
    return v;
}

// Función para cargar la tarjeta vinculada
async function cargarTarjetaVinculada() {
    try {
        const response = await fetch('/Wallet/ObtenerTarjetaVinculada', {
            method: 'GET',
            credentials: 'include'
        });

        if (!response.ok) {
            throw new Error('Error al obtener la tarjeta vinculada');
        }

        const result = await response.json();

        if (result.success && result.tarjetaVinculada) {
            document.getElementById('linkedCard').style.display = 'block';
            document.getElementById('cardInputForm').style.display = 'none';
            document.getElementById('addCardSection').style.display = 'none';
            document.getElementById('paypalSection').style.display = 'none';

            document.getElementById('linkedCardNumber').textContent = '**** **** **** ' + result.tarjeta.numeroTarjeta.slice(-4);
            document.getElementById('linkedCardName').textContent = result.tarjeta.nombreTitular;
            document.getElementById('linkedCardBalance').textContent = `Saldo: $${result.tarjeta.saldo.toFixed(2)}`;
        } else {
            document.getElementById('linkedCard').style.display = 'none';
            document.getElementById('cardInputForm').style.display = 'none';
            document.getElementById('addCardSection').style.display = 'block';
            document.getElementById('paypalSection').style.display = 'block';
        }
    } catch (error) {
        console.error('Error al cargar la tarjeta vinculada:', error);
    }
}

// Función para actualizar el balance
async function actualizarBalance() {
    try {
        const response = await fetch('/Wallet/ObtenerTarjetaVinculada', {
            method: 'GET',
            credentials: 'include'
        });

        if (!response.ok) {
            throw new Error('Error al obtener la tarjeta vinculada');
        }

        const result = await response.json();

        if (result.success && result.tarjetaVinculada) {
            const balanceAmount = document.getElementById('balanceAmount');
            balanceAmount.textContent = `$${result.tarjeta.saldo.toFixed(2)}`;
        } else {
            const balanceAmount = document.getElementById('balanceAmount');
            balanceAmount.textContent = '$0.00';
        }
    } catch (error) {
        console.error('Error al actualizar el balance:', error);
    }
}

// Evento al cargar la página
document.addEventListener('DOMContentLoaded', () => {
    cargarTarjetaVinculada();
    actualizarBalance();
});

// Evento para el botón de vincular PayPal
document.getElementById('addPaypalButton').addEventListener('click', function () {
    const paypalPlaceholder = document.getElementById('paypalPlaceholder');
    const linkedAccount = document.getElementById('linkedAccount');

    paypalPlaceholder.style.display = 'none';
    linkedAccount.style.display = 'block';
});

// Evento para el botón de vincular tarjeta
document.getElementById('addCardButton').addEventListener('click', function () {
    const paypalSection = document.getElementById('paypalSection');
    const cardInputForm = document.getElementById('cardInputForm');

    paypalSection.style.display = 'none';
    cardInputForm.style.display = 'block';
    addCardButton.style.display = 'none';
});

// Evento para el botón de enviar tarjeta
document.getElementById('submitCardButton').addEventListener('click', async () => {
    const cardName = document.getElementById('cardName').value.trim();
    const cardNumber = document.getElementById('cardNumber').value.replace(/\s/g, '');
    const cardExpiry = document.getElementById('cardExpiry').value.trim();
    const cardCVC = document.getElementById('cardCVC').value.trim();

    if (!cardName || !cardNumber || !cardExpiry || !cardCVC) {
        alert('Por favor completa todos los campos');
        return;
    }

    if (cardName.length > 100) {
        alert('El nombre del titular no debe exceder 100 caracteres');
        return;
    }

    if (!isValidCreditCard(cardNumber)) {
        alert('Por favor ingresa un número de tarjeta válido');
        return;
    }

    const expiryRegex = /^(0[1-9]|1[0-2])\/?([0-9]{2})$/;
    if (!expiryRegex.test(cardExpiry)) {
        alert('La fecha de expiración debe tener el formato MM/AA');
        return;
    }

    if (!/^\d{3}$/.test(cardCVC)) {
        alert('El CVV debe tener exactamente 3 dígitos');
        return;
    }

    const formData = {
        NombreTitular: cardName,
        NumeroTarjeta: cardNumber,
        FechaExpiracion: cardExpiry,
        CVV: cardCVC
    };

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    try {
        const response = await fetch('/Wallet/AgregarTarjeta', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(formData)
        });

        const contentType = response.headers.get('content-type');
        if (!contentType || !contentType.includes('application/json')) {
            const textResponse = await response.text();
            throw new Error('La respuesta del servidor no es JSON');
        }

        const result = await response.json();

        if (!response.ok) {
            const errorMessage = result && result.message
                ? result.message
                : (result && result.errors ? result.errors.join(', ') : 'Error en la solicitud');
            throw new Error(errorMessage);
        }

        // Mostrar la tarjeta vinculada
        document.getElementById('linkedCard').style.display = 'block';
        document.getElementById('cardInputForm').style.display = 'none';
        document.querySelector('.card-number').textContent = '**** **** **** ' + formData.NumeroTarjeta.slice(-4);
        document.querySelector('.card-name').textContent = formData.NombreTitular;
        document.getElementById('linkedCardBalance').textContent = `Saldo: $${result.tarjeta.saldo.toFixed(2)}`;

        // Actualizar el balance en el dashboard
        await actualizarBalance();

        alert(result.message);

    } catch (error) {
        console.error('Error:', error);
        alert(`Error: ${error.message}`);
    }
});

// Función para validar el número de tarjeta
function isValidCreditCard(number) {
    number = number.replace(/[\s-]/g, '');
    if (!/^\d+$/.test(number)) return false;
    if (number.length < 13 || number.length > 19) return false;

    let sum = 0;
    let doubleUp = false;

    for (let i = number.length - 1; i >= 0; i--) {
        let digit = parseInt(number.charAt(i));

        if (doubleUp) {
            digit *= 2;
            if (digit > 9) {
                digit -= 9;
            }
        }

        sum += digit;
        doubleUp = !doubleUp;
    }

    return (sum % 10) === 0;
}

// Evento para el botón de desvincular tarjeta
document.getElementById('unlinkCardButton').addEventListener('click', async () => {
    try {
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const response = await fetch('/Wallet/DesvincularTarjeta', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            }
        });

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error response:', errorText);
            throw new Error(`Error al desvincular la tarjeta: ${response.status} ${response.statusText}`);
        }

        const result = await response.json();

        if (result.success) {
            document.getElementById('linkedCard').style.display = 'none';
            document.getElementById('cardInputForm').style.display = 'none';
            document.getElementById('addCardSection').style.display = 'block';

            // Actualizar el balance en el dashboard
            await actualizarBalance();

            alert(result.message);
        } else {
            console.error('Error en la respuesta:', result.message);
            alert('Error: ' + result.message);
        }
    } catch (error) {
        console.error('Error al desvincular la tarjeta:', error);
        alert(`Error: ${error.message}`);
    }
});