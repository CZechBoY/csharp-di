# di
Simple Dependency injection container for C#

## Basic usage
### Basic service class example
There is no need to specify that class is service! You can make service from whatever, even system classes.

```csharp
namespace MyNamespace
{
    class MyApplication
    {
        public MyApplication(MyClass c1, MyClass2 c2, MyClass3 c3)
        {
            ...
        }
    }
}
```
### And usage
```csharp
Container dic = new Container();

// make MyApplication instance with resolved dependencies (in constructor only)
MyApplication app = dic.GetService<MyApplication>();
app.run();
```

## Manual setup of services
Consider some HandlerResolver, which iterates over all known IHandler instances and do some work on them...

### Basic service class with additional setup
```csharp
namespace MyNamespace
{
    class MyHandlerResolver
    {
        private readonly List<IHandler> handlers = new List<IHandler>();
        
        public void AddHandler(IHandler handler)
        {
            this.handlers.Add(handler);
        }
    }
}
```

### And final setup
```csharp
Container dic = new Container();

// get instance of handler resolver which we can setup
MyHandlerResolver handlerResolver = dic.GetService<MyHandlerResolver>();

// get all classes that implements IHandler interface
// note: not classses, but already instanced objects
var handlers = dic.GetByInterface<IHandler>();

// and iterate over all handlers
foreach (IHandler handler in handlers)
{
    // add new handler to resolver that it knows about it
    handlerResolver.AddHandler(handler);
}
```
