      *** IMPORTANT NOTE: 
	  Changes in Web.config and in the configuration file:
      Since version 1.9.2 the section name in Web.config has changed from name="fileUpload" to name="backload" (see below). 
      The root element must also be changed in your config file from <fileUpload> to <backload>
      The ConfigurationSection class has changed to <section type="Backload.Configuration.BackloadSection ..." />
	  Backload has implemented a fallback routine for the old schema, but it is best practice to update your config files.
	  See examples on GitHub.
	  
	  Release notes:
	  Version 1.9.2.0:
	  - Read/write access to the configuration at runtime (IBackloadContext.Configuration).
	  - Standard .NET Trace support (TraceSource name: "Backload").
	  - Fine Uploader from Widen Enterprises now supported out of the box.
	  
	  Bug fixes:
	  - Unique file names feature not working
	  - Param.CustomFormValues has the values of the form element send with a request.
