# TasteTrailOwnerExperience

This repository contains a microservice designed for the TasteTrail application — a platform for discovering and rating restaurants. The microservice enables restaurant owners to register and manage their establishments, providing a seamless experience for adding and updating venue details.

## Features:
* **Create and Manage Venues**: Allows restaurant owners to add detailed information about their menues, dishes, location, description, and etc.
* **Venue Updates**: Supports editing venue details to keep information up-to-date.
* **Secure Access**: Includes role-based access controls to ensure only authorized owners can manage their venues.

## Tech Stack:
* **Framework**: ASP.NET Core Web API
* **Database**: PostgreSQL
* **Authentication**: JWT and AspIdentity
* **Hosting**: Docker-ready for deployment in microservice environments
* **Storage**: Azure Blob Storage (for media uploads and large data storage)
