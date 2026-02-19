# 📘 CheckFlow Reports

CheckFlow Reports é uma aplicação desktop desenvolvida em **.NET 8**, utilizando **Avalonia UI** e **QuestPDF**, responsável por gerar **relatórios em PDF** a partir dos dados exportados pelo aplicativo mobile **CheckFlow**.

O objetivo do projeto é transferir o processamento pesado do dispositivo móvel para o desktop, garantindo uma experiência mais rápida e confiável em campo.

---

## ✨ Funcionalidades

Atualmente, o CheckFlow Reports permite:

- Selecionar um arquivo ZIP exportado pelo app mobile
- Ler e validar o arquivo `metadata.json`
- Verificar a existência das fotos associadas aos itens
- Identificar automaticamente:
	- Fotos ausentes no dispositivo durante a exportação
	- Fotos removidas manualmente do ZIP
- Gerar um **relatório final em PDF** contendo:
	- Informações do checklist
	- Itens
	- Evidências fotográficas
- Salvar o PDF na mesma pasta do ZIP original

---

## 🧱 Escopo do Projeto

Este repositório contém **exclusivamente a aplicação desktop**.

- ❌ Não coleta dados
- ❌ Não captura fotos
- ❌ Não possui autenticação
- ❌ Não possui sincronização em nuvem

Toda a **coleta de dados em campo** é responsabilidade do aplicativo mobile (**CheckFlow**).

O foco aqui é:

> **processamento estruturado dos dados exportados e geração de relatórios em PDF.**

---

## 📦 Fluxo de Funcionamento

1. O usuário exporta um checklist no aplicativo mobile em formato ZIP
2. O ZIP é selecionado no CheckFlow Reports
3. O conteúdo é extraído para uma pasta temporária
4. O arquivo `metadata.json` é lido para identificar:
	- Checklist
	- Itens
	- Fotos associadas
5. A existência de cada foto é validada
6. O relatório em PDF é gerado usando **QuestPDF**
7. O PDF final é salvo na mesma pasta do ZIP

---

## 🏗️ Arquitetura

O projeto foi estruturado com foco em **clareza, manutenção e separação de responsabilidades**, seguindo uma organização inspirada em **Clean Architecture**.

Mesmo estando atualmente em um único projeto, as camadas já estão claramente separadas:

- **Domain** — modelos e regras centrais
- **Application** — contratos e interfaces
- **Infrastructure** — acesso a arquivos, ZIP, JSON e geração de PDF
- **UI** — Avalonia + ViewModels

Essa organização facilita futuras evoluções sem adicionar complexidade prematura.

---

## 🛠️ Tecnologias Utilizadas

- .NET 8
- Avalonia UI
- QuestPDF
- SixLabors.ImageSharp (redimensionamento e otimização de imagens para geração de PDF)

---

## 🎯 Objetivo

Fornecer uma ferramenta desktop:

- simples
- rápida
- confiável

para transformar dados coletados em campo em **relatórios profissionais em PDF**.
