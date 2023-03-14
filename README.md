# ChatBotNewGeneration

The project is divided in 4 parts

    - API: Contains .Net Identity, Signal R, JWT authentication
    - BOT: Keep monitoring all chats to check if any command is received, contains Signal R client and RabbitMQ
    - Consumer: Consume the queue and send information to the api, contains RabbitMQ Client
    - UI: Authenticate and Register new User, create room and chat

## To Run Docker

To run the application in docker please type docker-compose up --build inside the project folder.
    
    -The UI is hosted at http://localhost:7147/
    -The API is hosted at https://localhost:7203/
