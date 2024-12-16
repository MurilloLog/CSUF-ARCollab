# CSUF-ARCollab
This repository outlines the design, development, and implementation of a local server tailored to collaborative augmented reality (AR) applications. It introduces a synchronous remote communication model that enables real-time interaction among multiple users, serving as a foundation for developing collaborative experiences in AR environments. The project is organized into two main sections: the first covers the architecture and technical configuration of the local server (_Server folder_), describing the components and processes required to ensure its functionality; the second focuses on the design (_App folder_), evaluation, and validation of the application, identifying issues encountered during development, analyzing potential solutions, and proposing improvements for future iterations.

## Requirements
The following are the necessary requirements for the implementation and operation of the project. These are divided into two main categories: Server and Application.

### Server
See [SERVER.md](./Server/SERVER.md) for instructions on how to setting up the Local Server from source and a list of supported platforms.
- **NodeJs**. It is used to facilitate server functions such as reading and sending messages.
- **Typescript**. Programming language selected to program the functionalities and operations of the server.
- **Mongoose**. Library for *Node.js* that facilitates the execution of queries to a MongoDB database.
- **Typegoose**. Library based on mongoose that facilitates the integration of its syntax using Typescript.
- **MongoDB**. NoSQL database system used to store the objects created in the mobile application.
- **JSON**. Object notation used for sending messages.
### App
See [APP.md](./App/APP.md) for instructions on how to setting up the App from source and a list of supported platforms.
- **Unity**. Cross-platform game engine used to create the mobile app.
- **ARCore**. Platform used for building augmented reality experiences on Android devices.
- **.NET**. Platform used for message control between the server and the mobile application.

Don't worry, in each section you will be guided to install each requirement listed above.
