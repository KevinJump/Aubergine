@ECHO OFF
REM Builds the packages (for our.umbraco.org)
REM 
REM Our prefered install is via Nuget, but we 
REM put in a complete install package so people
REM can try it out that way. 

RMDIR .\PkgTemp /s /q
MKDIR .\PkgTemp

ECHO [Coping Files]
XCOPY ..\Aubergine.Web\App_Plugins\*.* .\PkgTemp\App_Plugins\ /S /Q /I
XCOPY ..\Aubergine.Web\Content\*.* .\PkgTemp\Content\ /S /Q /I
XCOPY ..\Aubergine.Web\css\*.* .\PkgTemp\css\ /S /Q /I
XCOPY ..\Aubergine.Web\fonts\*.* .\PkgTemp\fonts\ /S /Q /I
XCOPY ..\Aubergine.Web\Scripts\*.* .\PkgTemp\Scripts\ /S /Q /I
XCOPY ..\Aubergine.Web\Views\*.* .\PkgTemp\Views\ /S /Q /I

ECHO [Media]
XCOPY ..\Aubergine.Web\Media\*.* .\PkgTemp\Media\ /S /Q /I

ECHO [uSync Files]
XCOPY ..\Aubergine.Web\uSync\Data\*.* .\PkgTemp\App_Data\Aubergine\Complete /S /Q /I

ECHO [Extra File Clean up]

ECHO [Removing Models Builder]
RMDIR .\PkgTemp\App_Plugins\ModelsBuilder\ /s /q

ECHO [Removing Web.Configs]
DEL .\PkgTemp\Media\Web.Config /Q
DEL .\PkgTemp\Views\Web.Config /Q