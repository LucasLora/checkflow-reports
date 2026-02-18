# CheckFlow Reports

CheckFlow Reports é uma aplicação desktop desenvolvida em **.NET 8**, utilizando **Avalonia UI** e **QuestPDF**, responsável pela geração de relatórios em PDF a partir de dados exportados por um aplicativo mobile.

## Visão Geral

O aplicativo mobile exporta um **arquivo ZIP** contendo:

- Fotos associadas aos itens de um checklist  
- Um arquivo `metadata.json` com a estrutura do checklist, seus itens e a relação entre itens e imagens  

O CheckFlow Reports consome esse ZIP no desktop, processa os dados e gera um relatório em PDF com base nessas informações.

Essa abordagem foi escolhida para evitar processamento pesado no dispositivo móvel, transferindo a geração do relatório para um ambiente mais adequado, como o computador.

## Fluxo de Funcionamento

1. O usuário exporta um checklist no aplicativo mobile em formato ZIP  
2. O ZIP é selecionado no aplicativo desktop  
3. O conteúdo é descompactado em uma pasta temporária  
4. O arquivo `metadata.json` é lido para identificar:
   - O checklist exportado  
   - Os itens que o compõem  
   - As fotos associadas a cada item  
5. O relatório em PDF é gerado utilizando o **QuestPDF**  
6. O PDF final é salvo na mesma pasta onde o arquivo ZIP original está localizado  

Durante esse processo, o aplicativo lida corretamente com:

- Fotos removidas antes da exportação  
- Fotos removidas manualmente de dentro do ZIP  
- Estruturas de checklist inválidas ou inconsistentes no `metadata.json`  

## Organização do Projeto

O projeto segue uma separação de pastas inspirada em **Clean Code / Clean Architecture**, mesmo estando atualmente concentrado em um único projeto.

A estrutura foi organizada desde o início para facilitar uma futura separação em múltiplos projetos, se necessário, sem adicionar complexidade desnecessária neste momento.

Exemplos de camadas organizadas:

- Domain
- Application  
- Infrastructure  
- UI

## Tecnologias Utilizadas

- .NET 8  
- Avalonia UI  
- QuestPDF  

---
