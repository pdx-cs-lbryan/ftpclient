# FTP Client

Team Members:<br>
Marcella Billett: mbillett<br>
Saraswathi Datar: saraswathidatar<br>
Colin Gregory: c-gregory<br>
Bryan Logan: pdx-cs-lbryan<br>
Dennis Mesanovic: dmesa2<br>


Recreation of CS410/510 Agile Summer 2019 Group 7 FTP Repository.<br>
**Please visit this link for the original repository and its commit history**: https://github.com/mbillett/ftpClient<br>
**See our trello board here**: https://trello.com/b/Ev8rTDfC/agileftp

Makes use of:<br>
https://fakeiteasy.github.io/<br>
Copyright (c) FakeItEasy contributors. (fakeiteasy@hagne.se)<br>
Licence: https://github.com/FakeItEasy/FakeItEasy/blob/master/License.txt<br><br>
https://github.com/robinrodricks/FluentFTP<br>
Copyright (c) 2015 Robin Rodricks and FluentFTP Contributors<br>
License: https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT<br>
https://wixtoolset.org/ (For the Installer)<br>
License: https://wixtoolset.org/about/license/

## Building and Running

With Visual Studio 2019 IDE:
Git clone the project into a directory.

In Visual Studio, open the project folder and open the solution (.sln) file.

Set the project to FtpClientApp and click the green arrow.

You can right click 'FtpClientAppTest' -> Run tests to automatically run all unit tests.

If fakeiteasy and/or FluentFTP cannot be found, use the following guide to install and add NUGET packages to the solution:
https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio

fakeiteasy should be version 5.1.1 and fluentftp version 27.0


A log file will be kept in the directory where the application is launched.



## Setting Up, Running, and Removing the Installer (Written for and Tested on Windows 10)

Note: When a path is given, it will be preceded with 'path\to\...' This should be changed to your path to that location.

1. Download the Wix Toolset build tool (v 3.11) and the Wix Toolset Visual Studio Extension (2019): https://wixtoolset.org/releases/

2. In visuals studio with the project open, right click on the project solution and go to configuration manager

3. FtpClientApp and FtpClientAppTest should be set to debug, any cpu, and with build checked. Installer should be set to debug, x86, and nothing checked.

4. Go to the properties for FtpClientApp and FtpClientAppTest, and set suppress warnings as: `'1701;1702;NU1605'`

5. From the project directory (directory containin the .sln file), run: `dotnet publish -r win-x86` (ignore errors, warnings so long as the publish folder listed below is created)

6a. modify components.wxs to your path in line 2:

`<?define source = "Path\To\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish"?>`

6b. If 6a fails for you:

Remove from the Installer project: `components.wxs`

From the command line, run

`"%wix%\bin\heat.exe" dir path\to\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish -cg GeneratedComponents -var var.source -gg -g1 -ke -srd -scom -sreg -out components.wxs`

right click the Installer in visual studio and add existing item: `components.wxs` from where it was created

Add the following to components.wxs on line 2:

`<?define source = "Path\To\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish"?>`

Then go to Edit -> Find and Replace -> Quick Replace

Enter `TARGETDIR` as the search term and `INSTALLFOLDER` as the term to replace it with, then click 'Replace All'

7. Build the Installer project to generate the .msi file.

The installer is a .msi file located at:

`Path\To\ftpclient\FtpClientApp\Installer\bin\Debug`

Double click the file to begin the installation.

8. The installation files will be located in your default location for 32 bit applications (likely Program Files (x86)). Look for:

'FtpClientApp.exe' to run the program. Run the program as an administrator.

9. To uninstall, go to "Apps and Features" in the Windows' settings and uninstall "Team 7 FTP Client"



## Sources

1.) https://docs.microsoft.com/en-us/dotnet/api/system.net.ftpwebrequest?view=netframework-4.8 <br>
2.) The following video was used as a reference in creating the installer: AngelSix - How To Create Windows Installer MSI - .Net Core Wix: https://www.youtube.com/watch?v=6Yf-eDsRrnM in addition to Wix Documentation: https://wixtoolset.org/documentation/ <br>
3.) Timer: https://docs.microsoft.com/en-us/dotnet/api/system.timers.timer?view=netframework-4.8<br>
4.) Encryption: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=netframework-4.8
