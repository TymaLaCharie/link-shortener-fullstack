==============================================
====Link Shortener & Analytics Dashboard======

This project is a full-stack URL shortening application built in a modern web development practices. 
The application allows authenticated users to convert long URLs into short, manageable links and view analytics on how many times those links have been clicked.
The entire application is containerized with Docker, ensuring a consistent development and deployment environment that can be launched with a single command.

Key Features
URL Shortening: A secure endpoint for authenticated users to submit a long URL and receive a unique short code.
Redirection & Click Logging: A public-facing redirect service that logs each click before sending the user to the original destination.
User Authentication: A complete JWT-based authentication system for user registration and login.
Analytics Dashboard: A simple, clean dashboard for logged-in users to view all their created links and the total click count for each.
Containerized Environment: The entire application (backend, frontend, database) is managed by Docker Compose for a one-command setup.


&&&&&
%%Technical Choices & Architecture
The technologies for this project were chosen to create a robust, modern, and scalable application.
Backend: .NET 8 Web API
.NET is a high-performance, open-source framework ideal for building resilient and secure web APIs.

Entity Framework Core: Used as the Object-Relational Mapper (ORM). We employed a Database-First approach, where the database schema was defined in database/init.sql. 
This script serves as the single source of truth for our data structure, and EF Core was used to scaffold the C# models from this live database.

Frontend: Next.js 14 & TypeScript
Next.js offers a fantastic developer experience with features like Server-Side Rendering (SSR) for fast initial page loads and a file-based App Router for intuitive routing.
TypeScript adds static typing to JavaScript, which is invaluable for catching errors during development, improving code quality, and making the application easier to maintain and scale.
Styling: Tailwind CSS was chosen for its utility-first approach, allowing for rapid and consistent UI development directly within the markup.

Database: Microsoft SQL Server
As a relational database, it enforces data integrity through schemas and foreign key constraints, which is perfect for the structured data in this application (Users, URLs, Clicks). 
It integrates seamlessly with .NET's Entity Framework Core.

Containerization: Docker & Docker Compose
Docker solves the "it works on my machine" problem. By containerizing each part of the application (backend, frontend, database), we guarantee that the environment is identical for every developer, every time. 
This is the cornerstone of the project's portability and ease of setup.
Docker Compose acts as the orchestrator, defining how these isolated containers should be built, configured, and networked together to function as a single, cohesive application.


&&&&&&&
&&Instructions to Run the Application
This project is designed to be run with a single command.
Docker Desktop: You must have Docker Desktop installed and running on your system.

Step 1: Get the Code
Clone the repository to your local machine:
Generated bash
git clone <>
cd <>

Step 2: Launch the Application
From the root directory of the project (the one containing the docker-compose.yml file), run the following command in your terminal:
docker-compose up --build

Step 3: Access the Application
Once the logs in your terminal slow down and indicate the services are running, you can access the application:
Frontend User Interface: http://localhost:3000
Backend API (Swagger UI): http://localhost:8080/swagger
Stopping the Application
Go to the terminal where the application is running and press Ctrl + C.
To remove the containers and network, run: docker-compose down.