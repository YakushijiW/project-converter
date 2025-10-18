# Project Packer/Unpacker

[中文版 README](README.md) | **English README**

A simple and practical WinForms tool that packs an entire project folder (or specified files) into a single `.txt` file for easy sharing, transfer, and archiving. It can also fully restore the project structure from this text file.

![Application Screenshot](https://raw.githubusercontent.com/YakushijiW/project-converter/main/screenshot.png)

## Core Features

- **Dual Packing Modes**:
    - **By Directory**: Intelligently scans an entire project folder.
    - **By Files**: Manually add or drag-and-drop any number of files to be packed.
- **Project Restoration**: Parses the packed text file to completely restore the original project files and directory structure.
- **Robust Preset System**:
    - Built-in presets for many common project types (e.g., Unity, Unreal, Godot, Winform, Web Frontend).
    - Supports **adding**, **modifying**, and **deleting** custom presets.
    - Smart handling of built-in presets: modifications or deletions are saved as user preferences without altering the original preset file.
- **File Integrity Check**: Automatically verifies each file's integrity using its SHA256 hash during unpacking to ensure data was not corrupted.
- **User-Friendly Experience**:
    - Clean and intuitive interface with drag-and-drop support for almost all path inputs.
    - Real-time progress display for packing and unpacking operations.
    - Automatically saves the last used paths and settings.

## System Requirements

- Windows Operating System
- .NET 6.0 or higher

## Usage

### Packing a Project or Files

1.  **Select Source**:
    - **To pack an entire project**: On the `Pack` tab, click the `Browse...` button to select your project's root directory, or simply drag the folder into the `Project Folder` input box.
    - **To pack loose files**: Drag one or more files/folders directly into the `File List` box below.
2.  **Configure Filters**:
    - Choose the most suitable preset from the `Project Type Preset` dropdown, such as "Unreal Engine Project".
    - The `Include Extensions` and `Exclude Directories` fields will be auto-populated. You can modify them manually (use commas or spaces as separators).
3.  **Specify Output**:
    - Click the `Browse...` button to select an `Output Folder` where the resulting text file will be saved.
    - (Optional) You can change the default output filename in the `Settings` tab.
4.  **Start Packing**:
    - Click the green `Start Packing` button and wait for the process to complete.

### Unpacking (Restoring) a Project

1.  Switch to the `Unpack` tab.
2.  Click `Browse...` to select the `.txt` file that was previously packed.
3.  Click `Browse...` to select a folder `Extract to Folder` (an empty folder is recommended).
4.  Click the blue `Start Unpacking` button. The tool will recreate the directories and files, followed by an integrity check.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit an Issue or Pull Request.