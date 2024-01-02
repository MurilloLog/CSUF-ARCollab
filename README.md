# CSUF-ARCollab
This repository describes how to configure and use the full project for the augmented reality collaborative app. In general, the project is divided in two sections: the local server configuration (_Server folder_) and the app design (_App folder_). In each folder you will find complete instructions on how to configure all the necessary modules, but it's important to know that you have to satisfies the following:

## Requirements
### Server
- **NodeJs**. It is used to facilitate server functions such as reading and sending messages.
- **Typescript**. Programming language selected to program the functionalities and operations of the server.
- **Mongoose**. Library for *Node.js* that facilitates the execution of queries to a MongoDB database.
- **Typegoose**. Library based on mongoose that facilitates the integration of its syntax using Typescript.
- **MongoDB**. NoSQL database system used to store the objects created in the mobile application.
- **JSON**. Object notation used for sending messages.
### App
- **Unity**. Cross-platform game engine used to create the mobile app.
- **ARCore**. Platform used for building augmented reality experiences on Android devices.
- **.NET**. Platform used for message control between the server and the mobile application.

Don't worry, in each section you will be guided to install each requirement listed above.

## Table of contents

* [Preview](#preview)
  * [Android devices](#android-devices)
  * [iOS devices](#ios-devices)
* [Features](#features)
* [Download project](#download-project)
  * [App](#app)
  * [Server](#server)

## Preview

### Android devices

### iOS devices

## Features

## Download project

### App

See [APP.md](./App/APP.md) for instructions on how to setting up the App from source and a list of supported platforms.

### Server

See [SERVER.md](./Server/SERVER.md) for instructions on how to setting up the Local Server from source and a list of supported platforms.
