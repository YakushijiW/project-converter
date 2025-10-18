# Project Converter

[中文版 README](README.md) | **English README**

A simple and easy-to-use project file packaging and unpacking tool that supports packing project folders into a single text file and restoring project structure from text files.

## Features

- **Project Packaging**: Pack entire project folders into a single text file
- **Project Unpacking**: Restore complete project structure from packed text files
- **Preset Management**: Built-in presets for various project types with custom preset support
- **Smart Filtering**: Support file extension inclusion/exclusion and directory exclusion
- **Drag & Drop Support**: Support drag and drop operations for files and folders
- **Settings Memory**: Automatically remember last used paths and presets
- **User Friendly**: Intuitive graphical interface with simple operations

## System Requirements

- Windows Operating System
- .NET 6.0 or higher

## Usage

### Pack Project

1. **Select Project Path**: Click "Browse" button to select the project folder to pack
2. **Select Output File**: Specify the location to save the packed text file
3. **Select Preset**: Choose appropriate project type preset from dropdown list, or select "Custom..."
4. **Custom Settings** (Optional):
   - Include Extensions: Specify file types to include (e.g., .cs,.js,.html)
   - Exclude Directories: Specify directories to exclude (e.g., bin,obj,node_modules)
5. **Start Packing**: Click "Start Packing" button

### Unpack Project

1. **Select Text File**: Click "Browse" button to select the text file to unpack
2. **Select Target Path**: Specify the project folder location after unpacking
3. **Start Unpacking**: Click "Start Unpacking" button

### Preset Management

- **Add Preset**: After selecting "Custom...", set include extensions and exclude directories, then click "Add Preset"
- **Delete Preset**: Select the custom preset to delete, then click "Delete Preset" button
- **Built-in Presets**: Includes common project types like C#, JavaScript, Python, Java, Web, etc.

### Settings

- **Default Output Name**: Set default name for packed files
- **Auto Memory**: Program automatically remembers last used paths and presets

## Technical Features

- Developed with .NET 6.0 and Windows Forms
- Asynchronous operations without blocking user interface
- JSON format for storing user settings and presets
- Support for large file processing with progress display
- Complete error handling and user prompts

## Build and Run

```bash
# Clone the project
git clone https://github.com/YakushijiW/proj-2-text.git

# Enter project directory
cd project-converter

# Build project
dotnet build

# Run project
dotnet run
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Issues and Pull Requests are welcome to improve this project.

## Changelog

### v1.1.0
- Added delete preset functionality
- Added auto-memory for last used paths and presets
- Optimized user interface and experience
- Improved error handling mechanism