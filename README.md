# procosys_scaffold
Template repo for new ProCoSys backend solutions. Swagger enabled Api using Entity Framework and Mediator
This repo is ment to be used as startup code base for new ProCoSys solutions.

After using this as template for creating new repo, perform renames from PCS5 to suitable name. Rename file/folder names for:
* Solution file
* Folder names for projects
* csproj files

Edit solution file, csproj filess and Dockerfile in Notepad and fix paths to csproj-files according to new name

Solution should now load correctly in Visual Studio. Check Project references.

* Rename namespaces Equinor.ProCoSys.PCS5 in all *.cs-files

* Renames classes starting with PCS5 (as Equinor.ProCoSys.PCS5.Infrastructure.PCS5Context)

* Search and replace softstrings containing PCS5 (as ProCoSys PCS5)

