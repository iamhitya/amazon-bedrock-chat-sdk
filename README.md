# Amazon Bedrock Chat SDK (.NET 9 + Semantic Kernel)

## Overview
This repository contains a minimal .NET 9 console app that uses the official Microsoft Semantic Kernel with Amazon Bedrock for AI-powered text and streaming chat completion. It registers the `amazon.titan-text-premier-v1:0` model and shows both standard and streaming chat loops.

## Key Features
- Bedrock chat completion integration via Semantic Kernel service registration
- Maintains conversational `ChatHistory`
- Standard response and token streaming examples
- Clean DI setup using `Host.CreateApplicationBuilder`

## Project Structure
- `Program.cs` – Application entry point, service registration, chat loops
- `Amazon.Bedrock.Chat.SDK.csproj` – .NET 9 project file

## Prerequisites
- .NET 9 SDK installed
- AWS account with access to Amazon Bedrock and Titan model
- Valid AWS credentials available to the application (e.g. environment variables, shared credentials file, or configured IAM role)

## Configuration
The model is registered in `Program.cs`:
```csharp
builder.Services.AddBedrockChatCompletionService("amazon.titan-text-premier-v1:0");
```
If you need a different model, replace the model ID above with one you are entitled to use.

Ensure AWS credentials are resolvable by the default SDK chain. Typical environment variables:
```bash
AWS_ACCESS_KEY_ID=YOUR_KEY
AWS_SECRET_ACCESS_KEY=YOUR_SECRET
AWS_REGION=us-east-1
```
(Region must match your Bedrock endpoint availability.)

## Build & Run
```bash
dotnet restore
dotnet build
dotnet run --project AI.Tutorial/Amazon.Bedrock.Chat.SDK.csproj
```
You will enter messages after the `You:` prompt. Press Enter on an empty line to exit each loop.

## How It Works
1. Host builder creates a DI container.
2. Bedrock chat completion service is registered.
3. A `Kernel` is created and retrieved from DI.
4. First loop: non-streaming responses using `GetChatMessageContentAsync`.
5. Second loop: streaming responses using `GetStreamingChatMessageContentsAsync`.