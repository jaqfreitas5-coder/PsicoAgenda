# Criar README.md com seus dados
cat > README.md << 'EOF'
# PsicoAgenda - Sistema de Agendamento para Psicólogos

Sistema desenvolvido com ❤️ para auxiliar psicólogos no gerenciamento de consultas e comunicação com pacientes.

## 🎯 Objetivo
Automatizar o processo de agendamento e enviar lembretes automáticos via WhatsApp 15 minutos antes de cada sessão, melhorando a organização e reduzindo faltas.

## ✨ Funcionalidades Principais
- **Agendamento Inteligente** - Cadastro de pacientes e consultas
- **Lembretes Automáticos** - WhatsApp 15 minutos antes de cada sessão  
- **Interface Simples** - Fácil de usar no dia a dia
- **Seguro** - Credenciais protegidas no Azure

## 🚀 Como Funciona
1. Cadastro do paciente com número WhatsApp
2. Agendamento da consulta com data/hora
3. Sistema envia lembrete automático 15min antes
4. Paciente chega no horário! ✅

## 🛠 Tecnologias
- **Backend**: .NET 8 API + Entity Framework
- **Frontend**: HTML, CSS, JavaScript
- **Mensagens**: Twilio WhatsApp API
- **Cloud**: Azure App Service + SQL Database
- **CI/CD**: GitHub Actions

## 📞 Contato
**Desenvolvedor**: Jaqueline Freitas  
**Telefone**: (17) 98205-5602  
**Email**: jaqfreitas5@gmail.com

Desenvolvido com carinho para facilitar o trabalho dos psicólogos! 

*"A tecnologia a serviço do cuidado com a saúde mental"* 💙
EOF
