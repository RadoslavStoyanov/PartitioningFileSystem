# Partitioning

What could be done better:
- Extract magic strings and repeated values in a constant class.
- OffsetService should not call directly DistributedFS.Instance.FileLength and DistributedFS.Instance.Threshold. This should be re-routed through a Load balancer.
- WordsPerLineService could use some refactoring. The logic is a bit messy now and it violates the "O" principle from SOLID.
- Configuring of Publisher and Listener could be done through appsettings.
- Add logic for automatically identifying threshold and file chunk size.

I haven't accomplished:
- "Calling RemoteExecutor.run with the same serviceId in parallel throws an exception"
- "Calling RemoteExecutor.run with invalid serviceId throws an exception"
