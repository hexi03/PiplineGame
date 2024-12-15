# Build tips

1. Open your Unity editor
2. Install GoogleSignIn, Firebase plugins
3. Execute Google Dependency Resolving process
4. Specify android build target
5. Enable IL2CPP Compiling feature (needed for FireBase, SQLite lib)
6. Enable `armeabi-v7a` and `arm64-v8a` build features (needed for FireBase, SQLite lib)
7. Specify key storage and passphrases for apk signer (needed for FireBase). It may don't work correctly - alternatively use APK_SIGN script (change your android build tools directory first)

### Troubleshooting
	If build failed due to less API version try to change targetSdkVersion to 35 (path: Library/Bee/Android/Prj/IL2CPP/Gradle/launcher/build.gradle)
