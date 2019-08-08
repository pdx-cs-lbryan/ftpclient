# FTP Client

Recreation of CS410/510 Agile Summer 2019 Group 7 FTP Repository.<br>
**Please visit this link for the original repository and its commit history**: https://github.com/mbillett/ftpClient

Makes use of:<br>
https://fakeiteasy.github.io/<br>
Copyright (c) FakeItEasy contributors. (fakeiteasy@hagne.se)<br>
Licence: https://github.com/FakeItEasy/FakeItEasy/blob/master/License.txt<br><br>
https://github.com/robinrodricks/FluentFTP<br>
Copyright (c) 2015 Robin Rodricks and FluentFTP Contributors<br>
License: https://github.com/robinrodricks/FluentFTP/blob/master/LICENSE.TXT

## Building and Running

Git clone the project into Visual Studio IDE and 
run the code from there. 


Or from the command line:

```
$ cd ftpclient/FtpClientApp/FtpClientApp
```

 compile the following files:

```
$ csc *.cs
```

and then run it:

```
$ mono Program.exe
```

## Setting Up, Running, and Removing the Installer

1. Download the Wix Toolset build tool and the Wix Toolset Visual Studio Extension (2019): https://wixtoolset.org/releases/<br>
2. In visuals studio with the project open, right click on the project solution and go to configuration manager<br>
3. FtpClientApp and FtpClientAppTest should be set to debug, any cpu, and with build checked. Installer should be set to debug, x86, and nothing checked.<br>
4. Go to the properties for FtpClientApp and FtpClientAppTest, and set suppress warnings as: `'1701;1702;NU1605'`<br>
5. From the project directory (directory containin the .sln file), run: `dotnet publish -r win-x86`<br>
6a. modify components.wxs to your path in line 2:<br>
`<?define source = "Path\To\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish"?><br>`
6b. If 6a fails for you:<br>
Remove from the Installer project: `components.wxs`<br>
From the command line, run<br>
`"%wix%\bin\heat.exe" dir path\to\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish -cg GeneratedComponents -var var.source -gg -g1 -ke -srd -scom -sreg -out components.wxs`<br>
right click the Installer in visual studio and add existing item: `components.wxs` from where it was created<br>
Add the following to components.wxs on line 2:<br>
`<?define source = "Path\To\FtpClientApp\FtpClientApp\bin\Debug\netcoreapp2.1\win-x86\publish"?>`<br>
Then go to Edit -> Find and Replace -> Quick Replace<br>
Enter `TARGETDIR` as the search term and `INSTALLFOLDER` as the term to replace it with, then click 'Replace All'<br>
7. Build the Installer project to generate the .msi file.<br>
The installer is a .msi file located at:<br>
`Path\To\ftpclient\FtpClientApp\Installer\bin\Debug`<br>
Double click the file to begin the installation.<br>
8. The installation files will be located in your default location for 32 bit applications (likely Program Files (x86)). Look for:<br>
'FtpClientApp.exe' to run the program.
9. To uninstall, go to "Apps and Features" in the Windows' settings and uninstall "Team 7 FTP Client"<br>



## Sources

1.) https://docs.microsoft.com/en-us/dotnet/api/system.net.ftpwebrequest?view=netframework-4.8 
