# Calculus
Calculus is a basic open source computer algebra library for .Net written in C#

### Example

```csharp
string input = "x^2*x^(a+b)";
Expression exp = Parser.Parse(input);

exp = exp.Simplify();
string result = exp.Render();
// result = x^(a+b+2)
```
