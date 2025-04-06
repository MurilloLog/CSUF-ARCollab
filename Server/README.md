# Local Server Replication Guide

## Prerequisites
- Install MongoDB v8.0.4 ([Download MongoDB](https://www.mongodb.com/docs/manual/installation/))
- Install NodeJS v22.12.0 ([Download Node.js](https://nodejs.org/en))
- Visual Studio Code (recommended) or any code editor

## NodeJS Setup
1. Verify installation by running:
```bash
node --version && npm --version
```
### Install required modules:
```bash
npm install
```
## MongoDB Setup
1. Install MongoDB Community Edition
2. Add MongoDB to your system PATH
3. Start MongoDB service:
```bash
mongod
```
4. Keep this terminal open to maintain the MongoDB service
5. Optional: Use MongoDB Compass for GUI management

## Server Execution
1. Ensure MongoDB is running (mongod command)
2. Start the server:
```bash
npm start
```
3. For client connections:
- Ensure devices are on the same local network
- Provide server IP and port to clients

## Configuration
1. Modify src/Server.ts for:
- Minimum client requirements
- Connection port
- Database properties
