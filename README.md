# What?
- Adds a correlationId header to the outgoing http request, in *FunctionApp1.Function1.cs* and *FunctionApp2.Function2.cs*.
- The correlationId is either read from the **incoming http request** or **generated**, implemented in the *CorrelatedLogger.CorrelationIdProvider* class.
- The correlationId is added to the log message written and in *CorrelatedLogger.ExtendedLogger* implementation also added as a customDimension extra prop in Application Insights.