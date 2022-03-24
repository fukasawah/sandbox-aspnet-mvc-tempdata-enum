# sandbox-aspnet-mvc-tempdata-enum


- .NET Core 3.1.x
- ASP.NET 3.1.23

### Steps to reproduce

![image](https://user-images.githubusercontent.com/3121624/159949043-13a43882-8fe0-424f-82d5-fa04f5b04b44.png)

Click "OK" link.

![image](https://user-images.githubusercontent.com/3121624/159949165-b9e4bbea-41a5-4f9f-914a-22da812fd06a.png)

- Set `State=OK` and redirect to Index page
- Show `State=OK` (Cookies should be deleted here, **but they are not.**)

Click "Reload" link. (or F5)

![image](https://user-images.githubusercontent.com/3121624/159949212-7fb60021-f667-4600-84ff-5644d87abddc.png)

- Expect: State=VIEW
- Actual: **State=OK**


### Why undelete enum value on TempData?


The code here does not take enum into account.

https://github.com/dotnet/aspnetcore/blob/v3.1.23/src/Mvc/Mvc.ViewFeatures/src/Filters/SaveTempDataPropertyFilterBase.cs#L55

`OriginalValue` derived from Cookie is handled as int, but `newValue` is handled as Enum since it is derived from Property.

See Variables in the following debugger for type differences

eg.) `string Message` 

![image](https://user-images.githubusercontent.com/3121624/159950008-98a52d3f-1889-4040-aecd-3b8fde78438b.png)

eg.) `enum State`

![image](https://user-images.githubusercontent.com/3121624/159950027-348e91ad-50e2-4a1d-b2d1-2dd845880349.png)

Therefore, `newValue.Equals(OriginalValue)` becomes `(1.Equals((Enum)1)` and returns false.
Then `tempData[key] = newValue` is processed and `tempData._initialKeys` is set, thus creating a cookie.
