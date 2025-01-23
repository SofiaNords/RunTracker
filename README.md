# RunTracker

RunTracker is an application designed to help users log their running sessions efficiently. Users can record their runs by entering the date, distance, duration, and run type. The application allows users to create custom run types, which can be selected when registering new running sessions.

RunTracker offers full CRUD (Create, Read, Update, Delete) functionality for both RunningSessions and RunTypes, ensuring comprehensive data management. The application automatically creates a MongoDB database, provided that the user has entered their MongoDB connection string in the User Secrets.

## Features

- Log running sessions with date, distance, duration, and run type.
- Create and manage custom run types.
- Full CRUD functionality for RunningSessions and RunTypes.
- Automatic MongoDB database creation.

## Technologies Used

### Languages

- C#

### Database

- MongoDB

### Libraries

- FontAwesome.Sharp
- Microsoft.Extensions.Configuration.UserSecrets
- MongoDB C# Driver

### Tools

- MongoDB Compass
- Visual Studio Community

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/RunTracker.git
    cd RunTracker
    ```

2. Open the solution in Visual Studio.

3. Restore the NuGet packages:
    ```bash
    dotnet restore
    ```

## Configuration

1. Add your MongoDB connection string to the User Secrets:
    ```bash
    dotnet user-secrets set "MongoDB:ConnectionString" "your_connection_string"
    ```

2. Ensure MongoDB is running on your machine or accessible via the connection string.

## Usage

1. Run the application from Visual Studio or using the .NET CLI:
    ```bash
    dotnet run
    ```

2. Use the application to log your running sessions and manage run types.

