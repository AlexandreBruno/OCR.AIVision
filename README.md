# OCR AI VISION

Este projeto foi desenvolvido para fins de estudo e utiliza **.NET 8**, **DDD (Domain-Driven Design)** e **Minimal API**. Ele permite o processamento de imagens utilizando o **AI Vision da Oracle**.

## 📦 Tecnologias e Dependências

O projeto utiliza os seguintes pacotes para autenticação e envio das imagens para análise:

- `OCI.DotNetSDK.Aivision`
- `OCI.DotNetSDK.Common`

## ⚙️ Configuração

No arquivo `appsettings.json`, preencha a chave `CompartmentId` dentro da seção `OracleSettings`:

```json
{
  "OracleSettings": {
    "CompartmentId": "seu-compartment-id-aqui"
  }
}
```

Além disso, é necessário configurar os arquivos de autenticação da Oracle:

1. **Arquivo `oci.config`**:
   ```ini
   [DEFAULT]
   user=
   fingerprint=
   tenancy=
   region=
   key_file=oci.pem
   ```

2. **Arquivo `oci.pem`**: Deve ser preenchido com a chave privada utilizada para autenticação.

## 🔄 Fluxo das Requisições

As respostas da API retornam um objeto `OCRResponse`, que contém a propriedade `CorrelationId`. Esse identificador deve ser informado nas requisições subsequentes para rastrear e acompanhar o fluxo entre microsserviços.

## 🛠️ Endpoints Disponíveis

Foram desenvolvidos dois endpoints para o envio de imagens para análise:

### 1️⃣ **Processar Imagem Base64**

- **Rota:** `POST /OCR/ProcessarImagemDeStringBase64`
- **Descrição:** Recebe uma string Base64 contendo a imagem a ser analisada.
- **Exemplo de requisição:**
  ```json
  {
    "imagemBase64": ""
  }
  ```

### 2️⃣ **Processar Imagem de Arquivo**

- **Rota:** `POST /OCR/ProcessarImagemDeArquivo`
- **Descrição:** Recebe um arquivo de imagem para análise.
- **Formatos suportados:** `jpg`, `jpeg`, `png`, `bmp`, `gif`.
- **Exemplo de requisição:**
  ```http
  Content-Type: multipart/form-data
  ```

## 🚀 Como Executar

1. Clone o repositório e abra o **Visual Studio**.
2. No Visual Studio, clique em **Abrir um projeto ou solução** e selecione o arquivo `.sln` do projeto.
3. Restaure as dependências:
   - No **Gerenciador de Soluções**, clique com o botão direito no projeto e selecione **Restaurar Pacotes NuGet**.
4. Configure as credenciais no `appsettings.json`, `oci.config` e `oci.pem`.
5. Para rodar o projeto, clique em **Iniciar (F5)** ou selecione o perfil desejado e execute o projeto.

## 🐳 Executando com Docker

Para rodar a aplicação utilizando Docker, siga os passos abaixo:

1. **Construa a imagem Docker:**
   ```sh
   docker build -t ocr-ai-vision .
   ```

2. **Execute o contêiner:**
   ```sh
   docker run -p 5000:5000 --name ocr-container ocr-ai-vision
   ```

Agora, a API estará disponível em: [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

## 📜 Licença

Este projeto foi desenvolvido para fins de estudo e não possui uma licença específica.

---

Se encontrar algum ponto de melhoria, sinta-se livre para me avisar! 😊

