# Data Security Project
ASP.NET Core web app for securely storing user passwords.

## Table of contents
* [Motivation](#motivation)
* [General information](#general-information)
* [Technologies](#technologies)
* [Security measures](#security-measures)
* [Setup](#setup)
* [Sample views](#Sample-views)

## Motivation
This project was created as a study task for the subject "Data Security in Information Technology Systems".  
Thanks to this project I learned a lot of practical things related to application security.

## General information
A project is a website that allows the user to store his passwords.
It offers the following features:

- Adding new secrets
- Viewing passwords to secrets
- Registration and logging in
- Viewing recent account activity
- Password reset

## Technologies

- C# 9
- .NET 5
- Docker
- MSSQL
- EF Core 5

### Used libraries and packages

- IpInfo | [website](https://ipinfo.io/)
- Wangkanai.Detection | [github repository](https://github.com/wangkanai/Wangkanai)
- Swagger

## Security measures
The project includes the following security measures:
- Encrypting user passwords
- Hashing user master password
- Encrypted connection over HTTPS
- Verification of failed login attempts
- Operation delays (to prevent brute-force attacks)
- Protection against CSRF attacks
- Content-Security-Policy mechanism
- Modification of the Server header

## Setup
To set up a website, follow these steps:

1. Clone repository from github
2. Build solution
3. Provide the 3 needed files with environment variables

- db.env
```
ACCEPT_EULA=Y
SA_PASSWORD=Your-Password
TZ=Your-Timezone
```
Timezone must be provided in [database time zone format](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)

- api.env

```
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://+:443;http://+:80
JWTKEY=Your-JWT-KEY
AESKEY=Some-32-Bits-Key
PurposeForTokenProviderProtector=Some-String
TZ=Your-Timezone
DbServerName=db
DatabaseName=AppDbContext
DbUser=Db-user
DbPassword=Your-password
```

- ui.env

```
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://+:443;http://+:80
PurposeForPasswordResetControllerProtector=Some-String
IpInfoToken=Your-token-for-ipInfo-api
CertificatePassword=Certificate-Password
CertificateFileLocation=/https/aspnetapp.pfx
ApiUrl=https://api/
TZ=Your-Timezone
```

6. In .NET Core CLI run  
  `docker compose up`  
  and open app on https://localhost:5042  
  or/and just run app in your IDE

## Sample views
<p align="center">
    <b>Registration page</b><br>  
    <img src="/images/registration-page.png">  
    <b>Last connections page</b><br>  
    <img src="/images/last-connections-page.png">  
    <b>Secrets page</b><br>  
    <img src="/images/secrets-page.png">  
</p>
