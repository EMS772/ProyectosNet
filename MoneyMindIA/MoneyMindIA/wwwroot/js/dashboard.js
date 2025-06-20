// dashboard.js
document.addEventListener('DOMContentLoaded', function() {
    // Cargar datos iniciales
    loadTransactions();
    loadGoals();
    
    // Event listeners
    document.getElementById('goalForm').addEventListener('submit', handleGoalSubmit);
});

function loadTransactions() {
    // Simulación de datos de transacciones
    const transactions = [
        { name: 'SHEIN LLC', type: 'ACCESORIO X', amount: 15.00 },
        { name: 'Carlos Castro', type: 'TRANSFERENCIA', amount: 300.00 },
        { name: 'TEMU TZ', type: 'ROPA Y ACC', amount: 40.00 },
        { name: 'AMAZON USA', type: 'ELECTRO.', amount: 170.00 }
    ];
    
    const transactionsList = document.querySelector('.transactions-list');
    transactions.forEach(transaction => {
        const div = document.createElement('div');
        div.className = 'transaction-item';
        div.innerHTML = `
            <span>${transaction.name}</span>
            <span>${transaction.type}</span>
            <span>$${transaction.amount.toFixed(2)}</span>
        `;
        transactionsList.appendChild(div);
    });
}

function loadGoals() {
    // Simulación de datos de metas
    const goals = [
        'REDUCIR GASTOS EN ROPA',
        'AHORRAR XX'
    ];
    
    const goalsList = document.querySelector('.goals-list');
    goals.forEach(goal => {
        const div = document.createElement('div');
        div.className = 'goal-item';
        div.textContent = goal;
        goalsList.appendChild(div);
    });
}

function handleGoalSubmit(e) {
    e.preventDefault();
    const form = e.target;
    const description = form.querySelector('input[type="text"]').value;
    const startDate = form.querySelector('input[type="date"]:nth-of-type(1)').value;
    const endDate = form.querySelector('input[type="date"]:nth-of-type(2)').value;
    
    console.log('Nueva meta:', { description, startDate, endDate });
    // Aquí iría la lógica para guardar la meta
    
    form.reset();
}