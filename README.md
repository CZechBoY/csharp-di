# di
Simple Dependency injection container for C#

## Basic usage
```csharp
Container dic = new Container();

// make MyApplication instance with resolved dependencies (in constructor only)
MyApplication app = (MyApplication)dic.GetService(typeof(MyApplication));
app.run();
```

### Manual setup of services
Consider some HandlerResolver, which iterates over all known IHandler instances and do some work on them...
```csharp
Container dic = new Container();

// get instance of handler resolver which we can setup
MyHandlerResolver handlerResolver = (MyHandlerResolver) dic.GetService(typeof(MyHandlerResolver));

// get all classes that implements IHandler interface
// note: not classses, but already instanced objects
var handlers = dic.GetByInterface(typeof(IHandler));

// and iterate over all handlers
foreach (IHandler handler in handlers)
{
    // add new handler to resolver that it knows about it
    handlerResolver.AddHandler(handler);
}
```
