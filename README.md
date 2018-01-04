# PolicyServer (local version)
We've been talking about separation of concerns of authentication and authorization quite a bit in the past (see here...and here).
This lead to a commercial product called PolicyServer (see here).

This repo contains a simplified version of PolicyServer, but has all the necessary code to implement the authorization pattern we are recommending.

This library does not have the advanced features of PolicyServer like hierarchical policies, client/server separation, management APIs, caching, auditing etc.

## Defining an authorization policy
The authorization policy is defined as a JSON document (typically in `appsettings.json`). In the policy you can define two things

* application roles (and who are the members of these roles)
* mapping of roles to permissions

### Defining application roles
Role membership can be defined based on the IDs (aka subject IDs) of the users, e.g.:

```javascript
{
  "Policy": {
    "roles": [
      {
        "name": "doctor",
        "subjects": [ "1", "2" ]
      }
    ]
  }
}
```

The value of the `sub` claim is used to determine role membership at runtime.

Additionally identity roles coming from the authentication system can be mapped to application roles, e.g.:

```javascript
{
  "Policy": {
    "roles": [
      {
        "name": "patient",
        "identityRoles": [ "customer" ]
      }
    ]
  }
}
```

The value of the `role` claim is used to map identity roles to application roles.

### Mapping permissions to application roles
In the permissions element you can define permissions, and which roles they are mapped to:

```javascript
{
  "Policy": {
    "roles": [
      {
        "name": "doctor",
        "subjects": [ "1", "2" ],
        "identityRoles": [ "surgeon" ]
      },
      {
        "name": "nurse",
        "subjects": [ "11", "12" ],
        "identityRoles": [ "RN" ]
      },
      {
        "name": "patient",
        "identityRoles": [ "customer" ]
      }
    ],
    "permissions": [
      {
        "name": "SeePatients",
        "roles": [ "doctor", "nurse" ]
      },
      {
        "name": "PerformSurgery",
        "roles": [ "doctor" ]
      },
      {
        "name": "PrescribeMedication",
        "roles": [ "doctor", "nurse" ]
      },
      {
        "name": "RequestPainMedication",
        "roles": [ "patient" ]
      }
    ]
  }
}
```

### Using the PolicyServer client library in your ASP.NET Core application
Fist, you need to register the PolicyServer client with the DI system. This is where you specify the configuration section that holds your policy.

```csharp
services.AddPolicyServerClient(Configuration.GetSection("Policy"));
```

After that you can inject the `PolicyServerClient` into your application code, e.g.:

```csharp
public class HomeController : Controller
{
    private readonly PolicyServerClient _client;

    public HomeController(PolicyServerClient client)
    {
        _client = client;
    }
}
```

`PolicyServerClient` has three methods:

* `EvaluateAsync` - returns roles and permissions for a given user.
* `IsInRoleAsync` - queries for a specific role
* `HasPermissionAsync` - queries for a specific permission

```csharp
public async Task<IActionResult> Secure()
{
    // get roles and permission for current user
    var result = await _client.EvaluateAsync(User);
    var roles = result.Roles;
    var permissions = result.Permissions;

    // check for doctor application role
    var isDoctor = await _client.IsInRoleAsync(User, "doctor");
    
    // check for PrescribeMedication permission
    var canPrescribeMedication = await _client.HasPermissionAsync(User, "PrescribeMedication");

    // rest omitted
}
```

You can now this simple client library directly, or build higher level abstractions for your applications.

## Mapping permissions and application roles to user claims
Instead of using the our client library directly, you can also turn the permissions and application roles into user claims.
This is mainly useful if you want to use the standard `ClaimsPrincipal`-based APIs or the class `[Authorize(Roles = "...")]` attribute.

We provide a middleware for this purpose, which runs on every request and turns the user's authorization data into claims:

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseAuthentication();
    app.UsePolicyServerClaimsTransformation();

    app.UseStaticFiles();
    app.UseMvcWithDefaultRoute();
}
```

This way you could query roles or permissions like this:

```csharp
// get all roles
var roles = User.FindAll("role");

// or
var isDoctor = User.HasClaim("role", "doctor");
```

## Mapping permissions to ASP.NET Core authorization policies
Another option is to automatically map permissions of a user to ASP.NET Core authorization policies.
This way you can use the standard ASP.NET Core authorization APIs or the `[Authorize("permissionName")]` syntax.

To enable that, you need to register a custom authorization policy provider when adding the PolicyServer client library:

```csharp
services.AddPolicyServerClient(Configuration.GetSection("Policy"))
    .AddAuthorizationPermissionPolicies();
```

This will allow you to decorate controllers/actions like this:

```csharp
[Authorize("PerformSurgery")]
public async Task<IActionResult> PerformSurgery()
{
    // omitted
}
```