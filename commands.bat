@echo off
REM - In the following the commands to create a solution along with the projects of AppForMovies are presented.


REM - First, CLONE YOUR REPO, preferably in c:/repos
REM - Second, copy this file to the folder where your repo has been cloned 
REM - Third, replace AppForMovies in the next line with the name (WITHOUT WHITESPACES) of the product you are going to develop 
REM - Fourth, run this .bat in the command line
REM - Commit and push the changes done

SET PROJECT_NAME=AppForSEII2526
if PROJECT_NAME=="" SET PROJECT_NAME=AppForMovies

REM - Fourth, run commands.bat opening a command line interface like cmd and writting commands

SET NETCORE_VERSION=net8.0
SET NETCORE_LIB_VERSION=8.0.16
SET WEB_CODE_GENERATION_LIB_VERSION=8.0.6
SET SWASHBUCKLE_VERSION=8.1.4
SET DATA_ANNOTATIONS_LIB_VERSION=3.2.0-rc1.20223.4
SET XUNIT_VERSION=2.9.3
SET XUNIT_RUNNER_VERSION=3.1.1
SET MOQ_VERSION=4.20.72
SET SELENIUM_VERSION=4.33.0
SET CHROME_DRIVER_VERSION=137.0.7151.6800
SET COVERLET_VERSION=6.0.4




@echo *********************************************************
@echo.                                                               
@echo       PROJECT: %PROJECT_NAME%   
@echo. 
@echo *********************************************************

REM - Create your solution
dotnet new sln --name "%PROJECT_NAME%"

@echo ----------------------------REQ------------------------------------------------

REM - Create the folder for requirements Documents
md req
cd req
REM -----------------------  add a file for explaining the goal of this folder
echo This folder must be used to upload your use case documents >  readme.txt

cd ..

@echo ----------------------------SRC------------------------------------------------

REM - Create the folder for the source code and create the projects
md src
cd src

REM -------- create the API project with authorization using framework 8.0 --------

dotnet new  webapi -au none -f %NETCORE_VERSION% -n "%PROJECT_NAME%".API


REM -----------------------  add packages to the project
cd "%PROJECT_NAME%".API
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Tools --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version %WEB_CODE_GENERATION_LIB_VERSION%
dotnet add package Microsoft.AspNetCore.Components.DataAnnotations.Validation --version %DATA_ANNOTATIONS_LIB_VERSION%


REM -----------------------  add a file for the design of the Class Diagram
echo ^<?xml version="1.0" encoding="utf-8"?^>^<ClassDiagram^>^</ClassDiagram^> >  ClassDiagram.cd

cd ..

REM -------- create the Web Project with authorization, interactive mode and SQL Server using framework 8.0 ------------
dotnet new  blazor -au Individual --all-interactive -uld -f %NETCORE_VERSION% -n "%PROJECT_NAME%".Web


REM ----------------------- add packages to the project 
cd "%PROJECT_NAME%".Web
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version %WEB_CODE_GENERATION_LIB_VERSION%
cd ..

REM - add the created projects to the solution
cd..
dotnet sln "%PROJECT_NAME%".sln add src/"%PROJECT_NAME%".API
dotnet sln "%PROJECT_NAME%".sln add src/"%PROJECT_NAME%".Web


REM -------------------------------TEST-----------------------------------------------------------------------
REM - Create the folder for the testing code and create the projects
md test
cd test

REM -------- create the project for the unit test
dotnet new xunit -f %NETCORE_VERSION% -n "%PROJECT_NAME%".UT

REM ----------------------- add the reference to the project to be tested
cd "%PROJECT_NAME%".UT
dotnet add reference ../../src/"%PROJECT_NAME%".API/"%PROJECT_NAME%".API.csproj


REM -----------------------  add packages
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Tools --version %NETCORE_LIB_VERSION%
dotnet add package xunit --version %XUNIT_VERSION%
dotnet add package coverlet.collector --version %COVERLET_VERSION%
dotnet add package Moq --version %MOQ_VERSION%
cd ..

REM -------- create the project for the functional test
dotnet new xunit -f %NETCORE_VERSION% -n "%PROJECT_NAME%".UIT

REM -----------------------  add packages
cd "%PROJECT_NAME%".UIT
dotnet add package Selenium.Support --version %SELENIUM_VERSION%
dotnet add package Selenium.WebDriver --version %SELENIUM_VERSION%
dotnet add package Selenium.WebDriver.ChromeDriver --version %CHROME_DRIVER_VERSION%
dotnet add package xunit --version %XUNIT_VERSION%
dotnet add package xunit.runner.visualstudio --version %XUNIT_VERSION%
dotnet add package coverlet.collector --version %COVERLET_VERSION%
cd..

REM - add the created projects to the solution
cd..
dotnet sln "%PROJECT_NAME%".sln add test/"%PROJECT_NAME%".UT
dotnet sln "%PROJECT_NAME%".sln add test/"%PROJECT_NAME%".UIT

@echo.
@echo [END] Projects created for %PROJECT_NAME%.
