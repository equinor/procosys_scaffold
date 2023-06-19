# procosys-scaffold
Template repo for new ProCoSys backend solutions/application. Swagger enabled Api using Entity Framework and Mediator
This repo is ment to be used as startup code base for new ProCoSys solutions.

After using this as template for creating new repo, perform renames from PCS5 to a suitable name. 
This will typical be the name of the new application. Sample: Completion.

Rename file/folder names for:
* Solution file
* Folder names for projects
* csproj files

Edit solution file, csproj files and Dockerfile in Notepad and fix paths to csproj-files according to new name

Solution should now load correctly in Visual Studio. Check Project references.

* Rename namespaces Equinor.ProCoSys.PCS5 in all *.cs-files and launchSettings.json

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
All tests should pass.

# Permissions
All Foo endpoints are secured with the AuthorizeAny attribute, each endpoint secured with 2
permissions (roles). AuthorizeAny is used to secure with a list of permission, where clients
are allowed to use endpoint if having any of listed permission.
The first permission on Foo-endpoints, is a non-existing fictive permissions as FOO/READ etc.
This permission shoild be replaced with a real permission in the final solution. The integration tests 
are setup to test that each endpoint actually ARE secured by a such FOO-permission.
The second permision, APPLICATION_EXPLORER/EXECUTE, are there to be able to test these endpoints 
in swagger as a logged in ProCoSys Administrator (which normally have this permission). This
permission should be considered to be removed in the final solution.