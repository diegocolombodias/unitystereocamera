# Unity Stereo Camera

Este repositório contém uma implementação modular para configuração de câmeras estéreo e gerenciamento de múltiplos displays no Unity. O projeto é voltado para aplicações que exigem projeção assimétrica (asymmetric frustum), comum em sistemas de realidade virtual e visualização científica.

## 🚀 Componentes do Projeto

O projeto é composto pelos seguintes scripts principais:

### 🎥 Sistema de Câmera
* **`StereoCameraSetup.cs`**: O núcleo do sistema. Este script calcula a matriz de projeção assimétrica para as câmeras. Ele utiliza um `pivot` como referência e aplica um *offset* para criar o efeito estéreo real sem distorções de convergência (off-axis).
* **`DisplayManager.cs`**: Essencial para setups de hardware estéreo (como monitores duplos ou projetores). Ele ativa automaticamente o segundo e o terceiro display detectados pelo sistema ao iniciar a aplicação.

### 🧊 Scripts de Utilidade (Teste)
* **`CubeMovement.cs`**: Permite mover objetos na cena usando as setas do teclado ou as teclas WASD, facilitando o teste da percepção de profundidade.
* **`CubeRotation.cs`**: Aplica uma rotação contínua em torno do eixo Y para verificar a estabilidade do efeito estéreo em objetos em movimento.

---

## 🛠️ Guia de Configuração

Siga estes passos para configurar a câmera estéreo na sua cena:

### 1. Preparação da Hierarquia
1. Crie um objeto vazio chamado `CameraPivot`.
2. Dentro dele, crie duas câmeras: `LeftCamera` e `RightCamera`.
3. Adicione o script `StereoCameraSetup.cs` ao objeto pai ou a um controlador de cena.

### 2. Configuração do Script Stereo
No Inspector do script `StereoCameraSetup`: Este deve estar no mesmo obejto do DisplayManager
* Arraste o objeto `CameraPivot` para o campo **Pivot**.
* O script aplicará a função `ApplyStereoProjection` para ajustar as matrizes de projeção de ambas as câmeras com base no deslocamento lateral (IPD).

### 3. Gerenciamento de Displays
Para utilizar múltiplos monitores:
1. Crie um objeto vazio chamado `DisplayManager`.
2. Anexe o script `DisplayManager.cs`.
3. No Unity, configure o **Target Display** de cada câmera (Ex: Câmera Esquerda -> Display 1, Câmera Direita -> Display 2).

### 4. Objetos de Teste
Para testar a cena:
1. Crie um Cubo.
2. Adicione os scripts `CubeMovement.cs` e `CubeRotation.cs` para interagir com o objeto durante o Play Mode.

---

## 🔬 Contexto de Pesquisa

Este projeto foi desenvolvido com foco em:
* **Visão Computacional:** Geração de pares de imagens para algoritmos de disparidade.
* **Interação Humano-Computador:** Estudos sobre fadiga visual em ambientes 3D.
* **Aplicações Acadêmicas:** Ferramenta de suporte para disciplinas de computação gráfica e estatística aplicada à visão.

---

## 📄 Licença

Este projeto está sob a licença MIT.

---
**Desenvolvido por [Diego Roberto Colombo Dias](https://github.com/diegocolombodias)**
*Departamento de Estatística (DEST) - Universidade Federal do Espírito Santo (UFES)*
