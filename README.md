# procosys_scaffold
Template repo for new ProCoSys backend solutions/application. Swagger enabled Api using Entity Framework and Mediator
This repo is ment to be used as startup code base for new ProCoSys solutions.

After using this as template for creating new repo, perform renames from PCS5 to a suitable name. 
This will typical be the name of the new application. Sample: Completion.

Rename file/folder names for:
* Solution file
* Folder names for projects
* csproj files

Edit solution file, csproj filess and Dockerfile in Notepad and fix paths to csproj-files according to new name

Solution should now load correctly in Visual Studio. Check Project references.

* Rename namespaces Equinor.ProCoSys.PCS5 in all *.cs-files

* Rename classes and filenames starting with PCS5 (as Equinor.ProCoSys.PCS5.Infrastructure.PCS5Context)

* Search and replace softstrings containing PCS5 (as ProCoSys PCS5)

(Tip: search and replace with case sensitive using Notepad++. Filenames must be fixed to match names of classes / interfaces)

* Generate a new Guid for UserSecretsId in Equinor.ProCoSys.*.WebApi.csproj

* Setup appsettings.json and create a user sercret file according to new application (usersecrets_minimum.json is a minimum user sercret file)

In repo it is made an AggregateRoot called Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate.Foo. This can typical be a domain object in ProCoSys. Sample: Punch.
* Rename classes, interfaces, variables containing Foo or foo to something suitable.

After all renaming completed, there should not exists any phrases as foo or pcs5 in any file in solution.
(Tip: Search case insensitive in all files (*.*) for foo and pcs5.)

* Create a new migration for database by:
 1) Remove files under Equinor.ProCoSys.PCS5.Infrastructure/Migrations (Typical some .cs-files with dates as 20230316075815_InitialSetup.cs + *ContextModelSnapshot.cs)
 2) Open Package Manager Console and add new migration with command: add-migration InitialSetup

With correct connectionstring in appsettings.json or user sercret file, pointing to an existing database, the solution should now be able to run and test in swagger.

# Test the solution
NB 1! When testing Foo endpoints, these are secured with fictive permissions as FOO/READ etc.
As a result, all users will get 401 Access denied on these since these permissions can't be given as-is.
To test endpoints without needing permissions, remove [Authorize(Roles = *** 
from Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo.FoosController. 
NB 2! Without the Authorize-attribute will cause some negative integration tests in 
Equinor.ProCoSys.PCS5.WebApi.IntegrationTests.Foos.FoosControllerNegativeTests
to fail since some tests expect HttpStatusCode.Forbidden)
