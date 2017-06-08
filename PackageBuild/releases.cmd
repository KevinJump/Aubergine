@ECHO OFF
REM		Builds the Nuget Packages for the Aubergine Suite. 
REM		The logic for what goes into a package is in each
REM		.nuspec file. but in genral, they are just zips of 
REM		the files needed. with a install script that 
REM		puts some usync files into app_data so migrations
REM		can setup umbraco on first run. 


ECHO Nuget
nuget pack ..\Aubergine.Core\Aubergine.Core.nuspec
nuget pack ..\Aubergine.Auth\Aubergine.Auth.nuspec
nuget pack ..\Aubergine.UserContent\Aubergine.UserContent.nuspec
nuget pack ..\Aubergine.Comments\Aubergine.Comments.nuspec
nuget pack ..\Aubergine.Blog\Aubergine.Blog.nuspec
nuget pack ..\Aubergine.Forums\Aubergine.Forums.nuspec
nuget pack ..\Aubergine.Web\Aubergine.Web.nuspec
nuget pack ..\Aubergine.Complete\Aubergine.Complete.nuspec

ECHO Copy to local git
copy *.nupkg c:\source\localgit /y
