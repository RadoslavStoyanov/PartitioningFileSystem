# Partitioning

Prerequisite for running the application:
- Windows OS.
- .NET 6 runtime and .NET 6 SDK.
- Visual studio 2022 Community.
- RabbitMQ service installed and running.
- Text file must be placed in Resources folder.
- Text file name ise set in Partitioning.ServiceImplementations.DistributedFileSystem.fileName.
  
What could be done better:
- Extract magic strings and repeated values in a constant class.
- OffsetService should not call directly DistributedFS.Instance.FileLength and DistributedFS.Instance.Threshold. This should be re-routed through a Load balancer.
- WordsPerLineService could use some refactoring. The logic is a bit messy now and it violates the "O" principle from SOLID.
- Configuring of Publisher and Listener could be done through appsettings.
- Add logic for automatically identifying threshold and file chunk size.
- Add throttling mechanism in the Load balancer.

I haven't accomplished:
- "Calling RemoteExecutor.run with the same serviceId in parallel throws an exception"
- "Calling RemoteExecutor.run with invalid serviceId throws an exception"
