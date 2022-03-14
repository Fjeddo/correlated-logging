# What?
This is a rough implementation of log correlation across service boundaries. The goals with the "library" is to have a small footprint:
- It supports Azure functions:
   - http triggered 
   - orchestration triggered
   - activities
- When registering the needed services, invoking the AddCorrelationDecoratedLogging, the defualt implementation of the ILogger<> interface is replaced by the CorrelationIdDecoratedLogger implementation.
- The footprint:
   - In the functions class, inherit the LogCorrelatedFunctions
   - Inject an instance of the ICorrelatedLoggingProvider<> interface
   - Use the protectd Log property of the base class in the functions class
   - Inject ILogger<> in other classes that need logging (it will be the correlation decorated implementation)

## Classes of interest:
- The context extension classes, `ContextExtensions`
- `Input<>` to be used in durable functions
