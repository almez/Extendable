# Extendable

"Extendable" is an open source project under .net core framework, with "Extendable" you can add dynamic fields to your objects with literally no effort.


## Features

* Introduce new fields to your types without changing underlying table in database.


* You can use it as simple as  

  ```
  obj.SetAttribute("LastName", "Menz");
  string lastName = obj.GetAttribute<string>("LastName")
  ```

* Multi-Languages support for your fields.

  ```
  obj.SetAttribute("LastName", "Menz", "ar");
  string arLastName = obj.GetAttribute<string>("LastName", "ar")
  ```

* Work with any data provider (SQL Server, Oracle ..etc) as you are the one who implements the field provider.


* Caching layer for your fields to enhance the performance of your application.

## Getting Started

I'll walk you through this library and how to install and integrate to your exising project step by step.

### Prerequisites

This library is targeting .net core 2.0 application, feel free to contribute to this repository to target more frameworks.

### Installing

This projects is available as NuGet package being updated whenever ther's a successful build (continuous integration).

You need to get this package installed in your project.

Package Link : [https://www.nuget.org/packages/Extendable](https://www.nuget.org/packages/Extendable)

```
PM> Install-Package Extendable
```

### Integrate with your project

* Firstly, Implement IExtendable interface in the types that you want to extend with dynamic fields

<pre>
public class User : <b>IExtendable</b>
{
    public int Id { get; set; }

    public string Name { get; set; }
}
</pre>

* Then, create your own field provider to store field values to your permanent store (SQL Server, Oracle, Files, Xml ..etc)
All what you need is to override these 3 methods. All other methods will work fine after your changes.
 
 Here you can find example of Implementing such provider with EntityFramework for instance.
 
<pre>
public class ApplicationDbContext : DbContext
{
    // ...

    <b>public DbSet<Field> Fields { set; get; }</b>

    // ...
}

public class EntityFrameworkFieldProvider : <b>BaseFieldProvider</b>
{
    <b>public override void UpdateFieldInDb(Field field)</b>
    {
        using (var context = new ApplicationDbContext())
        {
            var entity = context.Fields.Find(field.Id);

            entity.LastUpdatedUtc = field.LastUpdatedUtc;
            entity.FieldValue = field.FieldValue;

            context.SaveChanges();
        }
    }

    <b>public override Field GetFieldFromDb(string holderType, string holderId, string fieldName, string language = "en")</b>
    {
        using (var context = new ApplicationDbContext())
        {
            return context.Fields.SingleOrDefault(f => f.HolderType == holderType
                                                        && f.HolderId == holderId
                                                        && f.FieldName == fieldName
                                                        && f.Language == language);
        }
    }

    <b>public override void AddFieldValueToDb(Field field)</b>
    {
        using (var context = new ApplicationDbContext())
        {
            context.Fields.Add(field);

            context.SaveChanges();
        }
    }
}
</pre>

You can find another example here [InMemoryFieldProvider](https://github.com/almez/Extendable/blob/master/src/Extendable.Tests/Providers/InMemoryFieldProvider.cs) for testing purposes.

### Configuration

At the application startup you need to configure this library properly, as following :

<pre>
Extendable.Configuration.<b>FieldProvider</b> = new EntityFrameworkFieldProvider(); // your field provider, mandatory.
Extendable.Configuration.<b>CacheEnabled</b> = true; // true by default
Extendable.Configuration.<b>CacheSizeLimit</b> = 1 * 1024 * 1024; // 1 MB by default
</pre>

### Library API and how to use it

* Now you can set new dynamic fields to your object as simple as 

<pre>
<b>//Set Attributes</b>
user.SetAttribute("LastName", <b>"Menz"</b>);
user.SetAttribute("LastName", "الكنية", "ar");
user.SetAttribute("LastName", "soyadı", "tr");

user.SetAttribute("Age", <b>28</b>);

<b>//Get Attributes</b>
string enLastName = user.GetAttribute<<b>string</b>>("LastName");
string arLastName = user.GetAttribute<string>("LastName", "ar");
string trLastName = user.GetAttribute<string>("LastName", "tr");

int age = user.GetAttribute<<b>int</b>>("Age");
</pre>

## Build Server

[![Build status](https://ci.appveyor.com/api/projects/status/ny9uauwnnyo70s3l?svg=true)](https://ci.appveyor.com/project/almez/extendable)

## Dependencies

* [CachingManager](https://github.com/almez/CachingManager) - The caching library
* [xUnit](https://xunit.github.io) - Unit Testing Framework

## Contributing

Feel free to contribute to this repository, you can reach me out by the email [eng.ngmalden@gmail.com](mailto://eng.ngmalden@gmail.com)

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## Authors

* **Alden Menzalgy** - *Software Engineer* - [https://github.com/almez](https://github.com/almez)

See also the list of [contributors](https://github.com/almez/CachingManager/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/almez/Extendable/blob/master/LICENSE) file for details
