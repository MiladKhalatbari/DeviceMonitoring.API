Introduction

The DeviceMonitoring Repository Library provides a set of classes and methods to manage devices and measurements. This library is designed to be used in conjunction with the DeviceMonitoring API project.

Database Setup: The DeviceMonitoring API uses a code-first approach for database setup. Before using the repository library, ensure you have set up your database and applied migrations.

Seeding Data: For testing purposes, the DeviceMonitoring API includes a seed data script that adds one user, 5 devices, and 40 measurements to the database.

Logging with Serilog:
In our project, we've implemented logging using Serilog. Serilog is a logging library that helps us track important information and errors in our application.

How It Works:

Log Files: Serilog creates log files to store information. Each day, a new log file is made with the date, so we have separate files for each day's logs.

Different Levels: We can log messages at different levels like Information, Warning, and Error. This helps us categorize how important each log message is.

Logging Events: We use _logger.LogError(e, $"{DateTime.Now.ToShortTimeString()}_{e.Message}"); to log errors. This includes a timestamp and error details in the log files.

Log Format: Log messages show timestamps, log levels, and the message itself. This makes it easier to understand what's happening.

Purpose: Logging helps us track issues, understand how our app is running, and find and fix problems.

In the DeviceMonitoring.API project, there are multiple layers that help organize and structure the application. Here's an overview of the layers and their responsibilities:

Presentation Layer (Controllers):

Responsible for handling HTTP requests and responses.
Interacts with the outside world, receiving requests from clients and sending back responses.
Validates input, maps data, and calls appropriate services.
Examples: DevicesController, MonitoringController

Application Layer (Services):

Contains business logic and application-specific rules.
Orchestrates interactions between different parts of the application.
Handles use cases, processes requests from controllers, and executes business operations.
Examples: DeviceService, MonitoringService

Domain Layer (Entities and DTOs):

Represents the core business logic and domain concepts.
Contains entity classes that model the data and behavior of your application's domain.
Also includes DTO (Data Transfer Object) classes used to transfer data between layers and to/from clients.
Examples: Device, Measurement, DeviceForInsertDto

Data Access Layer (Repositories and DbContext):

Manages data access and persistence to the database.
Contains repository classes that encapsulate CRUD operations for entities.
DbContext class represents the database context and provides an abstraction over data access.
Examples: IDeviceRepository, DeviceMonitoringDbContext

Test Projects:

Contain unit tests, integration tests, and other automated tests to ensure the correctness of the application's behavior.
Follow similar structure to the main project and test individual components in isolation.


Controllers and their Actions :

AuthenticationsController :
This controller handles user authentication using a username and password. It generates a JSON Web Token (JWT) for authorized users. The Authenticate action takes a AuthenticationRequestBodyDto as input, which contains the username and password. The controller validates the credentials using the IAuthenticationService.
If the credentials are valid, the controller creates a JWT token using the provided configuration settings for issuer, audience, and secret key. It includes the user's username as a claim. The token is set to expire after one hour. Finally, the controller returns the generated JWT token as a response, allowing the user to access protected resources using this token.

MonitoringController :
This controller is responsible for providing various monitoring-related actions for devices. It interacts with the IMonitoringService to perform calculations and retrieve data. Here's a breakdown of the actions:

GetTotalMeasurementCount - Retrieves the total number of measurements for a given device.

GetDeviceNameAndNumberOfMeasurements - Retrieves a dictionary of device names and their corresponding number of measurements.

CalculateMinValue - Calculates and returns the minimum measurement value for a device.

CalculateMaxValue - Calculates and returns the maximum measurement value for a device.

CalculateAverageValue - Calculates and returns the average measurement value for a device.

GetMeasurementCountAboveThreshold - Retrieves the count of measurements above a specified threshold value.

GetMeasurementCountBelowThreshold - Retrieves the count of measurements below a specified threshold value.

CalculateAverageValueForDateRange - Calculates and returns the average measurement value within a specified date range.

CalculateMinValueForDateRange - Calculates and returns the minimum measurement value within a specified date range.

CalculateMaxValueForDateRange - Calculates and returns the maximum measurement value within a specified date range.

GetMeasurementsForDateRange - Retrieves measurements within a specified date range for a device.

GetLatestMeasurements - Retrieves the latest measurements for a device, up to a specified count.

GetNumberAndAverageValueInHourByDate - Retrieves the number of measurements and their average value for each hour on a given date.

Each action follows a similar pattern: it calls the corresponding method in the IMonitoringService, handles exceptions if any, and returns either the calculated values or an error response.

This controller is responsible for handling incoming data measurements from devices. It interacts with the IDeviceService and IMeasurementService to store the received data. Here's what the actions in this controller do:

DataController :
Listen - This action is called when a new data measurement is received from a device. It expects parameters deviceName (the name of the device) and value (the measurement value). The action attempts to find the device by its name using the _deviceService.GetByNameAsync method. If the device doesn't exist, it adds a new device using the _deviceService.AddAsync method. Then, it adds a new measurement using the _measurementService.AddAsync method, associating the measurement with the device. If everything is successful, the action returns an Ok response. If an exception occurs during this process, it's logged, and a BadRequest response is returned.

BaseController:
This code defines a base controller class for the API controllers in the Device Monitoring application. Let's break down what each part of the code does:

[Authorize]: This attribute specifies that access to the actions in controllers derived from this base controller requires authentication. In other words, users must be logged in to access the actions.

[Route("API/[controller]/[action]"): This attribute sets the route template for the actions in controllers derived from this base controller. The route template includes the [controller] placeholder, which will be replaced by the actual controller name, and the [action] placeholder, which will be replaced by the action name. This helps generate the URL routes for the actions.

public class BaseController : ControllerBase: This class inherits from ControllerBase, which is the base class for controllers in ASP.NET Core. It doesn't have any additional functionality beyond the attributes and behavior specified by its parent classes. It serves as a common base for other controllers in the application.

In summary, this code defines a base controller with authorization, route templates, and ApiController attributes. Controllers derived from this base controller will inherit these behaviors and attributes.

The DevicesController and MeasurementsController are designed to handle CRUD (Create, Read, Update, Delete) operations for devices and measurements, respectively, in an asynchronous manner. These controllers provide endpoints that allow you to perform operations on devices and measurements using HTTP methods such as GET, POST, PUT, and DELETE. Here's a breakdown of what each of these controllers does:

DevicesController
GET /API/Devices:

Retrieves a list of all devices.
GET /API/Devices/{id}:

Retrieves a specific device by its ID.
POST /API/Devices:

Creates a new device using the data provided in the request body.
PUT /API/Devices/{id}:

Updates an existing device with the provided data in the request body.
DELETE /API/Devices/{id}:

Deletes a device with the specified ID.
MeasurementsController
GET /API/Measurements:

Retrieves a list of all measurements.
GET /API/Measurements/{id}:

Retrieves a specific measurement by its ID.
POST /API/Measurements:

Creates a new measurement using the data provided in the request body.
PUT /API/Measurements/{id}:

Updates an existing measurement with the provided data in the request body.
DELETE /API/Measurements/{id}:

Deletes a measurement with the specified ID.
