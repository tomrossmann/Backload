## Backload
===========
**Backload** is a professional, full featured ASP.NET MVC file upload server side controller and handler. It has been developed as part of a commercial product for the aero craft industry. 
The version in these examples is tailored for the famous client side [jQuery File Upload Plugin](https://github.com/blueimp/jQuery-File-Upload) from blueimp.


### Highlights

**Backload** is a feature rich server side component witch can be fully declaratively customized within the web.config or a linked config file. <u>Complex storage structures</u> are supported and handled easily by the component, whether it is the file system or a database. If you want to upload different file types (images, pdfs, doc, etc) content type based sub folders can be configured in order automatically store different file types in different sub folders (e.g. /images, /pdfs, /movies, etc).

The Zero Configuration feature allows quick setups where a default MVC controller is ready to handle incoming requests. For more complex scenarios where you have to run your own business logic, you can easily derive from the default controller or call the handler method from your code. 

**Backload** supports cropping and resizing of images. These parameters can be set up within the web.config file or by an incoming request from the client. Multiple <u>image manipulation features</u> are implemented. Type conversion is also supported.

**Backload** can create unique file names (GUIDs) in order files cannot be overwritten or, if this is the purpose, not accessed from the web. Mapping of a original file name to the new file name and back is also implemented. This can also be used to send a friendly name back to the client. 

### Features
* Zero configuration: The defaults set up a fully functional server side file upload controller and handler.
* Declarative configuration: Features will be setup within the web.config or a linked config file.
* Storage context: Supported locations are file system and databases (by the Entity Framework).
* Object context based locations: Based on the context (e.g. UserX, UserY, ArtistX, ArtistY, HouseA, HouseB) different storage locations can automatically be routed.
* Content type subfolders: Based on the content type files can automatically be stored in an appropriate subfolder. This feature is fully customizable.
* Unique file names: Files can be stored with a unique name and also remapped to their original name.
* Cropping and resizing: Can be setup in the web.config or in a request from the client.
* Image type convertion: Images can be converted to a different target type.

