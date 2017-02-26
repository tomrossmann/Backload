## Backload
**Backload** is a professional, full featured server side file handler for ASP.NET (MVC, Web API, Web Forms, HTML) and ASP.NET Core running on Windows, Linux and Mac. It has been developed as part of a commercial product for the aero craft industry. 
While Backload out of the box handles the client side [jQuery File Upload Plugin](https://github.com/blueimp/jQuery-File-Upload) from blueimp, [PlUpload](https://github.com/moxiecode/plupload) from Moxiecode and [Fine Uploader](http://fineuploader.com/) from Widen Enterprises, it can be easily customized to work with any client side plugin.

<br />
### ASP.NET Core support
Updated: Support status for the new ASP.NET Core based project types. [More ...](https://github.com/blackcity/Backload/wiki/ASP.NET-Core-support)

<br />
### Project website
General information, editions and how to get a Pro/Enterprise license:
[http://backload.org](http://backload.org). 

<br />
### Current release 2.2.7/2.2.8:
[Release 2.2.8 ASP.NET Core package (cross-platform)](https://github.com/blackcity/Backload/raw/master/Examples/Backload.Standard.2.2/Backload.ASPNETCore.Developer.zip?raw=true)<br />
Filesystem and database storage examples, including file chunking demos, classic html/aspx demos, server side events, Web Api controller, post processing and more for the ASP.NET Core framework (Windows/Linux/Mac).<br />
[Release 2.2.7 Database package](https://github.com/blackcity/Backload/blob/master/Examples/Backload.Standard.2.2/Backload.Database.Demo.zip?raw=true)<br />
24 database storage examples, including support for Entity Framework, Sql Server FILESTREAMS, Sql Server FileTables, external file data storage, client side plugins, file chunking, classic html/aspx, Web Api and more.<br />
[Release 2.2.7 FileSystem package](https://github.com/blackcity/Backload/blob/master/Examples/Backload.Standard.2.2/Backload.FileSystem.Demo.zip?raw=true)<br />
20 filesystem storage examples, including file chunking demos, classic html/aspx demos, server side events, Web Api controller, post processing and more.<br />
[Release 2.2.7 Azure Blob Storage package](https://github.com/blackcity/Backload/blob/master/Examples/Backload.Standard.2.2/Backload.AzureBlobStorage.Demo.zip?raw=true)<br />
20 Microsoft Azure Blob Storage examples, including file chunking demos, classic html/aspx demos, server side events, Web Api controller, post processing and more.


<br />
### Cloud storage
Currently, we are developing the cloud storage feature and we need your help! Tell us the features you really need, how your application will use cloud storage, what cloud provider you prefer, etc. Your [feedback](#direct-contact) will influence the development directly.

<br />
### Highlights
**Backload** is a feature rich server side component which can be fully customized (declaratively) within the web.config file (resp. a linked config file). Complex storage structures are supported and handled by the component without a single line of code, whether the storage location is the file system or a database. If you want to upload different file types (images, pdfs, doc, etc) content type based subfolders can be configured to automatically store different file types in different sub folders (e.g. /images, /pdfs, /movies, etc).

The zero configuration feature allows quick setups, a default MVC controller is ready to handle incoming requests. For more complex scenarios, where you have to run your own business logic, you can derive from the default controller or call the handler method from your code. 

**Backload** supports cropping and resizing of images. The parameters can be set within the web.config file or by an incoming request from the client. Multiple image manipulation features are implemented. Type conversion is also supported.

**Backload** can create unique file names (GUIDs). So files cannot be overwritten or, if this is the purpose of using this feature, cannot be accessed from the web without knowledge of the new name. Mapping of the original file name to the new file name and back is also implemented. This feature can be used to send a friendly name back to the client. Generation of client side GUIDs is also supported (seel release 2.0 demos). 

**Backload** has proven its scalability in production with hundreds of thousands uploads a day. Internally it is designed to work asynchronously where possible.

Upload files to **Windows Azure** cloud storage services from your Azure VM or web role and access the files from anywhere.

<br />
### Features
* Running on Windows, Linux and Mac OS X (MacOs)
* Zero configuration: The defaults set up a fully functional server side file upload controller and handler.
* Declarative configuration: Features will be setup within the web.config or a linked config file.
* Storage context: Supported locations are file system, UNC shares, database and cloud (next milestone).
* Object context based locations: Based on the context (e.g. UserX, UserY, ArtistX, ArtistY, HouseA, HouseB) different storage locations can automatically be routed.
* Content type subfolders: Based on the content type files can automatically be stored in an appropriate subfolder. This feature is fully customizable.
* Unique file names: Files can be stored with a unique name and also remapped to their original name.
* Cropping and resizing: This can be setup in the web.config file or in a request from the client.
* Image type conversion: Images can be converted to a different target type.
* Thumbnails: Static (stored in a subfolder) or dynamically (created on a request). 
* Large files: Support for chunked file uploads (release 1.3.6) and optimized internal memory usage.
* Security: Access control with authentication and authorization (roles based).
+ Extensibility (coming release): Dynamically hook in your own extensions. (Multiple extensions for a specific processing step are supported).
+ Eventing: Backloads processing pipeline events enable full control of the request/response data and execution flow.
+ Scalability by asynchronous internal code and asynchronous support for events and extensions.
+ Tracing: Use standard .NET tracing to find proplems during development or log errors in production.
+ CORS: Support for Cross Origin Resource Sharing (release 2.1, current dev).
+ Database storage: Store files in a database, a Sql Server FILESTREAM blob, a Sql Server FileTable or on a related file in the filesystem. (File chunking support for all methods).
+ Cloud storage: [Upload files](https://github.com/blackcity/Backload/wiki/Azure-File-Storage) to Windows Azure cloud services and access the files from anywhere.

<br />
### Documentation
[General](https://github.com/blackcity/Backload/wiki)<br />
[Setup instructions](https://github.com/blackcity/Backload/wiki/Setup)<br />
[Options, settings and enumerations](https://github.com/blackcity/Backload/wiki/Configuration)

<br />
### Examples
Note: Release 2.0 demo package includes 15 new examples.<br />
[Example 01: Zero configuration](https://github.com/blackcity/Backload/wiki/Example-01)<br />
[Example 02: Configuration basics: Using web.config](https://github.com/blackcity/Backload/wiki/Example-02)<br />
[Example 03: Configuration basics: Using an external config file](https://github.com/blackcity/Backload/wiki/Example-03)<br />
[Example 04: Using your own controllers](https://github.com/blackcity/Backload/wiki/Example-04)<br />
[Example 05: Using server side image manipulation features](https://github.com/blackcity/Backload/wiki/Example-05)<br />
[Example 06: Managing subfolders: Using the object context](https://github.com/blackcity/Backload/wiki/Example-06)<br />
[Example 07: Managing subfolders: Using the upload context](https://github.com/blackcity/Backload/wiki/Example-07)<br />
[Example 12: Eventing: Using Backloads server side events](https://github.com/blackcity/Backload/wiki/Example-12)<br />
[Example 13: Tracing: Use tracing to identify problems and log errors](https://github.com/blackcity/Backload/wiki/Example-13)<br />
[Example 14: Large files: How to setup file chunking](https://github.com/blackcity/Backload/wiki/Example-14)<br />

<br />
### Roadmap
#### Cloud storage
Cloud storage will mark our next milestone. We start with giving you the basic means storing data in a cloud storage in the same manner Backload provides for the local file system or databases. Then we will support popular cloud storage providers out of the box. Help is much appreciated! Don't hesitate to show us your code. 

<br />
### Frequently asked questions
Before posting read the [FAQ](https://github.com/blackcity/Backload/wiki/faq)

<br />
### Licenses and editions
You can get a license for a Professional or Enterprise edition here: [http://backload.org/](http://backload.org/). 
Or read the FAQ about the different [licenses](https://github.com/blackcity/Backload/wiki/faq#versions-and-licenses)

<br />
### News, releases, plans and more
Follow us on Twitter (just started) [@Backload_org](https://twitter.com/backload_org)

<br />
### Direct contact
For customers and commercial requests only: backload.org [at] gmail [dot] com.

<br />
### License
[Backload. (Standard version)](https://github.com/blackcity/Backload): Copyright 2016, Steffen Habermehl, License (Standard version): MIT license<br />
Professional and Enterprise (source code) version are available under a commercial license.<br/>
Follow us on Twitter: [@Backload_org](https://twitter.com/backload_org)
