# LWJ.Injection

权限验证
``` c#
[Permission("get_name", MyAuthenticationProviderName)]
public string GetName()
{
   return "name";
}
```


参数检查
``` c#
public void Add([Range(1, 100)] int a,int b)
{
  return a+b;  
}

public void SetUrl([Regex("\\s*http://.*")] string url)
{
}

public int StringToInteger([Regex("\\d+")] string integer)
{
    return int.Parse(integer);
}
```

缓存
``` c#
[CachingCall]
[CachingLifetime(Method = "LifetimeTimeout_100")]
public int Lifetime_ValueType_100()
{
    return DateTime.Now.Millisecond;
}

ILifetime LifetimeTimeout_100(int result)
{
    var lifetime = new TimeoutLifetime(Timeout);
    lifetime.SetValue(result);
    return lifetime;
}
```

获取调用者信息，日志/异常追踪
``` c#
public CallerInfo MethodCall(string message,
             [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
{
    StringBuilder trace = new StringBuilder();

    trace.AppendLine("member name: " + memberName);
    trace.AppendLine("source file path: " + sourceFilePath);
    trace.AppendLine("source line number: " + sourceLineNumber);
    trace.Append("message: " + message);
    Console.WriteLine(trace.ToString());
    return new CallerInfo(memberName, sourceFilePath, sourceLineNumber);
}
```
