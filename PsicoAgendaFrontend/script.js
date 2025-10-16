// Configuração
const API_BASE_URL = 'https://localhost:7044/api';

// Estado da Aplicação
let appState = {
    token: null,
    currentPage: 'dashboard',
    patients: [],
    appointments: []
};

// === TESTE SEGURO DE COMUNICAÇÃO ===
function setupDebugTest() {
    // Adicionar botão de teste seguro
    const testButton = document.createElement('button');
    testButton.textContent = '🧪 Testar Conexão API';
    testButton.style.cssText = `
        position: fixed;
        bottom: 20px;
        right: 20px;
        padding: 10px 15px;
        background: #4a6fa5;
        color: white;
        border: none;
        border-radius: 5px;
        cursor: pointer;
        z-index: 10000;
        font-size: 12px;
    `;
    
    testButton.onclick = async function() {
        console.log('🧪 Iniciando teste de conexão...');
        await testAPIConnection();
    };
    
    document.body.appendChild(testButton);
}

async function testAPIConnection() {
    console.clear();
    console.log('🚀 ===== TESTE DE CONEXÃO COM API =====');
    
    // Teste 1: Health Check
    console.log('1. 🏥 Testando Health Check...');
    try {
        const healthResponse = await fetch(`${API_BASE_URL}/health/status`);
        console.log('   📡 Status:', healthResponse.status);
        console.log('   ✅ OK:', healthResponse.ok);
        
        if (healthResponse.ok) {
            const healthData = await healthResponse.json();
            console.log('   📊 Dados:', healthData);
            console.log('   🎯 Health Check: ✅ SUCESSO');
        } else {
            console.log('   ❌ Health Check: FALHOU');
        }
    } catch (error) {
        console.log('   💥 Health Check ERRO:', error.message);
    }
    
    // Teste 2: Login
    console.log('2. 🔐 Testando Login...');
    try {
        const loginResponse = await fetch(`${API_BASE_URL}/Auth/login`, {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                email: 'admin@psico.com',
                password: 'Senha123!'
            }),
        });
        
        console.log('   📡 Status:', loginResponse.status);
        console.log('   ✅ OK:', loginResponse.ok);
        console.log('   📝 Status Text:', loginResponse.statusText);
        
        if (loginResponse.ok) {
            const loginData = await loginResponse.json();
            console.log('   📊 Dados:', loginData);
            console.log('   🎯 Login: ✅ SUCESSO');
            console.log('   🔑 Token Recebido:', loginData.token ? 'SIM' : 'NÃO');
            
            // Se funcionou, mostrar sucesso visual
            alert('✅ Teste bem-sucedido! Login funcionando.\nToken: ' + (loginData.token ? 'Recebido' : 'Não recebido'));
        } else {
            console.log('   ❌ Login: FALHOU');
            try {
                const errorData = await loginResponse.json();
                console.log('   📋 Erro da API:', errorData);
                alert('❌ Login falhou: ' + (errorData.message || 'Erro desconhecido'));
            } catch (e) {
                const errorText = await loginResponse.text();
                console.log('   📋 Erro (sem JSON):', errorText);
                alert('❌ Login falhou. Status: ' + loginResponse.status);
            }
        }
    } catch (error) {
        console.log('   💥 Login ERRO:', error.message);
        console.log('   🔍 Tipo do erro:', error.name);
        alert('💥 Erro de conexão: ' + error.message);
    }
    
    console.log('🎯 ===== FIM DO TESTE =====');
}

// Inicialização
document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 Aplicação inicializada');
    setupDebugTest();
    initializeApp();
});

function initializeApp() {
    checkAuthentication();
    setupEventListeners();
}

function checkAuthentication() {
    const token = sessionStorage.getItem('jwtToken');
    console.log('🔐 Verificando autenticação:', token ? 'Token encontrado' : 'Sem token');
    
    if (token) {
        appState.token = token;
        showMainSystem();
        loadInitialData();
    } else {
        showLoginPage();
    }
}

function setupEventListeners() {
    // Login
    const loginForm = document.getElementById('login-form');
    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    }
    
    // Logout
    const logoutButton = document.getElementById('logout-button');
    if (logoutButton) {
        logoutButton.addEventListener('click', handleLogout);
    }
    
    // Navegação
    document.querySelectorAll('.nav-item').forEach(item => {
        item.addEventListener('click', function(e) {
            switchPage(e.currentTarget.dataset.page);
        });
    });
    
    // Quick Actions
    document.querySelectorAll('.action-btn').forEach(btn => {
        btn.addEventListener('click', function(e) {
            const page = e.currentTarget.dataset.page;
            if (page) {
                switchPage(page);
            }
        });
    });
    
    // Formulários
    const patientForm = document.getElementById('add-patient-form');
    if (patientForm) {
        patientForm.addEventListener('submit', handleAddPatient);
    }
    
    const appointmentForm = document.getElementById('add-appointment-form');
    if (appointmentForm) {
        appointmentForm.addEventListener('submit', handleAddAppointment);
    }
    
    // Botões de Ação
    const addPatientBtn = document.getElementById('add-patient-btn');
    if (addPatientBtn) {
        addPatientBtn.addEventListener('click', function() { switchPage('patients'); });
    }
    
    const addAppointmentBtn = document.getElementById('add-appointment-btn');
    if (addAppointmentBtn) {
        addAppointmentBtn.addEventListener('click', function() { switchPage('appointments'); });
    }
    
    const cancelPatientBtn = document.getElementById('cancel-patient');
    if (cancelPatientBtn) {
        cancelPatientBtn.addEventListener('click', function() { switchPage('patients'); });
    }
    
    const cancelAppointmentBtn = document.getElementById('cancel-appointment');
    if (cancelAppointmentBtn) {
        cancelAppointmentBtn.addEventListener('click', function() { switchPage('appointments'); });
    }
}

// --- AUTENTICAÇÃO ---
async function handleLogin(e) {
    e.preventDefault();
    console.log('🔐 Tentativa de login iniciada');
    
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;
    const errorElement = document.getElementById('login-error');
    
    console.log('📧 Email:', email);
    console.log('🔑 Password:', password ? '***' : 'vazio');
    
    // Validar campos
    if (!email || !password) {
        if (errorElement) {
            errorElement.textContent = 'Please fill in all fields';
        }
        return;
    }
    
    // Mostrar loading
    const button = e.target.querySelector('.signin-button');
    if (button) {
        const buttonText = button.querySelector('.button-text');
        const originalText = buttonText.textContent;
        buttonText.textContent = 'Signing In...';
        button.disabled = true;
    }
    
    try {
        console.log('🌐 Fazendo requisição para API...');
        console.log('📡 URL:', `${API_BASE_URL}/Auth/login`);
        
        const response = await fetch(`${API_BASE_URL}/Auth/login`, {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({ 
                email: email.trim(), 
                password: password 
            }),
        });

        console.log('📡 Resposta da API:', {
            status: response.status,
            statusText: response.statusText,
            ok: response.ok
        });

        if (response.ok) {
            const data = await response.json();
            console.log('✅ Login bem-sucedido:', data);
            
            if (data.token) {
                sessionStorage.setItem('jwtToken', data.token);
                appState.token = data.token;
                showMainSystem();
                loadInitialData();
                showNotification('Login successful! Welcome back!', 'success');
            } else {
                throw new Error('No token received from API');
            }
        } else {
            let errorMessage = 'Invalid email or password';
            
            // Tentar obter mensagem de erro da API
            try {
                const errorData = await response.json();
                errorMessage = errorData.message || errorMessage;
                console.log('❌ Erro da API:', errorData);
            } catch (parseError) {
                console.log('❌ Erro de parse:', parseError);
            }
            
            if (errorElement) {
                errorElement.textContent = errorMessage;
            }
            showNotification(errorMessage, 'error');
        }
    } catch (error) {
        console.error('💥 Erro no login:', error);
        
        let userMessage = 'Connection error. Please check:';
        userMessage += '\n• API is running on port 7044';
        userMessage += '\n• CORS is configured';
        userMessage += '\n• Network connection';
        
        if (errorElement) {
            errorElement.textContent = userMessage;
        }
        showNotification('Network error - check console for details', 'error');
    } finally {
        // Restaurar botão
        if (button) {
            const buttonText = button.querySelector('.button-text');
            buttonText.textContent = 'Sign In';
            button.disabled = false;
        }
    }
}

function handleLogout() {
    console.log('🚪 Fazendo logout...');
    sessionStorage.removeItem('jwtToken');
    appState.token = null;
    appState.patients = [];
    appState.appointments = [];
    showLoginPage();
    showNotification('You have been signed out', 'info');
}

// --- NAVEGAÇÃO ---
function switchPage(pageName) {
    console.log('🔄 Mudando para página:', pageName);
    
    // Atualizar navegação
    document.querySelectorAll('.nav-item').forEach(item => {
        item.classList.remove('active');
    });
    
    const activeNav = document.querySelector(`[data-page="${pageName}"]`);
    if (activeNav) {
        activeNav.classList.add('active');
    }
    
    // Esconder todas as páginas
    document.querySelectorAll('.system-page').forEach(page => {
        page.classList.add('hidden');
    });
    
    // Mostrar página selecionada
    const targetPage = document.getElementById(pageName);
    if (targetPage) {
        targetPage.classList.remove('hidden');
    }
    
    appState.currentPage = pageName;
    
    // Carregar dados específicos da página
    if (pageName === 'patients') {
        loadPatients();
    } else if (pageName === 'appointments') {
        loadAppointments();
    } else if (pageName === 'dashboard') {
        updateDashboardStats();
    }
}

// --- FUNÇÕES DE API ---
async function secureFetch(url, options = {}) {
    console.log('🌐 secureFetch:', url, options);
    
    const headers = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        ...options.headers,
    };

    if (appState.token) {
        headers['Authorization'] = `Bearer ${appState.token}`;
        console.log('🔐 Token incluído na requisição');
    }

    try {
        const response = await fetch(url, { ...options, headers });
        console.log('📡 Resposta secureFetch:', {
            url: url,
            status: response.status,
            statusText: response.statusText
        });

        if (response.status === 401) {
            console.log('🚫 401 Unauthorized - fazendo logout');
            handleLogout();
            throw new Error('Unauthorized');
        }

        return response;
    } catch (error) {
        console.error('💥 Erro no secureFetch:', error);
        throw error;
    }
}

// --- GERENCIAMENTO DE DADOS ---
async function loadInitialData() {
    console.log('📦 Carregando dados iniciais...');
    try {
        await Promise.all([
            loadPatients(),
            loadAppointments()
        ]);
        updateDashboardStats();
        console.log('✅ Dados iniciais carregados com sucesso');
    } catch (error) {
        console.error('❌ Erro ao carregar dados iniciais:', error);
    }
}

async function loadPatients() {
    console.log('👥 Carregando pacientes...');
    try {
        const response = await secureFetch(`${API_BASE_URL}/Pacientes`);
        console.log('📊 Resposta pacientes:', response);
        
        if (response.ok) {
            const patients = await response.json();
            console.log('✅ Pacientes carregados:', patients.length);
            appState.patients = patients;
            renderPatients();
            populatePatientSelect();
        } else {
            console.error('❌ Erro ao carregar pacientes:', response.status);
            showNotification('Error loading patients', 'error');
        }
    } catch (error) {
        console.error('💥 Erro ao carregar pacientes:', error);
        showNotification('Error loading patients', 'error');
    }
}

async function loadAppointments() {
    console.log('📅 Carregando consultas...');
    try {
        const response = await secureFetch(`${API_BASE_URL}/Consultas`);
        console.log('📊 Resposta consultas:', response);
        
        if (response.ok) {
            const appointments = await response.json();
            console.log('✅ Consultas carregadas:', appointments.length);
            appState.appointments = appointments;
            renderAppointments();
        } else {
            console.error('❌ Erro ao carregar consultas:', response.status);
            showNotification('Error loading appointments', 'error');
        }
    } catch (error) {
        console.error('💥 Erro ao carregar consultas:', error);
        showNotification('Error loading appointments', 'error');
    }
}

// --- RENDERIZAÇÃO ---
function renderPatients() {
    console.log('🎨 Renderizando pacientes...');
    const container = document.getElementById('patient-list');
    if (!container) return;
    
    if (appState.patients.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-icon">👥</div>
                <p>No patients registered yet</p>
            </div>
        `;
        return;
    }

    container.innerHTML = appState.patients.map(patient => `
        <div class="list-item" data-patient-id="${patient.id}">
            <div class="list-item-header">
                <span class="list-item-title">${patient.nome}</span>
                <span class="list-item-meta">
                    <span>📞 ${patient.telefone}</span>
                </span>
            </div>
            <div class="list-item-description">
                <div>📱 ${patient.whatsApp || 'No WhatsApp'}</div>
                <div>✉️ ${patient.email || 'No email'}</div>
                ${patient.observacoes ? `<div>📝 ${patient.observacoes}</div>` : ''}
            </div>
        </div>
    `).join('');
}

function renderAppointments() {
    console.log('🎨 Renderizando consultas...');
    const container = document.getElementById('appointment-list');
    if (!container) return;
    
    if (appState.appointments.length === 0) {
        container.innerHTML = `
            <div class="empty-state">
                <div class="empty-icon">📅</div>
                <p>No appointments scheduled</p>
            </div>
        `;
        return;
    }

    // Ordenar por data mais próxima
    const sortedAppointments = [...appState.appointments].sort((a, b) => 
        new Date(a.dataHora) - new Date(b.dataHora)
    );

    container.innerHTML = sortedAppointments.map(appointment => {
        const appointmentDate = new Date(appointment.dataHora);
        const now = new Date();
        const timeDiff = appointmentDate - now;
        const hoursDiff = timeDiff / (1000 * 60 * 60);
        
        let status = 'scheduled';
        let statusText = 'Scheduled';
        
        if (hoursDiff < 0) {
            status = 'completed';
            statusText = 'Completed';
        } else if (hoursDiff < 24) {
            status = 'soon';
            statusText = 'Soon';
        }
        
        return `
            <div class="list-item" data-appointment-id="${appointment.id}">
                <div class="list-item-header">
                    <span class="list-item-title">${appointment.pacienteNome}</span>
                    <span class="status-badge ${status}">${statusText}</span>
                </div>
                <div class="list-item-description">
                    <div>📅 ${formatDateTime(appointment.dataHora)}</div>
                    <div>${appointment.observacoes || 'No additional notes'}</div>
                    ${hoursDiff > 0 && hoursDiff < 24 ? 
                        `<div class="reminder-note">🔔 WhatsApp reminder in ${Math.ceil(hoursDiff * 60)} minutes</div>` : ''}
                </div>
            </div>
        `;
    }).join('');
}

function populatePatientSelect() {
    console.log('📋 Populando select de pacientes...');
    const select = document.getElementById('appointment-patient-select');
    if (!select) return;
    
    select.innerHTML = '<option value="">Choose a patient...</option>';
    
    appState.patients.forEach(patient => {
        const option = document.createElement('option');
        option.value = patient.id;
        option.textContent = patient.nome;
        select.appendChild(option);
    });
}

function updateDashboardStats() {
    console.log('📊 Atualizando estatísticas...');
    
    const totalPatients = document.getElementById('total-patients');
    const todayAppointments = document.getElementById('today-appointments');
    const pendingReminders = document.getElementById('pending-reminders');
    
    if (totalPatients) {
        totalPatients.textContent = appState.patients.length;
    }
    
    const todayCount = appState.appointments.filter(apt => {
        const aptDate = new Date(apt.dataHora);
        const today = new Date();
        return aptDate.toDateString() === today.toDateString();
    }).length;
    
    if (todayAppointments) {
        todayAppointments.textContent = todayCount;
    }
    
    const pendingCount = appState.appointments.filter(apt => {
        const aptDate = new Date(apt.dataHora);
        const now = new Date();
        const timeDiff = aptDate - now;
        return timeDiff > 0 && timeDiff <= 24 * 60 * 60 * 1000; // Próximas 24h
    }).length;
    
    if (pendingReminders) {
        pendingReminders.textContent = pendingCount;
    }
}

// --- FORM HANDLERS ---
async function handleAddPatient(e) {
    e.preventDefault();
    console.log('➕ Adicionando paciente...');
    
    const formData = {
        nome: document.getElementById('patient-name').value,
        telefone: document.getElementById('patient-phone').value,
        email: document.getElementById('patient-email').value,
        whatsApp: document.getElementById('patient-whatsapp').value,
        dataNascimento: document.getElementById('patient-birthdate').value,
        observacoes: document.getElementById('patient-notes').value
    };

    console.log('📝 Dados do paciente:', formData);

    try {
        const response = await secureFetch(`${API_BASE_URL}/Pacientes`, {
            method: 'POST',
            body: JSON.stringify(formData),
        });

        if (response.ok) {
            showNotification('Patient registered successfully!', 'success');
            document.getElementById('add-patient-form').reset();
            await loadPatients();
            updateDashboardStats();
        } else {
            showNotification('Error registering patient', 'error');
        }
    } catch (error) {
        console.error('💥 Erro ao adicionar paciente:', error);
        showNotification('Error registering patient', 'error');
    }
}

async function handleAddAppointment(e) {
    e.preventDefault();
    console.log('🕐 Agendando consulta...');
    
    const pacienteId = document.getElementById('appointment-patient-select').value;
    const dataHora = document.getElementById('appointment-datetime').value;

    console.log('📅 Dados da consulta:', { pacienteId, dataHora });

    if (!pacienteId) {
        showNotification('Please select a patient', 'error');
        return;
    }

    const formData = {
        pacienteId: parseInt(pacienteId),
        dataHora: dataHora,
        observacoes: document.getElementById('appointment-notes').value,
        confirmada: true
    };

    try {
        const response = await secureFetch(`${API_BASE_URL}/Consultas`, {
            method: 'POST',
            body: JSON.stringify(formData),
        });

        if (response.ok) {
            showNotification('Appointment scheduled successfully!', 'success');
            document.getElementById('add-appointment-form').reset();
            await loadAppointments();
            updateDashboardStats();
            
            // WhatsApp será enviado automaticamente pelo background service
            showNotification('WhatsApp reminder will be sent 15 minutes before the session', 'info');
        } else {
            showNotification('Error scheduling appointment', 'error');
        }
    } catch (error) {
        console.error('💥 Erro ao agendar consulta:', error);
        showNotification('Error scheduling appointment', 'error');
    }
}

// --- FUNÇÕES UTILITÁRIAS ---
function formatDateTime(dateTimeString) {
    const date = new Date(dateTimeString);
    return date.toLocaleString('en-US', {
        weekday: 'short',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function showNotification(message, type = 'info') {
    console.log(`💬 Notificação [${type}]:`, message);
    
    // Criar notificação temporária
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.textContent = message;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 12px 20px;
        border-radius: 8px;
        color: white;
        font-weight: 500;
        z-index: 1000;
        animation: slideInRight 0.3s ease-out;
        max-width: 400px;
        word-wrap: break-word;
    `;
    
    if (type === 'success') {
        notification.style.background = '#10b981';
    } else if (type === 'error') {
        notification.style.background = '#ef4444';
    } else if (type === 'info') {
        notification.style.background = '#3b82f6';
    }
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        if (notification.parentNode) {
            notification.parentNode.removeChild(notification);
        }
    }, 4000);
}

function showLoginPage() {
    console.log('👤 Mostrando página de login');
    const loginPage = document.getElementById('login-page');
    const mainSystem = document.getElementById('main-system');
    
    if (loginPage) loginPage.classList.remove('hidden');
    if (mainSystem) mainSystem.classList.add('hidden');
}

function showMainSystem() {
    console.log('🏠 Mostrando sistema principal');
    const loginPage = document.getElementById('login-page');
    const mainSystem = document.getElementById('main-system');
    
    if (loginPage) loginPage.classList.add('hidden');
    if (mainSystem) mainSystem.classList.remove('hidden');
}

// CSS para animação da notificação
const style = document.createElement('style');
style.textContent = `
    @keyframes slideInRight {
        from {
            opacity: 0;
            transform: translateX(100%);
        }
        to {
            opacity: 1;
            transform: translateX(0);
        }
    }
    
    .status-badge {
        padding: 4px 8px;
        border-radius: 12px;
        font-size: 0.8rem;
        font-weight: 600;
    }
    
    .status-badge.scheduled {
        background: #dbeafe;
        color: #1e40af;
    }
    
    .status-badge.soon {
        background: #fef3c7;
        color: #92400e;
    }
    
    .status-badge.completed {
        background: #d1fae5;
        color: #065f46;
    }
    
    .reminder-note {
        background: #fef3c7;
        padding: 4px 8px;
        border-radius: 6px;
        font-size: 0.8rem;
        margin-top: 8px;
        display: inline-block;
    }
`;
document.head.appendChild(style);