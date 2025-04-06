# Unity Project Setup Guide

## Prerequisites
- Unity Hub installed ([Download Unity Hub](https://unity.com/download))
- Unity Editor (2020.3.48f1)
- Android Build Support for Unity Editor

## Installation Steps
1. **Install Unity Hub**
   - Download from official site
   - Follow installation wizard
   - Sign in with your Unity ID

2. **Clone the project repository**
```bash
git clone https://github.com/MurilloLog/CollabAR.git
```

3. Open Project in Unity Hub
- Click "Add" in Projects tab
- Select the cloned project folder
- Unity Hub will detect required editor version

4. Install Required Unity Version
- If prompted, install the exact version through Unity Hub
- This may take time depending on components selected

5. Project Dependencies
- Open the project
- Wait for Unity to import assets and compile scripts
- Resolve any missing package warnings

## Configuration
1. Build Settings
- Go to File > Build Settings
- Select target platform (Android)
- Add required scenes to build (Linker, DeveloperLinker, StudyGroup)

2. Building the Project
- Select build location
- Wait for build process to complete
- Test built executable

## Troubleshooting
1. If scripts show errors:
- Wait for compilation to finish
- Check console for specific errors
- Verify API compatibility level

2. For missing assets:
- Check if all assets were properly cloned
- Verify asset store packages are downloaded
