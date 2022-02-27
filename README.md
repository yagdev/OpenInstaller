
![OpenInstaller Icon](https://i.imgur.com/8nzJilO.png)
# OpenInstaller
OpenInstaller is a free and open source solution for modded Minecraft Servers to easily install the required mods across clients.

# Documentation
###### 1. – How to set up (Package)
###### 1.1 – Creating package
###### 1.1.1 – Adding mods
###### 1.1.2 – Extra folders
###### 1.1.3 – Versioning system (not yet implemented)
###### 1.2 – Uploading the files
###### 2. – Testing
###### 2.1 – Debug features
###### 3 – Implementing changes in code level
###### 4 – Changing design

## 1. - How to set up (Package)
Setting up your modpacks to work with OpenInstaller is actually really simple and flexible as it even features a system to add other folders that aren’t the regular mods folder.

## 1.1	– Creating package
Here we’ll cover creating the actual *.zip file to upload to a server for the OpenInstaller client to download.

###### 1.1.1	– Adding mods
Simply create a folder and drop your mods folder inside like so:
![Folder Creation](https://i.imgur.com/zcvggJF.jpg)
Then compress them that whole folder in *.zip format and skip to 1.2 unless you need to use more folders or if you want to use the versioning system. In these cases, continue reading.

###### 1.1.2	– Extra Folders
OpenInstaller features a system for those who need to add extra folders alongside the mods folder, so here’s how we set that up:
Firstly you add all of the extra folders alongside the mods folder in the folder that we created earlier like so:
![Extra Folders](https://i.imgur.com/i4vuwoy.jpg)
Then you create a text file named “script.txt”, like shown here:
![Creating the text file](https://i.imgur.com/mM9qZse.jpg)
Inside that text file you will write this:
/C cd .minecraft & RD /s /q mods & RD /s /q “Extra Folder’s name goes here” & RD /s /q “Second Extra Folder’s name goes here”
This is how it should look for me:
![Final text file](https://i.imgur.com/fDacaVK.png)
Then you save that script file. 
Then compress all of these files into a *.zip file. If you want to use the versioning system, skip to 1.2.

###### 1.1.3	– Versioning System
Future versions of OpenInstaller will feature an updating system that checks if the modpack is up to date. If you wish to prepare your modpack for that feature, follow these steps:
Start by creating a text file apart from the *.zip file that you made on using the previous steps. This is an example of that being done:
![Second Text File](https://i.imgur.com/JxcBvch.png)
Then inside the file you will write:
Server: 1.0
(replace 1.0 with the version string you want)
Here’s an example of a completed text file:
![Second Text File Example](https://i.imgur.com/V8k0yJ1.png)
Save that file and move on to the next step.

## 1.2	– Uploading package
For the modpack to be downloadable by OpenInstaller, you will need to place your files in a server that uses static links (I recommend Dropbox, and that’s what we’ll be covering here).
In the example of Dropbox, you will need to change the link’s end from dl=0 to dl=1 like this:
https://www.dropbox.com/s/s2g6cqgztwzf50f/release.zip?dl=0 
becomes
https://www.dropbox.com/s/s2g6cqgztwzf50f/release.zip?dl=1
Copy these links and keep them for the next steps.

## 2.	– Testing
We will now test the modpack before implementing it into the source code. Fortunately, OpenInstaller comes with some debugging tools that might come in handy, so let’s explore those.

## 2.1	– Debug features
If you compile OpenInstaller (or use the precompiled exe) and press LCTRL+D, you will enable debug mode. This screen should appear:
![Debug Menu](https://i.imgur.com/OsRpSdC.png)
From there, you can test your package by inserting the download URL for the *.zip file on “Custom URL (Modpack *.zip)” and the version file (optional and unused for now) on “Custom URL (Modpack Version)” and checking the “Use custom URL” option. Then, press install. If everything goes according to plan, this should appear:
![Download Finished](https://i.imgur.com/ao4AT3Q.png)
You then should check your .minecraft folder to check if the mods are there and launch your game to check for corruption.
If everything goes according to plan, move on to the next step.

## 3.	– Implementing changes in code level
Okay, so now that everything is ready, open the source code and open HomeScreen.xaml.cs. Then on the line that contains “string url = “https://www.dropbox.com/s/s2g6cqgztwzf50f/release.zip?dl=1";” change that link to your *.zip file’s download link and on the line that contains “string remote_version_url = "https://www.dropbox.com/s/iavj5ah7cy7v9ei/version.txt?dl=1";” change that link to your version file’s link (if you choose to use that feature).
Then if you don’t want to make any further changes, compile it and you’re done with setting OpenInstaller up. You should also always do a few test runs after compiling too. In case you want to tweak other aspects, read below.

## 4.	– Changing design
In case you want to change the design of the app, you’re completely free to do so, so here are a few things to keep in mind:
- OpenInstaller supports dynamic images, meaning that they change depending on the theme that Windows is using. To change these images, change the files mentioned in UpdateUI on HomeScreen.xaml.cs.
- By default, the available resources are ChaseLabs.Updater (& its dependencies), Costura.Fody (& its dependencies), FluentWPF (modded to work in Windows 11 correctly) and NHotkey.Wpf.

## ©2022 YAG-dev | OpenInstaller
