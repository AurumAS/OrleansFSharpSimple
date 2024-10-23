# What is this?

This is a .NET Solution showing how one can use Microsoft Orleans 8 in an F# project.

To do this we need to:

1. Have a C# project which will generate code for us.
1. Reference the grain project from the C# project.
1. Reference the C# project from the host.
1. In the C# project, we need to tell Orleans to generate code for our Grains.
1. In the host project, we'll have a special module called Load, which will ensure that the necessary DLLs are loaded as application parts.

Use the included `.http` file to execute queries against the web API.

The dashboard is available at https://localhost:7046/dashboard