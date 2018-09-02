# PolicyServer (local version)
We've been talking about separation of concerns of authentication and authorization quite a bit in the past (see the [blog post](https://leastprivilege.com/2016/12/16/identity-vs-permissions/) that started all and the [video](https://vimeo.com/223982185) that showed off our first prototype).
As a result, we have developed a commercial product called PolicyServer as part of a joint venture with [Solliance](https://solliance.net). Here are a few links to the product and pricing page: 

* [PolicyServer Overview](https://solliance.net/products/policyserver)
* [PolicyServer Pricing](https://solliance.net/products/policyserverpricing)

In this repository we have provided a free, open source, and simplified version of the authorization pattern we propose - with the necessary code to create a simple implementation in your applications. 

> NOTE: This open source library does not have the advanced features of the PolicyServer product like hierarchical policies, client/server separation, management APIs and UI, caching, auditing etc., but the client library is syntax-compatible with its "big brother" in terms of integration to your applications. This allows an upgrade path with minimal code changes if you start with this client library.

## Defining an authorization policy
The authorization policy is defined as a JSON document (typically in `appsettings.json`). In the policy you can define two things

* application roles (and the users that are members of these roles)
* application permissions (and which application roles have these permissions)

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

The value of the user's `sub` claim is used to determine role membership at runtime.

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

The user's `role` claims are used to map identity roles to application roles.

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

After that you can inject the `IPolicyServerClient` anywhere into your application code, e.g.:

```csharp
public class HomeController : Controller
{
    private readonly IPolicyServerRuntimeClient _client;

    public HomeController(IPolicyServerRuntimeClient client)
    {
        _client = client;
    }
}
```

`PolicyServerClient` has three methods:

* `EvaluateAsync` - returns application roles and permissions for a given user.
* `IsInRoleAsync` - queries for a specific application role
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

You can now use this simple client library directly, or build higher level abstractions for your applications.

## Mapping permissions and application roles to user claims
Instead of using the `PolicyServerClient` class directly, you might prefer a programming model where the current user's claims are automatically populated with the policy's application roles and permissions. This is mainly useful if you want to use the standard `ClaimsPrincipal`-based APIs or the `[Authorize(Roles = "...")]` attribute.

A middleware (registered with `UsePolicyServerClaims`) is provided for this purpose, and runs on every request to map the user's authorization data into claims:

```csharp
public void Configure(IApplicationBuilder app)
{
    app.UseAuthentication();
    app.UsePolicyServerClaims();

    app.UseStaticFiles();
    app.UseMvcWithDefaultRoute();
}
```

Then in the application code you could query application roles or permissions like this:

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
