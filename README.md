## Backload
===========
**Backload** is a professional, full featured ASP.NET MVC 4 file upload controller and handler (server side). It has been developed as part of a commercial product for the aero craft industry. 
The version in these examples is tailored for the famous client side [jQuery File Upload Plugin](https://github.com/blueimp/jQuery-File-Upload) from blueimp, but it can be easily customized to work with a different client side plugins.


### Highlights

**Backload** is a feature rich server side component witch can be fully customized (declaratively) within the web.config file file (resp. a linked config file). Complex storage structures are supported and handled by the component without a single line of code, whether the storage location is the file system or a database. If you want to upload different file types (images, pdfs, doc, etc) content type based subfolders can be configured to automatically store different file types in different sub folders (e.g. /images, /pdfs, /movies, etc).

The zero configuration feature allows quick setups, a default MVC controller is ready to handle incoming requests. For more complex scenarios, where you have to run your own business logic, you can derive from the default controller or call the handler method from your code. 

**Backload** supports cropping and resizing of images. The parameters can be set within the web.config file or by an incoming request from the client. Multiple image manipulation features are implemented. Type conversion is also supported.

**Backload** can create unique file names (GUIDs). So files cannot be overwritten or, if this is the purpose of using this feature, cannot accessed from the web withour the knowledge of the new name. Mapping of the original file name to the new file name and back is also implemented. This feature can be used to send a friendly name back to the client. 

### Features
* Zero configuration: The defaults set up a fully functional server side file upload controller and handler.
* Declarative configuration: Features will be setup within the web.config or a linked config file.
* Storage context: Supported locations are file system and databases (by the Entity Framework).
* Object context based locations: Based on the context (e.g. UserX, UserY, ArtistX, ArtistY, HouseA, HouseB) different storage locations can automatically be routed.
* Content type subfolders: Based on the content type files can automatically be stored in an appropriate subfolder. This feature is fully customizable.
* Unique file names: Files can be stored with a unique name and also remapped to their original name.
* Cropping and resizing: This can be setup in the web.config file or in a request from the client.
* Image type convertion: Images can be converted to a different target type.
* Security: Access control with authentication and authorization (roles based).

### Examples
[Example 01: Zero configuration](https://github.com/blackcity/Backload/wiki/Example-01)<br />
[Example 02: Configuration basics: Using web.config](https://github.com/blackcity/Backload/wiki/Example-02)
[Example 03: Configuration basics: Using an external config file](https://github.com/blackcity/Backload/wiki/Example-03)

### Demos
You can find all demos in the repositories.

### License
[jQuery File Upload Plugin](https://github.com/blueimp/jQuery-File-Upload): Copyright 2013, Sebastian Tschan, [https://blueimp.net](https://blueimp.net), License: MIT license<br />
[Backload. (Standard version)](https://github.com/blackcity/Backload): Copyright 2013, Steffen Habermehl, License (Standard version): MIT license, (Professional version is only available under a commercial license. 

