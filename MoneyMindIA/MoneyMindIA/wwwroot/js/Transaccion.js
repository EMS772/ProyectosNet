// Mostrar popup
document.getElementById('btnAddTransaction').addEventListener('click', function () {
    document.getElementById('popupOverlay').style.display = 'flex';
});

// Ocultar popup al cancelar
document.getElementById('btnCancel').addEventListener('click', function () {
    document.getElementById('popupOverlay').style.display = 'none';
});

async function cargarTransacciones() {
    try {
        console.log("Cargando transacciones...");

        const response = await fetch('/Wallet/ObtenerTransacciones', {
            method: 'GET',
            credentials: 'include'
        });

        console.log("Respuesta recibida:", response);

        if (!response.ok) {
            const errorText = await response.text();
            console.error('Error response:', errorText);
            throw new Error(`Error al obtener las transacciones: ${response.status} ${response.statusText}`);
        }

        const result = await response.json();
        console.log("Resultado:", result);

        if (result.success) {
            const transactionsList = document.getElementById('transactionsList');
            transactionsList.innerHTML = ''; // Limpiar la lista

            // Access the transactions array from the $values property
            const transacciones = result.transacciones.$values || [];

            if (transacciones.length > 0) {
                transacciones.forEach(transaccion => {
                    const transactionItem = document.createElement('div');
                    transactionItem.className = 'transaction-item';
                    transactionItem.innerHTML = `
                            <p><strong>Descripción:</strong> ${transaccion.descripcion || 'Sin descripción'}</p>
                            <p><strong>Monto:</strong> $${transaccion.monto.toFixed(2)}</p>
                            <p><strong>Fecha:</strong> ${new Date(transaccion.fecha).toLocaleDateString()}</p>
                            <p><strong>Tipo:</strong> ${transaccion.tipo === 0 ? 'Ingreso' : 'Gasto'}</p>
                            <p><strong>Categoría:</strong> ${transaccion.categoriaNombre || 'Sin categoría'}</p>
                        `;
                    transactionsList.appendChild(transactionItem);
                });
            } else {
                transactionsList.innerHTML = `
                        <div class="empty-state">
                            <i class="fas fa-coins"></i>
                            <p>No hay transacciones recientes</p>
                        </div>
                    `;
            }
        } else {
            console.error('Error en la respuesta:', result.message);
            throw new Error(result.message);
        }
    } catch (error) {
        console.error('Error al cargar las transacciones:', error);

        // Show a more user-friendly error message
        const transactionsList = document.getElementById('transactionsList');
        transactionsList.innerHTML = `
                <div class="empty-state error">
                    <i class="fas fa-exclamation-triangle"></i>
                    <p>No se pudieron cargar las transacciones. Inténtalo de nuevo más tarde.</p>
                </div>
            `;
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
            throw new Error('Error al obtener el balance');
        }

        const result = await response.json();

        if (result.success && result.tarjetaVinculada) {
            document.getElementById('balanceAmount').textContent = `$${result.tarjeta.saldo.toFixed(2)}`;
        } else {
            document.getElementById('balanceAmount').textContent = '$0.00';
        }
    } catch (error) {
        console.error('Error al actualizar el balance:', error);
        alert('Error al actualizar el balance. Inténtalo de nuevo.');
    }
}

// Llamar a las funciones al cargar la página
document.addEventListener('DOMContentLoaded', () => {
    cargarTransacciones();
    actualizarBalance();
});

document.getElementById('transactionForm').addEventListener('submit', async (e) => {
    e.preventDefault();

    // Obtener los datos del formulario
    const tipoTransaccionElement = document.getElementById('tipoTransaccion');
    const tipoTransaccionValue = tipoTransaccionElement.value;
    const tipoTransaccionEnum = tipoTransaccionValue === "Ingreso" ? 0 : 1;

    const formData = {
        Monto: parseFloat(document.getElementById('monto').value),
        Fecha: document.getElementById('fecha').value,
        Descripcion: document.getElementById('descripcion').value,
        Tipo: tipoTransaccionEnum, // Enviar el valor entero correspondiente al enum
        CategoriaId: parseInt(document.getElementById('categoria').value)
    };

    console.log('Datos de la transacción a enviar:', formData); // Debug log

    // Mostrar confirmación con SweetAlert
    const confirmacion = await Swal.fire({
        title: '¿Estás seguro?',
        text: '¿Deseas agregar esta transacción?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Sí, agregar',
        cancelButtonText: 'Cancelar',
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
    });

    // Si el usuario confirma, proceder con la solicitud
    if (confirmacion.isConfirmed) {
        try {
            const response = await fetch('/Wallet/AgregarTransaccion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
                credentials: 'include'
            });

            const result = await response.json();

            if (response.ok && result.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Transacción agregada exitosamente',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    document.getElementById('popupOverlay').style.display = 'none';
                    document.getElementById('transactionForm').reset();
                    cargarTransacciones();
                    actualizarBalance();
                });
            } else {
                console.error('Error del servidor:', result);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Error: ' + result.message +
                        (result.error ? '\n\nDetalles: ' + result.error : ''),
                    confirmButtonText: 'OK'
                });
            }
        } catch (error) {
            console.error('Error en la solicitud:', error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error al agregar la transacción. Inténtalo de nuevo.',
                confirmButtonText: 'OK'
            });
        }
    } else {
        // Si el usuario cancela, mostrar un mensaje opcional
        Swal.fire({
            icon: 'info',
            title: 'Operación cancelada',
            text: 'La transacción no fue agregada.',
            showConfirmButton: false,
            timer: 1500
        });
    }
});