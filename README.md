# SendGrid Mock API

A simple, lightweight mock for the Twilio SendGrid API. This application provides a drop-in replacement for the SendGrid `v3/mail/send` endpoint, allowing you to test email sending in your applications without actually sending emails. It includes a web UI to view, search, and inspect the intercepted emails.

## Features

- **SendGrid API Compatibility**: Mocks the `POST /v3/mail/send` endpoint.
- **Web UI**: View sent emails, inspect headers, and preview HTML/Text content.
- **Attachment Support**: Handles and allows downloading of email attachments.
- **Persistence**: Supports both in-memory and file-based storage (for persistence across restarts).
- **Authentication**: Validates SendGrid API Keys (if configured) to simulate production security.
- **Message IDs**: Returns SendGrid-style `X-Message-ID` headers.

## Getting Started

### Prerequisites

- Docker
- Or .NET 10.0 SDK for local development

### Using Docker

The easiest way to run SendGrid Mock is using the Docker image.

```bash
docker run -d -p 5110:8080 \
  -e SendGridApiKey="your_test_api_key" \
  mdi1984/sendgrid-mock
```

You can then access the UI at [http://localhost:5110](http://localhost:5110) and configure your application to use `http://localhost:5110` as the SendGrid host.

#### Persistence

To persist emails across container restarts, mount a volume to `/app/data` and set the `MailStoragePath` environment variable.

```bash
docker run -d -p 5110:8080 \
  -e SendGridApiKey="your_test_api_key" \
  -e MailStoragePath="/app/data" \
  -v sendgrid_data:/app/data \
  mdi1984/sendgrid-mock
```

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `SendGridApiKey` | The API Key to validate against `Authorization` header. | `null` (No auth) |
| `MailStoragePath` | Path to store email JSON files. If unset, uses **In-Memory** storage. | `null` |

## Local Development

1. Clone the repository.
2. Navigate to the `src` directory.
3. Run the application:

```bash
dotnet run --project SendGridMock/SendGridMock.csproj
```

The application will start on the configured port (defaulting to http://localhost:5110 in development).

## API Usage

Configure your SendGrid client to point to this service:

**Host**: `http://localhost:5110`  
**API Key**: Matches `SendGridApiKey` env var (e.g., `your_test_api_key`)

Example cURL:

```bash
curl --request POST \
  --url http://localhost:5110/v3/mail/send \
  --header 'Authorization: Bearer your_test_api_key' \
  --header 'Content-Type: application/json' \
  --data '{
    "personalizations": [
      {
        "to": [{"email": "recipient@example.com"}]
      }
    ],
    "from": {"email": "sender@example.com"},
    "subject": "Hello, World!",
    "content": [{"type": "text/plain", "value": "This is a test!"}]
  }'
```
