# it-school

## Project idea

- It-School is a dynamic platform that bridges the gap between professionals and students, enabling skill enhancement, career advancement, and income growth. The platform offers diverse functionalities, including:
- Real Projects: Professionals host projects, allowing students to collaborate as real-world engineers, gaining invaluable skills.
- Courses: Professionals create skill-boosting courses, accessible to students for free or at minimal fees.
- Events: Users can organize both offline and online events, such as discussions, seminars, or workshops. Professionals host these events to share their knowledge and experiences, providing valuable insights to students and junior professionals.
- Appointments: Schedule one-on-one meetings with professionals, either in person or virtually. These appointments serve as a platform for guidance, mentorship, and skill-building. Whether you need advice or a deeper understanding of a specific subject, these sessions offer personalized learning experiences.
- Online Interviews: Simulate real interviews, helping students prepare effectively."

## How to build

- We built the core platform with .net core 8, EF core 8, and [asp.net core API ](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
- For the web application, we're using [next 13](https://nextjs.org/) and [the Chakra UI](https://chakra-ui.com/) framework

# Prepare development env

- Install Sdk 8.0 https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- Updarade dotnet tool
  `dotnet tool update --global dotnet-ef --version 8.0.100
`
- At this phase, we only ensure the api will work on Postgresql. And plan to support Oracle, MySql, MariaDB, and SqlServer after completing all the user stories.
- Run PostgreSQL with docker-compose `https://github.com/chuongxl/dev-awesome/blob/main/docker-compose/postgres/compose.yml`
- Config PostgreSQL connection string

`"ConnectionStrings": {
    "Provider": "Postgresql",
    "Postgresql": "Host=127.0.0.1; Port=5432;;Database=it_school; Username=sa; Password=123456!@"
  }`

- Run the API
  `cd CleanArchitecture.Presentation.Api && dotnet restore && dotnet run --launch-profile https`
- Use dotnet jwt to get JWT token for local test. For more information on the dotnet user-jwts tool, read the complete documentation https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows
- `dotnet user-jwts create`

- `dotnet user-jwts create --scope "admin_api" --claim "email=admin@gmail.com" --role "Admin" --scheme Bearer --issuer https://it-school.vn` - for admin api

- `dotnet user-jwts create --scope "user_api" --claim "email=user@gmail.com" --role "User" --scheme Bearer --issuer https://it-school.vn` - for user api

- Test the jwt token
- `curl -i -H "Authorization: Bearer {token}" https://localhost:{port}/user`

# run migration

- Change setting ConnectionStrings:Provider to Database provider you want to generate migration

## SQL Server

- `dotnet ef migrations add InitialCreate --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Presentation --context SqlServerContext`

## Postgresql

- `dotnet ef migrations add InitialCreate --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Presentation --context PostgresqlContext`

## MySql

- `dotnet ef migrations add InitialCreate --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Presentation --context MySqlContext`

## MariaDb

- Verions : 11.0.3
- Change setting ConnectionStrings: Provider to MariaDb
- `dotnet ef migrations add InitialCreate --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Presentation --context MariaDbContext`

## Oracle

- `dotnet ef migrations add InitialCreate --project CleanArchitecture.Infrastructure --startup-project CleanArchitecture.Presentation --context OracleContext`

# Auth

## With Google.

- Following this document to setup the google Auth : https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-6.0#create-the-google-oauth-20-client-id-and-secret
- Add setting
-

```
"Authentication": {
    "DefaultScheme": "LocalAuthIssuer",
    "Schemes": {
      "Bearer": {
        "ValidAudiences": [
          "http://localhost:5025",
          "https://localhost:7133"
        ],
        "ValidIssuer": "dotnet-user-jwts"
      },
      "Google.Bearer": {
        "ValidAudiences": [
          "http://localhost:5025",
          "https://localhost:7133"
        ],
        "ValidIssuer": "accounts.google.com"
      },
      "Microsoft.Bearer": {
        "ValidAudiences": [
          "http://localhost:5025",
          "https://localhost:7133"
        ],
        "ValidIssuer": "login.microsoftonline.com/common/v2.0"
      },

    },
    "Google":{
      "ClientId":"",
      "ClientSecret":"",
      "DiscoveryUrl":"https://accounts.google.com/.well-known/openid-configuration"
    },
     "Microsoft": {
      "ClientId": "",
      "ClientSecret": "",
      "DiscoveryUrl": "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration"

    },
    "Github":{
      "ClientId":"",
      "ClientSecret":"",
      "DiscoveryUrl":""
      "ApiUrl":"https://api.github.com"
    }
  }
```

## Config Email service

- Add this setting to Appsetting

```
"EmailService": {
    "Provider": "UseSendGrid",
    "SendGrid": {
        "FromEmail":"",
        "ApiKey":""
    },
    "Brevo":{
      "FromEmail":"",
       "ApiKey":"",
       "ApiUrl":"https://api.brevo.com/v3"
    },
    "Smtp": {
        "FromEmail":"",
        "Host":"smtp.gmail.com",
        "Port":578,
        "UserName":"",
        "Password":""
    }
  }
```

## Configuration - the encryption key

`"Encryption":{
  "Key":"",
  "Salt":""
}`

## Configuration - The Domain

`"Hosting":{
  "Domain":"",
  "AbsoluteUri"
}`

## Configuration system setting

```
"SystemSetting":{
  "OwnerEmail":"",
  "EventFeeRate":"",
  "AppointmentFeeRate":"",
  "MonthlyFee":""
}
```

## Naming convention

- Command starts with these words (Create, Update, Delete). Sample: CreateProject, UpdateProject,UpdateProjectStatus
- Query starts with these words (Get, List). Sample :GetProject, ListProject
- The suffix for model object:
  - Entity: for the database entity
  - Dto: For data transfer object. This object can be used for the request model or response model
  - Request: Only used for command or query class
  - Response: Only used for the return result of query or command. Following the request-response pattern.

## Debug

- skill the process on Mac: kill -9 $(lsof -ti:7133)
- get the process id on mac: lsof -nP -iTCP -sTCP:LISTEN | grep 7133
- Run the project with the command dotnet run or dotnet watch
- Using vs code attach .net debug to start debugging.

## note for dev

- Init blazor client
  `dotnet new blazor -o CleanArchitecture.Presentation.Balzor -ai false -int auto`
