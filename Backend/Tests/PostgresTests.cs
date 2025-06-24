< Project Sdk = "Microsoft.NET.Sdk.Web" >

  < PropertyGroup >
    < TargetFramework > net8.0 </ TargetFramework >
    < Nullable > enable </ Nullable >
    < ImplicitUsings > enable </ ImplicitUsings >
  </ PropertyGroup >

  < ItemGroup >
    < None Update = "appsettings.json" >
      < CopyToOutputDirectory > PreserveNewest </ CopyToOutputDirectory >
    </ None >
    < PackageReference Include = "Microsoft.AspNetCore.Authentication.JwtBearer" Version = "8.0.14" />
    < PackageReference Include = "Microsoft.EntityFrameworkCore" Version = "9.0.6" />
    < PackageReference Include = "Microsoft.EntityFrameworkCore.Design" Version = "9.0.6" >
      < PrivateAssets > all </ PrivateAssets >
      < IncludeAssets > runtime; build; native; contentfiles; analyzers; buildtransitive </ IncludeAssets >
    </ PackageReference >
    < PackageReference Include = "Microsoft.EntityFrameworkCore.Sqlite" Version = "9.0.6" />
    < PackageReference Include = "Microsoft.EntityFrameworkCore.SqlServer" Version = "9.0.6" />
    < PackageReference Include = "Microsoft.EntityFrameworkCore.Tools" Version = "9.0.6" >
      < PrivateAssets > all </ PrivateAssets >
      < IncludeAssets > runtime; build; native; contentfiles; analyzers; buildtransitive </ IncludeAssets >
    </ PackageReference >
    < PackageReference Include = "Npgsql.EntityFrameworkCore.PostgreSQL" Version = "9.0.4" />
    < PackageReference Include = "Swashbuckle.AspNetCore" Version = "9.0.1" />
    < PackageReference Include = "xunit" Version = "2.9.3" />
    < PackageReference Include = "Microsoft.NET.Test.Sdk" Version = "*" />
    < PackageReference Include = "xunit.runner.visualstudio" Version = "*" />
  </ ItemGroup >


  < ItemGroup >
    < None Include = "Keys\**\*.*" >
      < CopyToOutputDirectory > Always </ CopyToOutputDirectory >
    </ None >
  </ ItemGroup >

  < !--Include EF Core Migrations from Data/Migrations -->
  <ItemGroup>
    <Compile Include="Data\Migrations\**\*.cs" />
  </ItemGroup>

</Project>
