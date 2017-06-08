# Aubergine :eggplant:

**Umbraco Starterkit and Module platform:**

**[Status: This code is not yet production/beta ready, but is a work in progress - feel free to look around and contribute if you want]**


Aubergine is a starter kit for umbraco with extendable elements that will let you build and extend your site, covering the basics of most sites, with add ons for blogs, forums, comments etc. 

## Nuget 
One of the key goals of the aubergine starterkit is to make all elements installable via nuget. Using nuget will make the kit easier to deploy and will allow us to seperate out the diffrent elements as needed. 

**Migration Helpers** Using nuget as the primary deployment method does raise some challenges, namely getting umbraco settings installed. through out the kit umbraco migrations are used to create/alter settings and tables. *(Aubergine uses the uSync Core library to manage these umbraco settings. using usync core does not mean you have to use usync for the package, the core just contains the code to add/alter settings, it does not hook into umbraco events or attempt to update anything automatically - this should keep the kit cloud friendly)*

## Modules 

Each module has been developed to make it independent of the wider start kit where possible, **this means you shouldn't need to install the starter kit package to then use the blogs, forums or user content elements** 

**User Generated Content**

A core library that offers basic low level management of user generated content, this is a steamlined content layer - seperate from the umbraco content that allows you to manage and control user content without the performance hit you might get if you where to use umbraco content for the same purpose.

**Comments**

Allows you to add comments to any content node, comes with a property editor that allows the editors to moderate comments at the node level.

**Blogs**

Blogging nodes - comes with a ContentFinder and URL provider that lets you have wordpress like urls for your blog (e.g /year/month/date/post/)

**Authentication**

Basic Authentication library, with the core, login/register/reset password functionality - this is a lightweight library to get people started, the kit and the modules don't actually care how you authenticate your users - but this package gives you the basics you might need. 

**Forums**

Threaded forums, again using the User Generated Content node - simple forums that allow for users to have conversations on your site.

**Starter Kit** 

The basic core of a site, with content, gateway, homepage, contact, venue etc... The basics of a site - also comes with the aubergine dashboard that will help you install / manage any extra bits you may want. 

**Core**

The only package that all other elements will rely on - contains the helper code for the migrations needed when packages are installed. 
