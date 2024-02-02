# Azure Functions Serverless API for File Exfiltration

## Introduction

This Azure Functions serverless API is developed for the purpose of demonstrating secure file handling, exfiltration techniques, and storage solutions in cloud environments. It's designed to prepend the current date to filenames, record the transaction in Cosmos DB, and store the files securely in Azure Blob Storage. This project is intended for educational use and security research within legal and ethical boundaries.

## Features

- **Date Modification**: Automatically modifies file names by prepending the current date, facilitating better file management and tracking.
- **Cosmos DB Integration**: Seamlessly creates records in Cosmos DB for each file processed, ensuring data traceability and integrity.
- **Azure Blob Storage**: Utilizes Azure Blob Storage for the secure and scalable storage of files after processing.

## Dependencies

The API is built with the following Azure services and requires certain environment variables for configuration:

- Azure Functions
- Azure Cosmos DB
- Azure Blob Storage

### Required Environmental Variables

- `COSMOS_DB_CONNECTION_STRING`: Your Cosmos DB connection string.
- `COSMOS_DB_NAME`: The name of your Cosmos DB database.
- `COSMOS_DB_CONTAINER_NAME`: The name of your Cosmos DB container.
- `AZURE_BLOB_CONNECTION_STRING`: Your Azure Blob Storage connection string.
- `AZURE_BLOB_NAME`: The name of your Azure Blob container.

## Configuration

### Environment Setup

To configure the serverless API, you'll need to set up the required environment variables within your Azure Functions application. This involves:

1. **Accessing the Azure Portal**: Log in to your Azure Portal and navigate to your Azure Functions app.
2. **Configuring Settings**: Go to the "Configuration" section, and add the environmental variables listed above. Ensure their values are correctly set according to your Azure Cosmos DB and Blob Storage accounts.

### Preparing Azure Services

- **Cosmos DB**: Set up a Cosmos DB account, database, and container, and retrieve your connection string.
- **Azure Blob Storage**: Create a Blob Storage account and container, and obtain the connection string.

## Getting Started

1. **Repository Setup**: Clone this repository to your development environment.
2. **Environment Configuration**: Apply the necessary configuration by setting up the environment variables as described in the Configuration section.
3. **Deployment**: Deploy the function app to Azure using the Azure CLI, Azure Portal, or Visual Studio Code.
4. **Testing**: Test the functionality by uploading a file to the function's endpoint and verifying the file's presence in Blob Storage and its corresponding record in Cosmos DB.

## Usage

This API is designed to be triggered via HTTP requests. When a file is sent to the function's endpoint, the API processes the file according to the described features and securely stores the file in Azure Blob Storage, with metadata recorded in Cosmos DB.

## Legal and Ethical Considerations

This project is intended strictly for educational and ethical use in security research. Users must ensure all activities conducted with this API comply with applicable laws, regulations, and ethical standards. Unauthorized use of this software for data exfiltration or any form of malicious activity is prohibited.


